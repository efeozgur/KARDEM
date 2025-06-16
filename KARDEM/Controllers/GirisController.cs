using KARDEM.Context;
using KARDEM.Migrations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace KARDEM.Controllers
{
    public class GirisController : Controller
    {
        private MyContext _context;

        public GirisController(MyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string kullaniciAdi, string sifre)
        {
            var hashliSifre = Sifrele(sifre);
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(p => p.KullaniciAdi == kullaniciAdi && /*p.Sifre == hashliSifre && */p.AktifMi);

            if (kullanici != null)
            {
                HttpContext.Session.SetInt32("KullaniciId", kullanici.Id);
                HttpContext.Session.SetString("Rol", kullanici.Rol);
                HttpContext.Session.SetString("AdSoyad", kullanici.AdSoyad);
                HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
                if (kullanici.Rol == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else
                    return RedirectToAction("Panel", "Mudur");
            }

            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private object Sifrele(string sifre)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(sifre);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}