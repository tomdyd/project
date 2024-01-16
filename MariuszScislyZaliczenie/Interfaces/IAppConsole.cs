namespace MariuszScislyZaliczenie.Interfaces
{
    public interface IAppConsole
    {
        int Response();
        string Data(string msg);

        string Login();

        string Password();

        void Clear();

        string ReadLine();

        void WriteLine(object msg);

        void Write(object msg);
    }
}