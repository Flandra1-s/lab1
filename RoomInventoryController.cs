using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Data;
using ReservationSystem.API.Models;

namespace ReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomInventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoomInventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomInventory>>> GetRoomInventories()
        {
            return await _context.RoomInventories.Include(r => r.Room).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomInventory>> GetRoomInventory(int id)
        {
            var roomInventory = await _context.RoomInventories.FindAsync(id);
            if (roomInventory == null) return NotFound();
            return roomInventory;
        }

        [HttpPost]
        public async Task<ActionResult<RoomInventory>> PostRoomInventory(RoomInventory roomInventory)
        {
            _context.RoomInventories.Add(roomInventory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRoomInventory), new { id = roomInventory.RoomInventoryId }, roomInventory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomInventory(int id, RoomInventory roomInventory)
        {
            if (id != roomInventory.RoomInventoryId) return BadRequest();
            _context.Entry(roomInventory).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomInventory(int id)
        {
            var roomInventory = await _context.RoomInventories.FindAsync(id);
            if (roomInventory == null) return NotFound();
            _context.RoomInventories.Remove(roomInventory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
