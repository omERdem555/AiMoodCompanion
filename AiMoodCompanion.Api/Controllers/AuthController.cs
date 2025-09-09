using Microsoft.AspNetCore.Mvc;
using AiMoodCompanion.Api.Data;
using AiMoodCompanion.Api.DTOs;
using AiMoodCompanion.Api.Models;
using AiMoodCompanion.Api.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace AiMoodCompanion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(UserRegistrationDto request)
        {
            // Check if user already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User with this email already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = passwordHash,
                Age = request.Age,
                ProfilePicture = request.ProfilePicture,
                Gender = request.Gender,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    ProfilePicture = user.ProfilePicture,
                    Gender = user.Gender,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(UserLoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Invalid email or password");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Age = user.Age,
                    ProfilePicture = user.ProfilePicture,
                    Gender = user.Gender,
                    CreatedAt = user.CreatedAt
                }
            };
        }
    }
}
