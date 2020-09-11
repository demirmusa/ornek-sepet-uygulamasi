using Sepet.Core.Models;

namespace Sepet.Core
{
    public interface ISepetRepository
    {
        public MusteriSepeti MusteriSepetiGetir(int musteriId);

        public void SepeteUrunEkle(int musteriId, int urunId, int adet);
    }
}