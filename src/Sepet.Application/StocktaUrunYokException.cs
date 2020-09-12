using System;

namespace Sepet.Application
{
    public class StocktaUrunYokException : Exception
    {
        public readonly int UrunId;

        public StocktaUrunYokException(int urunId) : base($"{urunId} idli ürün stokta bulunmamaktadır.")
        {
            UrunId = urunId;
        }
    }
}