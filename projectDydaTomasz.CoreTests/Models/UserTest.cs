using projectDydaTomaszCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace projectDydaTomasz.CoreTests.Models
{
    public class UserTest
    {
        [Fact]

        public void UserToString_ReturnCorrectData()
        {
            var user = new User
            {
                UserId = "test",
                Username = "test",
                PasswordHash = "test",
                Email = "test"
            };

            var str = user.ToString();
            Assert.Equal("UserID: test\nUsername: test\nPassword: test\nEmail: test", str);
        }
    }
}
