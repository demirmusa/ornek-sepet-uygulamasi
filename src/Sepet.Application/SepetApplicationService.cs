using System.Threading.Tasks;
using Sepet.Application.Models;
using Sepet.Core;
using Sepet.Core.Models;

namespace Sepet.Application
{
    public class SepetApplicationService : ISepetApplicationService
    {
        private readonly IStokKontrolAppService _stokKontrolAppService;
        private readonly ISepetRepository _sepetRepository;
        private readonly IUrunApplicationService _urunApplicationService;

        public SepetApplicationService(
            IStokKontrolAppService stokKontrolAppService,
            ISepetRepository sepetRepository,
            IUrunApplicationService urunApplicationService)
        {
            _stokKontrolAppService = stokKontrolAppService;
            _sepetRepository = sepetRepository;
            _urunApplicationService = urunApplicationService;
        }

        public async Task<MusteriSepetiDto> MusteriSepetiGetir(int musteriId)
        {
            var sepet = await _sepetRepository.MusteriSepetiGetir(musteriId);
            var dto = await SepetDtoOlustur(sepet);
            return dto;
        }

        public Task SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            throw new System.NotImplementedException();
        }

        private async Task<MusteriSepetiDto> SepetDtoOlustur(MusteriSepeti sepet)
        {
            if (sepet == null)
            {
                return null;
            }

            var sepetDto = new MusteriSepetiDto {MusteriId = sepet.MusteriId};

            foreach (var sepetItem in sepet.Items)
            {
                var sepetItemDto = new SepetItemDto
                {
                    Adet = sepetItem.Adet,
                    UrunBilgileri = await _urunApplicationService.GetirUrunBilgileri(sepetItem.UrunId)
                };

                sepetDto.Items.Add(sepetItemDto);
            }

            return sepetDto;
        }
    }
}