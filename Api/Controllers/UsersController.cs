using Core.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // POST: /api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            // Busca el usuario por email y verifica si está activo
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.IsActive);

            // Verifica si el usuario existe y si el hash de la contraseña coincide
            if (user == null || !VerifyPasswordHash(loginDto.Password, user.Password))
                return Unauthorized("Invalid credentials.");

            return Ok("Login successful.");
        }

        // POST: /api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            // Hashea la contraseña antes de guardar el usuario
            user.Password = HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            // Devuelve la ubicación del recurso creado
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        // GET: /api/users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            // Obtiene todos los usuarios de la base de datos
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: /api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // Busca un usuario por ID
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: /api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User userUpdate)
        {
            // Busca el usuario que se quiere actualizar
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Actualiza los datos del usuario
            user.Name = userUpdate.Name;
            user.Email = userUpdate.Email;
            user.IsActive = userUpdate.IsActive;

            // Si se proporciona una nueva contraseña, la hashea antes de guardar
            if (!string.IsNullOrEmpty(userUpdate.Password))
                user.Password = HashPassword(userUpdate.Password);

            // Marca el usuario como modificado
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Busca el usuario por ID antes de eliminar
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            // Elimina el usuario de la base de datos
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Método privado para hashear la contraseña
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        // Método privado para verificar el hash de la contraseña
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string hash = HashPassword(password);
            return hash == storedHash;
        }
    }

    // DTO para el inicio de sesión de usuario
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
