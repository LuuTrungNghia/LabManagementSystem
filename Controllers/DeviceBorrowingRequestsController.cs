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
                var user = await _context.Users.FindAsync(model.UserId) ?? throw new Exception("User does not exist.");
                
                if (user.Status == 1)
                    return BadRequest("User is already approved for booking.");

                var devices = await _context.Devices.Where(d => model.DeviceIds.Contains(d.DeviceId)).ToListAsync();
                
                if (!devices.Any())
                    return BadRequest("No devices found for the provided IDs.");

                List<DeviceBorrowingRequest> requests = new List<DeviceBorrowingRequest>();

                foreach (var device in devices)
                {
                    var conditionDetail = await _context.DeviceConditionDetails
                        .Where(dc => dc.DeviceTypeId == device.DeviceTypeId && dc.Condition == "Good")
                        .FirstOrDefaultAsync();
                    
                    if (conditionDetail == null || conditionDetail.Quantity < model.Quantity)
                        continue;

                    var request = new DeviceBorrowingRequest
                    {
                        UserId = model.UserId,
                        DeviceId = device.DeviceId,
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        CreatedAt = DateTime.UtcNow,
                        Quantity = model.Quantity,
                        Status = "Pending"
                    };
                    
                    conditionDetail.Quantity -= model.Quantity;
                    requests.Add(request);
                }

                await _context.AddRangeAsync(requests);
                var result = await _context.SaveChangesAsync() > 0;
                return result ? Ok("Success") : BadRequest("Error in processing requests.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("/ar-request-booking-devices")]
        public async Task<IActionResult> ArRequestBorrowingDevices(ArRequestBorrowingDevicesDto model)
        {
            try
            {
                foreach (var id in model.DeviceBorrowingRequestIds)
                {
                    var request = await _context.DeviceBorrowingRequests
                        .Include(r => r.Device)
                        .FirstOrDefaultAsync(r => r.RequestId == id);
                    
                    if (request == null)
                        continue;

                    var conditionDetail = await _context.DeviceConditionDetails
                        .Where(dc => dc.DeviceTypeId == request.Device.DeviceTypeId && dc.Condition == "Good")
                        .FirstOrDefaultAsync();

                    if (conditionDetail == null)
                        throw new Exception("Device condition detail not found.");
                    
                    request.Status = model.Status;
                    
                    if (model.Status == "Approved")
                        conditionDetail.Quantity -= request.Quantity;

                    _context.DeviceBorrowingRequests.Update(request);
                    _context.DeviceConditionDetails.Update(conditionDetail);
                }
                
                var result = await _context.SaveChangesAsync() > 0;
                return result ? Ok("Successfully updated requests.") : BadRequest("Error updating requests.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        private bool DeviceBorrowingRequestExists(int id)
        {
            return _context.DeviceBorrowingRequests.Any(e => e.RequestId == id);
        }
    }
}
