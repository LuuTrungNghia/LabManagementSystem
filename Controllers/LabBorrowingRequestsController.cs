using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using LabManagementSystem.Dtos;

namespace LabManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabBorrowingRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LabBorrowingRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.LabBorrowingRequests
                .Include(l => l.User)
                .Include(l => l.Lab)
                .ToListAsync();
            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LabBorrowingRequest labBorrowingRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            labBorrowingRequest.CreatedAt = DateTime.UtcNow;
            labBorrowingRequest.UpdatedAt = DateTime.UtcNow;

            _context.LabBorrowingRequests.Add(labBorrowingRequest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRequests), new { id = labBorrowingRequest.RequestId }, labBorrowingRequest);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.LabBorrowingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] LabBorrowingRequest labBorrowingRequest)
        {
            if (id != labBorrowingRequest.RequestId) return BadRequest("Request ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                labBorrowingRequest.UpdatedAt = DateTime.UtcNow;
                _context.LabBorrowingRequests.Update(labBorrowingRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabBorrowingRequestExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var labBorrowingRequest = await _context.LabBorrowingRequests.FindAsync(id);
            if (labBorrowingRequest == null) return NotFound();

            _context.LabBorrowingRequests.Remove(labBorrowingRequest);
            await _context.SaveChangesAsync();
            return NoContent();
        } 
        [HttpPost("/request-borrowing-labs")]
        public async Task<IActionResult> RequestBorrowingLabs(RequestBorrowingLabsDto model)
        {
            try
            {
                var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId) ?? throw new Exception("User does not exist.");
                if (checkUser.Status == 1)
                {
                    return BadRequest("User is already booked for this request.");
                }

                var lab = await _context.Labs.FirstOrDefaultAsync(l => l.LabId == model.LabId) 
                          ?? throw new Exception("Lab does not exist.");
                if (lab.Status == 2)
                {
                    return BadRequest("Lab is not available for booking.");
                }

                var labBorrowingRequest = new LabBorrowingRequest
                {
                    UserId = model.UserId,
                    LabId = model.LabId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Reason = model.Reason,
                    ResponsibleLecturerId = model.ResponsibleLecturerId,
                    UserType = model.UserType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.LabBorrowingRequests.Add(labBorrowingRequest);
                await _context.SaveChangesAsync();

                return Ok("Lab borrowing request created successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPatch("/ar-request-borrowing-labs")]
        public async Task<IActionResult> ArRequestBorrowingLabs(ArRequestBorrowingLabsDto model)
        {
            try
            {
                foreach (var id in model.LabBorrowingRequestIds)
                {
                    var request = await _context.LabBorrowingRequests.FindAsync(id);
                    if (request != null)
                    {
                        request.Status = model.Status; 
                        _context.Update(request);
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

        private bool LabBorrowingRequestExists(int id)
        {
            return _context.LabBorrowingRequests.Any(e => e.RequestId == id);
        }
    }
}
