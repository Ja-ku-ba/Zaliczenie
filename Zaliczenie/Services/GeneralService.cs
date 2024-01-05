using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Zaliczenie.Database;
using Zaliczenie.Models;
using Zaliczenie.Interfaces;

namespace Zaliczenie.Services
{
    public class GeneralService <T>
        where T : IArt
    {
        private readonly IMongoCollection<T> _artCollection;

        public GeneralService(IOptions<DatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.Connection);
            var mongoDb = mongoClient.GetDatabase(settings.Value.DatabaseName);

            if (typeof(T) == typeof(MovieModel))
            {
                _artCollection = mongoDb.GetCollection<T>(settings.Value.MovieCollection);
            }
            else if (typeof(T) == typeof(SongModel))
            {
                _artCollection = mongoDb.GetCollection<T>(settings.Value.SongCollection);
            }

        }

        public IMongoCollection<T> ArtCollection => _artCollection;

        // add new element
        public async Task CreateAsync(T newElement)
        {
            await _artCollection.InsertOneAsync(newElement);
        }

        // get all elements
        public async Task<List<T>> GetAsync()
        {
            return await _artCollection.Find(_ => true).ToListAsync();
        }

        // get element by id
        public async Task<T> GetAsync(string id)
        {
            return await _artCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        // update element by id
        public async Task UpdateAsync(string id, T updateElement)
        {
            await _artCollection.ReplaceOneAsync(x => x.Id == id, updateElement);
        }

        // delete element by id
        public async Task DeleteAsync(string id)
        {
            await _artCollection.DeleteOneAsync(x => x.Id == id);
        }
    }
}
