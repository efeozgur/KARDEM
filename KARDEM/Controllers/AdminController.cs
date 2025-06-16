using KARDEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using KARDEM.Context;

namespace KARDEM.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyContext _context;

        public AdminController(MyContext context)
        {
            _context = context;
        }

        private bool AdminMi()
        {
            return HttpContext.Session.GetString("Rol") == "Admin";
        }

        public IActionResult Index()
        {
            if (!AdminMi())
                return RedirectToAction("Index", "Giris");

            ViewBag.Kullanicilar = _context.Kullanicilar.Where(k => k.Rol == "Mudur").ToList();
            ViewBag.Mahkemeler = _context.Mahkemeler.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult KullaniciEkle(string kullaniciAdi, string sifre, string adsoyad)
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            var hashliSifre = Sifrele(sifre);
            var yeni = new Kullanici
            {
                KullaniciAdi = kullaniciAdi,
                Sifre = hashliSifre,
                AdSoyad = adsoyad,
                Rol = "Mudur",
                AktifMi = true
            };

            _context.Kullanicilar.Add(yeni);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult MahkemeEkle(string ad, int kesinlesme, int istinaf, int temyiz)
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            var mahkeme = new Mahkeme
            {
                Ad = ad,
                KesinlesmeSuresiGun = kesinlesme,
                IstinafSuresiGun = istinaf,
                TemyizSuresiGun = temyiz
            };

            _context.Mahkemeler.Add(mahkeme);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult YetkiAta(int kullaniciId, int mahkemeId)
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            bool zatenVar = _context.MahkemeYetkileri.Any(x => x.KullaniciId == kullaniciId && x.MahkemeId == mahkemeId);
            if (!zatenVar)
            {
                _context.MahkemeYetkileri.Add(new MahkemeYetki
                {
                    KullaniciId = kullaniciId,
                    MahkemeId = mahkemeId
                });
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private string Sifrele(string sifre)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(sifre);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public IActionResult YetkiListesi()
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            var yetkiler = _context.MahkemeYetkileri
                .Include(y => y.Kullanici)
                .Include(y => y.Mahkeme)
                .ToList();

            return View(yetkiler);
        }

        [HttpPost]
        public IActionResult YetkiSil(int id)
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            var yetki = _context.MahkemeYetkileri.Find(id);
            if (yetki != null)
            {
                _context.MahkemeYetkileri.Remove(yetki);
                _context.SaveChanges();
            }

            return RedirectToAction("YetkiListesi");
        }

        public IActionResult Dashboard()
        {
            if (!AdminMi()) return RedirectToAction("Index", "Giris");

            ViewBag.ToplamMudur = _context.Kullanicilar.Count(k => k.Rol == "Mudur");
            ViewBag.ToplamMahkeme = _context.Mahkemeler.Count();
            ViewBag.ToplamDosya = _context.Dosyalar.Count();
            ViewBag.ToplamYetki = _context.MahkemeYetkileri.Count();

            var bugun = DateTime.Today;
            ViewBag.BugunEklenenDosya = _context.Dosyalar.Count(d => d.KayitTarihi.Date == bugun);

            var populerMudur = _context.MahkemeYetkileri
                .GroupBy(y => y.Kullanici)
                .Select(grp => new { Kullanici = grp.Key, YetkiSayisi = grp.Count() })
                .OrderByDescending(x => x.YetkiSayisi)
                .FirstOrDefault();

            ViewBag.PopulerMudur = populerMudur?.Kullanici?.KullaniciAdi ?? "Yok";
            ViewBag.PopulerMudurSayisi = populerMudur?.YetkiSayisi ?? 0;

            ViewBag.SonGirisKullanici = HttpContext.Session.GetString("KullaniciAdi");
            ViewBag.SonGirisTarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            // Haftalık dosya girişi verisi (Chart için)
            var yediGun = Enumerable.Range(0, 7)
                .Select(i => bugun.AddDays(-i))
                .OrderBy(d => d)
                .Select(tarih => new
                {
                    Tarih = tarih.ToString("dd MMM"),
                    Adet = _context.Dosyalar.Count(d => d.KayitTarihi.Date == tarih)
                }).ToList();

            ViewBag.HaftalikVeri = Newtonsoft.Json.JsonConvert.SerializeObject(yediGun);

            var mahkemeDagilimi = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .GroupBy(d => d.Mahkeme.Ad)
                .Select(grp => new
                {
                    MahkemeAdi = grp.Key,
                    DosyaSayisi = grp.Count()
                }).ToList();

            ViewBag.MahkemeDagilimi = Newtonsoft.Json.JsonConvert.SerializeObject(mahkemeDagilimi);

            return View();
        }
    }
}