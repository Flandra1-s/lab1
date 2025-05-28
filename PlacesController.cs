using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Data;
using ReservationSystem.API.Models;

namespace ReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlacesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
        {
            return Ok(await _context.Places.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Place>> GetPlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null)
                return NotFound(new { message = "Vendi nuk u gjet." });

            return Ok(place);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Place>> PostPlace([FromBody] Place place)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(place.Name))
                return BadRequest(new { message = "Emri i vendit eshte i detyrueshem." });

            _context.Places.Add(place);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlace), new { id = place.Id }, place);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPlace(int id, [FromBody] Place place)
        {
            if (id != place.Id)
                return BadRequest(new { message = "ID nuk perputhet." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Entry(place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Vendi u perditesua me sukses." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Places.AnyAsync(p => p.Id == id))
                    return NotFound(new { message = "Vendi nuk ekziston." });

                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null)
                return NotFound(new { message = "Vendi nuk ekziston." });

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Vendi u fshi me sukses." });
        }
    }
}
