using EMedicalRecordAPISystemTask.BussinessLogic.Entities;
using EMedicalRecordAPISystemTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EMedicalRecordAPISystemTask.Interfaces
{
    public interface IEMedicalRecordDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Clinic> Clinics { get; set; }
        DbSet<HumanInfo> HumanInfos { get; set; }

        public Task<User> GetUserByIdAsync(int id);
        public Task<User> GetUserByUsernameAsync(string username);
        public Task<User> GetUserByPersonalCodeAsync(double personalCode);
        public Task<User> GetUserByPhoneNumberAsync(double phoneNumber);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<List<User>> ReturnUsersAsync();
        public Task SaveUserAsync(User user);
        public Task<string> DeleteUserAsync(User user);
    }
}
