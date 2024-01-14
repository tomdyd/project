using MongoDB.Driver;
using projectDydaTomaszCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projectDydaTomasz.Core.Interfaces
{
    public interface IDatabaseConnectionExtended<T> : IDatabaseConnection<T>
    {
        public void Connect(string connectionString, string databaseName, string collectionName); // Metoda do nawiązywania połączenia z bazą danych.
        public void Disconnect(); // Metoda do zamykania połączenia z bazą danych.
        public IMongoCollection<T> GetCollection(string collectionName);
    }
}
