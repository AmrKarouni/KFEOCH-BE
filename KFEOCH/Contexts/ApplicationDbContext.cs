using KFEOCH.Models;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Identity;
using KFEOCH.Models.Site;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KFEOCH.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<OfficeType>? OfficeTypes { get; set; }
        public DbSet<Office>? Offices { get; set; }
        public DbSet<Area>? Areas { get; set; }
        public DbSet<CertificateEntity>? CertificateEntities { get; set; }
        public DbSet<Country>? Countries { get; set; }
        public DbSet<CourseCategory>? CourseCategories { get; set; }
        public DbSet<Gender>? Genders { get; set; }
        public DbSet<Governorate>? Governorates { get; set; }
        public DbSet<OfficeActivity>? OfficeActivities { get; set; }
        public DbSet<OfficeEntity>? OfficeEntities { get; set; }
        public DbSet<OfficeLegalEntity>? OfficeLegalEntities { get; set; }
        public DbSet<OfficeSpeciality>? OfficeSpecialities { get; set; }
        public DbSet<OfficeStatus>? officeStatuses { get; set; }
        public DbSet<Article>? Articles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //Write Fluent API configurations here

            //Property Configurations
            modelBuilder.Entity<ApplicationUser>(entity => {
                entity.HasIndex(e => new { e.LicenseId, e.OfficeTypeId})
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity => {
                entity.HasIndex(e => new { e.LicenseId,e.TypeId })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity => {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity => {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
