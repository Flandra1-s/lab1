using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Data;
using ReservationSystem.API.Models;

namespace ReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypePriceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomTypePriceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomTypePrice>>> GetRoomTypePrices()
        {
            return await _context.RoomTypePrices.Include(r => r.Room).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypePrice>> GetRoomTypePrice(int id)
        {
            var roomTypePrice = await _context.RoomTypePrices.FindAsync(id);
            if (roomTypePrice == null) return NotFound();
            return roomTypePrice;
        }

        [HttpPost]
        public async Task<ActionResult<RoomTypePrice>> PostRoomTypePrice(RoomTypePrice roomTypePrice)
        {
            _context.RoomTypePrices.Add(roomTypePrice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoomTypePrice), new { id = roomTypePrice.PriceId }, roomTypePrice);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomTypePrice(int id, RoomTypePrice roomTypePrice)
        {
            if (id != roomTypePrice.PriceId) return BadRequest();
            _context.Entry(roomTypePrice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomTypePrice(int id)
        {
            var roomTypePrice = await _context.RoomTypePrices.FindAsync(id);
            if (roomTypePrice == null) return NotFound();
            _context.RoomTypePrices.Remove(roomTypePrice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
