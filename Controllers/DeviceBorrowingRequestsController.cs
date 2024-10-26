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

        // GET: api/DeviceBorrowingRequests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.DeviceBorrowingRequests
                .Include(d => d.Device)
                .Include(u => u.User)
                .ToListAsync();
            return Ok(requests);
        }

        // POST: api/DeviceBorrowingRequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DeviceBorrowingRequest request)
        {
            if (ModelState.IsValid)
            {
                request.CreatedAt = DateTime.Now;
                request.UpdatedAt = DateTime.Now;

                _context.Add(request);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetRequests), new { id = request.RequestId }, request);
            }
            return BadRequest(ModelState);
        }

        // GET: api/DeviceBorrowingRequests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.DeviceBorrowingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        // PUT: api/DeviceBorrowingRequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] DeviceBorrowingRequest request)
        {
            if (id != request.RequestId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    request.UpdatedAt = DateTime.Now;
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceBorrowingRequestExists(request.RequestId)) return NotFound();
                    throw;
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/DeviceBorrowingRequests/{id}
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
