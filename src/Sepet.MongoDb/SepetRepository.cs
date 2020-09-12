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

        private MusteriSepetiEntity GetirMusteriSepetiEntity(int musteriId)
        {
            return _context.MusteriSepeti
                .Find(x => x.MusteriId == musteriId)
                .FirstOrDefault();
        }

        public MusteriSepeti MusteriSepetiGetir(int musteriId)
        {
            var entity = GetirMusteriSepetiEntity(musteriId);
            if (entity == null)
            {
                return new MusteriSepeti(musteriId);
            }

            return new MusteriSepeti(musteriId) {Items = entity.Items};
        }

        public void SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            var entity = GetirMusteriSepetiEntity(musteriId);

            var sepet = MusteriSepetiGetir(musteriId);
            sepet.UrunEkle(urunId, adet);

            if (entity == null)
            {
                entity = new MusteriSepetiEntity()
                {
                    MusteriId = musteriId,
                    Items = sepet.Items
                };
                _context.MusteriSepeti.InsertOne(entity);
            }
            else
            {
                entity.Items = sepet.Items;

                _context.MusteriSepeti.ReplaceOne(
                    x => x.MusteriId == musteriId,
                    entity
                );
            }
        }
    }
}