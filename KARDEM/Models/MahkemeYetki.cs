namespace KARDEM.Models
{
    public class MahkemeYetki
    {
        public int Id { get; set; }

        public int KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }

        public int MahkemeId { get; set; }
        public Mahkeme Mahkeme { get; set; }
    }
}