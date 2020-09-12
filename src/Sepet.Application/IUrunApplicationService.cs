using System.Threading.Tasks;
using Sepet.Application.Models;

namespace Sepet.Application
{
    public interface IUrunApplicationService
    {
        Task<UrunBilgileriDto> GetirUrunBilgileri(int urunId);
    }
}