using projectDydaTomasz.Core;
using projectDydaTomasz.Core.Interfaces;
using projectDydaTomasz.Core.Models;
using projectDydaTomasz.Core.Services;
using projectDydaTomasz.Interfaces;
using projectDydaTomaszCore.Interfaces;
using projectDydaTomaszCore.Models;
using projectDydaTomaszCore.Services;

namespace projectDydaTomasz
{
    class Program
    {
        static void Main(string[] args)
        {
            IMenu menu = new Menu();
            IAppConsole console = new AppConsole();
            IDatabaseConnectionExtended<User> userMongoClient = new MongoDbDatabaseConnection<User>();
            IDatabaseConnectionExtended<Car> carMongoClient = new MongoDbDatabaseConnection<Car>();
            IDatabaseConnectionExtended<Apartment> apartmentMongoClient = new MongoDbDatabaseConnection<Apartment>();
            IDatabaseConnection<Car> carSqlClient = new SqlitedatabaseConnection<Car>();
            IDatabaseConnection<User> userSqlClient = new SqlitedatabaseConnection<User>();
            IDatabaseConnection<Apartment> apartmentSqlClient = new SqlitedatabaseConnection<Apartment>();
            IUserService userMongoService = new UserService(userMongoClient);
            ICarService carMongoService = new CarService(carMongoClient);
            IApartmentService apartmentMongoService = new ApartmentService(apartmentMongoClient);
            IUserService userSqlService = new UserService(userSqlClient);
            ICarService carSqlService = new CarService(carSqlClient);
            IApartmentService apartmentSqlService = new ApartmentService(apartmentSqlClient);            

            var appRunner = new AppRunner(menu, console, userMongoClient, carMongoClient, apartmentMongoClient, userMongoService, carMongoService, apartmentMongoService, userSqlService, carSqlService, apartmentSqlService);
            appRunner.StartApp();            
        }
    }
}