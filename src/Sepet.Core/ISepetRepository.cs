using System.Threading.Tasks;
using Sepet.Core.Models;

namespace Sepet.Core
{
    public interface ISepetRepository
    {
        public Task<MusteriSepeti> MusteriSepetiGetir(int musteriId);

        public Task SepeteUrunEkle(int musteriId, int urunId, int adet);
    }
}