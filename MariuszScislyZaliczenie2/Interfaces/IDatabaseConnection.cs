using MongoDB.Driver;
using MariuszScislyZaliczenie2.Models;


namespace MariuszScislyZaliczenie2.Interfaces
{
    public interface IDatabaseConnection<T>
    {
        public void AddToDb(T input);
        public T GetFilteredData(string property, string searchingTerm);
        public List<T> GetFilteredDataList(string property, string searchingTerm);
        public List<T> GetAllDataList();
        public void UpdateData(string property, string searchTerm, T updatingData);
        public void DeleteData(string property, string searchTerm);

        
    }
}