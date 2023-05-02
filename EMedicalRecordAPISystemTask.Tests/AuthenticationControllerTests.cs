using EMedicalRecordAPISystemTask.Controllers;
using EMedicalRecordAPISystemTask.DBContext;
using EMedicalRecordAPISystemTask.DTOs;
using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace EMedicalRecordAPISystemTask.Tests
{
    public class AuthenticationControllerTests
    {
        private readonly IConfiguration _configuration;
        private readonly Mock<IEMedicalRecordDbContext> _contextMock;
        private readonly AuthenticationController _authenticationController;
        private readonly Mock<UserRepository> _userRepositoryMock;

        public AuthenticationControllerTests()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _contextMock = new Mock<IEMedicalRecordDbContext>();
            _authenticationController = new AuthenticationController(_configuration, _contextMock.Object);
            _userRepositoryMock = new Mock<UserRepository>(_contextMock.Object);
        }

        [Fact]
        public async Task RegisterNewUser_ValidData_ReturnsOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<EMedicalRecordDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;
            var dbContext = new EMedicalRecordDbContext(options);
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            var filePath = "C:\\Users\\Zygimantas\\source\\repos\\EMedicalRecordAPISystemTask\\EMedicalRecordAPISystemTask.Tests\\test.png";
            var stream = new MemoryStream(File.ReadAllBytes(filePath));
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(filePath));

            var username = "testuser";
            var password = "password";
            var humanInfoDto = new HumanInfoDto
            {
                FirstName = "Test",
                LastName = "LTest",
                PersonalCode = 41321321,
                eMail = "testlt@example.com",
                PhoneNum = 41231232,
                ProfilePic = file
            };
            var addressInfoDto = new AddressInfoDto
            {
                Street = "123 Main St",
                City = "Anytown",
                HouseNum = 41,
                AppartmentNum = 11
            };

            var controller = new AuthenticationController(_configuration, dbContext);
            var userDto = new UserDto
            {
                Username = username,
                Password = password,
                HumanInfoDto = humanInfoDto,
                AddressInfoDto = addressInfoDto
            };

            // Act
            var result = await controller.RegisterNewUser(userDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully!", ((OkObjectResult)result).Value);

            var createdUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            Assert.NotNull(createdUser);
            Assert.Equal(username, createdUser.Username);
            Assert.NotNull(createdUser.PasswordHash);
            Assert.NotNull(createdUser.PasswordSalt);
            Assert.NotEmpty(createdUser.PasswordHash);
            Assert.NotEmpty(createdUser.PasswordSalt);
            Assert.Equal(humanInfoDto.FirstName, createdUser.HumanInfo.FirstName);
            Assert.Equal(humanInfoDto.LastName, createdUser.HumanInfo.LastName);
            Assert.Equal(humanInfoDto.PersonalCode, createdUser.HumanInfo.PersonalCode);
            Assert.Equal(humanInfoDto.eMail, createdUser.HumanInfo.eMail);
            Assert.Equal(humanInfoDto.PhoneNum, createdUser.HumanInfo.PhoneNum);
            Assert.Equal(addressInfoDto.City, createdUser.HumanInfo.AddressInfo.City);
            Assert.Equal(addressInfoDto.Street, createdUser.HumanInfo.AddressInfo.Street);
            Assert.Equal(addressInfoDto.HouseNum, createdUser.HumanInfo.AddressInfo.HouseNum);
            Assert.Equal(addressInfoDto.AppartmentNum, createdUser.HumanInfo.AddressInfo.AppartmentNum);
        }

        [Fact]
        public async Task Login_WithIncorrectCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "testpassword"
            };
            _userRepositoryMock.Setup(x => x.LoginAsync(loginDto.Username, loginDto.Password))
                               .ReturnsAsync(false);

            // Act
            var result = await _authenticationController.Login(loginDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Bad username or password!", badRequestResult.Value);
        }

    }
}
