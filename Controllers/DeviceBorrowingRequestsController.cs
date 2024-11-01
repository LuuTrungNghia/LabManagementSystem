using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;
using LabManagementSystem.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LabManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceBorrowingRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeviceBorrowingRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.DeviceBorrowingRequests
                .Include(d => d.Device)
                .Include(u => u.User)
                .ToListAsync();
            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceBorrowingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            request.CreatedAt = DateTime.UtcNow;
            request.UpdatedAt = DateTime.UtcNow;

            _context.DeviceBorrowingRequests.Add(request);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRequests), new { id = request.RequestId }, request);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.DeviceBorrowingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DeviceBorrowingRequest request)
        {
            if (id != request.RequestId) return BadRequest("Request ID mismatch.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                request.UpdatedAt = DateTime.UtcNow;
                _context.DeviceBorrowingRequests.Update(request);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceBorrowingRequestExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.DeviceBorrowingRequests.FindAsync(id);
            if (request == null) return NotFound();

            _context.DeviceBorrowingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPost("/request-booking-devices")]
        public async Task<IActionResult> RequestBorrowingDevices(RequestBorrowingDeviceDto model)
        {
            try
            {
                var checkUser = await _context.Users.FirstOrDefaultAsync(r => r.UserId == model.UserId) ?? throw new Exception("User dont exists.");
                if (checkUser.Status == 1)
                {
                    return BadRequest("User is already booked for this request.");
                }

                var checkDevices = await _context.Devices.Where(r => model.DeviceIds.Contains(r.DeviceId)).Distinct().ToListAsync();
                if (!checkDevices.Any())
                {
                    return BadRequest("No devices found for the provided IDs.");
                }

                IList<DeviceBorrowingRequest> requests = new List<DeviceBorrowingRequest>();

                for(int i = 0; i < checkDevices.Count; i++)
                {
                    if (checkDevices[i].Status == 2)
                    {
                        continue;
                    }

                    int quantity = checkDevices[i].Quantity > model.Quantity ? model.Quantity : checkDevices[i].Quantity;
                    DeviceBorrowingRequest deviceBorrowingRequest = new DeviceBorrowingRequest()
                    {
                        RequestId = 0,
                        UserId = model.UserId,
                        DeviceId = checkDevices[i].DeviceId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = model.UserId.ToString(),
                        Quantity = quantity,
                        Status = "Pending"
                    };
                    requests.Add(deviceBorrowingRequest);
                }
                await _context.AddRangeAsync(requests);
                var response = await _context.SaveChangesAsync() > 0;
                return response ? Ok("Success") : BadRequest("Error");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("/ar-request-booking-devices")]
        public async Task<IActionResult> ArRequestBorrowingDevices(ArRequestBorrowingDevicesDto model)
        {
            try
            {
                foreach (var id in model.DeviceBorrowingRequestIds)
                {
                    var arFound = await _context.DeviceBorrowingRequests.FirstOrDefaultAsync(r => r.RequestId == id);
            
                    if (arFound != null)
                    {
                        var deviceBorrowingRequest = await _context.Devices.FirstOrDefaultAsync(r => r.DeviceId == arFound.DeviceId)
                                                     ?? throw new Exception("Device not found.");
                        arFound.Status = model.Status;
                        deviceBorrowingRequest.Quantity -= arFound.Quantity;
                        deviceBorrowingRequest.Status = deviceBorrowingRequest.Quantity == 0 ? 2 : 1;
                        _context.Update(arFound);
                        _context.Update(deviceBorrowingRequest);
                    }
                }
                var response = await _context.SaveChangesAsync() > 0;
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        private bool DeviceBorrowingRequestExists(int id)
        {
            return _context.DeviceBorrowingRequests.Any(e => e.RequestId == id);
        }
    }
}
