using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Sepet.Application.Models;
using Sepet.Core.Models;
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
            services.AddSingleton(Substitute.For<IStokKontrolAppService>());
            services.AddSepetInMemory();
            services.AddSepetApplication();
        }
    }

    public class SepetApplicationServiceTests : TestBaseClass<SepetApplicationServiceTestFixture>
    {
        private ISepetApplicationService _sepetApplicationService;
        private IStokKontrolAppService _stokKontrolAppServiceMock;

        int _testMusteriId = 1;
        int _testUrun1Id = 1;
        int _testUrun2Id = 2;

        public SepetApplicationServiceTests(SepetApplicationServiceTestFixture fixture) : base(fixture)
        {
            _sepetApplicationService = GetService<ISepetApplicationService>();
            _stokKontrolAppServiceMock = GetService<IStokKontrolAppService>();
        }

        [Fact]
        public virtual void Sepete_Yeni_Urun_Ekleme_Kontrolu()
        {
            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual void Sepete_Yeni_Urun_Ekleme_Stok_Kontrolu()
        {
            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(false);

            var exception = Should.Throw<StocktaUrunYokException>(() =>
                _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1)
            );

            exception.UrunId.ShouldBe(_testUrun1Id);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 0);

            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);
        }

        [Fact]
        public virtual void Sepete_Aynı_Urunden_Ekleme_Kontrolu()
        {
            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(false);

            var exception = Should.Throw<StocktaUrunYokException>(() =>
                _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2)
            );

            exception.UrunId.ShouldBe(_testUrun1Id);

            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 2);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 6);

            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual void Musteri_Sepeti_Getir_Kontrolu()
        {
            _stokKontrolAppServiceMock.StoktaUrunVarmi(Arg.Any<int>()).Returns(true);

            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 3);
            _sepetApplicationService.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 5);

            var sepet = _sepetApplicationService.MusteriSepetiGetir(_testMusteriId);

            sepet.ShouldNotBeNull();
            sepet.MusteriId.ShouldBe(_testMusteriId);
            sepet.Items.ShouldNotBeEmpty();
            sepet.Items.Count.ShouldBe(2);

            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun1Id, 3);
            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun2Id, 5);
        }

        protected void SepettekiUrunAdediniKontrolEt(int musteriId, int urunId, int olmasiGerekenAdet)
        {
            var sepet = _sepetApplicationService.MusteriSepetiGetir(musteriId);
            sepet.ShouldNotBeNull();

            SepettekiUrunAdediniKontrolEt(sepet.Items, urunId, olmasiGerekenAdet);
        }

        protected void SepettekiUrunAdediniKontrolEt(List<SepetItemDto> sepetItems, int urunId, int olmasiGerekenAdet)
        {
            sepetItems.ShouldNotBeNull();
            sepetItems.ShouldNotBeEmpty();

            var urun = sepetItems.SingleOrDefault(x => x.UrunId == urunId);
            urun.ShouldNotBeNull();
            urun.Adet.ShouldBe(olmasiGerekenAdet);
        }
    }
}