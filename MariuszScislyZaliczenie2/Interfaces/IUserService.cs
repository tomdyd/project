using MariuszScislyZaliczenie2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariuszScislyZaliczenie2.Interfaces
{
    public interface IUserService
    {
        public User AuthorizeUser(string username, string password);
        public void RegisterUser(User newUser);
    }
}
