using System.ComponentModel.DataAnnotations;

namespace KARDEM.Models
{
    public class Kullanici
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50)]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100)]
        public string Sifre { get; set; }

        [Required]
        public string Rol { get; set; }

        public bool AktifMi { get; set; } = true;

        // Navigation property: Müdürün yetkili olduğu mahkemeler
        public List<MahkemeYetki> MahkemeYetkileri { get; set; }
    }
}