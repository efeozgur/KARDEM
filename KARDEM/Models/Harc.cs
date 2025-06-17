using System.ComponentModel.DataAnnotations;

namespace KARDEM.Models
{
    public class Harc
    {
        public int Id { get; set; }
        public HarcTuru HarcTuru { get; set; }
        public Taraf HarcinTarafi { get; set; }
        public decimal HarcMiktari { get; set; }
        public DateTime HarcYazilacakTarih { get; set; }
        public bool YazilmaDurumu { get; set; }

        public int DosyaId { get; set; }
        public Dosya? Dosya { get; set; }
    }

    public enum Taraf
    {
        [Display(Name = "Davacı")]
        Davaci = 0,

        [Display(Name = "Davalı")]
        Davali = 1
    }

    public enum HarcTuru
    {
        [Display(Name = "Bakiye Karar Harcı")]
        BakiyeKararHarci = 0,

        [Display(Name = "Onama Harcı")]
        OnamaHarci = 1,

        [Display(Name = "Arabuluculuk Ücreti")]
        ArabulucuUcreti = 2,

        [Display(Name = "Karar Düzeltme Para Cezası")]
        KararDuzeltmeParaCezasi = 3,
    }
}