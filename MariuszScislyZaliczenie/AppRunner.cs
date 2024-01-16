
using MariuszScislyZaliczenie.Interfaces;
using MariuszScislyZaliczenie2.Models;
using MariuszScislyZaliczenie2.Interfaces;

namespace MariuszScislyZaliczenie
{
    public class AppRunner
    {
        private readonly IMenu _menu;
        private readonly IAppConsole _console;
        private readonly IDatabaseConnectionExtended<User> _userMongoClient;
        private readonly IDatabaseConnectionExtended<Song> _songMongoClient;   
        private readonly IUserService _userMongoService;
        private readonly ISongService _songMongoService;
        private readonly IUserService _userSqlService;
        private readonly ISongService _songSqlService;
     

        public AppRunner(
            IMenu menu,
            IAppConsole console,
            IDatabaseConnectionExtended<User> userMongoClient,
            IDatabaseConnectionExtended<Song> songMongoClient,
            IUserService userMongoService,
            ISongService songMongoService,
            IUserService userSqlService,
            ISongService songSqlService)
        {
            _menu = menu;
            _console = console;
            _userMongoClient = userMongoClient;
            _songMongoClient = songMongoClient;
            _userMongoService = userMongoService;
            _songMongoService = songMongoService;
            _userSqlService = userSqlService;
            _songSqlService = songSqlService;
        }

