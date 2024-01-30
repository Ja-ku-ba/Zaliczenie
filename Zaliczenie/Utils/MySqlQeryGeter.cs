using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MySqlConnector;
using Zaliczenie.Interfaces;
using Zaliczenie.Models;

namespace Zaliczenie.Utils
{
    public class MySqlQueryGetter<T>
        where T : class, IArt
    {
        private readonly string _databaseConnection;

        public MySqlQueryGetter(string databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<List<T>> GetResults(string table)
        {
            return await GetMysql($"SELECT * FROM {table}");
        }

        public async Task<List<T>> GetById(string table, string id)
        {
            return await GetMysql($"SELECT * FROM {table} WHERE id = {id}");
        }


        public async Task<List<T>> GetMysql(string querry)
        {
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                try
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(querry, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        var resultList = new List<T>();

                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance<T>();

                            if (item is T art)
                            {
                                art.Id = reader.GetValue(0)?.ToString();
                                art.Title = reader.GetValue(1)?.ToString();
                                art.Author = reader.GetValue(2)?.ToString();
                                art.Relased = Convert.ToDateTime(reader.GetValue(3));
                                art.Rating = reader.GetValue(4) as int?;
                            }

                            resultList.Add(item);
                        }

                        return resultList;

                    }
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    return null;
                }
            }
        }


        private async Task<bool> ChcekIfExists(string table, string id)
        {
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                try
                {
                    conn.Open();

                    string query = $"SELECT COUNT(*) FROM {table} WHERE id = {id}";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        return count > 0;
                    }

                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    return false;
                }
            }
        }

        // Działa tylko dla jednego użytkownika, np. w update gdy chcemy sprawdzić
        // czy element się zaktualizował, a w międzyczasie inny użytkownik doda coś,
        // wtedy istnieje szansa że do sprawdzenia atualizacji dostaniemy wartość dla
        // nowej instacji dodanej przez innego użytkowniaka. Bardzo długie zdanie.
        public async Task<int> GetFreeId (string table)
        {
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                try
                {
                    conn.Open();

                    string query = $"SELECT MAX(id) FROM {table}";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        return count;
                    }
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    return -1;
                }
            }
        }

        public async Task<string> UpdateResult(string table, string oldId, T updateElement)
        {
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                try
                {
                    conn.Open();
                    string formattedDate = updateElement.Relased.ToString("yyyy-MM-dd");
                    string query = $"UPDATE {table} " + 
                                   $"SET Title = '{updateElement.Title}', Author = '{updateElement.Author}', " +
                                   $"Relased = '{formattedDate}', Rating = {updateElement.Rating} " + 
                                   $"WHERE id = {oldId};";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                }

                var ifCreated = await ChcekIfExists(table, $"{oldId}");
                if (ifCreated)
                {
                    return $"OK";
                }

                return null;
            }
        }

        public async Task<string> AddResult(string table, T newElement)
        {
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                int id = await GetFreeId(table);
                try
                {
                    conn.Open();
                    string formattedDate = newElement.Relased.ToString("yyyy-MM-dd");
                    string query = $"INSERT INTO {table} (Id, Title, Author, Relased, Rating) " +
                                   $"VALUES ({id + 1}, '{newElement.Title}', '{newElement.Author}', " +
                                   $"'{formattedDate}', {newElement.Rating});";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                }

                var ifCreated = await ChcekIfExists(table, $"{id + 1}");
                if (ifCreated)
                {
                    return $"{id + 1}";
                }

                return null;
            }
        }


        public async Task<string> DeleteResult(string table, string id)
        {
            string querry = $"DELETE FROM {table} WHERE id = {id}";
            using (var conn = new MySqlConnection(_databaseConnection))
            {
                try
                {
                    conn.Open();
                    var ifExists = ChcekIfExists(table, id);
                    if (ifExists  != null)
                    {
                        new MySqlCommand(querry, conn).ExecuteReader();
                        return "OK";
                    }
                    return null;
                }
                catch (Exception error)
                {
                    System.Diagnostics.Debug.WriteLine(error);
                    return null;
                }
            
            }
        }
    }
}
