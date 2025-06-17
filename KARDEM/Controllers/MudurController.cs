using KARDEM.Context;
using KARDEM.Migrations;
using KARDEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace KARDEM.Controllers
{
    public class MudurController : Controller
    {
        private readonly MyContext _context;

        public MudurController(MyContext context)
        {
            _context = context;
        }

        public IActionResult Panel()
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            // ViewBag için adSoyad ve kullanıcı adı
            var kullanici = _context.Kullanicilar.Find(kullaniciId);
            ViewBag.AdSoyad = kullanici?.AdSoyad;
            ViewBag.KullaniciAdi = kullanici?.KullaniciAdi;

            // Yetkili mahkemeler
            var mahkemeler = _context.MahkemeYetkileri
                .Where(y => y.KullaniciId == kullaniciId)
                .Include(y => y.Mahkeme)
                .Select(y => y.Mahkeme)
                .ToList();

            ViewBag.Mahkemeler = mahkemeler;

            // Kesinleşme süresi yaklaşan dosyalar (önceki verdiğimiz LINQ çözümü)
            var bugun = DateTime.Today;

            var dosyalar = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .Where(d => mahkemeler.Select(m => m.Id).Contains(d.MahkemeId))
                .ToList();

            var yaklasan = dosyalar
                .Where(d =>
                    d.KesinlesmeTarihi != null &&
                    (d.KesinlesmeTarihi - bugun).TotalDays <= 7 &&
                    (d.KesinlesmeTarihi - bugun).TotalDays >= 0)
                .OrderBy(d => d.KesinlesmeTarihi)
                .Select(d => new
                {
                    d.EsasNo,
                    d.KararNo,
                    d.KesinlesmeTarihi,
                    MahkemeAdi = d.Mahkeme.Ad
                })
                .ToList();

            ViewBag.YaklasanDosyalar = yaklasan;

            return View();
        }

        public IActionResult MahkemeDosyalari(int id)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var dosyalar = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .Include(d => d.IstinafBilgileri)
                .Where(d => d.MahkemeId == id && d.EkleyenKullaniciId == kullaniciId)
                .ToList();

            ViewBag.Mahkeme = _context.Mahkemeler.Find(id);
            return View(dosyalar);
        }

        [HttpGet]
        public IActionResult DosyaEkle(int mahkemeId)
        {
            var mahkeme = _context.Mahkemeler.Find(mahkemeId);
            ViewBag.Mahkeme = mahkeme;
            return View();
        }

        [HttpPost]
        public IActionResult DosyaEkle(Dosya dosya)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var mahkeme = _context.Mahkemeler.FirstOrDefault(m => m.Id == dosya.MahkemeId);
            if (mahkeme == null) return NotFound();

            dosya.EkleyenKullaniciId = kullaniciId.Value;
            // Kesinleşme tarihi otomatik hesaplanıyor zaten modelde

            _context.Dosyalar.Add(dosya);
            _context.SaveChanges();

            return RedirectToAction("MahkemeDosyalari", new { id = dosya.MahkemeId });
        }

        [HttpGet]
        public IActionResult IstinafEkle(int dosyaId)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var dosya = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .Include(d => d.IstinafBilgileri)
                .FirstOrDefault(d => d.Id == dosyaId && d.EkleyenKullaniciId == kullaniciId);

            if (dosya == null) return NotFound();

            ViewBag.Dosya = dosya;
            return View(new IstinafBilgisi { DosyaId = dosya.Id });
        }

        [HttpPost]
        public IActionResult IstinafEkle(IstinafBilgisi model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var dosya = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .Include(d => d.IstinafBilgileri)
                .FirstOrDefault(d => d.Id == model.DosyaId && d.EkleyenKullaniciId == kullaniciId);

            if (dosya == null) return NotFound();

            // Aynı taraf için daha önce girilmiş mi?
            var ayniTarafVarMi = dosya.IstinafBilgileri
                .Any(i => i.IstinafEdenTaraf == model.IstinafEdenTaraf);

            if (ayniTarafVarMi)
            {
                ModelState.AddModelError("", $"{model.IstinafEdenTaraf} için istinaf bilgisi zaten girilmiş.");
                ViewBag.Dosya = dosya;
                return View(model);
            }

            if (ModelState.IsValid)
            {
                _context.IstinafBilgileri.Add(model);
                _context.SaveChanges();
                return RedirectToAction("MahkemeDosyalari", new { id = dosya.MahkemeId });
            }

            ViewBag.Dosya = dosya;
            return View(model);
        }

        [HttpGet]
        public IActionResult IstinafDuzenle(int istinafId)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var istinaf = _context.IstinafBilgileri
                .Include(i => i.Dosya)
                .ThenInclude(d => d.Mahkeme)
                .FirstOrDefault(i => i.Id == istinafId && i.Dosya.EkleyenKullaniciId == kullaniciId);

            if (istinaf == null) return NotFound();

            ViewBag.Dosya = istinaf.Dosya;
            return View(istinaf);
        }

        [HttpPost]
        public IActionResult IstinafDuzenle(IstinafBilgisi model)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var orjinal = _context.IstinafBilgileri
                .Include(i => i.Dosya)
                .FirstOrDefault(i => i.Id == model.Id && i.Dosya.EkleyenKullaniciId == kullaniciId);

            if (orjinal == null) return NotFound();

            // Güncelleme
            orjinal.IstinafEdenTaraf = model.IstinafEdenTaraf;
            orjinal.IstinafTarihi = model.IstinafTarihi;
            orjinal.DilekceTebligTarihi = model.DilekceTebligTarihi;

            orjinal.SureKontroluYapildiMi = model.SureKontroluYapildiMi;
            orjinal.DosyaGonderildiMi = model.DosyaGonderildiMi;

            _context.SaveChanges();

            return RedirectToAction("MahkemeDosyalari", new { id = orjinal.Dosya.MahkemeId });
        }

        [HttpPost]
        public IActionResult HarcEkle(Harc harc)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var dosya = _context.Dosyalar
                .Include(d => d.Mahkeme)
                .Include(d => d.HarcBilgileri)
                .FirstOrDefault(d => d.Id == harc.DosyaId && d.EkleyenKullaniciId == kullaniciId);

            if (ModelState.IsValid)
            {
                _context.HarcBilgileri.Add(harc);
                _context.SaveChanges();
                return RedirectToAction("HarcEkle");
            }
            ViewBag.Dosya = dosya;
            return View();
        }

        [HttpGet]
        public IActionResult HarcEkle(int dosyaId)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null) return RedirectToAction("Index", "Giris");

            var dosya = _context.Dosyalar.FirstOrDefault(d => d.Id == dosyaId && d.EkleyenKullaniciId == kullaniciId);
            if (dosya == null) return NotFound();

            ViewBag.Dosya = dosya;
            return View(new Harc { DosyaId = dosya.Id });
        }

        public IActionResult HarcListesi(string arama = "", string yazilma = "tum", string sirala = "")
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId == null)
                return RedirectToAction("Index", "Giris");

            var harclar = _context.HarcBilgileri
                .Include(h => h.Dosya)
                .Where(h => h.Dosya.EkleyenKullaniciId == kullaniciId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(arama))
            {
                harclar = harclar.Where(h =>
                    h.Dosya.EsasNo.Contains(arama) ||
                    h.Dosya.KararNo.Contains(arama));
            }

            ViewBag.AktifArama = arama;
            return View(harclar.ToList());
        }
    }
}