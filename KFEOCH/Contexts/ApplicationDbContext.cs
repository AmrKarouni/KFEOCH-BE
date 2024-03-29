﻿using KFEOCH.Models;
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
        public DbSet<Activity>? Activities { get; set; }
        public DbSet<OfficeEntity>? OfficeEntities { get; set; }
        public DbSet<OfficeLegalEntity>? OfficeLegalEntities { get; set; }
        public DbSet<Speciality>? Specialities { get; set; }
        public DbSet<OfficeOwnerSpeciality>? OfficeOwnerSpecialities { get; set; }
        public DbSet<OfficeOwner>? OfficeOwners { get; set; }
        public DbSet<OfficeStatus>? Statuses { get; set; }
        public DbSet<OfficeSpeciality>? OfficeSpecialities { get; set; }
        public DbSet<OfficeActivity>? OfficeActivities { get; set; }
        public DbSet<OfficeDocumentType>? OfficeDocumentTypes { get; set; }
        public DbSet<OwnerDocumentType>? OwnerDocumentTypes { get; set; }
        public DbSet<OwnerDocument>? OwnerDocuments { get; set; }
        public DbSet<ContactType>? ContactTypes { get; set; }
        public DbSet<OfficeContact>? OfficeContacts { get; set; }
        public DbSet<RequestType>? RequestTypes { get; set; }
        public DbSet<PaymentType>? PaymentTypes { get; set; }
        public DbSet<OfficePayment>? OfficePayments { get; set; }
        public DbSet<OfficeRequest>? OfficeRequests { get; set; }
        public DbSet<OfficeDocument>? OfficeDocuments { get; set; }
        public DbSet<Page>? Pages { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Section>? Sections { get; set; }
        public DbSet<OwnerPositionType>? OwnerPositionTypes { get; set; }
        public DbSet<Nationality>? Nationalities { get; set; }
        public DbSet<License>? Licenses { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<SiteMessage> SiteMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Write Fluent API configurations here

            //Property Configurations
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(e => new { e.LicenseId, e.OfficeTypeId })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => new { e.LicenseId, e.TypeId })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => new { e.Email })
                .IsUnique();
            });
            modelBuilder.Entity<Office>(entity =>
            {
                entity.HasIndex(e => new { e.Email })
                .IsUnique();
            });
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<CertificateEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<CertificateEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<CourseCategory>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<CourseCategory>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<Gender>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<Governorate>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<Governorate>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeLegalEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeLegalEntity>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic, e.ParentId })
                .IsUnique();
            });
            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish, e.ParentId })
                .IsUnique();
            });

            modelBuilder.Entity<OfficeOwnerSpeciality>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeOwnerSpeciality>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<OfficeStatus>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeStatus>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<OfficeSpeciality>(entity =>
            {
                entity.HasIndex(e => new { e.OfficeId, e.SpecialityId })
                .IsUnique();
            });

            modelBuilder.Entity<OfficeActivity>(entity =>
            {
                entity.HasIndex(e => new { e.OfficeId, e.ActivityId })
                .IsUnique();
            });

            modelBuilder.Entity<OfficeDocumentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OfficeDocumentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<OwnerDocumentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OwnerDocumentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<ContactType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic})
                .IsUnique();
            });
            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasIndex(e => new { e.HostUrl })
                .IsUnique();
            });

            modelBuilder.Entity<OwnerPositionType>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<OwnerPositionType>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<Nationality>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasIndex(e => new { e.NameArabic })
                .IsUnique();
            });
            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasIndex(e => new { e.NameEnglish })
                .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
