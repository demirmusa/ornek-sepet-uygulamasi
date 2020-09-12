using System.Threading.Tasks;
using Sepet.Application.Models;

namespace Sepet.Application
{
    public interface ISepetApplicationService
    {
        public Task<MusteriSepetiDto> MusteriSepetiGetir(int musteriId);

        public Task SepeteUrunEkle(int musteriId, int urunId, int adet);
    }
}