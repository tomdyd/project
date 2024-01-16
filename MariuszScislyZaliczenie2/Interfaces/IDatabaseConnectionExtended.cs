using MongoDB.Driver;


namespace MariuszScislyZaliczenie2.Interfaces
{
    public interface IDatabaseConnectionExtended<T> : IDatabaseConnection<T>
    {
        public void Connect(string connectionString, string databaseName, string collectionName); 
        public void Disconnect(); 
        public IMongoCollection<T> GetCollection(string collectionName);
    }
}
