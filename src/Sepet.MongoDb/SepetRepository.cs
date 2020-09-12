using System.Threading.Tasks;
using MongoDB.Driver;
using Sepet.Core;
using Sepet.Core.Models;
using Sepet.MongoDb.Models;

namespace Sepet.MongoDb
{
    public class SepetRepository : ISepetRepository
    {
        private ISepetMongoDbContext _context;

        public SepetRepository(ISepetMongoDbContext context)
        {
            _context = context;
        }

        private Task<MusteriSepetiEntity> GetirMusteriSepetiEntity(int musteriId)
        {
            return _context.MusteriSepeti
                .Find(x => x.MusteriId == musteriId)
                .FirstOrDefaultAsync();
        }

        public async Task<MusteriSepeti> MusteriSepetiGetir(int musteriId)
        {
            var entity = await GetirMusteriSepetiEntity(musteriId);

            return MusteriSepetiEntityToMusteriSepetiOrDefault(musteriId, entity);
        }

        private MusteriSepeti MusteriSepetiEntityToMusteriSepetiOrDefault(int musteriId, MusteriSepetiEntity entity)
        {
            var sepet = new MusteriSepeti(musteriId);
            if (entity != null)
            {
                sepet.Items = entity.Items;
            }

            return sepet;
        }

        public async Task SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            var entity = await GetirMusteriSepetiEntity(musteriId);

            var sepet = MusteriSepetiEntityToMusteriSepetiOrDefault(musteriId, entity);
            sepet.UrunEkle(urunId, adet);

            if (entity == null)
            {
                entity = new MusteriSepetiEntity()
                {
                    MusteriId = musteriId,
                    Items = sepet.Items
                };
                await _context.MusteriSepeti.InsertOneAsync(entity);
            }
            else
            {
                entity.Items = sepet.Items;

                await _context.MusteriSepeti.ReplaceOneAsync(
                    x => x.MusteriId == musteriId,
                    entity
                );
            }
        }
    }
}