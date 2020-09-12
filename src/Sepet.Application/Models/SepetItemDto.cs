namespace Sepet.Application.Models
{
    public class SepetItemDto
    {
        public UrunBilgileriDto UrunBilgileri { get; set; } = new UrunBilgileriDto();
        public int Adet { get; set; }
    }
}