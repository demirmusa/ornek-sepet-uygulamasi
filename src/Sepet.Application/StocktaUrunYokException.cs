using System;
using Sepet.Core;

namespace Sepet.Application
{
    public class StocktaUrunYokException : UserFriendlyException
    {
        public readonly int UrunId;

        public StocktaUrunYokException(int urunId) : base($"{urunId} idli ürün stokta bulunmamaktadır.")
        {
            UrunId = urunId;
        }
    }
}