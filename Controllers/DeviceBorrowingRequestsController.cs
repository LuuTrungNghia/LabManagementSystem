using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System.Linq;
using System.Threading.Tasks;

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

        private bool DeviceBorrowingRequestExists(int id)
        {
            return _context.DeviceBorrowingRequests.Any(e => e.RequestId == id);
        }
    }
}
