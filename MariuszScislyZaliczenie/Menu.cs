using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MariuszScislyZaliczenie
{
    public class Menu : IMenu
    {
        public void MainMenu()
        {
            Console.WriteLine("1. Połącz z bazą danych MongoDb");
            Console.WriteLine("2. Połącz z bazą danych  SQL");
            Console.WriteLine("3. Wyjdź");
        }

        public void LoginMenu()
        {
            Console.WriteLine("1. Logowanie");
            Console.WriteLine("2. Rejestracja");
            Console.WriteLine("3. Powrót");
        }
        public void CollectionsMenu()
        {
            Console.WriteLine("1. Piosenki");
            Console.WriteLine("2. Wyloguj");
        }

        public void songMenu()
        {
            Console.WriteLine("1. Dodaj nową piosenke");
            Console.WriteLine("2. Pobierz listę piosenek");
            Console.WriteLine("3. Wyszukaj piosenkę po tytule");
            Console.WriteLine("4. Aktualizacja piosenek");
            Console.WriteLine("5. Usuwanie piosenek");
            Console.WriteLine("6. Powrót");
        }

      
    }
}
