// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BvDOrganizationVocabulary.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the BvDOrganizationVocabulary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.BvD.Vocabularies;

/// <summary>The BvD organization vocabulary</summary>
/// <seealso cref="CluedIn.Core.Data.Vocabularies.SimpleVocabulary" />
public class BvDOrganizationVocabulary : SimpleVocabulary
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BvDOrganizationVocabulary" /> class.
    /// </summary>
    public BvDOrganizationVocabulary()
    {
        VocabularyName = "BvD Organization";
        KeyPrefix = "BvD.organization";
        KeySeparator = ".";
        Grouping = Core.Data.EntityType.Organization;

        AddGroup("Metadata", group =>
        {
            AddressLine1 = group.Add(new VocabularyKey("addressLine1"));
            AddressLine2 = group.Add(new VocabularyKey("addressLine2"));
            AddressLine3 = group.Add(new VocabularyKey("addressLine3"));
            AddressLine4 = group.Add(new VocabularyKey("addressLine4"));
            Postcode = group.Add(new VocabularyKey("postcode"));
            City = group.Add(new VocabularyKey("city"));
            CityStandardized = group.Add(new VocabularyKey("cityStandardized"));
            Country = group.Add(new VocabularyKey("country"));
            CountryIsoCode = group.Add(new VocabularyKey("countryIsoCode"));
            CountryRegion = group.Add(new VocabularyKey("countryRegion"));
            CountryRegionType = group.Add(new VocabularyKey("countryRegionType"));
            Nuts1 = group.Add(new VocabularyKey("nuts1"));
            Nuts2 = group.Add(new VocabularyKey("nuts2"));
            Nuts3 = group.Add(new VocabularyKey("nuts3"));
            WorldRegion = group.Add(new VocabularyKey("worldRegion"));
            UsState = group.Add(new VocabularyKey("usState"));
            AddressType = group.Add(new VocabularyKey("addressType"));
            PhoneNumber = group.Add(new VocabularyKey("phoneNumber"));
            FaxNumber = group.Add(new VocabularyKey("faxNumber"));
            Domain = group.Add(new VocabularyKey("domain"));
            Website = group.Add(new VocabularyKey("website"));
            Email = group.Add(new VocabularyKey("email"));
            Building = group.Add(new VocabularyKey("building"));
            Street = group.Add(new VocabularyKey("street"));
            StreetNumber = group.Add(new VocabularyKey("streetNumber"));
            StreetNumberExtension = group.Add(new VocabularyKey("streetNumberExtension"));
            StreetAndStreetNumber = group.Add(new VocabularyKey("streetAndStreetNumber"));
            StreetSupplement = group.Add(new VocabularyKey("streetSupplement"));
            PoBox = group.Add(new VocabularyKey("poBox"));
            MinorTown = group.Add(new VocabularyKey("minorTown"));
            AddressLine1Additional = group.Add(new VocabularyKey("addressLine1Additional"));
            AddressLine2Additional = group.Add(new VocabularyKey("addressLine2Additional"));
            AddressLine3Additional = group.Add(new VocabularyKey("addressLine3Additional"));
            AddressLine4Additional = group.Add(new VocabularyKey("addressLine4Additional"));
            PostcodeAdditional = group.Add(new VocabularyKey("postcodeAdditional"));
            CityAdditional = group.Add(new VocabularyKey("cityAdditional"));
            CityStandardizedAdditional = group.Add(new VocabularyKey("cityStandardizedAdditional"));
            CountryAdditional = group.Add(new VocabularyKey("countryAdditional"));
            CountryIsoCodeAdditional = group.Add(new VocabularyKey("countryIsoCodeAdditional"));
            LatitudeAdditional = group.Add(new VocabularyKey("latitudeAdditional"));
            LongitudeAdditional = group.Add(new VocabularyKey("longitudeAdditional"));
            CountryRegionAdditional = group.Add(new VocabularyKey("countryRegionAdditional"));
            CountryRegionTypeAdditional = group.Add(new VocabularyKey("countryRegionTypeAdditional"));
            AddressTypeAdditional = group.Add(new VocabularyKey("addressTypeAdditional"));
            PhoneNumberAdditional = group.Add(new VocabularyKey("phoneNumberAdditional"));
            FaxNumberAdditional = group.Add(new VocabularyKey("faxNumberAdditional"));
            BuildingAdditional = group.Add(new VocabularyKey("buildingAdditional"));
            StreetAdditional = group.Add(new VocabularyKey("streetAdditional"));
            StreetNumberAdditional = group.Add(new VocabularyKey("streetNumberAdditional"));
            StreetNumberExtensionAdditional = group.Add(new VocabularyKey("streetNumberExtensionAdditional"));
            StreetAndStreetNumberAdditional = group.Add(new VocabularyKey("streetAndStreetNumberAdditional"));
            StreetSupplementAdditional = group.Add(new VocabularyKey("streetSupplementAdditional"));
            PoBoxAdditional = group.Add(new VocabularyKey("poBoxAdditional"));
            MinorTownAdditional = group.Add(new VocabularyKey("minorTownAdditional"));
            TradeDescriptionEn = group.Add(new VocabularyKey("tradeDescriptionEn"));
            TradeDescriptionOriginal = group.Add(new VocabularyKey("tradeDescriptionOriginal"));
            TradeDescriptionLanguage = group.Add(new VocabularyKey("tradeDescriptionLanguage"));
            ProductsServices = group.Add(new VocabularyKey("productsServices"));
            BvdSectorCoreLabel = group.Add(new VocabularyKey("bvdSectorCoreLabel"));
            IndustryClassification = group.Add(new VocabularyKey("industryClassification"));
            IndustryPrimaryCode = group.Add(new VocabularyKey("industryPrimaryCode"));
            IndustryPrimaryLabel = group.Add(new VocabularyKey("industryPrimaryLabel"));
            IndustrySecondaryCode = group.Add(new VocabularyKey("industrySecondaryCode"));
            IndustrySecondaryLabel = group.Add(new VocabularyKey("industrySecondaryLabel"));
            Nace2MainSection = group.Add(new VocabularyKey("nace2MainSection"));
            Nace2CoreCode = group.Add(new VocabularyKey("nace2CoreCode"));
            Nace2CoreLabel = group.Add(new VocabularyKey("nace2CoreLabel"));
            Nace2PrimaryCode = group.Add(new VocabularyKey("nace2PrimaryCode"));
            Nace2PrimaryLabel = group.Add(new VocabularyKey("nace2PrimaryLabel"));
            Nace2SecondaryCode = group.Add(new VocabularyKey("nace2SecondaryCode"));
            Nace2SecondaryLabel = group.Add(new VocabularyKey("nace2SecondaryLabel"));
            Naics2017CoreCode = group.Add(new VocabularyKey("naics2017CoreCode"));
            Naics2017CoreLabel = group.Add(new VocabularyKey("naics2017CoreLabel"));
            Naics2017PrimaryCode = group.Add(new VocabularyKey("naics2017PrimaryCode"));
            Naics2017PrimaryLabel = group.Add(new VocabularyKey("naics2017PrimaryLabel"));
            Naics2017SecondaryCode = group.Add(new VocabularyKey("naics2017SecondaryCode"));
            Naics2017SecondaryLabel = group.Add(new VocabularyKey("naics2017SecondaryLabel"));
            UssicCoreCode = group.Add(new VocabularyKey("ussicCoreCode"));
            UssicCoreLabel = group.Add(new VocabularyKey("ussicCoreLabel"));
            UssicPrimaryCode = group.Add(new VocabularyKey("ussicPrimaryCode"));
            UssicPrimaryLabel = group.Add(new VocabularyKey("ussicPrimaryLabel"));
            UssicSecondaryCode = group.Add(new VocabularyKey("ussicSecondaryCode"));
            UssicSecondaryLabel = group.Add(new VocabularyKey("ussicSecondaryLabel"));
            BvdIdNumber = group.Add(new VocabularyKey("bvdIdNumber"));
            BvdAccountNumber = group.Add(new VocabularyKey("bvdAccountNumber"));
            OrbisId = group.Add(new VocabularyKey("orbisId"));
            NationalId = group.Add(new VocabularyKey("nationalId"));
            NationalIdLabel = group.Add(new VocabularyKey("nationalIdLabel"));
            NationalIdType = group.Add(new VocabularyKey("nationalIdType"));
            TradeRegisterNumber = group.Add(new VocabularyKey("tradeRegisterNumber"));
            VatNumber = group.Add(new VocabularyKey("vatNumber"));
            EuropeanVatNumber = group.Add(new VocabularyKey("europeanVatNumber"));
            Lei = group.Add(new VocabularyKey("lei"));
            Giin = group.Add(new VocabularyKey("giin"));
            StatisticalNumber = group.Add(new VocabularyKey("statisticalNumber"));
            CompanyIdNumber = group.Add(new VocabularyKey("companyIdNumber"));
            InformationProviderId = group.Add(new VocabularyKey("informationProviderId"));
            InformationProviderIdLabel = group.Add(new VocabularyKey("informationProviderIdLabel"));
            Ticker = group.Add(new VocabularyKey("ticker"));
            Isin = group.Add(new VocabularyKey("isin"));
            LeiStatus = group.Add(new VocabularyKey("leiStatus"));
            LeiFirstAssignmentDate = group.Add(new VocabularyKey("leiFirstAssignmentDate"));
            LeiAnnualRenewalDate = group.Add(new VocabularyKey("leiAnnualRenewalDate"));
            LeiManagingLocalOfficeUnitStr = group.Add(new VocabularyKey("leiManagingLocalOfficeUnitStr"));
            ReleaseDate = group.Add(new VocabularyKey("releaseDate"));
            InformationProvider = group.Add(new VocabularyKey("informationProvider"));
            Name = group.Add(new VocabularyKey("name"));
            PreviousName = group.Add(new VocabularyKey("previousName"));
            PreviousNameDate = group.Add(new VocabularyKey("previousNameDate"));
            AkaName = group.Add(new VocabularyKey("akaName"));
            Status = group.Add(new VocabularyKey("status"));
            StatusDate = group.Add(new VocabularyKey("statusDate"));
            StatusChangeDate = group.Add(new VocabularyKey("statusChangeDate"));
            LocalStatus = group.Add(new VocabularyKey("localStatus"));
            LocalStatusDate = group.Add(new VocabularyKey("localStatusDate"));
            LocalStatusChangeDate = group.Add(new VocabularyKey("localStatusChangeDate"));
            StandardisedLegalForm = group.Add(new VocabularyKey("standardisedLegalForm"));
            NationalLegalForm = group.Add(new VocabularyKey("nationalLegalForm"));
            IncorporationDate = group.Add(new VocabularyKey("incorporationDate"));
            IncorporationState = group.Add(new VocabularyKey("incorporationState"));
            EntityType = group.Add(new VocabularyKey("entityType"));
            IcijDataPresenceIndicator = group.Add(new VocabularyKey("icijDataPresenceIndicator"));
            ConsolidationCode = group.Add(new VocabularyKey("consolidationCode"));
            ClosingDateLastAnnualAccounts = group.Add(new VocabularyKey("closingDateLastAnnualAccounts"));
            YearLastAccounts = group.Add(new VocabularyKey("yearLastAccounts"));
            LimitedFinancialIndicator = group.Add(new VocabularyKey("limitedFinancialIndicator"));
            NoRecentFinancialIndicator = group.Add(new VocabularyKey("noRecentFinancialIndicator"));
            NumberYears = group.Add(new VocabularyKey("numberYears"));
            GuoBvdIdNumber = group.Add(new VocabularyKey("guoBvdIdNumber"));
            RawMatches = group.Add(new VocabularyKey("rawMatches"));
            BvdIdNeedsAttention = group.Add(new VocabularyKey("bvdIdNeedsAttention"));
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
    public VocabularyKey GuoBvdIdNumber { get; protected set; }
    public VocabularyKey RawMatches { get; protected set; }
    public VocabularyKey BvdIdNeedsAttention { get; protected set; }
}
