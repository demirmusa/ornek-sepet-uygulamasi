using System.Collections.Generic;

namespace Sepet.Application.Models
{
    public class MusteriSepetiDto
    {
        public int MusteriId { get; set; }

        public List<SepetItemDto> Items { get; set; } = new List<SepetItemDto>();
    }
}