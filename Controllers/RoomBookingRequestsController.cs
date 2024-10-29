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
    public class RoomBookingRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomBookingRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.RoomBookingRequests
                .Include(u => u.User)
                .Include(l => l.Lab)
                .ToListAsync();
            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomBookingRequest roomBookingRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            roomBookingRequest.CreatedAt = DateTime.UtcNow;
            roomBookingRequest.UpdatedAt = DateTime.UtcNow;

            _context.RoomBookingRequests.Add(roomBookingRequest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRequests), new { id = roomBookingRequest.BookingId }, roomBookingRequest);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.RoomBookingRequests.FindAsync(id);
            if (request == null) return NotFound();
            return Ok(request);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RoomBookingRequest roomBookingRequest)
        {
            if (id != roomBookingRequest.BookingId) return BadRequest("Booking ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                roomBookingRequest.UpdatedAt = DateTime.UtcNow;
                _context.RoomBookingRequests.Update(roomBookingRequest);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomBookingRequestExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var roomBookingRequest = await _context.RoomBookingRequests.FindAsync(id);
            if (roomBookingRequest == null) return NotFound();

            _context.RoomBookingRequests.Remove(roomBookingRequest);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool RoomBookingRequestExists(int id)
        {
            return _context.RoomBookingRequests.Any(e => e.BookingId == id);
        }
    }
}
