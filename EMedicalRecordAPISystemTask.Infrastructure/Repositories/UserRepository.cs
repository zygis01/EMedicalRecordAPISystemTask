using EMedicalRecordAPISystemTask.BussinessLogic.Entities;
using EMedicalRecordAPISystemTask.BussinessLogic.Interfaces;
using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Interfaces;
using System.Security.Cryptography;

namespace EMedicalRecordAPISystemTask.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IEMedicalRecordDbContext _context;

        public UserRepository(IEMedicalRecordDbContext context)
        {
            _context = context;
        }

        private bool CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new Exception("Password is null");
            }

            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }

        public async Task<User> CreateUserAsync(string username, string password, HumanInfoDto humanInfoDto, AddressInfoDto addressInfoDto, byte[] profilePicBytes)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var humanInfo = CreateHumanInfo(humanInfoDto, addressInfoDto, profilePicBytes);
            var user = CreateUser(username, passwordHash, passwordSalt, humanInfo);
            await _context.SaveUserAsync(user);
            return user;
        }

        private HumanInfo CreateHumanInfo(HumanInfoDto humanInfoDto, AddressInfoDto addressInfoDto, byte[] profilePicBytes)
        {
            return new HumanInfo
            {
                FirstName = humanInfoDto.FirstName,
                LastName = humanInfoDto.LastName,
                PersonalCode = humanInfoDto.PersonalCode,
                PhoneNum = humanInfoDto.PhoneNum,
                eMail = humanInfoDto.eMail,
                ProfilePic = profilePicBytes,
                AddressInfo = new AddressInfo
                {
                    City = addressInfoDto.City,
                    Street = addressInfoDto.Street,
                    HouseNum = addressInfoDto.HouseNum,
                    AppartmentNum = addressInfoDto.AppartmentNum
                }
            };
        }

        private User CreateUser(string username, byte[] passwordHash, byte[] passwordSalt, HumanInfo humanInfo)
        {
            return new User
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "User",
                HumanInfo = humanInfo
            };
        }

        public virtual async Task<bool> LoginAsync(string username, string password)
        {
            var user = await _context.GetUserByUsernameAsync(username);
            if (user != null && VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return true;
            }
            return false;
        }

        public byte[] GetUserProfilePic(User user)
        {
            return user.HumanInfo.ProfilePic;
        }

        public object GetHumanInfo(User user, byte[] userProfilePic)
        {
            return new
            {
                user.HumanInfo.FirstName,
                user.HumanInfo.LastName,
                user.HumanInfo.PersonalCode,
                user.HumanInfo.PhoneNum,
                user.HumanInfo.eMail,
                ProfilePic = userProfilePic
            };
        }

        public object GetAddressInfo(User user)
        {
            return new
            {
                user.HumanInfo.AddressInfo.City,
                user.HumanInfo.AddressInfo.Street,
                user.HumanInfo.AddressInfo.HouseNum,
                user.HumanInfo.AddressInfo.AppartmentNum
            };
        }
    }
}
