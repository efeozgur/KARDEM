using KARDEM.Models;
using Microsoft.EntityFrameworkCore;

namespace KARDEM.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options) { }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Mahkeme> Mahkemeler { get; set; }
        public DbSet<MahkemeYetki> MahkemeYetkileri { get; set; }
        public DbSet<Dosya> Dosyalar { get; set; }
        public DbSet<IstinafBilgisi> IstinafBilgileri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // KesinlesmeTarihi hesaplanan bir alan olduğu için EF'e kaydetme diyoruz
            modelBuilder.Entity<Dosya>()
                .Ignore(d => d.KesinlesmeTarihi);

            // Kullanici ↔ MahkemeYetki (1 - Çok)
            modelBuilder.Entity<MahkemeYetki>()
                .HasOne(my => my.Kullanici)
                .WithMany(k => k.MahkemeYetkileri)
                .HasForeignKey(my => my.KullaniciId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mahkeme ↔ MahkemeYetki (1 - Çok)
            modelBuilder.Entity<MahkemeYetki>()
                .HasOne(my => my.Mahkeme)
                .WithMany(m => m.YetkiliKullanicilar)
                .HasForeignKey(my => my.MahkemeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Mahkeme ↔ Dosya (1 - Çok)
            modelBuilder.Entity<Dosya>()
                .HasOne(d => d.Mahkeme)
                .WithMany(m => m.Dosyalar)
                .HasForeignKey(d => d.MahkemeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Kullanici ↔ Dosya (1 - Çok)
            modelBuilder.Entity<Dosya>()
                .HasOne(d => d.EkleyenKullanici)
                .WithMany()
                .HasForeignKey(d => d.EkleyenKullaniciId)
                .OnDelete(DeleteBehavior.NoAction); // Müdür silinse bile dosyalar silinmesin
        }
    }
}