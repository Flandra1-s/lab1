using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.API.Data;
using ReservationSystem.API.Models;
using ReservationSystem.API.Services;

namespace ReservationSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public UserController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(x => x.Email == user.Email))
                return BadRequest(new { message = "Ky email është i regjistruar tashmë." });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            if (string.IsNullOrEmpty(user.Role))
                user.Role = "User";

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Regjistrimi u krye me sukses!" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return NotFound(new { message = "Përdoruesi nuk u gjet." });

            bool isValid = BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash);
            if (!isValid)
                return BadRequest(new { message = "Fjalëkalimi është i gabuar." });

            var token = _authService.GenerateToken(user);
            return Ok(new { message = "Sukses!", token });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Përdoruesi nuk u gjet." });

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
                return BadRequest(new { message = "ID nuk përputhet." });

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Përdoruesi u përditësua me sukses." });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.Id == id))
                    return NotFound(new { message = "Përdoruesi nuk ekziston." });

                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "Përdoruesi nuk ekziston." });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Përdoruesi u fshi me sukses." });
        }
    }
}
