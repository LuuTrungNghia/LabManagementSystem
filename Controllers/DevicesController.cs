using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
using System.Threading.Tasks;

namespace LabManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DevicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Devices
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            return Ok(await _context.Devices.ToListAsync());
        }

        // POST: api/Devices
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Device device)
        {
            if (ModelState.IsValid)
            {
                device.CreatedAt = DateTime.Now;
                device.UpdatedAt = DateTime.Now;

                _context.Add(device);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDevices), new { id = device.DeviceId }, device);
            }
            return BadRequest(ModelState);
        }

        // GET: api/Devices/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();
            return Ok(device);
        }

        // PUT: api/Devices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Device device)
        {
            if (id != device.DeviceId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    device.UpdatedAt = DateTime.Now;
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.DeviceId)) return NotFound();
                    throw;
                }
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Devices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var device = await _context.Devices.FindAsync(id);
            if (device == null) return NotFound();

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool DeviceExists(int id)
        {
            return _context.Devices.Any(e => e.DeviceId == id);
        }
    }
}
