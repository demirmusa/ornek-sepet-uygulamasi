using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Sepet.Application.Models;

namespace Sepet.Application
{
    // ürün bilgilerini getiren application service, ürün ile ilgili endpointten verileri çekip getirecek
    public class UrunApplicationService : IUrunApplicationService
    {
        private readonly IMemoryCache _cache;

        public UrunApplicationService(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string GetCacheKey(int urunId) => $"UrunBilgileri.{urunId}";

        public Task<UrunBilgileriDto> GetirUrunBilgileri(int urunId)
        {
            return _cache.GetOrCreateAsync(GetCacheKey(urunId), x =>
            {
                x.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return EndpointtenUrunBilgileriniGetir(urunId);
            });
        }

        //bu fonksiyonlar normalde asenkron olacakları için task türünde yazıldı
        private Task<UrunBilgileriDto> EndpointtenUrunBilgileriniGetir(int urunId)
        {
            var rnd = new Random().Next(100);
            return Task.FromResult(new UrunBilgileriDto()
            {
                UrunId = urunId,
                UrunUcreti = rnd,
                UrunAdi = $"Dummy data ürün adı{rnd}",
                UrunResimAdresi = $"Dummy data ürün resim adresi {rnd}"
            });
        }
    }
}