using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System.Threading.Tasks;

namespace LabManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomBookingRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomBookingRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RoomBookingRequests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            return Ok(await _context.RoomBookingRequests.ToListAsync());
        }

        // POST: api/RoomBookingRequests
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomBookingRequest request)
        {
            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetRequests), new { id = request.BookingId }, request);
            }
            return BadRequest(ModelState);
        }

        // GET: api/RoomBookingRequests/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.RoomBookingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        // PUT: api/RoomBookingRequests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RoomBookingRequest request)
        {
            if (id != request.BookingId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomBookingRequestExists(request.BookingId)) return NotFound();
                    throw;
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/RoomBookingRequests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.RoomBookingRequests.FindAsync(id);
            if (request == null) return NotFound();

            _context.RoomBookingRequests.Remove(request);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RoomBookingRequestExists(int id)
        {
            return _context.RoomBookingRequests.Any(e => e.BookingId == id);
        }
    }
}
