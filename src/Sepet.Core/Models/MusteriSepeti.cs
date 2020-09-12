using System;
using System.Collections.Generic;
using System.Linq;

namespace Sepet.Core.Models
{
    [Serializable]
    public class MusteriSepeti
    {
        public int MusteriId { get; set; }

        public List<SepetItem> Items { get; set; } = new List<SepetItem>();

        public MusteriSepeti(int musteriId)
        {
            MusteriId = musteriId;
        }

        public void UrunEkle(int urunId, int adet)
        {
            if (adet < 1)
            {
                throw new UserFriendlyException("Adet 1 den küçük olamaz");
            }

            if (Items.Any(x => x.UrunId == urunId))
            {
                Items.First(x => x.UrunId == urunId).Adet += adet;
            }
            else
            {
                Items.Add(new SepetItem(urunId, adet));
            }
        }
    }
}