using EMedicalRecordAPISystemTask.BussinessLogic.Entities;
using EMedicalRecordAPISystemTask.DTOs;

namespace EMedicalRecordAPISystemTask.BussinessLogic.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> CreateUserAsync(string username, string password, HumanInfoDto humanInfoDto, AddressInfoDto addressInfoDto, byte[] profilePicBytes);
        public Task<bool> LoginAsync(string username, string password);
        public byte[] GetUserProfilePic(User user);
        public object GetHumanInfo(User user, byte[] userProfilePic);
        public object GetAddressInfo(User user);
    }
}
