// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BvDOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the BvDOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.BvD.Vocabularies
{
    /// <summary>The BvD organization vocabulary</summary>
    /// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
    public class BvDOrganizationVocabulary : SimpleVocabulary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BvDOrganizationVocabulary"/> class.
        /// </summary>
        public BvDOrganizationVocabulary()
        {
            this.VocabularyName = "BvD Organization";
            this.KeyPrefix      = "BvD.organization";
            this.KeySeparator   = ".";
            this.Grouping       = CluedIn.Core.Data.EntityType.Organization;

            this.AddGroup("Metadata", group => 
            {
                this.AddressLine1 = group.Add(new VocabularyKey("addressLine1"));
                this.AddressLine2 = group.Add(new VocabularyKey("addressLine2"));
                this.AddressLine3 = group.Add(new VocabularyKey("addressLine3"));
                this.AddressLine4 = group.Add(new VocabularyKey("addressLine4"));
                this.Postcode = group.Add(new VocabularyKey("postcode"));
                this.City = group.Add(new VocabularyKey("city"));
                this.CityStandardized = group.Add(new VocabularyKey("cityStandardized"));
                this.Country = group.Add(new VocabularyKey("country"));
                this.CountryIsoCode = group.Add(new VocabularyKey("countryIsoCode"));
                this.CountryRegion = group.Add(new VocabularyKey("countryRegion"));
                this.CountryRegionType = group.Add(new VocabularyKey("countryRegionType"));
                this.Nuts1 = group.Add(new VocabularyKey("nuts1"));
                this.Nuts2 = group.Add(new VocabularyKey("nuts2"));
                this.Nuts3 = group.Add(new VocabularyKey("nuts3"));
                this.WorldRegion = group.Add(new VocabularyKey("worldRegion"));
                this.UsState = group.Add(new VocabularyKey("usState"));
                this.AddressType = group.Add(new VocabularyKey("addressType"));
                this.PhoneNumber = group.Add(new VocabularyKey("phoneNumber"));
                this.FaxNumber = group.Add(new VocabularyKey("faxNumber"));
                this.Domain = group.Add(new VocabularyKey("domain"));
                this.Website = group.Add(new VocabularyKey("website"));
                this.Email = group.Add(new VocabularyKey("email"));
                this.Building = group.Add(new VocabularyKey("building"));
                this.Street = group.Add(new VocabularyKey("street"));
                this.StreetNumber = group.Add(new VocabularyKey("streetNumber"));
                this.StreetNumberExtension = group.Add(new VocabularyKey("streetNumberExtension"));
                this.StreetAndStreetNumber = group.Add(new VocabularyKey("streetAndStreetNumber"));
                this.StreetSupplement = group.Add(new VocabularyKey("streetSupplement"));
                this.PoBox = group.Add(new VocabularyKey("poBox"));
                this.MinorTown = group.Add(new VocabularyKey("minorTown"));
                this.AddressLine1Additional = group.Add(new VocabularyKey("addressLine1Additional"));
                this.AddressLine2Additional = group.Add(new VocabularyKey("addressLine2Additional"));
                this.AddressLine3Additional = group.Add(new VocabularyKey("addressLine3Additional"));
                this.AddressLine4Additional = group.Add(new VocabularyKey("addressLine4Additional"));
                this.PostcodeAdditional = group.Add(new VocabularyKey("postcodeAdditional"));
                this.CityAdditional = group.Add(new VocabularyKey("cityAdditional"));
                this.CityStandardizedAdditional = group.Add(new VocabularyKey("cityStandardizedAdditional"));
                this.CountryAdditional = group.Add(new VocabularyKey("countryAdditional"));
                this.CountryIsoCodeAdditional = group.Add(new VocabularyKey("countryIsoCodeAdditional"));
                this.LatitudeAdditional = group.Add(new VocabularyKey("latitudeAdditional"));
                this.LongitudeAdditional = group.Add(new VocabularyKey("longitudeAdditional"));
                this.CountryRegionAdditional = group.Add(new VocabularyKey("countryRegionAdditional"));
                this.CountryRegionTypeAdditional = group.Add(new VocabularyKey("countryRegionTypeAdditional"));
                this.AddressTypeAdditional = group.Add(new VocabularyKey("addressTypeAdditional"));
                this.PhoneNumberAdditional = group.Add(new VocabularyKey("phoneNumberAdditional"));
                this.FaxNumberAdditional = group.Add(new VocabularyKey("faxNumberAdditional"));
                this.BuildingAdditional = group.Add(new VocabularyKey("buildingAdditional"));
                this.StreetAdditional = group.Add(new VocabularyKey("streetAdditional"));
                this.StreetNumberAdditional = group.Add(new VocabularyKey("streetNumberAdditional"));
                this.StreetNumberExtensionAdditional = group.Add(new VocabularyKey("streetNumberExtensionAdditional"));
                this.StreetAndStreetNumberAdditional = group.Add(new VocabularyKey("streetAndStreetNumberAdditional"));
                this.StreetSupplementAdditional = group.Add(new VocabularyKey("streetSupplementAdditional"));
                this.PoBoxAdditional = group.Add(new VocabularyKey("poBoxAdditional"));
                this.MinorTownAdditional = group.Add(new VocabularyKey("minorTownAdditional"));
                this.TradeDescriptionEn = group.Add(new VocabularyKey("tradeDescriptionEn"));
                this.TradeDescriptionOriginal = group.Add(new VocabularyKey("tradeDescriptionOriginal"));
                this.TradeDescriptionLanguage = group.Add(new VocabularyKey("tradeDescriptionLanguage"));
                this.ProductsServices = group.Add(new VocabularyKey("productsServices"));
                this.BvdSectorCoreLabel = group.Add(new VocabularyKey("bvdSectorCoreLabel"));
                this.IndustryClassification = group.Add(new VocabularyKey("industryClassification"));
                this.IndustryPrimaryCode = group.Add(new VocabularyKey("industryPrimaryCode"));
                this.IndustryPrimaryLabel = group.Add(new VocabularyKey("industryPrimaryLabel"));
                this.IndustrySecondaryCode = group.Add(new VocabularyKey("industrySecondaryCode"));
                this.IndustrySecondaryLabel = group.Add(new VocabularyKey("industrySecondaryLabel"));
                this.Nace2MainSection = group.Add(new VocabularyKey("nace2MainSection"));
                this.Nace2CoreCode = group.Add(new VocabularyKey("nace2CoreCode"));
                this.Nace2CoreLabel = group.Add(new VocabularyKey("nace2CoreLabel"));
                this.Nace2PrimaryCode = group.Add(new VocabularyKey("nace2PrimaryCode"));
                this.Nace2PrimaryLabel = group.Add(new VocabularyKey("nace2PrimaryLabel"));
                this.Nace2SecondaryCode = group.Add(new VocabularyKey("nace2SecondaryCode"));
                this.Nace2SecondaryLabel = group.Add(new VocabularyKey("nace2SecondaryLabel"));
                this.Naics2017CoreCode = group.Add(new VocabularyKey("naics2017CoreCode"));
                this.Naics2017CoreLabel = group.Add(new VocabularyKey("naics2017CoreLabel"));
                this.Naics2017PrimaryCode = group.Add(new VocabularyKey("naics2017PrimaryCode"));
                this.Naics2017PrimaryLabel = group.Add(new VocabularyKey("naics2017PrimaryLabel"));
                this.Naics2017SecondaryCode = group.Add(new VocabularyKey("naics2017SecondaryCode"));
                this.Naics2017SecondaryLabel = group.Add(new VocabularyKey("naics2017SecondaryLabel"));
                this.UssicCoreCode = group.Add(new VocabularyKey("ussicCoreCode"));
                this.UssicCoreLabel = group.Add(new VocabularyKey("ussicCoreLabel"));
                this.UssicPrimaryCode = group.Add(new VocabularyKey("ussicPrimaryCode"));
                this.UssicPrimaryLabel = group.Add(new VocabularyKey("ussicPrimaryLabel"));
                this.UssicSecondaryCode = group.Add(new VocabularyKey("ussicSecondaryCode"));
                this.UssicSecondaryLabel = group.Add(new VocabularyKey("ussicSecondaryLabel"));
                this.BvdIdNumber = group.Add(new VocabularyKey("bvdIdNumber"));
                this.BvdAccountNumber = group.Add(new VocabularyKey("bvdAccountNumber"));
                this.OrbisId = group.Add(new VocabularyKey("orbisId"));
                this.NationalId = group.Add(new VocabularyKey("nationalId"));
                this.NationalIdLabel = group.Add(new VocabularyKey("nationalIdLabel"));
                this.NationalIdType = group.Add(new VocabularyKey("nationalIdType"));
                this.TradeRegisterNumber = group.Add(new VocabularyKey("tradeRegisterNumber"));
                this.VatNumber = group.Add(new VocabularyKey("vatNumber"));
                this.EuropeanVatNumber = group.Add(new VocabularyKey("europeanVatNumber"));
                this.Lei = group.Add(new VocabularyKey("lei"));
                this.Giin = group.Add(new VocabularyKey("giin"));
                this.StatisticalNumber = group.Add(new VocabularyKey("statisticalNumber"));
                this.CompanyIdNumber = group.Add(new VocabularyKey("companyIdNumber"));
                this.InformationProviderId = group.Add(new VocabularyKey("informationProviderId"));
                this.InformationProviderIdLabel = group.Add(new VocabularyKey("informationProviderIdLabel"));
                this.Ticker = group.Add(new VocabularyKey("ticker"));
                this.Isin = group.Add(new VocabularyKey("isin"));
                this.LeiStatus = group.Add(new VocabularyKey("leiStatus"));
                this.LeiFirstAssignmentDate = group.Add(new VocabularyKey("leiFirstAssignmentDate"));
                this.LeiAnnualRenewalDate = group.Add(new VocabularyKey("leiAnnualRenewalDate"));
                this.LeiManagingLocalOfficeUnitStr = group.Add(new VocabularyKey("leiManagingLocalOfficeUnitStr"));
                this.ReleaseDate = group.Add(new VocabularyKey("releaseDate"));
                this.InformationProvider = group.Add(new VocabularyKey("informationProvider"));
                this.Name = group.Add(new VocabularyKey("name"));
                this.PreviousName = group.Add(new VocabularyKey("previousName"));
                this.PreviousNameDate = group.Add(new VocabularyKey("previousNameDate"));
                this.AkaName = group.Add(new VocabularyKey("akaName"));
                this.Status = group.Add(new VocabularyKey("status"));
                this.StatusDate = group.Add(new VocabularyKey("statusDate"));
                this.StatusChangeDate = group.Add(new VocabularyKey("statusChangeDate"));
                this.LocalStatus = group.Add(new VocabularyKey("localStatus"));
                this.LocalStatusDate = group.Add(new VocabularyKey("localStatusDate"));
                this.LocalStatusChangeDate = group.Add(new VocabularyKey("localStatusChangeDate"));
                this.StandardisedLegalForm = group.Add(new VocabularyKey("standardisedLegalForm"));
                this.NationalLegalForm = group.Add(new VocabularyKey("nationalLegalForm"));
                this.IncorporationDate = group.Add(new VocabularyKey("incorporationDate"));
                this.IncorporationState = group.Add(new VocabularyKey("incorporationState"));
                this.EntityType = group.Add(new VocabularyKey("entityType"));
                this.IcijDataPresenceIndicator = group.Add(new VocabularyKey("icijDataPresenceIndicator"));
                this.ConsolidationCode = group.Add(new VocabularyKey("consolidationCode"));
                this.ClosingDateLastAnnualAccounts = group.Add(new VocabularyKey("closingDateLastAnnualAccounts"));
                this.YearLastAccounts = group.Add(new VocabularyKey("yearLastAccounts"));
                this.LimitedFinancialIndicator = group.Add(new VocabularyKey("limitedFinancialIndicator"));
                this.NoRecentFinancialIndicator = group.Add(new VocabularyKey("noRecentFinancialIndicator"));
                this.NumberYears = group.Add(new VocabularyKey("numberYears"));



            });

        }

        public VocabularyKey AddressLine1 { get; protected set; }
        public VocabularyKey AddressLine2 { get; protected set; }
        public VocabularyKey AddressLine3 { get; protected set; }
        public VocabularyKey AddressLine4 { get; protected set; }
        public VocabularyKey Postcode { get; protected set; }
        public VocabularyKey City { get; protected set; }
        public VocabularyKey CityStandardized { get; protected set; }
        public VocabularyKey Country { get; protected set; }
        public VocabularyKey CountryIsoCode { get; protected set; }
        public VocabularyKey CountryRegion { get; protected set; }
        public VocabularyKey CountryRegionType { get; protected set; }
        public VocabularyKey Nuts1 { get; protected set; }
        public VocabularyKey Nuts2 { get; protected set; }
        public VocabularyKey Nuts3 { get; protected set; }
        public VocabularyKey WorldRegion { get; protected set; }
        public VocabularyKey UsState { get; protected set; }
        public VocabularyKey AddressType { get; protected set; }
        public VocabularyKey PhoneNumber { get; protected set; }
        public VocabularyKey FaxNumber { get; protected set; }
        public VocabularyKey Domain { get; protected set; }
        public VocabularyKey Website { get; protected set; }
        public VocabularyKey Email { get; protected set; }
        public VocabularyKey Building { get; protected set; }
        public VocabularyKey Street { get; protected set; }
        public VocabularyKey StreetNumber { get; protected set; }
        public VocabularyKey StreetNumberExtension { get; protected set; }
        public VocabularyKey StreetAndStreetNumber { get; protected set; }
        public VocabularyKey StreetSupplement { get; protected set; }
        public VocabularyKey PoBox { get; protected set; }
        public VocabularyKey MinorTown { get; protected set; }
        public VocabularyKey AddressLine1Additional { get; protected set; }
        public VocabularyKey AddressLine2Additional { get; protected set; }
        public VocabularyKey AddressLine3Additional { get; protected set; }
        public VocabularyKey AddressLine4Additional { get; protected set; }
        public VocabularyKey PostcodeAdditional { get; protected set; }
        public VocabularyKey CityAdditional { get; protected set; }
        public VocabularyKey CityStandardizedAdditional { get; protected set; }
        public VocabularyKey CountryAdditional { get; protected set; }
        public VocabularyKey CountryIsoCodeAdditional { get; protected set; }
        public VocabularyKey LatitudeAdditional { get; protected set; }
        public VocabularyKey LongitudeAdditional { get; protected set; }
        public VocabularyKey CountryRegionAdditional { get; protected set; }
        public VocabularyKey CountryRegionTypeAdditional { get; protected set; }
        public VocabularyKey AddressTypeAdditional { get; protected set; }
        public VocabularyKey PhoneNumberAdditional { get; protected set; }
        public VocabularyKey FaxNumberAdditional { get; protected set; }
        public VocabularyKey BuildingAdditional { get; protected set; }
        public VocabularyKey StreetAdditional { get; protected set; }
        public VocabularyKey StreetNumberAdditional { get; protected set; }
        public VocabularyKey StreetNumberExtensionAdditional { get; protected set; }
        public VocabularyKey StreetAndStreetNumberAdditional { get; protected set; }
        public VocabularyKey StreetSupplementAdditional { get; protected set; }
        public VocabularyKey PoBoxAdditional { get; protected set; }
        public VocabularyKey MinorTownAdditional { get; protected set; }
        public VocabularyKey TradeDescriptionEn { get; protected set; }
        public VocabularyKey TradeDescriptionOriginal { get; protected set; }
        public VocabularyKey TradeDescriptionLanguage { get; protected set; }
        public VocabularyKey ProductsServices { get; protected set; }
        public VocabularyKey BvdSectorCoreLabel { get; protected set; }
        public VocabularyKey IndustryClassification { get; protected set; }
        public VocabularyKey IndustryPrimaryCode { get; protected set; }
        public VocabularyKey IndustryPrimaryLabel { get; protected set; }
        public VocabularyKey IndustrySecondaryCode { get; protected set; }
        public VocabularyKey IndustrySecondaryLabel { get; protected set; }
        public VocabularyKey Nace2MainSection { get; protected set; }
        public VocabularyKey Nace2CoreCode { get; protected set; }
        public VocabularyKey Nace2CoreLabel { get; protected set; }
        public VocabularyKey Nace2PrimaryCode { get; protected set; }
        public VocabularyKey Nace2PrimaryLabel { get; protected set; }
        public VocabularyKey Nace2SecondaryCode { get; protected set; }
        public VocabularyKey Nace2SecondaryLabel { get; protected set; }
        public VocabularyKey Naics2017CoreCode { get; protected set; }
        public VocabularyKey Naics2017CoreLabel { get; protected set; }
        public VocabularyKey Naics2017PrimaryCode { get; protected set; }
        public VocabularyKey Naics2017PrimaryLabel { get; protected set; }
        public VocabularyKey Naics2017SecondaryCode { get; protected set; }
        public VocabularyKey Naics2017SecondaryLabel { get; protected set; }
        public VocabularyKey UssicCoreCode { get; protected set; }
        public VocabularyKey UssicCoreLabel { get; protected set; }
        public VocabularyKey UssicPrimaryCode { get; protected set; }
        public VocabularyKey UssicPrimaryLabel { get; protected set; }
        public VocabularyKey UssicSecondaryCode { get; protected set; }
        public VocabularyKey UssicSecondaryLabel { get; protected set; }
        public VocabularyKey BvdIdNumber { get; protected set; }
        public VocabularyKey BvdAccountNumber { get; protected set; }
        public VocabularyKey OrbisId { get; protected set; }
        public VocabularyKey NationalId { get; protected set; }
        public VocabularyKey NationalIdLabel { get; protected set; }
        public VocabularyKey NationalIdType { get; protected set; }
        public VocabularyKey TradeRegisterNumber { get; protected set; }
        public VocabularyKey VatNumber { get; protected set; }
        public VocabularyKey EuropeanVatNumber { get; protected set; }
        public VocabularyKey Lei { get; protected set; }
        public VocabularyKey Giin { get; protected set; }
        public VocabularyKey StatisticalNumber { get; protected set; }
        public VocabularyKey CompanyIdNumber { get; protected set; }
        public VocabularyKey InformationProviderId { get; protected set; }
        public VocabularyKey InformationProviderIdLabel { get; protected set; }
        public VocabularyKey Ticker { get; protected set; }
        public VocabularyKey Isin { get; protected set; }
        public VocabularyKey LeiStatus { get; protected set; }
        public VocabularyKey LeiFirstAssignmentDate { get; protected set; }
        public VocabularyKey LeiAnnualRenewalDate { get; protected set; }
        public VocabularyKey LeiManagingLocalOfficeUnitStr { get; protected set; }
        public VocabularyKey ReleaseDate { get; protected set; }
        public VocabularyKey InformationProvider { get; protected set; }
        public VocabularyKey Name { get; protected set; }
        public VocabularyKey PreviousName { get; protected set; }
        public VocabularyKey PreviousNameDate { get; protected set; }
        public VocabularyKey AkaName { get; protected set; }
        public VocabularyKey Status { get; protected set; }
        public VocabularyKey StatusDate { get; protected set; }
        public VocabularyKey StatusChangeDate { get; protected set; }
        public VocabularyKey LocalStatus { get; protected set; }
        public VocabularyKey LocalStatusDate { get; protected set; }
        public VocabularyKey LocalStatusChangeDate { get; protected set; }
        public VocabularyKey StandardisedLegalForm { get; protected set; }
        public VocabularyKey NationalLegalForm { get; protected set; }
        public VocabularyKey IncorporationDate { get; protected set; }
        public VocabularyKey IncorporationState { get; protected set; }
        public VocabularyKey EntityType { get; protected set; }
        public VocabularyKey IcijDataPresenceIndicator { get; protected set; }
        public VocabularyKey ConsolidationCode { get; protected set; }
        public VocabularyKey ClosingDateLastAnnualAccounts { get; protected set; }
        public VocabularyKey YearLastAccounts { get; protected set; }
        public VocabularyKey LimitedFinancialIndicator { get; protected set; }
        public VocabularyKey NoRecentFinancialIndicator { get; protected set; }
        public VocabularyKey NumberYears { get; protected set; }


    }
}
