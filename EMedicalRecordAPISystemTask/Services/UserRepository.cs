using EMedicalRecordAPISystemTask.DBContext;
using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Models;
using System.Security.Cryptography;

namespace EMedicalRecordAPISystemTask.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly EMedicalRecordDbContext _context;

        public UserRepository(EMedicalRecordDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                throw new Exception("Password is required");

            if (_context.Users.Any(u => u.Username == userDto.Username))
                throw new Exception("Username \"" + userDto.Username + "\" is already taken");

            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var addressInfo = new AddressInfo
            {
                City = userDto.AddressInfoDto.City,
                Street = userDto.AddressInfoDto.Street,
                HouseNum = userDto.AddressInfoDto.HouseNum,
                AppartmentNum = userDto.AddressInfoDto.AppartmentNum
            };

            var humanInfo = new HumanInfo
            {
                FirstName = userDto.HumanInfoDto.FirstName,
                LastName = userDto.HumanInfoDto.LastName,
                PersonalCode = userDto.HumanInfoDto.PersonalCode,
                PhoneNum = userDto.HumanInfoDto.PhoneNum,
                eMail = userDto.HumanInfoDto.eMail,
                AddressInfo = addressInfo
            };

            var user = new User
            {
                HumanInfo = humanInfo,
                Username = userDto.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
