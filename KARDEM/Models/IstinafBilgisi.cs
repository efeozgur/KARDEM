using System;
using System.ComponentModel.DataAnnotations;

namespace KARDEM.Models
{
    public class IstinafBilgisi
    {
        public int Id { get; set; }

        [Required]
        public int DosyaId { get; set; }

        public Dosya? Dosya { get; set; }

        [Required]
        public string IstinafEdenTaraf { get; set; }

        [Required(ErrorMessage = "İstinaf tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime IstinafTarihi { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DilekceTebligTarihi { get; set; }

        public bool SureKontroluYapildiMi { get; set; } = false;

        public bool DosyaGonderildiMi { get; set; } = false;
    }
}