using MariuszScislyZaliczenie2.Interfaces;
using MariuszScislyZaliczenie2.Models;
using System;
using System.Runtime.CompilerServices;

namespace MariuszScislyZaliczenie2.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabaseConnection<User> _userRepository;

        public UserService(IDatabaseConnection<User> mongoUserRepository)
        {
            _userRepository = mongoUserRepository;
        }

        public User AuthorizeUser(string username, string password)
        {
            var user = _userRepository.GetFilteredData("username", username);

            if (user != null)
            {
                if (user.username == username && user.passwordHash == password)
                {
                    Console.WriteLine("Zalogowano użytkownika");
                    Console.ReadLine();
                    return user;
                }
                else
                {
                    Console.WriteLine("ZŁe dane. Spóbuj ponownie");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Spróbuj ponownie");
                Console.ReadLine();
            }
            return null;
        }

        public void RegisterUser(User newUser)
        {
            var userCollection = _userRepository.GetAllDataList();
            var loginExists = userCollection.Find(x => x.username == newUser.username);

            if (newUser != null && loginExists == null)
            {
                if(newUser.username != "" && newUser.passwordHash != "" && newUser.email != "")
                {
                    var registerUser = new User()
                    {
                        username = newUser.username,
                        passwordHash = newUser.passwordHash,
                        email = newUser.email
                    };

                    _userRepository.AddToDb(registerUser);
                    Console.WriteLine("Zarejestrowano nowego użytkownika");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Podaj wszystkie dane");
                    Console.ReadLine();
                }

            }
            else if(loginExists != null)
            {
                Console.WriteLine("ten login jest już zarejetrowany");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Nazwa użytkownika nie może być pusta");
                Console.ReadLine();
            }
        }
    }
}
