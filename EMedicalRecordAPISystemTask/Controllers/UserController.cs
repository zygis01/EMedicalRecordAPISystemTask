using EMedicalRecordAPISystemTask.DBContext;
using EMedicalRecordAPISystemTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EMedicalRecordAPISystemTask.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private EMedicalRecordDbContext context;

        public UserController(EMedicalRecordDbContext _context)
        {
            context = _context;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            User userToDelete = await context.GetUserByIdAsync(id, context);

            if (userToDelete == null)
            {
                return NotFound();
            }

            await context.DeleteUserAsync(userToDelete, context);
            return Ok("User deleted successfully!");
        }
    }
}
