using System.Threading.Tasks;
using Sepet.Application.Models;
using Sepet.Core;
using Sepet.Core.Models;

namespace Sepet.Application
{
    public class SepetApplicationService : ISepetApplicationService
    {
        private readonly IStokKontrolApplicationService _stokKontrolApplicationService;
        private readonly ISepetRepository _sepetRepository;
        private readonly IUrunApplicationService _urunApplicationService;

        public SepetApplicationService(
            IStokKontrolApplicationService stokKontrolApplicationService,
            ISepetRepository sepetRepository,
            IUrunApplicationService urunApplicationService)
        {
            _stokKontrolApplicationService = stokKontrolApplicationService;
            _sepetRepository = sepetRepository;
            _urunApplicationService = urunApplicationService;
        }

        public async Task<MusteriSepetiDto> MusteriSepetiGetir(int musteriId)
        {
            var sepet = await _sepetRepository.MusteriSepetiGetir(musteriId);
            var dto = await SepetDtoOlustur(sepet);
            return dto;
        }

        public async Task SepeteUrunEkle(int musteriId, int urunId, int adet)
        {
            if (!_stokKontrolApplicationService.StoktaUrunVarmi(urunId))
            {
                throw new StocktaUrunYokException(urunId);
            }

            if (adet < 1)
            {
                throw new UserFriendlyException("Adet 1 den küçük olamaz");
            }

            await _sepetRepository.SepeteUrunEkle(musteriId, urunId, adet);
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