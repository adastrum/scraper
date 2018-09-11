using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Scraper.Core.Dto;
using Scraper.Core.Interfaces;

namespace Scraper.Data.MongoDb
{
    public class ShowStorageService : IShowStorageService
    {
        private const string DefaultConnectionString = "mongodb://localhost";
        private const string ScraperDbName = "scraper";
        private const string ShowsRawCollectionName = "showsRaw";
        private const string ShowsCollectionName = "shows";

        private readonly IMongoDatabase _database;

        public ShowStorageService() : this(DefaultConnectionString)
        {

        }

        public ShowStorageService(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(ScraperDbName);

            RegisterMappings();
        }

        private static void RegisterMappings()
        {
            BsonClassMap.RegisterClassMap<Show>(cm =>
            {
                cm.MapProperty(x => x.Id).SetElementName("_id");
                cm.MapProperty(x => x.Name).SetElementName("name");
                cm.MapProperty(x => x.Cast).SetElementName("cast");
            });

            BsonClassMap.RegisterClassMap<Person>(cm =>
            {
                cm.MapProperty(x => x.Id).SetElementName("id");
                cm.MapProperty(x => x.Name).SetElementName("name");
                cm.MapProperty(x => x.Birthday).SetElementName("birthday");
            });
        }

        public async Task SaveRawDataAsync(string showJson, string castJson)
        {
            var collection = _database.GetCollection<BsonDocument>(ShowsRawCollectionName);

            var show = BsonSerializer.Deserialize<BsonDocument>(showJson);
            show["_id"] = show["id"];
            show["cast"] = BsonSerializer.Deserialize<BsonArray>(castJson);

            await collection.InsertOneAsync(show);
        }

        public async Task ExtractDataAsync(CancellationToken token)
        {
            var collection = _database.GetCollection<BsonDocument>(ShowsRawCollectionName);

            var project = new BsonDocument
            {
                {"name", 1},
                {"cast", "$cast.person"}
            };

            await collection.Aggregate()
                .Project(project)
                .OutAsync(ShowsCollectionName, token);
        }

        public async Task<IEnumerable<Show>> GetShowsAsync(int pageSize, int pageNumber)
        {
            var collection = _database.GetCollection<Show>(ShowsCollectionName);

            return await collection
                .Find(Builders<Show>.Filter.Empty)
                .Sort(Builders<Show>.Sort.Ascending(x => x.Id))
                .Skip(pageSize * pageNumber)
                .Limit(pageSize)
                .ToListAsync();
        }
    }
}
