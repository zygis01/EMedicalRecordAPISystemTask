using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Models;

namespace EMedicalRecordAPISystemTask.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(UserDto userDto);
    }
}
