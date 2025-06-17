using System;
using System.ComponentModel.DataAnnotations;

namespace KARDEM.Models
{
    public class Dosya
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Esas No zorunludur.")]
        [StringLength(50)]
        public string EsasNo { get; set; }

        [Required(ErrorMessage = "Karar No zorunludur.")]
        [StringLength(50)]
        public string KararNo { get; set; }

        [Required(ErrorMessage = "Karar tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime KararTarihi { get; set; }

        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kararın tebliğ tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime KararTebligTarihi { get; set; }

        public string? DosyaNotu { get; set; }

        public int MahkemeId { get; set; }
        public Mahkeme Mahkeme { get; set; }

        public int EkleyenKullaniciId { get; set; }
        public Kullanici EkleyenKullanici { get; set; }

        public List<IstinafBilgisi> IstinafBilgileri { get; set; }

        // Otomatik hesaplanan Kesinleşme Tarihi
        public DateTime KesinlesmeTarihi
        {
            get
            {
                var hesaplanan = KararTebligTarihi.AddDays(Mahkeme?.KesinlesmeSuresiGun ?? 0);
                var gun = hesaplanan.DayOfWeek;
                if (gun == DayOfWeek.Saturday)
                    return hesaplanan.AddDays(3); // Salı
                if (gun == DayOfWeek.Sunday)
                    return hesaplanan.AddDays(2); // Salı
                if (gun == DayOfWeek.Monday)
                    return hesaplanan.AddDays(1); // Salı
                return hesaplanan;
            }
        }
    }
}