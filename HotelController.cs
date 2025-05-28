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
    public class HotelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            return await _context.Hotels.Include(h => h.Rooms).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _context.Hotels.Include(h => h.Rooms)
                                             .FirstOrDefaultAsync(h => h.HotelId == id);

            if (hotel == null)
                return NotFound(new { message = "Hoteli nuk u gjet." });

            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(Hotel hotel)
        {
            if (string.IsNullOrWhiteSpace(hotel.Name) || string.IsNullOrWhiteSpace(hotel.Address))
                return BadRequest(new { message = "Emri dhe adresa janë të detyrueshme." });

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Hoteli u shtua me sukses!", hotel });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
        {
            if (id != hotel.HotelId)
                return BadRequest(new { message = "ID nuk përputhet." });

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Hoteli u përditësua me sukses." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Hotels.Any(h => h.HotelId == id))
                    return NotFound(new { message = "Hoteli nuk ekziston." });

                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
                return NotFound(new { message = "Hoteli nuk ekziston." });

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Hoteli u fshi me sukses." });
        }
    }
}
