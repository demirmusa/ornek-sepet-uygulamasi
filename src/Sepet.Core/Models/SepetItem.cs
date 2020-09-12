using System;

namespace Sepet.Core.Models
{
    [Serializable]
    public class SepetItem
    {
        public int UrunId { get; set; }

        public int Adet { get; set; }

        public SepetItem(int urunId, int adet)
        {
            UrunId = urunId;
            
            if (adet < 1)
            {
                throw new UserFriendlyException("Adet 1 den küçük olamaz");
            }

            Adet = adet;
        }
    }
}