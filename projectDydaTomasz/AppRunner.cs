using projectDydaTomasz.Core.Interfaces;
using projectDydaTomasz.Core.Models;
using projectDydaTomasz.Interfaces;
using projectDydaTomaszCore.Interfaces;
using projectDydaTomaszCore.Models;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using ZstdSharp.Unsafe;
using static System.Net.Mime.MediaTypeNames;

namespace projectDydaTomasz
{
    public class AppRunner
    {
        private readonly IMenu _menu;
        private readonly IAppConsole _console;
        private readonly IDatabaseConnectionExtended<User> _userMongoClient;
        private readonly IDatabaseConnectionExtended<Car> _carMongoClient;
        private readonly IDatabaseConnectionExtended<Apartment> _apartmentMongoClient;
        private readonly IUserService _userMongoService;
        private readonly ICarService _carMongoService;
        private readonly IApartmentService _apartmentMongoService;
        private readonly IUserService _userSqlService;
        private readonly ICarService _carSqlService;
        private readonly IApartmentService? _apartmentSqlService;

        public AppRunner(
            IMenu menu,
            IAppConsole console,
            IDatabaseConnectionExtended<User> userMongoClient,
            IDatabaseConnectionExtended<Car> carMongoClient,
            IDatabaseConnectionExtended<Apartment> apartmentMongoClient,
            IUserService userMongoService,
            ICarService carMongoService,
            IApartmentService apartmentMongoService,
            IUserService userSqlService,
            ICarService carSqlService,
            IApartmentService apartmentSqlService)
        {
            _menu = menu;
            _console = console;
            _userMongoClient = userMongoClient;
            _carMongoClient = carMongoClient;
            _apartmentMongoClient = apartmentMongoClient;
            _userMongoService = userMongoService;
            _carMongoService = carMongoService;
            _apartmentMongoService = apartmentMongoService;
            _userSqlService = userSqlService;
            _carSqlService = carSqlService;
            _apartmentSqlService = apartmentSqlService;
        }

