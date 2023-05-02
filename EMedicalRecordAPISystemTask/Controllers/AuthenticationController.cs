using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMedicalRecordAPISystemTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;
        public IEMedicalRecordDbContext context;
        public JwtService jwtService;
        public UserRepository service;
        public ProfilePicService pictureService;

        public AuthenticationController(IConfiguration _configuration, IEMedicalRecordDbContext _context)
        {
            context = _context;
            configuration = _configuration;
            jwtService = new JwtService(_configuration);
            service = new UserRepository(context);
            pictureService = new ProfilePicService();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
        {
            if (!await service.LoginAsync(loginDto.Username, loginDto.Password))
                return BadRequest($"Bad username or password!");

            string token = jwtService.GetJwtTokenUser(loginDto.Username);
            return Ok(token);
        }

        [HttpPost("LoginAdmin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest("Wrong credentials!");
            }

            var userFromDb = await context.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Role == "Admin");

            if (userFromDb != null && await service.LoginAsync(loginDto.Username, loginDto.Password))
            {
                var tokenString = jwtService.GetJwtTokenAdmin(userFromDb.Username, userFromDb.Role);

                return Ok(new { Token = tokenString });
            }

            return Unauthorized();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest("Invalid registration data!");

            var existingUser = await context.GetUserByUsernameAsync(userDto.Username);
            if (existingUser != null)
                return BadRequest("User with same username already exists!");

            existingUser = await context.GetUserByPersonalCodeAsync(userDto.HumanInfoDto.PersonalCode);
            if (existingUser != null)
                return BadRequest("User with same personal code already exists!");

            existingUser = await context.GetUserByPhoneNumberAsync(userDto.HumanInfoDto.PhoneNum);
            if (existingUser != null)
                return BadRequest("User with same phone number already exists!");

            existingUser = await context.GetUserByEmailAsync(userDto.HumanInfoDto.eMail);
            if (existingUser != null)
                return BadRequest("User with same email already exists!");

            if (userDto.HumanInfoDto.ProfilePic != null)
            {
                byte[] profilePicBytes = await pictureService.ProcessProfilePicAsync(userDto.HumanInfoDto.ProfilePic);

                var createdUser = await service.CreateUserAsync(userDto.Username, userDto.Password, userDto.HumanInfoDto, userDto.AddressInfoDto, profilePicBytes);
                if (createdUser == null)
                {
                    return BadRequest("Could not create user!");
                }
            }

            return Ok("User registered successfully!");
        }
    }
}
