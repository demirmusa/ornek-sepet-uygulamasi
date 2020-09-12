using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Sepet.MongoDb.Models;

namespace Sepet.MongoDb
{
    public interface ISepetMongoDbContext
    {
        public IMongoCollection<MusteriSepetiEntity> MusteriSepeti { get; }
    }
    
    public class SepetMongoDbContext: ISepetMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public SepetMongoDbContext(IOptions<SepetMongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.Database);
        }
        
        public IMongoCollection<MusteriSepetiEntity> MusteriSepeti => 
            _database.GetCollection<MusteriSepetiEntity>("MusteriSepeti");
    }
}