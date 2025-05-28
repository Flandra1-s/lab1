using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Data;
using ReservationSystem.API.Models;
using System.Security.Claims;

namespace ReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            var reservations = await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .ThenInclude(room => room.Hotel)
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetMyReservations()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Room)
                .ThenInclude(room => room.Hotel)
                .ToListAsync();

            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reservation.DateFrom >= reservation.DateTo)
                return BadRequest(new { message = "Data e perfundimit duhet te jete pas dates se fillimit." });

            reservation.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rezervimi u krijua me sukses.", reservation });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation updated)
        {
            if (id != updated.ReservationId)
                return BadRequest(new { message = "ID e dhene nuk perputhet me rezervimin." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound(new { message = "Rezervimi nuk ekziston." });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (reservation.UserId != userId)
                return Forbid("Nuk mund te perditesoni rezervimet e te tjereve.");

            reservation.DateFrom = updated.DateFrom;
            reservation.DateTo = updated.DateTo;
            reservation.RoomId = updated.RoomId;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Rezervimi u perditesua me sukses." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                return NotFound(new { message = "Rezervimi nuk ekziston." });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && reservation.UserId != userId)
                return Forbid("Nuk mund te fshini rezervimet e te tjereve.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rezervimi u fshi me sukses." });
        }
    }
}
