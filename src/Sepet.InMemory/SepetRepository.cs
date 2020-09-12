using System.Collections.Concurrent;
using System.Threading.Tasks;
using Sepet.Core;
using Sepet.Core.Models;

namespace Sepet.InMemory
{
    public class SepetRepository : ISepetRepository
    {
        ConcurrentDictionary<int, MusteriSepeti> _dictionary = new ConcurrentDictionary<int, MusteriSepeti>();

        public Task<MusteriSepeti> MusteriSepetiGetir(int musteriId)
        {
            if (!_dictionary.ContainsKey(musteriId))
            {
                var yeniSepet = new MusteriSepeti(musteriId);
                _dictionary.TryAdd(musteriId, yeniSepet);
                return Task.FromResult(yeniSepet);
            }

            _dictionary.TryGetValue(musteriId, out MusteriSepeti sepet);
            return Task.FromResult(sepet);
        }

        public async Task SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            var sepet = await MusteriSepetiGetir(musteriId);

            sepet.UrunEkle(urunId, adet);
        }
    }
}