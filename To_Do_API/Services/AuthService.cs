using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using To_Do_API.Exceptions;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO.User.Auth;

namespace To_Do_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ToDoContext _context;

        public AuthService(ILogger<AuthService> logger, ToDoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            _logger.LogInformation("Yeni kullanıcı kaydı başlatıldı. Username {username}", registerRequest.Username);

            User existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerRequest.Username || u.Email == registerRequest.Email);

            if (existingUser != null)
            {
                _logger.LogWarning("Kullanıcı adı veya email zaten mevcut. Username : {Username}", registerRequest.Username);
                throw new BadRequestException("Username veya Email zaten kullanılıyor.");
            }

            // Şifre Hashleme 
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            User newUser = new User
            {
                Username = registerRequest.Username,
                Email = registerRequest.Email,
                PasswordHash = passwordHash,
                Address = registerRequest.Address,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Kullanıcı başarıyla oluşturuldu. UserId : {}", newUser.Id);

            // JWT token üret (daha sonra eklenecek)

            string token = "temproray-token"; // Şimdilik placeholder 

            return new AuthResponseDto
            {
                UserId = newUser.Id,
                Token = token,
                Username = newUser.Username,
                Email = newUser.Email
            };

        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest)
        {
            _logger.LogInformation("Login denemesi yapılıyor. Username : {Username}", loginRequest.Username);

            User loginUser = await _context.Users.FindAsync(loginRequest.Username);

            if (loginUser == null)
            {
                _logger.LogWarning("Böyle bir kullanıcı bulunmamaktadır. Username : {username}", loginRequest.Username);
                throw new UnauthorizeException($"Kullanıcı bulunmamaktadır. Username {loginRequest.Username}");
            }

            // Şifre kontrolü
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, loginUser.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Girdiğiniz şifre hatalıdır.Lütfen tekrar deneyiniz. Username : {username}", loginRequest.Username);
                throw new UnauthorizeException($"Girdiğiniz şifre hatalıdır. {loginRequest.Username}");
            }

            _logger.LogInformation("Tebrikler başarıyla giriş yaptınız. UserId : {userId}", loginUser.Id);

            // JWT Token Uret
            string token = "temproray-token";


            return new AuthResponseDto
            {
                UserId = loginUser.Id,
                Username = loginUser.Username,
                Email = loginUser.Email,
                Token = token
            };

        }

        
    }
}
