using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabManagementSystem.Data;
using LabManagementSystem.Models;
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
            return Ok(await _context.Labs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lab lab)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lab);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetLabs), new { id = lab.LabId }, lab);
            }
            return BadRequest(ModelState);
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
            if (id != lab.LabId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lab);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabExists(lab.LabId)) return NotFound();
                    throw;
                }
                return NoContent();
            }
            return BadRequest(ModelState);
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
