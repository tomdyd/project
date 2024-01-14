namespace projectDydaTomasz.Interfaces
{
    public interface IAppConsole
    {
        int GetResponseFromUser();
        string GetDataFromUser(string msg);

        string GetLoginFromUser();

        string GetPasswordFromUser();

        void Clear();

        string ReadLine();

        void WriteLine(object msg);

        void Write(object msg);
    }
}