using EMedicalRecordAPISystemTask.Interfaces;
using EMedicalRecordAPISystemTask.Services;
using Moq;

namespace EMedicalRecordAPISystemTask.Tests
{
    public class UserRepositoryTests
    {
        private readonly Mock<IEMedicalRecordDbContext> _dbContextMock;
        private readonly Mock<UserRepository> _userRepositoryMock;


        public UserRepositoryTests()
        {
            _dbContextMock = new Mock<IEMedicalRecordDbContext>();
            _userRepositoryMock = new Mock<UserRepository>(_dbContextMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFalseWhenUserNotFound()
        {
            // Arrange
            var username = "testusername";
            var password = "testpassword";

            _dbContextMock.Setup(x => x.GetUserByUsernameAsync(username));

            // Act
            var result = await _userRepositoryMock.Object.LoginAsync(username, password);

            // Assert
            Assert.False(result);
        }

    }
}
