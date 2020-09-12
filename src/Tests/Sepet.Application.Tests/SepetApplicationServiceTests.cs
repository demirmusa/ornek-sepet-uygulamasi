using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sepet.Application.Models;
using Sepet.InMemory;
using Sepet.TestBase;
using Shouldly;
using Xunit;

namespace Sepet.Application.Tests
{
    public class SepetApplicationServiceTestFixture : FixtureBase
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            services.AddSepetInMemory();
            services.AddSepetApplication();
            services.AddSingleton<IStokKontrolApplicationService>(Substitute.For<IStokKontrolApplicationService>());
            services.AddMemoryCache();
        }
    }

    public class SepetApplicationServiceTests : TestBaseClass<SepetApplicationServiceTestFixture>
    {
        private ISepetApplicationService _sepetApplicationService;
        private IStokKontrolApplicationService _stokKontrolApplicationServiceMock;

        int _testMusteriId = 1;
        int _testUrun1Id = 1;
        int _testUrun2Id = 2;

        public SepetApplicationServiceTests(SepetApplicationServiceTestFixture fixture) : base(fixture)
        {
            _sepetApplicationService = GetService<ISepetApplicationService>();
            _stokKontrolApplicationServiceMock = GetService<IStokKontrolApplicationService>();
        }

        [Fact]
        public virtual async Task Sepete_Yeni_Urun_Ekleme_Kontrolu()
        {
            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual async Task Sepete_Yeni_Urun_Ekleme_Stok_Kontrolu()
        {
            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(false);

            var exception = await Should.ThrowAsync<StocktaUrunYokException>(() =>
                _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1)
            );

            exception.UrunId.ShouldBe(_testUrun1Id);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 0);

            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);
        }

        [Fact]
        public virtual async Task Sepete_Aynı_Urunden_Ekleme_Kontrolu()
        {
            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(false);

            var exception = await Should.ThrowAsync<StocktaUrunYokException>(() =>
                _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2)
            );

            exception.UrunId.ShouldBe(_testUrun1Id);

            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 2);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 6);

            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual async Task Musteri_Sepeti_Getir_Kontrolu()
        {
            _stokKontrolApplicationServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 3);
            await _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 5);

            var sepet = await _sepetApplicationService.MusteriSepetiGetir(_testMusteriId);

            sepet.ShouldNotBeNull();
            sepet.MusteriId.ShouldBe(_testMusteriId);
            sepet.Items.ShouldNotBeEmpty();
            sepet.Items.Count.ShouldBe(2);

            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun1Id, 3);
            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun2Id, 5);
        }

        protected async Task SepettekiUrunAdediniKontrolEt(int musteriId, int urunId, int olmasiGerekenAdet)
        {
            var sepet = await _sepetApplicationService.MusteriSepetiGetir(musteriId);
            sepet.ShouldNotBeNull();

            SepettekiUrunAdediniKontrolEt(sepet.Items, urunId, olmasiGerekenAdet);
        }

        protected void SepettekiUrunAdediniKontrolEt(List<SepetItemDto> sepetItems, int urunId, int olmasiGerekenAdet)
        {
            sepetItems.ShouldNotBeNull();
            if (olmasiGerekenAdet == 0)
            {
                var urun = sepetItems.SingleOrDefault(x => x.UrunBilgileri.UrunId == urunId);
                if (urun == null)
                {
                    return;
                }

                urun.Adet.ShouldBeLessThan(1);
            }
            else
            {
                sepetItems.ShouldNotBeEmpty();

                var urun = sepetItems.SingleOrDefault(x => x.UrunBilgileri.UrunId == urunId);
                urun.ShouldNotBeNull();
                urun.Adet.ShouldBe(olmasiGerekenAdet);
            }
        }
    }
}