using Microsoft.EntityFrameworkCore;
using To_Do_API.Exceptions;
using To_Do_API.Helpers;
using To_Do_API.Models;
using To_Do_API.Models.ToDoDTO.User.Auth;

namespace To_Do_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ToDoContext _context;
        private readonly JwtHelper _jwtHelper;

        public AuthService(ILogger<AuthService> logger, ToDoContext context, JwtHelper jwtHelper)
        {
            _logger = logger;
            _context = context;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
        {
            _logger.LogInformation("Yeni kullanıcı kaydı başlatıldı. Username {username}", registerRequest.Username);

            User existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == registerRequest.Username || u.Email == registerRequest.Email);

            if (existingUser != null)
            {
                _logger.LogWarning("Kullanıcı adı veya email zaten mevcut. " );
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

            _logger.LogInformation("Kullanıcı başarıyla oluşturuldu. UserId : {userId}", newUser.Id);

            // JWT token üret (daha sonra eklenecek)

            string token = _jwtHelper.GenerateToken(newUser); 

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

            User loginUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (loginUser == null)
            {
                _logger.LogWarning("Kullanıcı adı veya şifre hatalı ");
                throw new UnauthorizeException("Kullanıcı adı veya şifre hatalı");
            }

            // Şifre kontrolü
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, loginUser.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Kullanıcı adı veya şifre hatalı ");
                throw new UnauthorizeException("Kullanıcı adı veya şifre hatalı ");
            }

            _logger.LogInformation("Tebrikler başarıyla giriş yaptınız. UserId : {userId}", loginUser.Id);

            // JWT Token Uret
            string token = _jwtHelper.GenerateToken(loginUser);


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
