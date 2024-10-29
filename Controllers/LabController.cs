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
    public class LabsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LabsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetLabs()
        {
            var labs = await _context.Labs.ToListAsync();
            return Ok(labs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lab lab)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Labs.Add(lab);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLabs), new { id = lab.LabId }, lab);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLab(int id)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab == null) return NotFound();
            return Ok(lab);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Lab lab)
        {
            if (id != lab.LabId) return BadRequest("Lab ID mismatch.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                lab.UpdatedAt = DateTime.UtcNow;
                _context.Labs.Update(lab);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabExists(id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lab = await _context.Labs.FindAsync(id);
            if (lab == null) return NotFound();

            _context.Labs.Remove(lab);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool LabExists(int id)
        {
            return _context.Labs.Any(e => e.LabId == id);
        }
    }
}
