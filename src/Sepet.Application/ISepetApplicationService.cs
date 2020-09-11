using Sepet.Application.Models;

namespace Sepet.Application
{
    public interface ISepetApplicationService
    {
        public MusteriSepetiDto MusteriSepetiGetir(int musteriId);

        public void SepeteUrunEkle(int musteriId, int urunId, int adet);
    }
}