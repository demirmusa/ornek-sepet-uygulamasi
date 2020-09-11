using System.Collections.Concurrent;
using Sepet.Core;
using Sepet.Core.Models;

namespace Sepet.InMemory
{
    public class SepetRepository : ISepetRepository
    {
        ConcurrentDictionary<int, MusteriSepeti> _dictionary = new ConcurrentDictionary<int, MusteriSepeti>();

        public MusteriSepeti MusteriSepetiGetir(int musteriId)
        {
            if (!_dictionary.ContainsKey(musteriId))
            {
                var yeniSepet = new MusteriSepeti(musteriId);
                _dictionary.TryAdd(musteriId, yeniSepet);
                return yeniSepet;
            }

            _dictionary.TryGetValue(musteriId, out MusteriSepeti sepet);
            return sepet;
        }

        public void SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            var sepet = MusteriSepetiGetir(musteriId);

            sepet.UrunEkle(urunId, adet);
        }
    }
}