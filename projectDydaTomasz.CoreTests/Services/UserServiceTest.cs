using Moq;
using projectDydaTomaszCore.Interfaces;
using projectDydaTomaszCore.Services;
using Xunit;
using projectDydaTomaszCore.Models;

namespace projectDydaTomasz.CoreTestes.Services
{

    public class UserServiceTest
    {
        [Fact]
        public void GetAllData_ShouldReturnUsersFromDatabase()
        {
            // Arrange
            var mockDatabaseConnection = new Mock<IDatabaseConnection<User>>();
            var testData = new List<User>();
            mockDatabaseConnection.Setup(x => x.GetData()).Returns(testData);

            var dataService = new UserService(mockDatabaseConnection.Object);

            // Act
            var result = dataService.GetAllData();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testData, result);
        }
    }
}
