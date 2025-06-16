using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KARDEM.Models
{
    public class Mahkeme
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mahkeme adı zorunludur.")]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Kesinleşme süresi zorunludur.")]
        public int KesinlesmeSuresiGun { get; set; }

        [Required(ErrorMessage = "İstinaf süresi zorunludur.")]
        public int IstinafSuresiGun { get; set; }

        [Required(ErrorMessage = "Temyiz süresi zorunludur.")]
        public int TemyizSuresiGun { get; set; }

        // Navigation
        public List<MahkemeYetki> YetkiliKullanicilar { get; set; }

        public List<Dosya> Dosyalar { get; set; }
    }
}