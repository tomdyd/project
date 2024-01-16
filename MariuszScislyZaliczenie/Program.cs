using MariuszScislyZaliczenie2;
using MariuszScislyZaliczenie2.Interfaces;
using MariuszScislyZaliczenie2.Models;
using MariuszScislyZaliczenie2.Services;
using MariuszScislyZaliczenie.Interfaces;


namespace MariuszScislyZaliczenie
{
    class Program
    {
        static void Main(string[] args)
        {
            IMenu menu = new Menu();
            IAppConsole console = new AppConsole();
            IDatabaseConnectionExtended<User> userMongoClient = new MongoDbDatabaseConnection<User>();
            IDatabaseConnectionExtended<Song> songMongoClient = new MongoDbDatabaseConnection<Song>();
            IDatabaseConnection<Song> songSqlClient = new SqlitedatabaseConnection<Song>();
            IDatabaseConnection<User> userSqlClient = new SqlitedatabaseConnection<User>();
            IUserService userMongoService = new UserService(userMongoClient);
            ISongService songMongoService = new SongService(songMongoClient);
            IUserService userSqlService = new UserService(userSqlClient);
            ISongService songSqlService = new SongService(songSqlClient);


            var appRunner = new AppRunner(menu, console, userMongoClient, songMongoClient,  userMongoService, songMongoService, userSqlService, songSqlService);
            appRunner.StartApp();            
        }
    }
}