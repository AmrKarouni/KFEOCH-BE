using KFEOCH.Constants;
using KFEOCH.Models.Dictionaries;
using KFEOCH.Models.Identity;
using KFEOCH.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KFEOCH.Contexts
{
    public class ApplicationDbContextSeed
    {
        public async Task<bool> SeedEssentialsAsync(UserManager<ApplicationUser> userManager,
                                              RoleManager<IdentityRole> roleManager,
                                              IDictionaryService dictionaryService)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.SuperUser.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Administrator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Office.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.OfficeManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.DictionaryManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.SiteManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.ReportManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.BillingManager.ToString()));

            //Seed Office Types
            await dictionaryService.PostOfficeTypeAsync(new OfficeType
            {
                Id = 0,
                NameArabic = "مشرف",
                NameEnglish = "Admin",
                DescriptionArabic = "مشرف",
                DescriptionEnglish = "Admin",
                IsLocal = false,
                IsAdmin = true,
                IsDeleted = false,
            });
            await dictionaryService.PostOfficeTypeAsync(new OfficeType
            {
                Id = 0,
                NameArabic = "المكاتب المحلية",
                NameEnglish = "Local Offices",
                DescriptionArabic = "المكاتب المحلية",
                DescriptionEnglish = "Local Offices",
                IsLocal = true,
                IsAdmin = false,
                IsDeleted = false
            });
            await dictionaryService.PostOfficeTypeAsync(new OfficeType
            {
                Id = 0,
                NameArabic = "المكاتب الأجنبية",
                NameEnglish = "Foreign Offices",
                DescriptionArabic = "المكاتب الأجنبية",
                DescriptionEnglish = "Foreign Offices",
                IsLocal = false,
                IsAdmin = false,
                IsDeleted = false
            });
            await dictionaryService.PostOfficeTypeAsync(new OfficeType
            {
                Id = 0,
                NameArabic = "المكاتب المتخصصة",
                NameEnglish = "Specialized Offices",
                DescriptionArabic = "المكاتب المتخصصة",
                DescriptionEnglish = "Specialized Offices",
                IsLocal = false,
                IsAdmin = false,
                IsDeleted = false
            });

            //Seed Request Types
            await dictionaryService.PostRequestTypeAsync(new RequestType
            {
                Id = 0,
                NameArabic = "شهادة العضوية",
                NameEnglish = "Membership Certificate",
                DescriptionArabic = "شهادة العضوية",
                DescriptionEnglish = "Membership Certificate",
                Amount = 15,
                IsDeleted = false
            });
            await dictionaryService.PostRequestTypeAsync(new RequestType
            {
                Id = 0,
                NameArabic = "شهادة تحديث بيانات",
                NameEnglish = "Information Update Certificate",
                DescriptionArabic = "شهادة تحديث بيانات",
                DescriptionEnglish = "Information Update Certificate",
                Amount = 15,
                IsDeleted = false
            });

            //Seed Payment Types

            await dictionaryService.PostPaymentTypeAsync(new PaymentType
            {
                Id = 0,
                NameArabic = "نقدي",
                NameEnglish = "Cash On Hand",
                DescriptionArabic = "نقدي",
                DescriptionEnglish = "Cash On Hand",
                IsDeleted = false
            });
            await dictionaryService.PostPaymentTypeAsync(new PaymentType
            {
                Id = 0,
                NameArabic = "تحويل بنكي",
                NameEnglish = "Bank Gateway",
                DescriptionArabic = "تحويل بنكي",
                DescriptionEnglish = "Bank Gateway",
                IsDeleted = false
            });

            //Seed Countries Dictionary
            await dictionaryService.PostCountryAsync(new Country
            {
                Id = 0,
                NameArabic = "الكويت",
                NameEnglish = "Kuwait",
                DescriptionArabic = "الكويت",
                DescriptionEnglish = "Kuwait",
                IsDeleted = false,
            });

            //Seed Office Entity Dictionary
            await dictionaryService.PostOfficeEntityAsync(new OfficeEntity
            {
                Id = 0,
                NameArabic = "مكتب هندسي",
                NameEnglish = "Engineering Office",
                DescriptionArabic = "مكتب هندسي",
                DescriptionEnglish = "Engineering Office",
                YearlyFees = 100.0,
                IsDeleted = false
            });
            await dictionaryService.PostOfficeEntityAsync(new OfficeEntity
            {
                Id = 0,
                NameArabic = "دار استشارية",
                NameEnglish = "Consulting House",
                DescriptionArabic = "دار استشارية",
                DescriptionEnglish = "Consulting House",
                YearlyFees = 0.0,
                IsDeleted = false
            });
            await dictionaryService.PostOfficeEntityAsync(new OfficeEntity
            {
                Id = 0,
                NameArabic = "شركة مهنية هندسية",
                NameEnglish = "Engineering Professional Company",
                DescriptionArabic = "شركة مهنية هندسية",
                DescriptionEnglish = "Engineering Professional Company",
                YearlyFees = 0.0,
                IsDeleted = false
            });

            //Seed Default User
            var defaultUser = new ApplicationUser { UserName = Authorization.default_username,
                                                    Email = Authorization.default_email,
                                                    EmailConfirmed = true,
                                                    PhoneNumberConfirmed = true,
                                                    IsPasswordChanged = true,
                                                    OfficeTypeId = 1,
                                                    IsActive = true };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                await userManager.CreateAsync(defaultUser, Authorization.default_password);
                await userManager.AddToRoleAsync(defaultUser, Authorization.superuser_role.ToString());
            }

            return true;
        }
    }
}
