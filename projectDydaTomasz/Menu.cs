using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectDydaTomasz
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
            Console.WriteLine("1. Zaloguj się");
            Console.WriteLine("2. Zarejestruj się");
            Console.WriteLine("3. Wróć");
        }
        public void CollectionsMenu()
        {
            Console.WriteLine("1. Samochody");
            Console.WriteLine("2. Mieszkania");
            Console.WriteLine("3. Wyloguj");
        }

        public void carMenu()
        {
            Console.WriteLine("1. Dodaj nowy samochód");
            Console.WriteLine("2. Wczytaj listę swoich samochodów");
            Console.WriteLine("3. Wyszukaj samochody po marce");
            Console.WriteLine("4. Zaktualizuj swój samochód");
            Console.WriteLine("5. Usuń swój samochód");
            Console.WriteLine("6. Wróć");
        }

        public void apartmentMenu()
        {
            Console.WriteLine("1. Dodaj nowe mieszkanie");
            Console.WriteLine("2. Wczytaj listę swoich mieszkań");
            Console.WriteLine("3. Zaktualizuj swój mieszkanie");
            Console.WriteLine("4. Usuń swój mieszkanie");
            Console.WriteLine("5. Wróć");
        }
    }
}
