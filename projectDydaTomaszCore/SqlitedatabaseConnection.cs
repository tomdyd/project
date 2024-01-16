using projectDydaTomaszCore.Interfaces;
using projectDydaTomaszCore.Models;
using System.Data;
using System.Data.SQLite;

namespace projectDydaTomasz.Core
{
    public class SqlitedatabaseConnection<T> : IDatabaseConnection<T>
    {
        private readonly string _connectionString = "Data Source=" + Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\sqlite.db;Version=3";

        public void AddToDb(T item)
        {
            var tableName = typeof(T).Name; 
            var properties = item.GetType().GetProperties().ToArray();

            var columnNameWithoutPrefix = string.Join(", ", properties.Select(prop => prop.Name));
            string columnNameWithPrefixes = "";

            var valuesString = string.Join($", @{columnNameWithPrefixes}", properties.Select(prop => prop.Name));

            var valuesArray = valuesString.Split(' ');
            var columnNamesWithPrefixesArray = new string[valuesArray.Length - 1];

            Array.Copy(valuesArray, 1, columnNamesWithPrefixesArray, 0, columnNamesWithPrefixesArray.Length); //kopiujemy tablice bez pierwszego elementu ktorym zawsze jest objectID
            columnNameWithPrefixes = string.Join(" ", columnNamesWithPrefixesArray);

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    var idGenerator = "lower(hex(randomblob(4))) || lower(hex(randomblob(2))) || '4' || substr(lower(hex(randomblob(2))),2) || 'a' || substr('89ab',abs(random()) % 4 + 1,1) || '6' || substr(lower(hex(randomblob(2))),2) || lower(hex(randomblob(6)))";
                    cmd.CommandText = $"INSERT INTO {tableName}s ({columnNameWithoutPrefix}) VALUES ({idGenerator}, {columnNameWithPrefixes})";

                    var query = $"INSERT INTO cars ({columnNameWithoutPrefix}) VALUES ({idGenerator}, {columnNameWithPrefixes})";

                    foreach (var property in typeof(T).GetProperties())
                    {
                        cmd.Parameters.AddWithValue($"@{property.Name}", property.GetValue(item));
                    }

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void DeleteData(string property, string searchTerm)
        {
            var dataname = typeof(T).Name;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"DELETE FROM {dataname}s WHERE {property} = @searchTerm";
                    cmd.Parameters.AddWithValue("@searchTerm", searchTerm);

                    cmd.ExecuteReader();
                }

                connection.Close();
            }
        }

        public List<T> GetAllDataList()
        {
            CheckDatabase();

            List<T> dataList = new List<T>();
            var dataname = typeof(T).Name;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {dataname}s";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T data = Activator.CreateInstance<T>();

                            foreach (var prop in typeof(T).GetProperties())
                            {
                                prop.SetValue(data, reader[prop.Name]);
                            }

                            dataList.Add(data);
                        }

                    }
                }

                connection.Close();
            }
            return dataList;
        }

        public T GetFilteredData(string property, string searchingTerm)
        {
            CheckDatabase();
            var dataname = typeof(T).Name;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"SELECT * FROM {dataname}s WHERE {property} = @searchingTerm";
                    cmd.Parameters.AddWithValue("@searchingTerm", searchingTerm);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            T data = Activator.CreateInstance<T>();

                            foreach (var prop in typeof(T).GetProperties())
                            {
                                prop.SetValue(data, reader[prop.Name]);
                            }

                            return data;

                        }
                    }
                }
                connection.Close();
            }
            return default;
        }

        public List<T> GetFilteredDataList(string property, string searchingTerm)
        {
            CheckDatabase();
            List<T> dataList = new List<T>();        

            var dataType = typeof(T).Name;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    cmd.CommandText = $"{property} {searchingTerm}";

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            T data = Activator.CreateInstance<T>();
                            User dataUser = Activator.CreateInstance<User>();

                            foreach (var prop in typeof(T).GetProperties())
                            {
                                if (prop.PropertyType == typeof(int))
                                {
                                    prop.SetValue(data, Convert.ToInt32(reader[prop.Name]));
                                }
                                else if (prop.PropertyType == typeof(User))
                                {
                                    foreach (var usrProp in typeof(User).GetProperties())
                                    {
                                        usrProp.SetValue(dataUser, reader[usrProp.Name]);                                       
                                    }
                                    prop.SetValue(data, dataUser);
                                }
                                else
                                {
                                    prop.SetValue(data, reader[prop.Name]);
                                }
                            }

                            dataList.Add(data);
                        }
                    }
                }
                connection.Close();
            }

            return dataList;            
        }

        public void UpdateData(string property, string searchTerm, T updatingData)
        {
            var dataType = typeof(T).Name;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(connection))
                {
                    var properties = updatingData.GetType().GetProperties();
                    string setValues = string.Join(", ", properties.Select(prop => $"{prop.Name} = @{prop.Name}"));
                    cmd.CommandText = $"UPDATE {dataType}s SET {setValues} WHERE {property} = '{searchTerm}'";

                    foreach (var prop in properties)
                    {
                        cmd.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(updatingData));
                    }
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private void CheckDatabase()
        {
            var path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\sqlite.db";
            if (File.Exists(path))
            {

            }
            else
            {
                SQLiteConnection.CreateFile($"{path}");

                // Utwórz połączenie do bazy danych
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={path};Version=3;"))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        command.CommandText =
                            @"CREATE TABLE users (
                                            userId TEXT NOT NULL,
                                            username TEXT NOT NULL,
                                            passwordHash TEXT NOT NULL,
                                            email TEXT NOT NULL,
                                            PRIMARY KEY(userId)
                                            );

                                            CREATE TABLE cars (
                                            carId TEXT NOT NULL,
                                            carModel TEXT NOT NULL,
                                            carProductionYear TEXT NOT NULL,
                                            engineCapacity TEXT NOT NULL,
                                            user TEXT NOT NULL,
                                            PRIMARY KEY(carId));

                                            CREATE TABLE apartments (
                                            apartmentId TEXT NOT NULL,
                                            surface TEXT NOT NULL,
                                            cost TEXT NOT NULL,
                                            user TEXT NOT NULL,
                                            PRIMARY KEY(apartmentId));";

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
        }
    }
}
