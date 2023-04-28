using EMedicalRecordAPISystemTask.DBContext;
using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Options;
using EMedicalRecordAPISystemTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EMedicalRecordAPISystemTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private EMedicalRecordDbContext context;

        public AuthenticationController(IConfiguration configuration, EMedicalRecordDbContext _context)
        {
            context = _context;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            UserRepository userRepo = new(context);

            // Validate request body
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if user exists in database
            var user = await context.Users.SingleOrDefaultAsync(u => u.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            // Verify password hash
            if (!userRepo.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized();
            }

            // Create JWT token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "User")
            };

            var jwtOptions = new JwtOptions(_configuration);
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTime = DateTime.UtcNow.AddHours(jwtOptions.ExpirationHours);

            var tokenOptions = new JwtSecurityToken
                (
                    issuer: jwtOptions.ValidIssuer,
                    audience: jwtOptions.ValidAudience,
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: signingCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }

        [HttpPost("LoginAdmin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Invalid model");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserRepository userRepo = new(context);

            var userFromDb = await context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Role == "Admin");

            if (userFromDb != null && userRepo.VerifyPasswordHash(loginDto.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            {
                var jwtService = new JwtService(_configuration); 

                var tokenString = jwtService.GetJwtToken(userFromDb.Username);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid registration data!");
            }

            UserRepository userRepository = new UserRepository(context);

            // Check if the username already exists in database
            var existingUser = await context.GetUserByUsernameAsync(userDto.Username, context);

            if (existingUser!= null)
            {
                return BadRequest("User with same username already exists!");
            }


            // Check if the personal code already exists in database
            existingUser = await context.GetUserByPersonalCodeAsync(userDto.HumanInfoDto.PersonalCode, context);

            if(existingUser != null)
            {
                return BadRequest("User with same personal code already exists!");
            }

            // Check if the phone number already exists in database
            existingUser = await context.GetUserByPhoneNumberAsync(userDto.HumanInfoDto.PhoneNum, context);

            if (existingUser != null)
            {
                return BadRequest("User with same phone number already exists!");
            }

            // Check if the email already exists in database
            existingUser = await context.GetUserByEmailAsync(userDto.HumanInfoDto.eMail, context);

            if (existingUser != null)
            {
                return BadRequest("User with same email already exists!");
            }

            var createdUser = await userRepository.CreateUserAsync(userDto);

            if (createdUser == null)
            {
                return BadRequest("Could not create user!");
            }

            return Ok("User registered successfully!");
        }
    }
}
