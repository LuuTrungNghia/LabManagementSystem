using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        private bool LabBorrowingRequestExists(int id)
        {
            return _context.LabBorrowingRequests.Any(e => e.RequestId == id);
        }
    }
}
