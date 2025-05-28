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
    public class RoomController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.Include(r => r.Hotel).ToListAsync();
        }
 
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.Include(r => r.Hotel)
                                           .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
                return NotFound(new { message = "Dhoma nuk u gjet." });

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(Room room)
        {
            if (room.Price <= 0 || string.IsNullOrWhiteSpace(room.Type))
                return BadRequest(new { message = "Të dhënat e dhomës janë të pavlefshme." });

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dhoma u shtua me sukses!", room });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, Room room)
        {
            if (id != room.RoomId)
                return BadRequest(new { message = "ID nuk përputhet." });

            _context.Entry(room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Dhoma u përditësua me sukses." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rooms.Any(r => r.RoomId == id))
                    return NotFound(new { message = "Dhoma nuk ekziston." });

                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return NotFound(new { message = "Dhoma nuk ekziston." });

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dhoma u fshi me sukses." });
        }
    }
}
