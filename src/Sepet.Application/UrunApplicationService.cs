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
            return _cache.GetOrCreateAsync<UrunBilgileriDto>(GetCacheKey(urunId), x => EndpointtenUrunBilgileriniGetir(urunId));
        }

        //bu fonksiyonlar normalde asenkron olacakları için task türünde yazıldı
        private Task<UrunBilgileriDto> EndpointtenUrunBilgileriniGetir(int urunId)
        {
            var rnd = new Random().Next(100);
            return Task.FromResult(new UrunBilgileriDto()
            {
                UrunId = urunId,
                UrunAdi = $"Dummy data ürün adı{rnd}",
                UrunUcreti = rnd,
                UrunResimAdresi = $"Dummy data ürün resim adresi {rnd}"
            });
        }
    }
}