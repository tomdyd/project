using projectDydaTomaszCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace projectDydaTomasz.CoreTests.Models
{
    public class TestTest
    {
        [Fact]
        public void TestToString_RetunCorrectData()
        {
            var test = new Test
            {
                Id = "test",
                MyNum = 1,
                Name = "test",
            };

            var str = test.ToString();

            Assert.Equal("ID: test\nMyNum: 1\nName: test", str);
        }
    }
}
