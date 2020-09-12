using Sepet.Core;
using Sepet.Core.Models;
using Shouldly;
using Xunit;

namespace Sepet.TestBase
{
    public class SepetItemTests
    {
        [Fact]
        public void Adet_Birden_Kücük_Olamaz_Kontrolu()
        {
            var exception = Should.Throw<UserFriendlyException>(() => new SepetItem(1, -1));

            exception.Message.ShouldContain("Adet 1 den küçük olamaz");
        }
    }
}