        public void StartApp()
        {
            while (true)
            {
                _console.Clear();
                _menu.MainMenu();
                var res = _console.GetResponseFromUser();

                switch (res)
                {
                    case 1:

                        var runLoginMenu = true;
                        while (runLoginMenu)
                        {
                            _console.Clear();
                            _menu.LoginMenu();
                            res = _console.GetResponseFromUser();
                            switch (res)
                            {
                                case 1:
                                    _userMongoClient.Connect("mongodb://localhost:27017/", "test", "user");
                                    var login = _console.GetLoginFromUser();
                                    var password = _console.GetPasswordFromUser();
                                    var loggedUser = _userMongoService.AuthorizeUser(login, password);

                                    if (loggedUser != null)
                                    {
                                        bool runCollectionsMenu = true;
                                        while (runCollectionsMenu)
                                        {
                                            _console.Clear();
                                            _menu.CollectionsMenu();
                                            res = _console.GetResponseFromUser();

                                            switch (res)
                                            {
                                                case 1:
                                                    _carMongoClient.Connect("mongodb://localhost:27017/", "test", "car");
                                                    bool runCarMenu = true;

                                                    while (runCarMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.carMenu();
                                                        res = _console.GetResponseFromUser();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newCar = new Car()
                                                                    {
                                                                        carBrand = _console.GetDataFromUser("Podaj markę samochodu: "),
                                                                        carModel = _console.GetDataFromUser("Podaj model samochodu: "),
                                                                        carProductionYear = _console.GetDataFromUser("Podaj rok produkcji: "),
                                                                        engineCapacity = _console.GetDataFromUser("Podaj pojemność silnika: "),
                                                                        user = loggedUser.userId
                                                                    };
                                                                    _carMongoService.CreateCar(newCar);
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                };


                                                                break; // Dodawanie samochodu do bazy danych MongoDb

                                                            case 2:
                                                                try
                                                                {
                                                                    var carList = _carMongoService.GetCars("user", loggedUser.userId);
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Wczytywanie wszystkich samochodów użytkownika z bazy MognoDb

                                                            case 3:
                                                                try
                                                                {
                                                                    var searchTerm = _console.GetDataFromUser("Podaj marke szukanego samochodu: ");
                                                                    var carList = _carMongoService.GetCars("carBrand", searchTerm);
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Wczytywanie wszystkich samochodów użytkownika z bazy MongoDb i filtrowanie po marce

                                                            case 4:
                                                                try
                                                                {
                                                                    var carList = _carMongoService.GetCars("user", loggedUser.userId);
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                                 $"{i + 1}." +
                                                                                 $" Marka: {carList[i].carBrand}," +
                                                                                 $" model: {carList[i].carModel}," +
                                                                                 $" rok produkcji: {carList[i].carProductionYear}," +
                                                                                 $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }

                                                                    _console.Write("Podaj numer samochodu który chcesz zaktualizować: ");
                                                                    var carNumber = _console.GetResponseFromUser();

                                                                    if (carNumber <= carList.Count)
                                                                    {
                                                                        var updatingCar = carList[carNumber - 1];

                                                                        updatingCar.carId = updatingCar.carId;
                                                                        updatingCar.carBrand = _console.GetDataFromUser("Podaj markę samochodu: ");
                                                                        updatingCar.carModel = _console.GetDataFromUser("Podaj model samochodu: ");
                                                                        updatingCar.carProductionYear = _console.GetDataFromUser("Podaj rok produkcji: ");
                                                                        updatingCar.engineCapacity = _console.GetDataFromUser("Podaj pojemność silnika: ");
                                                                        updatingCar.user = loggedUser.userId;

                                                                        _carMongoService.UpdateCar(updatingCar);

                                                                        _console.WriteLine("Dane zaktualizowane!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono samochodu!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Aktualizacja samochodów użytkownika w bazie MongoDb

                                                            case 5:
                                                                try
                                                                {
                                                                    var carList = _carMongoService.GetCars("user", loggedUser.userId);
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                                 $"{i + 1}." +
                                                                                 $" Marka: {carList[i].carBrand}," +
                                                                                 $" model: {carList[i].carModel}," +
                                                                                 $" rok produkcji: {carList[i].carProductionYear}," +
                                                                                 $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }

                                                                    _console.Write("Podaj numer samochodu który chcesz usunąć: ");
                                                                    var carNumber = _console.GetResponseFromUser();
                                                                    carList = _carMongoService.GetCars("user", loggedUser.userId);

                                                                    if (carNumber <= carList.Count)
                                                                    {
                                                                        var deletingCar = carList[carNumber - 1];

                                                                        _carMongoService.DeleteCar(deletingCar.carId);
                                                                        _console.WriteLine("Samochód został usunięty!");
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono samochodu!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Usuwanie samochodów użytkownika z bazy MongoDb

                                                            case 6:
                                                                runCarMenu = false;
                                                                break; // Powrót

                                                            default:
                                                                Console.WriteLine("Nie ma takiej opcji");
                                                                break;
                                                        }
                                                    }
                                                    break; // wybór kolekcji samochodów

                                                case 2:
                                                    _apartmentMongoClient.Connect("mongodb://localhost:27017/", "test", "apartment");
                                                    bool runApartmentMenu = true;

                                                    while (runApartmentMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.apartmentMenu();
                                                        res = _console.GetResponseFromUser();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newApartment = new Apartment()
                                                                    {
                                                                        surface = _console.GetDataFromUser("Podaj powierzchnię mieszkania: "),
                                                                        street = _console.GetDataFromUser("Podaj adres mieszkania: "),
                                                                        cost = _console.GetDataFromUser("Podaj cenę mieszkania: "),
                                                                        user = loggedUser.userId
                                                                    };

                                                                    _apartmentMongoService.CreateApartment(newApartment);
                                                                    _console.WriteLine("Dodano do bazy danych!");
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                };

                                                                break; // Dodawanie mieszkań do bazy MongoDb

                                                            case 2:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentMongoService.GetApartments("user", loggedUser.userId);
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Wczytywanie mieszkań użytkownika z bazy MongoDb

                                                            case 3:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentMongoService.GetApartments("user", loggedUser.userId);
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }

                                                                    _console.Write("Podaj numer mieszkania które chcesz zaktualizować: ");
                                                                    var apartmentNumber = _console.GetResponseFromUser();

                                                                    if (apartmentNumber <= apartmentsList.Count)
                                                                    {
                                                                        var updatingApartment = apartmentsList[apartmentNumber - 1];

                                                                        updatingApartment.surface = _console.GetDataFromUser("Podaj powierzchnię mieszkania: ");
                                                                        updatingApartment.street = _console.GetDataFromUser("Podaj adres mieszkania: ");
                                                                        updatingApartment.cost = _console.GetDataFromUser("Podaj cenę mieszkania: ");
                                                                        updatingApartment.user = loggedUser.userId;

                                                                        _apartmentMongoService.UpdateApartment(updatingApartment);

                                                                        _console.WriteLine("Dane zaktualizowane!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono mieszkania!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Aktualizacja mieszkań użytkownika w bazie MongoDb

                                                            case 4:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentMongoService.GetApartments("user", loggedUser.userId);
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }

                                                                    _console.Write("Podaj numer mieszkania które chcesz usunąć: ");
                                                                    var apartmentNumber = _console.GetResponseFromUser();

                                                                    if (apartmentNumber <= apartmentsList.Count)
                                                                    {
                                                                        var deletingApartment = apartmentsList[apartmentNumber - 1];

                                                                        _apartmentMongoService.DeleteApartment(deletingApartment.apartmentId);

                                                                        _console.WriteLine("Mieszkanie zostało usunięte!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono mieszkania!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Usuwanie mieszkań użytkownika z bazy MongoDb

                                                            case 5:
                                                                runApartmentMenu = false;
                                                                break; // Powrót

                                                            default:
                                                                _console.WriteLine("Nie ma takiej opcji!");
                                                                _console.ReadLine();
                                                                break;
                                                        }
                                                    }
                                                    break; // wybór kolekcji mieszkań

                                                case 3:
                                                    runCollectionsMenu = false;
                                                    loggedUser = null;
                                                    break; // powrót

                                                default:
                                                    Console.WriteLine("Nie ma takiej opcji");
                                                    break;
                                            }
                                        }
                                    }
                                    break; // logowanie

                                case 2:
                                    var newUser = new User()
                                    {
                                        username = _console.GetDataFromUser("Podaj login: "),
                                        passwordHash = _console.GetPasswordFromUser(),
                                        email = _console.GetDataFromUser("Podaj adres email: ")
                                    };
                                    _userMongoClient.Connect("mongodb://localhost:27017/", "test", "user");
                                    _userMongoService.RegisterUser(newUser);

                                    break; // rejestracja

                                case 3:
                                    runLoginMenu = false;
                                    break; // powrót

                                default:
                                    _console.WriteLine("Nie ma takiej opcji!");
                                    break;
                            }

                        }
                        break; //logowanie do mongoDb

                    case 2:

                        runLoginMenu = true;
                        while (runLoginMenu)
                        {
                            _console.Clear();
                            _menu.LoginMenu();
                            res = _console.GetResponseFromUser();
                            switch (res)
                            {
                                case 1:
                                    var login = _console.GetLoginFromUser();
                                    var password = _console.GetPasswordFromUser();
                                    var loggedUser = _userSqlService.AuthorizeUser(login, password);

                                    if (loggedUser != null)
                                    {
                                        bool runCollectionsMenu = true;

                                        while (runCollectionsMenu)
                                        {
                                            _console.Clear();
                                            _menu.CollectionsMenu();
                                            res = _console.GetResponseFromUser();

                                            switch (res)
                                            {
                                                case 1:
                                                    bool runCarMenu = true;

                                                    while (runCarMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.carMenu();
                                                        res = _console.GetResponseFromUser();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newCar = new Car()
                                                                    {
                                                                        carBrand = _console.GetDataFromUser("Podaj markę samochodu: "),
                                                                        carModel = _console.GetDataFromUser("Podaj model samochodu: "),
                                                                        carProductionYear = _console.GetDataFromUser("Podaj rok produkcji: "),
                                                                        engineCapacity = _console.GetDataFromUser("Podaj pojemność silnika: "),
                                                                        user = loggedUser.userId
                                                                    };

                                                                    _carSqlService.CreateCar(newCar);
                                                                    _console.WriteLine("Dodano do bazy danych!");
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Dodawanie samochodów do bazy sqLite

                                                            case 2:
                                                                try
                                                                {
                                                                    var carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Wczytywanie samochodów użytkownika z bazy sqLite

                                                            case 3:
                                                                try
                                                                {
                                                                    var searchTerm = _console.GetDataFromUser("Podaj marke szukanego samochodu: ");
                                                                    var carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE carBrand = '{searchTerm}'");
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }

                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Wczytywanie samochodów użytkownika z bazy sqLite i filtorwanie ich po marce

                                                            case 4:
                                                                try
                                                                {
                                                                    var carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE user = '{loggedUser.userId}'");

                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }

                                                                    _console.Write("Podaj numer samochodu który chcesz zaktualizować: ");
                                                                    var carNumber = _console.GetResponseFromUser();
                                                                    carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (carNumber <= carList.Count)
                                                                    {
                                                                        var updatingCar = carList[carNumber - 1];

                                                                        updatingCar.carId = updatingCar.carId;
                                                                        updatingCar.carBrand = _console.GetDataFromUser("Podaj markę samochodu: ");
                                                                        updatingCar.carModel = _console.GetDataFromUser("Podaj model samochodu: ");
                                                                        updatingCar.carProductionYear = _console.GetDataFromUser("Podaj rok produkcji: ");
                                                                        updatingCar.engineCapacity = _console.GetDataFromUser("Podaj pojemność silnika: ");

                                                                        _carSqlService.UpdateCar(updatingCar);

                                                                        _console.WriteLine("Dane zaktualizowane!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono samochodu!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Aktualizacja samochodów użytkownika w bazie sqLite

                                                            case 5:
                                                                try
                                                                {
                                                                    var carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < carList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Marka: {carList[i].carBrand}," +
                                                                            $" model: {carList[i].carModel}," +
                                                                            $" rok produkcji: {carList[i].carProductionYear}," +
                                                                            $" pojemność silnika: {carList[i].engineCapacity}");
                                                                    }

                                                                    _console.Write("Podaj numer samochodu który chcesz usunąć: ");
                                                                    var carNumber = _console.GetResponseFromUser();
                                                                    carList = _carSqlService.GetCars("SELECT * FROM Cars", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (carNumber <= carList.Count)
                                                                    {
                                                                        var deletingCar = carList[carNumber];

                                                                        _carSqlService.DeleteCar(deletingCar.carId);
                                                                        _console.WriteLine("Samochód został usunięty!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono samochodu!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; // Usuwanie samochodów użytkownika z bazy sqLite

                                                            case 6:
                                                                runCarMenu = false;
                                                                break; // Powrót

                                                            default:
                                                                _console.WriteLine("Nie ma takiej opcji!");
                                                                _console.ReadLine();
                                                                break;
                                                        }
                                                    }
                                                    break; // wybór kolekcji samochodów w sqLite

                                                case 2:
                                                    bool runApartmentMenu = true;

                                                    while (runApartmentMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.apartmentMenu();
                                                        res = _console.GetResponseFromUser();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newApartment = new Apartment()
                                                                    {
                                                                        surface = _console.GetDataFromUser("Podaj powierzchnię mieszkania: "),
                                                                        street = _console.GetDataFromUser("Podaj adres mieszkania: "),
                                                                        cost = _console.GetDataFromUser("Podaj cenę mieszkania: "),
                                                                        user = loggedUser.userId
                                                                    };

                                                                    _apartmentSqlService.CreateApartment(newApartment);
                                                                    _console.WriteLine("Dodano do bazy danych!");
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Dodawanie mnieszkań do bazy sqLite

                                                            case 2:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentSqlService.GetApartments("SELECT * FROM Apartments", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Wczytywanie mieszkań użytkownika z bazy sqLite

                                                            case 3:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentSqlService.GetApartments("SELECT * FROM Apartments", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }

                                                                    _console.Write("Podaj numer mieszkania które chcesz zaktualizować: ");
                                                                    var apartmentNumber = _console.GetResponseFromUser();
                                                                    apartmentsList = _apartmentSqlService.GetApartments("SELECT * FROM Apartments", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (apartmentNumber <= apartmentsList.Count)
                                                                    {
                                                                        var updatingApartment = apartmentsList[apartmentNumber - 1];

                                                                        updatingApartment.surface = _console.GetDataFromUser("Podaj powierzchnię mieszkania: ");
                                                                        updatingApartment.street = _console.GetDataFromUser("Podaj adres mieszkania: ");
                                                                        updatingApartment.cost = _console.GetDataFromUser("Podaj cenę mieszkania: ");
                                                                        updatingApartment.user = loggedUser.userId;

                                                                        _apartmentSqlService.UpdateApartment(updatingApartment);

                                                                        _console.WriteLine("Dane zaktualizowane!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono mieszkania!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Aktualizacja mieszkań użytkownika w bazie sqLite

                                                            case 4:
                                                                try
                                                                {
                                                                    var apartmentsList = _apartmentSqlService.GetApartments("SELECT * FROM Apartments", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < apartmentsList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Powierzchnia mieszkania: {apartmentsList[i].surface}," +
                                                                            $" Adres mieszkania: {apartmentsList[i].street}," +
                                                                            $" Cena mieszkania: {apartmentsList[i].cost}");
                                                                    }

                                                                    _console.Write("Podaj numer mieszkania które chcesz usunąć: ");
                                                                    var apartmentNumber = _console.GetResponseFromUser();
                                                                    apartmentsList = _apartmentSqlService.GetApartments("SELECT * FROM Apartments", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (apartmentNumber <= apartmentsList.Count)
                                                                    {
                                                                        var deletingApartment = apartmentsList[apartmentNumber - 1];

                                                                        _apartmentSqlService.DeleteApartment(deletingApartment.apartmentId);

                                                                        _console.WriteLine("Mieszkanie zostało usunięte!");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie znaleziono mieszkania!");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; // Usuwanie mieszkań użytkownika z bazy sqLite

                                                            case 5:
                                                                runApartmentMenu = false;
                                                                break; // Powrót

                                                            default:
                                                                _console.WriteLine("Nie ma takiej opcji!");
                                                                break;
                                                        }
                                                    }

                                                    break; // wybór kolekcji mieszkań w sqLite

                                                case 3:
                                                    runCollectionsMenu = false;
                                                    loggedUser = null;
                                                    break; // powrót

                                                default:
                                                    _console.WriteLine("Nie ma takiej opcji!");
                                                    _console.ReadLine();
                                                    break;
                                            }
                                        }
                                    }
                                    break; // logowanie

                                case 2:
                                    var sqlUser = new User()
                                    {
                                        userId = "004371db-c3c3-49ab-a9-64c10592d41718f7",
                                        username = _console.GetDataFromUser("Podaj login: "),
                                        passwordHash = _console.GetPasswordFromUser(),
                                        email = _console.GetDataFromUser("Podaj adres email: ")
                                    };

                                    _userSqlService.RegisterUser(sqlUser);
                                    break; // rejestracja

                                case 3:
                                    runLoginMenu = false;
                                    break; // powrót

                                default:
                                    _console.WriteLine("Nie ma takiej opcji!");
                                    _console.ReadLine();
                                    break;
                            }
                        }

                        break; //logowanie do sqlite

                    case 3:
                        return; //Wyjście

                    default:
                        _console.WriteLine("Nie ma takiej opcji");
                        _console.ReadLine();
                        break;
                }
            }
        }
    }
}
