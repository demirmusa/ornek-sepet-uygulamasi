using System.Collections.Generic;
using System.Linq;
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
        public virtual void Sepete_Yeni_Urun_Ekleme_Kontrolu()
        {
            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual void Sepete_Aynı_Urunden_Ekleme_Kontrolu()
        {
            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 1);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 1);

            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 2);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3);

            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 4);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 4);

            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 2);
            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun2Id, 6);

            SepettekiUrunAdediniKontrolEt(_testMusteriId, _testUrun1Id, 3); //bir ürün eklendiğinde diğer ürünün sayısında değişme olmamalı
        }

        [Fact]
        public virtual void Musteri_Sepeti_Getir_Kontrolu()
        {
            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun1Id, 3);
            _sepetRepository.SepeteUrunEkle(_testMusteriId, _testUrun2Id, 5);

            var sepet = _sepetRepository.MusteriSepetiGetir(_testMusteriId);

            sepet.ShouldNotBeNull();
            sepet.MusteriId.ShouldBe(_testMusteriId);
            sepet.Items.ShouldNotBeEmpty();

            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun1Id, 3);
            SepettekiUrunAdediniKontrolEt(sepet.Items, _testUrun2Id, 5);
        }

        protected void SepettekiUrunAdediniKontrolEt(int musteriId, int urunId, int olmasiGerekenAdet)
        {
            var sepet = _sepetRepository.MusteriSepetiGetir(musteriId);
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