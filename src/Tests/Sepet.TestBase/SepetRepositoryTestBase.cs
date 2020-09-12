using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sepet.Core;
using Sepet.Core.Models;
using Shouldly;
using Xunit;

namespace Sepet.TestBase
{
    public abstract class SepetRepositoryTestBase<TFixture> : TestBaseClass<TFixture> where TFixture : FixtureBase
    {
        private ISepetRepository _sepetRepository;

        int _testMusteriId = 1;
        int _testUrun1Id = 1;
        int _testUrun2Id = 2;

        protected SepetRepositoryTestBase(TFixture fixture) : base(fixture)
        {
            _sepetRepository = GetService<ISepetRepository>();
        }

        [Fact]
        public virtual async Task Sepete_Yeni_Urun_Ekleme_Kontrolu()
        {
            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual async Task Sepete_Aynı_Urunden_Ekleme_Kontrolu()
        {
            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3);

            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 2);
            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 6);

            await SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual async Task Musteri_Sepeti_Getir_Kontrolu()
        {
            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 3);
            await _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 5);

            var sepet = await _sepetRepository.MusteriSepetiGetir(_testMusteriId);

            sepet.ShouldNotBeNull();
            sepet.MusteriId.ShouldBe(_testMusteriId);
            sepet.Items.ShouldNotBeEmpty();
            sepet.Items.Count.ShouldBe(2);

            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun1Id, 3);
            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun2Id, 5);
        }

        protected async Task SepettekiUrunAdediniKontrolEt(int musteriId, int urunId, int olmasiGerekenAdet)
        {
            var sepet = await _sepetRepository.MusteriSepetiGetir(musteriId);
            sepet.ShouldNotBeNull();

            SepettekiUrunAdediniKontrolEt(sepet.Items, urunId, olmasiGerekenAdet);
        }

        protected void SepettekiUrunAdediniKontrolEt(List<SepetItem> sepetItems, int urunId, int olmasiGerekenAdet)
        {
            sepetItems.ShouldNotBeNull();
            sepetItems.ShouldNotBeEmpty();

            var urun = sepetItems.SingleOrDefault(x => x.UrunId == urunId);
            urun.ShouldNotBeNull();
            urun.Adet.ShouldBe(olmasiGerekenAdet);
        }
    }
}