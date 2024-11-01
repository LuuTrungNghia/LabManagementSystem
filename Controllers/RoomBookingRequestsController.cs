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
        
        [HttpPost("/request-room-booking")]
        public async Task<IActionResult> RequestRoomBooking(RequestRoomBookingDto model)
        {
            try
            {
                var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.UserId) 
                                ?? throw new Exception("User does not exist.");
                
                if (checkUser.Status == 1)
                {
                    return BadRequest("User already has an active room booking request.");
                }

                var lab = await _context.Labs.FirstOrDefaultAsync(l => l.LabId == model.LabId)
                          ?? throw new Exception("Lab does not exist.");

                if (lab.Status == 2)
                {
                    return BadRequest("The selected lab is not available.");
                }

                var roomBookingRequest = new RoomBookingRequest
                {
                    UserId = model.UserId,
                    LabId = model.LabId,
                    RoomName = model.RoomName,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.RoomBookingRequests.Add(roomBookingRequest);
                await _context.SaveChangesAsync();

                return Ok("Room booking request created successfully.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("/ar-request-room-booking")]
        public async Task<IActionResult> ArRequestRoomBooking([FromBody] ArRequestRoomBookingDto model)
        {
            try
            {
                foreach (var id in model.RoomBookingIds)
                {
                    var request = await _context.RoomBookingRequests.FindAsync(id);
                    if (request != null)
                    {
                        request.Status = model.Status;
                        _context.RoomBookingRequests.Update(request);
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


        private bool RoomBookingRequestExists(int id)
        {
            return _context.RoomBookingRequests.Any(e => e.BookingId == id);
        }
    }
}
