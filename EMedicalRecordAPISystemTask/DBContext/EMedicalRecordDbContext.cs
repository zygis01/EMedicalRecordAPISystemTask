using EMedicalRecordAPISystemTask.Models;
using Microsoft.EntityFrameworkCore;

namespace EMedicalRecordAPISystemTask.DBContext
{
    public class EMedicalRecordDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<HumanInfo> HumanInfos { get; set; }

        private readonly ILogger<EMedicalRecordDbContext> _logger;

        public EMedicalRecordDbContext(DbContextOptions<EMedicalRecordDbContext> options) : base(options)
        {
            
        }
        private EMedicalRecordDbContext(ILogger<EMedicalRecordDbContext> logger)
        {
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

        public async Task<List<User>> ReturnUsersAsync(EMedicalRecordDbContext _context)
        {
            return await _context.Users.ToListAsync();
        }

        public async Task SaveUserAsync(User user, EMedicalRecordDbContext _context)
        {
            if (user != null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("User is null");
            }
        }
        public async Task<User> GetUserByUsernameAsync(string username, EMedicalRecordDbContext _context)
        {
            if (username == null)
            {
                _logger.LogWarning("Username is null");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetUserByPersonalCodeAsync(double personalCode, EMedicalRecordDbContext _context)
        {
            if (personalCode == null)
            {
                _logger.LogWarning("Personal code is null");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.HumanInfo.PersonalCode == personalCode);
        }

        public async Task<User> GetUserByPhoneNumberAsync(double phoneNumber, EMedicalRecordDbContext _context)
        {
            if (phoneNumber == null)
            {
                _logger.LogWarning("Phone number is null");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.HumanInfo.PhoneNum == phoneNumber);
        }

        public async Task<User> GetUserByEmailAsync(string email, EMedicalRecordDbContext _context)
        {
            if (email == null)
            {
                _logger.LogWarning("Email is null");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.HumanInfo.eMail == email);
        }

        public async Task<User> GetUserByIdAsync(int id, EMedicalRecordDbContext _context)
        {
            if (id == null)
            {
                _logger.LogWarning("Id is null");
            }

            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public async Task<string> DeleteUserAsync(User user, EMedicalRecordDbContext _context)
        {
            if (user == null)
            {
                _logger.LogWarning("User is null");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return "User was removed!";
        }
    }
}
