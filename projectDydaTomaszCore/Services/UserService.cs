using projectDydaTomasz.Core.Interfaces;
using projectDydaTomaszCore.Interfaces;
using projectDydaTomaszCore.Models;
using System;
using System.Runtime.CompilerServices;

namespace projectDydaTomaszCore.Services
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
                    Console.WriteLine("Zalogowano poprawnie");
                    Console.ReadLine();
                    return user;
                }
                else
                {
                    Console.WriteLine("Niepoprawne dane!");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("Niepoprawne dane!");
            Console.ReadLine();
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
                    Console.WriteLine("Rejestracja przebiegła pomyślnie!");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Należy wypełnić wszystkie pola!");
                    Console.ReadLine();
                }

            }
            else if(loginExists != null)
            {
                Console.WriteLine("Podany login już istnieje");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Użytkownik nie może być nullem");
                Console.ReadLine();
            }
        }
    }
}
