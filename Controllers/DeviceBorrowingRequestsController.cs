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
        public async Task<IActionResult> RequestBookingDevices(RequestBookingDeviceDto model)
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
                    return BadRequest("-------");
                }

                IList<DeviceBorrowingRequest> requests = new List<DeviceBorrowingRequest>();

                for(int i = 0; i < checkDevices.Count; i++)
                {
                    // 1: Con
                    // 2 : KHong con
                    if (checkDevices[i].Status == 2)
                    {
                        continue;
                    }
                    int quantity = checkDevices[i].Quantity > model.Quantity ? model.Quantity : checkDevices[i].Quantity;
                    string status = "AAAAAAA";
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
                        Status = status
                    };
                    requests.Add(deviceBorrowingRequest);
                }

                await _context.AddRangeAsync(requests);
                var response = await _context.SaveChangesAsync() > 0;

                if (response)
                {
                    return Ok("Success");
                }
                return BadRequest("Error"); 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            

        }
        
        [HttpPatch("/ar-request-booking-devices")]
        public async Task<IActionResult> ArRequestBookingDevices(ArRequestBookingDevicesDto model)
        {
            try
            {
                foreach (var id in model.ArRequestIds)
                {
                    var arFound = await _context.DeviceBorrowingRequests.FirstOrDefaultAsync(r => r.RequestId == id);
                    
                    if (arFound != null)
                    {
                        var deviceBorrowingRequest = await _context.Devices.FirstOrDefaultAsync(r => r.DeviceId == arFound.DeviceId)
                            ?? throw new Exception("Device not found.");
                        arFound.Status = model.Status;
                        deviceBorrowingRequest.Quantity -= deviceBorrowingRequest.Quantity;
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