        public void StartApp()
        {
            while (true)
            {
                _console.Clear();
                _menu.MainMenu();
                var res = _console.Response();

                switch (res)
                {
                    case 1:

                        var runLoginMenu = true;
                        while (runLoginMenu)
                        {
                            _console.Clear();
                            _menu.LoginMenu();
                            res = _console.Response();
                            switch (res)
                            {
                                case 1:
                                    _userMongoClient.Connect("mongodb://localhost:27017/", "dataBase", "user");
                                    var login = _console.Login();
                                    var password = _console.Password();
                                    var loggedUser = _userMongoService.AuthorizeUser(login, password);

                                    if (loggedUser != null)
                                    {
                                        bool runCollectionsMenu = true;
                                        while (runCollectionsMenu)
                                        {
                                            _console.Clear();
                                            _menu.CollectionsMenu();
                                            res = _console.Response();

                                            switch (res)
                                            {
                                                case 1:
                                                    _songMongoClient.Connect("mongodb://localhost:27017/", "dataBase", "song");
                                                    bool runSongMenu = true;

                                                    while (runSongMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.songMenu();
                                                        res = _console.Response();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newSong = new Song()
                                                                    {
                                                                        songAuthor = _console.Data("Podaj wykonawcę: "),
                                                                        songTitle = _console.Data("Podaj tytuł: "),
                                                                        songGenre = _console.Data("Podaj gatunek: "),
                                                                        songAlbum = _console.Data("Podaj album: "),
                                                                        user = loggedUser.userId
                                                                    };
                                                                    _songMongoService.CreateSong(newSong);
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                };


                                                                break;

                                                            case 2:
                                                                try
                                                                {
                                                                    var songList = _songMongoService.GetSongs("user", loggedUser.userId);
                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Wykonawca: {songList[i].songAuthor}," +
                                                                            $" Tytuł: {songList[i].songTitle}," +
                                                                            $" Gatunek: {songList[i].songGenre}," +
                                                                            $" Album: {songList[i].songAlbum}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 3:
                                                                try
                                                                {
                                                                    var searchTerm = _console.Data("Podaj tytuł szukanej piosenki: ");
                                                                    var songList = _songMongoService.GetSongs("userId", loggedUser.userId);

                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        if (songList[i].songTitle == searchTerm)
                                                                        {
                                                                            _console.WriteLine(
                                                                                $"{i + 1}." +
                                                                                $" Tytuł: {songList[i].songTitle}," +
                                                                                $" Wykonawca: {songList[i].songAuthor}," +
                                                                                $" Gatunek: {songList[i].songGenre}," +
                                                                                $" Album: {songList[i].songAlbum}");
                                                                        }
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; 

                                                            case 4:
                                                                try
                                                                {
                                                                    var songList = _songMongoService.GetSongs("user", loggedUser.userId);
                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                                 $"{i + 1}." +
                                                                                 $" Tytuł: {songList[i].songTitle}," +
                                                                                 $" Wykonawca: {songList[i].songAuthor}," +
                                                                                 $" Gatunek: {songList[i].songGenre}," +
                                                                                 $" Album: {songList[i].songAlbum}");
                                                                    }

                                                                    _console.Write("Podaj numer piosenki którą chcesz zaktualizować: ");
                                                                    var songNumber = _console.Response();

                                                                    if (songNumber <= songList.Count)
                                                                    {
                                                                        var updatingSong = songList[songNumber - 1];

                                                                        updatingSong.songId = updatingSong.songId;
                                                                        updatingSong.songTitle = _console.Data("Podaj tytuł piosenki: ");
                                                                        updatingSong.songAuthor = _console.Data("Podaj wykonawcę: ");
                                                                        updatingSong.songGenre = _console.Data("Podaj gatunek: ");
                                                                        updatingSong.songAlbum = _console.Data("Podaj album: ");
                                                                        updatingSong.user = loggedUser.userId;

                                                                        _songMongoService.UpdateSong(updatingSong);

                                                                        _console.WriteLine("Zaktualizowano poprawnie.");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie ma takiej piosenki w bazie");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 5:
                                                                try
                                                                {
                                                                    var songList = _songMongoService.GetSongs("user", loggedUser.userId);
                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                                 $"{i + 1}." +
                                                                                 $" Tytuł: {songList[i].songTitle}," +
                                                                                 $" Wykonawca: {songList[i].songAuthor}," +
                                                                                 $" Gatunek: {songList[i].songGenre}," +
                                                                                 $" Album: {songList[i].songAlbum}");
                                                                    }

                                                                    _console.Write("Podaj numer piosenki chcesz usunąć: ");
                                                                    var songNumber = _console.Response();
                                                                    songList = _songMongoService.GetSongs("user", loggedUser.userId);

                                                                    if (songNumber <= songList.Count)
                                                                    {
                                                                        var deletingSong = songList[songNumber - 1];

                                                                        _songMongoService.DeleteSong(deletingSong.songId);
                                                                        _console.WriteLine("Usunięto piosenkę z bazy");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie ma takiej piosenki w bazie");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break;

                                                            case 6:
                                                                runSongMenu = false;
                                                                break; 

                                                            default:
                                                                Console.WriteLine("Spróbuj ponownie");
                                                                break;
                                                        }
                                                    }
                                                    break; 

                                               

                                                case 2:
                                                    runCollectionsMenu = false;
                                                    loggedUser = null;
                                                    break; 

                                                default:
                                                    Console.WriteLine("Spróbuj ponownie");
                                                    break;
                                            }
                                        }
                                    }
                                    break;

                                case 2:
                                    var newUser = new User()
                                    {
                                        username = _console.Data("Podaj login: "),
                                        passwordHash = _console.Password(),
                                        email = _console.Data("Podaj email: ")
                                    };
                                    _userMongoClient.Connect("mongodb://localhost:27017/", "dataBase", "user");
                                    _userMongoService.RegisterUser(newUser);

                                    break; 

                                case 3:
                                    runLoginMenu = false;
                                    break; 

                                default:
                                    _console.WriteLine("Spróbuj ponownie");
                                    break;
                            }

                        }
                        break; 

                    case 2:

                        runLoginMenu = true;
                        while (runLoginMenu)
                        {
                            _console.Clear();
                            _menu.LoginMenu();
                            res = _console.Response();
                            switch (res)
                            {
                                case 1:
                                    var login = _console.Login();
                                    var password = _console.Password();
                                    var loggedUser = _userSqlService.AuthorizeUser(login, password);

                                    if (loggedUser != null)
                                    {
                                        bool runCollectionsMenu = true;

                                        while (runCollectionsMenu)
                                        {
                                            _console.Clear();
                                            _menu.CollectionsMenu();
                                            res = _console.Response();

                                            switch (res)
                                            {
                                                case 1:
                                                    bool runSongMenu = true;

                                                    while (runSongMenu)
                                                    {
                                                        _console.Clear();
                                                        _menu.songMenu();
                                                        res = _console.Response();

                                                        switch (res)
                                                        {
                                                            case 1:
                                                                try
                                                                {
                                                                    var newSong = new Song()
                                                                    {
                                                                        songTitle = _console.Data("Podaj tytuł piosenki: "),
                                                                        songAuthor = _console.Data("Podaj wykonawcę: "),
                                                                        songGenre = _console.Data("Podaj gatunek: "),
                                                                        songAlbum = _console.Data("Podaj album: "),
                                                                        user = loggedUser.userId
                                                                    };

                                                                    _songSqlService.CreateSong(newSong);
                                                                    _console.WriteLine("Pomyślnie dodano piosenkę do bazy");
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 2:
                                                                try
                                                                {
                                                                    var songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Tytuł: {songList[i].songTitle}," +
                                                                            $" Wykonawca: {songList[i].songAuthor}," +
                                                                            $" Gatunek: {songList[i].songGenre}," +
                                                                            $" Album: {songList[i].songAlbum}");
                                                                    }
                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }
                                                                break; 

                                                            case 3:
                                                                try
                                                                {
                                                                    var searchTerm = _console.Data("Podaj tytuł piosenki: ");
                                                                    var songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");                                                                   

                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        if (songList[i].songTitle == searchTerm)
                                                                        {
                                                                            _console.WriteLine(
                                                                                $"{i + 1}." +
                                                                                $" Tytuł: {songList[i].songTitle}," +
                                                                                $" Wykonawca: {songList[i].songAuthor}," +
                                                                                $" Gatunek: {songList[i].songGenre}," +
                                                                                $" Album: {songList[i].songAlbum}");
                                                                        }
                                                                    }

                                                                    _console.ReadLine();
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 4:
                                                                try
                                                                {
                                                                    var songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");

                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Tytuł: {songList[i].songTitle}," +
                                                                            $" Wykonawca: {songList[i].songAuthor}," +
                                                                            $" Gatunek: {songList[i].songGenre}," +
                                                                            $" Album: {songList[i].songGenre}");
                                                                    }

                                                                    _console.Write("Podaj numer piosenki którą chcesz zaktualizować: ");
                                                                    var songNumber = _console.Response();
                                                                    songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (songNumber <= songList.Count)
                                                                    {
                                                                        var updatingSong = songList[songNumber - 1];

                                                                        updatingSong.songId = updatingSong.songId;
                                                                        updatingSong.songTitle = _console.Data("Podaj tytuł piosenki: ");
                                                                        updatingSong.songAuthor = _console.Data("Podaj wykonawcę: ");
                                                                        updatingSong.songGenre = _console.Data("Podaj gatunek: ");
                                                                        updatingSong.songAlbum = _console.Data("Podaj album: ");

                                                                        _songSqlService.UpdateSong(updatingSong);

                                                                        _console.WriteLine("Dane zaktualizowane pomyślnie");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie ma takiej piosenki w bazie");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 5:
                                                                try
                                                                {
                                                                    var songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");
                                                                    for (int i = 0; i < songList.Count; i++)
                                                                    {
                                                                        _console.WriteLine(
                                                                            $"{i + 1}." +
                                                                            $" Tytuł: {songList[i].songTitle}," +
                                                                            $" Wykonawca: {songList[i].songAuthor}," +
                                                                            $" Gatunek: {songList[i].songGenre}," +
                                                                            $" Album: {songList[i].songAlbum}");
                                                                    }

                                                                    _console.Write("Podaj numer piosenki którą chcesz usunąć: ");
                                                                    var songNumber = _console.Response();
                                                                    songList = _songSqlService.GetSongs("SELECT * FROM Songs", $"WHERE user = '{loggedUser.userId}'");

                                                                    if (songNumber <= songList.Count)
                                                                    {
                                                                        var deletingSong = songList[songNumber - 1];

                                                                        _songSqlService.DeleteSong(deletingSong.songId);
                                                                        _console.WriteLine("Piosenkę usunięto pomyślnie");
                                                                        _console.ReadLine();
                                                                    }
                                                                    else
                                                                    {
                                                                        _console.WriteLine("Nie ma takiej piosenki w bazie");
                                                                        _console.ReadLine();
                                                                    }
                                                                }
                                                                catch (Exception e)
                                                                {
                                                                    _console.WriteLine(e.Message);
                                                                    _console.ReadLine();
                                                                }

                                                                break; 

                                                            case 6:
                                                                runSongMenu = false;
                                                                break; 

                                                            default:
                                                                _console.WriteLine("Spróbuj ponownie");
                                                                _console.ReadLine();
                                                                break;
                                                        }
                                                    }
                                                    break; 

                                               

                                                   
                                                case 2:
                                                    runCollectionsMenu = false;
                                                    loggedUser = null;
                                                    break; 
                                                default:
                                                    _console.WriteLine("Spróbuj ponownie");
                                                    _console.ReadLine();
                                                    break;
                                            }
                                        }
                                    }
                                    break; 

                                case 2:
                                    var sqlUser = new User()
                                    {
                                        userId = "004371db-c3c3-49ab-a9-64c10592d41718f7",
                                        username = _console.Data("Podaj login: "),
                                        passwordHash = _console.Password(),
                                        email = _console.Data("Podaj email: ")
                                    };

                                    _userSqlService.RegisterUser(sqlUser);
                                    break; 

                                case 3:
                                    runLoginMenu = false;
                                    break; 

                                default:
                                    _console.WriteLine("Spróbuj ponownie");
                                    _console.ReadLine();
                                    break;
                            }
                        }

                        break; 

                    case 3:
                        return; 

                    default:
                        _console.WriteLine("Spróbuj ponownie");
                        _console.ReadLine();
                        break;
                }
            }
        }
    }
}
