using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sepet.API.Models;
using Sepet.Application;
using Sepet.Application.Models;

namespace Sepet.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SepetController : ControllerBase
    {
        private readonly ISepetApplicationService _sepetApplicationService;

        public SepetController(ISepetApplicationService sepetApplicationService)
        {
            _sepetApplicationService = sepetApplicationService;
        }

        [HttpGet]
        public Task<MusteriSepetiDto> Get(int musteriId)
        {
            return _sepetApplicationService.MusteriSepetiGetir(musteriId);
        }

        [HttpPost]
        public Task Post(SepeteUrunEkleRequest input)
        {
            return _sepetApplicationService.SepeteUrunEkle(input.MusteriId, input.UrunId, input.Adet);
        }
    }
}