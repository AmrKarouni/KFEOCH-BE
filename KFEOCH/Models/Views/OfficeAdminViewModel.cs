﻿namespace KFEOCH.Models.Views
{
    public class OfficeAdminViewModel
    {
        public OfficeAdminViewModel()
        { }
        public OfficeAdminViewModel(Office model)
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            TypeId = model.TypeId;
            TypeNameArabic = model.Type?.NameArabic;
            TypeNameEnglish = model.Type?.NameEnglish;
            LicenseId = model.LicenseId;
            RegistrationDate = model.RegistrationDate;
            EstablishmentDate = model.EstablishmentDate;
            LicenseEndDate = model.LicenseEndDate;
            MembershipEndDate = model.MembershipEndDate;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            FaxNumber = model.FaxNumber;
            MailBox = model.MailBox;
            PostalCode = model.PostalCode;
            Address = model.Address;
            AutoNumberOne = model.AutoNumberOne;
            AutoNumberTwo = model.AutoNumberTwo;
            AreaId = model.AreaId;
            AreaNameArabic = model.Area?.NameArabic;
            AreaNameEnglish = model.Area?.NameEnglish;
            GovernorateId = model.GovernorateId;
            GovernorateNameArabic = model.Governorate?.NameArabic;
            GovernorateNameEnglish = model.Governorate?.NameEnglish;
            CountryId = model.CountryId;
            CountryNameArabic = model.Country?.NameArabic;
            CountryNameEnglish = model.Country?.NameEnglish;
            EntityId = model.EntityId;
            EntityNameArabic = model.Entity?.NameArabic;
            EntityNameEnglish = model.Entity?.NameEnglish;
            LegalEntityId = model.LegalEntityId;
            LegalEntityNameArabic = model.LegalEntity?.NameArabic;
            LegalEntityNameEnglish = model.LegalEntity?.NameEnglish;
            IsVerified = model.IsVerified;
            IsActive = model.IsActive;
            LogoUrl = model.LogoUrl;
            ShowInHome = model.ShowInHome;
            RenewYears = model.RenewYears;
        }
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public int TypeId { get; set; }
        public string? TypeNameArabic { get; set; }
        public string? TypeNameEnglish { get; set; }
        public long LicenseId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? AutoNumberOne { get; set; }
        public string? AutoNumberTwo { get; set; }
        public int? AreaId { get; set; }
        public string? AreaNameArabic { get; set; }
        public string? AreaNameEnglish { get; set; }
        public int? GovernorateId { get; set; }
        public string? GovernorateNameArabic { get; set; }
        public string? GovernorateNameEnglish { get; set; }
        public int? CountryId { get; set; }
        public string? CountryNameArabic { get; set; }
        public string? CountryNameEnglish { get; set; }
        public int? EntityId { get; set; }
        public string? EntityNameArabic { get; set; }
        public string? EntityNameEnglish { get; set; }
        public int? LegalEntityId { get; set; }
        public string? LegalEntityNameArabic { get; set; }
        public string? LegalEntityNameEnglish { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public string? LogoUrl { get; set; }
        public bool? ShowInHome { get; set; }
        public int RenewYears { get; set; }
    }

    public class OfficeAdminExportViewModel
    {
        public OfficeAdminExportViewModel()
        { }
        public OfficeAdminExportViewModel(Office model)
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            TypeNameArabic = model.Type?.NameArabic;
            TypeNameEnglish = model.Type?.NameEnglish;
            LicenseId = model.LicenseId;
            RegistrationDate = model.RegistrationDate;
            EstablishmentDate = model.EstablishmentDate;
            LicenseEndDate = model.LicenseEndDate;
            MembershipEndDate = model.MembershipEndDate;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            FaxNumber = model.FaxNumber;
            MailBox = model.MailBox;
            PostalCode = model.PostalCode;
            Address = model.Address;
            AutoNumberOne = model.AutoNumberOne;
            AutoNumberTwo = model.AutoNumberTwo;
            AreaNameArabic = model.Area?.NameArabic;
            AreaNameEnglish = model.Area?.NameEnglish;
            GovernorateNameArabic = model.Governorate?.NameArabic;
            GovernorateNameEnglish = model.Governorate?.NameEnglish;
            CountryNameArabic = model.Country?.NameArabic;
            CountryNameEnglish = model.Country?.NameEnglish;
            EntityNameArabic = model.Entity?.NameArabic;
            EntityNameEnglish = model.Entity?.NameEnglish;
            LegalEntityNameArabic = model.LegalEntity?.NameArabic;
            LegalEntityNameEnglish = model.LegalEntity?.NameEnglish;
            IsVerified = model.IsVerified;
            IsActive = model.IsActive;
            ShowInHome = model.ShowInHome;
            RenewYears = model.RenewYears;
        }
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }

        public string? TypeNameArabic { get; set; }
        public string? TypeNameEnglish { get; set; }
        public long LicenseId { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? AutoNumberOne { get; set; }
        public string? AutoNumberTwo { get; set; }
        public string? AreaNameArabic { get; set; }
        public string? AreaNameEnglish { get; set; }
        public string? GovernorateNameArabic { get; set; }
        public string? GovernorateNameEnglish { get; set; }
        public string? CountryNameArabic { get; set; }
        public string? CountryNameEnglish { get; set; }
        public string? EntityNameArabic { get; set; }
        public string? EntityNameEnglish { get; set; }
        public string? LegalEntityNameArabic { get; set; }
        public string? LegalEntityNameEnglish { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public bool? ShowInHome { get; set; }
        public int RenewYears { get; set; }
    }

    public class OfficeGuestViewModel
    {
        public OfficeGuestViewModel()
        { }
        public OfficeGuestViewModel(Office model)
        {
            Id = model.Id;
            NameArabic = model.NameArabic;
            NameEnglish = model.NameEnglish;
            TypeId = model.TypeId;
            TypeNameArabic = model.Type?.NameArabic;
            TypeNameEnglish = model.Type?.NameEnglish;
            Email = model.Email;
            PhoneNumber = model.PhoneNumber;
            FaxNumber = model.FaxNumber;
            MailBox = model.MailBox;
            PostalCode = model.PostalCode;
            Address = model.Address;
            AutoNumberOne = model.AutoNumberOne;
            AutoNumberTwo = model.AutoNumberTwo;
            AreaId = model.AreaId;
            AreaNameArabic = model.Area?.NameArabic;
            AreaNameEnglish = model.Area?.NameEnglish;
            GovernorateId = model.GovernorateId;
            GovernorateNameArabic = model.Governorate?.NameArabic;
            GovernorateNameEnglish = model.Governorate?.NameEnglish;
            CountryId = model.CountryId;
            CountryNameArabic = model.Country?.NameArabic;
            CountryNameEnglish = model.Country?.NameEnglish;
            EntityId = model.EntityId;
            EntityNameArabic = model.Entity?.NameArabic;
            EntityNameEnglish = model.Entity?.NameEnglish;
            LegalEntityId = model.LegalEntityId;
            LegalEntityNameArabic = model.LegalEntity?.NameArabic;
            LegalEntityNameEnglish = model.LegalEntity?.NameEnglish;
            LogoUrl = model.LogoUrl;    
        }
        public int Id { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public int TypeId { get; set; }
        public string? TypeNameArabic { get; set; }
        public string? TypeNameEnglish { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FaxNumber { get; set; }
        public string? MailBox { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? AutoNumberOne { get; set; }
        public string? AutoNumberTwo { get; set; }
        public int? AreaId { get; set; }
        public string? AreaNameArabic { get; set; }
        public string? AreaNameEnglish { get; set; }
        public int? GovernorateId { get; set; }
        public string? GovernorateNameArabic { get; set; }
        public string? GovernorateNameEnglish { get; set; }
        public int? CountryId { get; set; }
        public string? CountryNameArabic { get; set; }
        public string? CountryNameEnglish { get; set; }
        public int? EntityId { get; set; }
        public string? EntityNameArabic { get; set; }
        public string? EntityNameEnglish { get; set; }
        public int? LegalEntityId { get; set; }
        public string? LegalEntityNameArabic { get; set; }
        public string? LegalEntityNameEnglish { get; set; }
        public string? LogoUrl { get; set; }

    }
}
