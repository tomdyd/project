
using projectDydaTomasz.Interfaces;
using System.Text;

namespace projectDydaTomasz
{
    public class AppConsole : IAppConsole
    {
        public int GetResponseFromUser()
        {
            while (true)
            { 
                var res = Console.ReadLine();

                if (int.TryParse(res, out var intResponse))
                {
                    return intResponse;
                }
                Console.WriteLine("Jeszcze raz - to nie jest int");
            }
        }

        public string GetDataFromUser(string msg)
        {
            Console.Write(msg);
            var res = Console.ReadLine();
            return res;
        }

        public string GetLoginFromUser()
        {
            Console.Write("Podaj login: ");
            var response = Console.ReadLine();
            return response;
        }

        public string GetPasswordFromUser()
        {
            var password = new StringBuilder();

                Console.Write("Podaj hasło: ");
                while (true)
                {
                    ConsoleKeyInfo i = Console.ReadKey(true);
                    if (i.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    else if (i.Key == ConsoleKey.Backspace)
                    {
                        if (password.Length > 0)
                        {
                            password.Remove(password.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                    }
                    else
                    {
                        password.Append(i.KeyChar);
                        Console.Write("*");
                    }
                }
            return password.ToString();
        }

        public void Clear()
        {
            Console.Clear();
        }

        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(object msg)
        {
            Console.WriteLine(msg);
        }

        public void Write(object msg)
        {
            Console.Write(msg);
        }
    }
}
