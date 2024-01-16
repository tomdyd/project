
using MariuszScislyZaliczenie.Interfaces;
using System.Text;

namespace MariuszScislyZaliczenie
{
    public class AppConsole : IAppConsole
    {
        public int Response()
        {
            while (true)
            { 
                var result = Console.ReadLine();

                if (int.TryParse(result, out var intResult))
                {
                    return intResult;
                }
                Console.WriteLine("Wpisz ponownie");
            }
        }

        public string Data(string msg)
        {
            Console.Write(msg);
            var respo = Console.ReadLine();
            return respo;
        }

        public string Login()
        {
            Console.Write("Podaj login: ");
            var response = Console.ReadLine();
            return response;
        }

        public string Password()
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

        public void Write(object msg)
        {
            Console.Write(msg);
        }

        public void WriteLine(object msg)
        {
            Console.WriteLine(msg);
        }



        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
