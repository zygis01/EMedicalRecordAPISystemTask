using EMedicalRecordAPISystemTask.BussinessLogic.Entities;
using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMedicalRecordAPISystemTask.DBContext
{
    public class EMedicalRecordDbContext : DbContext, IEMedicalRecordDbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Clinic> Clinics { get; set; } = null!;
        public DbSet<HumanInfo> HumanInfos { get; set; } = null!;

        private readonly ILogger<EMedicalRecordDbContext>? _logger;

        public EMedicalRecordDbContext(DbContextOptions<EMedicalRecordDbContext> options, ILogger<EMedicalRecordDbContext>? logger = null) : base(options)
        {
            Users = Set<User>();
            Clinics = Set<Clinic>();
            HumanInfos = Set<HumanInfo>();
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships between User and HumanInformation models
            modelBuilder.Entity<User>()
                .HasOne(u => u.HumanInfo)
                .WithOne(hi => hi.User)
                .HasForeignKey<HumanInfo>(hi => hi.Id);

            modelBuilder.Entity<HumanInfo>()
                .HasOne(hi => hi.AddressInfo)
                .WithOne(ai => ai.HumanInfo)
                .HasForeignKey<AddressInfo>(ai => ai.Id);
        }

        public async Task<List<User>> ReturnUsersAsync()
        {
            return await Users.ToListAsync();
        }

        public async Task SaveUserAsync(User user)
        {
            if (user != null)
            {
                await Users.AddAsync(user);
                await SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("User is null");
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            if (username == null)
            {
                _logger.LogWarning("Username is null");
            }

            return await Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetUserByPersonalCodeAsync(double personalCode)
        {
            if (personalCode == 0)
            {
                _logger.LogWarning("Personal code is 0");
            }

            return await Users.FirstOrDefaultAsync(x => x.HumanInfo.PersonalCode == personalCode);
        }

        public async Task<User> GetUserByPhoneNumberAsync(double phoneNumber)
        {
            if (phoneNumber == 0)
            {
                _logger.LogWarning("Phone number is 0");
            }

            return await Users.FirstOrDefaultAsync(x => x.HumanInfo.PhoneNum == phoneNumber);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (email == null)
            {
                _logger.LogWarning("Email is null");
            }

            return await Users.FirstOrDefaultAsync(x => x.HumanInfo.eMail == email);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            if(id == 0)
            {
                _logger.LogWarning("Id is 0!");
            }

            return await Users
            .Include(x => x.HumanInfo)
            .ThenInclude(x => x.AddressInfo)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<string> DeleteUserAsync(User user)
        {
            if (user == null)
            {
                _logger.LogWarning("User is null");
                return "User was not removed. User object is null.";
            }

            Users.Remove(user);
            await SaveChangesAsync();
            return "User was removed!";
        }
    }
}
