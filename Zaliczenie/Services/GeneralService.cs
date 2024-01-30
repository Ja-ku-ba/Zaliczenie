using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zaliczenie.Database;
using Zaliczenie.Models;
using Zaliczenie.Interfaces;
using Microsoft.EntityFrameworkCore;
using Zaliczenie.Utils;
using Microsoft.AspNetCore.Mvc;
using Zaliczenie.Models;
using Zaliczenie.Services;

namespace Zaliczenie.Services
{
    //Table 'testor.element' doesn't exist
    public class GeneralService <T>
        where T : class, IArt
    {
        private readonly IMongoCollection<T> _mongoDatabase;
        private readonly string _mysqlDatabase;
        private readonly string _tableName;
        public GeneralService(IOptions<DatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.Connection);
            var mongoDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

            if (typeof(T) == typeof(SongModel))
            {
                _mongoDatabase = mongoDb.GetCollection<T>(settings.Value.SongCollection);
                _tableName = "song";
            }
            else
            {
                _mongoDatabase = mongoDb.GetCollection<T>(settings.Value.MovieCollection);
                _tableName = "movie";
            }
            _mysqlDatabase = settings.Value.MysqlConnectionString;
        }

        public IMongoCollection<T> ArtCollection => _mongoDatabase;

        // add new element
        public async Task<string> CreateAsync(string database, T newElement)
        {
            if (database == "mongo")
            {
                await _mongoDatabase.InsertOneAsync(newElement);
                return "mongo";
            }
            else
            {
                var result = new MySqlQueryGetter<T>( _mysqlDatabase);
                return await result.AddResult(_tableName, newElement);
            }
        }

        // get all elements
        public async Task<List<T>> GetAsync(string database)
        {
            if (database == "mongo")
            {
                return await _mongoDatabase.Find(_ => true).ToListAsync();
            }
            else
            {
                return await new MySqlQueryGetter<T>(_mysqlDatabase).GetResults(_tableName);
            }
        }

        // get element by id
        public async Task<T> GetAsync(string database, string id)
        {
            if (database == "mongo")
            {
                return await _mongoDatabase.Find(x => x.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                var resultTask = new MySqlQueryGetter<T>(_mysqlDatabase).GetById(_tableName, id);
                var result = await resultTask;
                return result.FirstOrDefault();
            }
        }

        // update element by id
        public async Task UpdateAsync(string database, string id, T updateElement)
        {
            if (database == "mongo")
            {
                await _mongoDatabase.ReplaceOneAsync(x => x.Id == id, updateElement);
            }
            else
            {
                await new MySqlQueryGetter<T>(_mysqlDatabase).UpdateResult(_tableName, id, updateElement);
            }
        }

        // delete element by id
        public async Task<string> DeleteAsync(string database, string id)
        {
            if (database == "mongo")
            {
                T ifExists = await GetAsync(database, id);
                if (ifExists == null)
                {
                    return null;
                }
                await _mongoDatabase.DeleteOneAsync(x => x.Id == id);
                return "OK";
            }
            else
            {
                var result = new MySqlQueryGetter<T>(_mysqlDatabase);
                var res = await result.DeleteResult(_tableName, id);
                return res;
            }
        }
    }
}
