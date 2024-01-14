using projectDydaTomaszCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectDydaTomasz.Core.Interfaces
{
    public interface IUserService
    {
        public User AuthorizeUser(string username, string password);
        public void RegisterUser(User newUser);
    }
}
