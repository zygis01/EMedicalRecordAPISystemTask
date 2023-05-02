using EMedicalRecordAPISystemTask.BussinessLogic.Entities;
using EMedicalRecordAPISystemTask.BussinessLogic.Interfaces;
using EMedicalRecordAPISystemTask.DBContext;
using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EMedicalRecordAPISystemTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EMedicalRecordDbContext context;
        private IUserRepository userService;
        private ProfilePicService profilePicService;

        public UserController(EMedicalRecordDbContext _context, IUserRepository _userService, ProfilePicService _profilePicService)
        {
            context = _context;
            userService = _userService;
            profilePicService = _profilePicService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/PersonalCode")]
        public async Task<IActionResult> UpdatePersonalCode([FromBody] PersonalCodeDto personalCodeDto)
        {
            // Get the authenticated user's username
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if(personalCodeDto.PersonalCode == 0)
            {
                return BadRequest("Personal code data is missing");
            }

            user.HumanInfo.PersonalCode = personalCodeDto.PersonalCode;
            await context.SaveChangesAsync();

            return Ok(new { message = "Personal code was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/FirstName")]
        public async Task<IActionResult> UpdateFirstName([FromBody] FirstNameDto firstNameDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if (firstNameDto == null || firstNameDto.FirstName == string.Empty)
            {
                return BadRequest("Firstname data is missing");
            }

            user.HumanInfo.FirstName = firstNameDto.FirstName;
            await context.SaveChangesAsync();

            return Ok(new { message = "Firstname was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/LastName")]
        public async Task<IActionResult> UpdateLastName([FromBody] LastNameDto lastNameDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if (lastNameDto == null || lastNameDto.LastName == string.Empty)
            {
                return BadRequest("Lastname data is missing");
            }

            user.HumanInfo.LastName = lastNameDto.LastName;
            await context.SaveChangesAsync();

            return Ok(new { message = "Lastname was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/PhoneNumber")]
        public async Task<IActionResult> UpdatePhoneNum([FromBody] PhoneNumDto phoneNumDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if(phoneNumDto.PhoneNum == 0)
            {
                return BadRequest("Phone number is missing");
            }

            user.HumanInfo.PhoneNum = phoneNumDto.PhoneNum;
            await context.SaveChangesAsync();

            return Ok(new { message = "Phone number was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/Email")]
        public async Task<IActionResult> UpdateEmail([FromBody] EmailDto emailDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if(emailDto == null || emailDto.eMail == string.Empty)
            {
                return BadRequest("Email data is missing");
            }

            user.HumanInfo.eMail = emailDto.eMail;
            await context.SaveChangesAsync();

            return Ok(new { message = "Email was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/ProfilePicture")]
        public async Task<IActionResult> UpdateProfilePic([FromForm] ProfilePicDto profilePicDto)
        {
            if (profilePicDto == null)
            {
                return BadRequest("Profile picture is required");
            }

            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            var proccessedProfilePic = await profilePicService.ProcessProfilePicAsync(profilePicDto.ProfilePic);
            user.HumanInfo.ProfilePic = proccessedProfilePic;
            await context.SaveChangesAsync();

            return Ok(new { message = "Profile picture was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/Address/City")]
        public async Task<IActionResult> UpdateCity([FromBody] CityDto cityDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).ThenInclude(u => u.AddressInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if (cityDto == null || cityDto.City == string.Empty)
            {
                return BadRequest("City data is missing.");
            }

            user.HumanInfo.AddressInfo.City = cityDto.City;
            await context.SaveChangesAsync();

            return Ok(new { message = "City was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/Address/Street")]
        public async Task<IActionResult> UpdateStreet([FromBody] StreetDto streetDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).ThenInclude(u => u.AddressInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if (streetDto == null || streetDto.Street == string.Empty)
            {
                return BadRequest("Street data is missing.");
            }

            user.HumanInfo.AddressInfo.Street = streetDto.Street;
            await context.SaveChangesAsync();

            return Ok(new { message = "Street was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/Address/HouseNum")]
        public async Task<IActionResult> UpdateHouseNum([FromBody] HouseNumDto houseNumDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).ThenInclude(u => u.AddressInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            if(houseNumDto.HouseNum == 0)
            {
                return BadRequest("House number data is missing");
            }

            user.HumanInfo.AddressInfo.HouseNum = houseNumDto.HouseNum;
            await context.SaveChangesAsync();

            return Ok(new { message = "House number was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("Update/Address/AppartmentNum")]
        public async Task<IActionResult> UpdateAppartmentNum([FromBody] AppartmentNumDto appartmentNumDto)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (username == null)
            {
                return Unauthorized();
            }

            var user = await context.Users.Include(u => u.HumanInfo).ThenInclude(u => u.AddressInfo).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound();
            }

            var userRole = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole != "User")
            {
                return Forbid();
            }

            user.HumanInfo.AddressInfo.AppartmentNum = appartmentNumDto.AppartmentNum;
            await context.SaveChangesAsync();

            return Ok(new { message = "Appartment number was updated successfully" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("DeleteBy{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            User userToDelete = await context.GetUserByIdAsync(id);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await context.DeleteUserAsync(userToDelete);
            return Ok("User deleted successfully!");

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("By{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Id is 0!");
            }

            var user = await context.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userProfilePic = userService.GetUserProfilePic(user);

            return Ok(new
            {
                user.Id,
                user.Username,
                HumanInfo = userService.GetHumanInfo(user, userProfilePic),
                AddressInfo = userService.GetAddressInfo(user)
            });
        }
    }
}
