using LabManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
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

        // GET: api/LabBorrowingRequests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.LabBorrowingRequests
                .Include(l => l.User)
                .Include(l => l.Lab)
                .ToListAsync();
            return Ok(requests);
        }

        // POST: api/LabBorrowingRequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LabBorrowingRequest labBorrowingRequest)
        {
            if (ModelState.IsValid)
            {
                labBorrowingRequest.CreatedAt = DateTime.Now;
                labBorrowingRequest.UpdatedAt = DateTime.Now;

                _context.Add(labBorrowingRequest);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetRequests), new { id = labBorrowingRequest.RequestId }, labBorrowingRequest);
            }
            return BadRequest(ModelState);
        }

        // GET: api/LabBorrowingRequests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.LabBorrowingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        // PUT: api/LabBorrowingRequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] LabBorrowingRequest labBorrowingRequest)
        {
            if (id != labBorrowingRequest.RequestId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    labBorrowingRequest.UpdatedAt = DateTime.Now;
                    _context.Update(labBorrowingRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabBorrowingRequestExists(labBorrowingRequest.RequestId)) return NotFound();
                    throw;
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/LabBorrowingRequests/{id}
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
