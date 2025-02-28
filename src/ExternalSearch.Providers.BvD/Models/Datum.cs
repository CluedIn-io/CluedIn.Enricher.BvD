using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models;

public class Datum
{
    [JsonProperty("ADDRESS_LINE1")] public string AddressLine1 { get; set; }

    [JsonProperty("ADDRESS_LINE2")] public string AddressLine2 { get; set; }

    [JsonProperty("ADDRESS_LINE3")] public string AddressLine3 { get; set; }

    [JsonProperty("ADDRESS_LINE4")] public string AddressLine4 { get; set; }

    [JsonProperty("POSTCODE")] public string Postcode { get; set; }

    [JsonProperty("CITY")] public string City { get; set; }

    [JsonProperty("CITY_STANDARDIZED")] public string CityStandardized { get; set; }

    [JsonProperty("COUNTRY")] public string Country { get; set; }

    [JsonProperty("COUNTRY_ISO_CODE")] public string CountryIsoCode { get; set; }

    [JsonProperty("COUNTRY_REGION")] public string CountryRegion { get; set; }

    [JsonProperty("COUNTRY_REGION_TYPE")] public string CountryRegionType { get; set; }

    [JsonProperty("NUTS1")] public string Nuts1 { get; set; }

    [JsonProperty("NUTS2")] public string Nuts2 { get; set; }

    [JsonProperty("NUTS3")] public string Nuts3 { get; set; }

    [JsonProperty("WORLD_REGION")] public string WorldRegion { get; set; }

    [JsonProperty("US_STATE")] public string UsState { get; set; }

    [JsonProperty("ADDRESS_TYPE")] public string AddressType { get; set; }

    [JsonProperty("PHONE_NUMBER")] public string PhoneNumber { get; set; }

    [JsonProperty("FAX_NUMBER")] public string FaxNumber { get; set; }

    [JsonProperty("DOMAIN")] public string Domain { get; set; }

    [JsonProperty("WEBSITE")] public string Website { get; set; }

    [JsonProperty("EMAIL")] public string Email { get; set; }

    [JsonProperty("BUILDING")] public string Building { get; set; }

    [JsonProperty("STREET")] public string Street { get; set; }

    [JsonProperty("STREET_NUMBER")] public string StreetNumber { get; set; }

    [JsonProperty("STREET_NUMBER_EXTENSION")]
    public string StreetNumberExtension { get; set; }

    [JsonProperty("STREET_AND_STREET_NUMBER")]
    public string StreetAndStreetNumber { get; set; }

    [JsonProperty("STREET_SUPPLEMENT")] public string StreetSupplement { get; set; }

    [JsonProperty("PO_BOX")] public string PoBox { get; set; }

    [JsonProperty("MINOR_TOWN")] public string MinorTown { get; set; }

    [JsonProperty("ADDRESS_LINE1_ADDITIONAL")]
    public string AddressLine1Additional { get; set; }

    [JsonProperty("ADDRESS_LINE2_ADDITIONAL")]
    public string AddressLine2Additional { get; set; }

    [JsonProperty("ADDRESS_LINE3_ADDITIONAL")]
    public string AddressLine3Additional { get; set; }

    [JsonProperty("ADDRESS_LINE4_ADDITIONAL")]
    public string AddressLine4Additional { get; set; }

    [JsonProperty("POSTCODE_ADDITIONAL")] public string PostcodeAdditional { get; set; }

    [JsonProperty("CITY_ADDITIONAL")] public string CityAdditional { get; set; }

    [JsonProperty("CITY_STANDARDIZED_ADDITIONAL")]
    public string CityStandardizedAdditional { get; set; }

    [JsonProperty("COUNTRY_ADDITIONAL")] public string CountryAdditional { get; set; }

    [JsonProperty("COUNTRY_ISO_CODE_ADDITIONAL")]
    public string CountryIsoCodeAdditional { get; set; }

    [JsonProperty("LATITUDE_ADDITIONAL")] public string LatitudeAdditional { get; set; }

    [JsonProperty("LONGITUDE_ADDITIONAL")] public string LongitudeAdditional { get; set; }

    [JsonProperty("COUNTRY_REGION_ADDITIONAL")]
    public string CountryRegionAdditional { get; set; }

    [JsonProperty("COUNTRY_REGION_TYPE_ADDITIONAL")]
    public string CountryRegionTypeAdditional { get; set; }

    [JsonProperty("ADDRESS_TYPE_ADDITIONAL")]
    public string AddressTypeAdditional { get; set; }

    [JsonProperty("PHONE_NUMBER_ADDITIONAL")]
    public string PhoneNumberAdditional { get; set; }

    [JsonProperty("FAX_NUMBER_ADDITIONAL")]
    public string FaxNumberAdditional { get; set; }

    [JsonProperty("BUILDING_ADDITIONAL")] public string BuildingAdditional { get; set; }

    [JsonProperty("STREET_ADDITIONAL")] public string StreetAdditional { get; set; }

    [JsonProperty("STREET_NUMBER_ADDITIONAL")]
    public string StreetNumberAdditional { get; set; }

    [JsonProperty("STREET_NUMBER_EXTENSION_ADDITIONAL")]
    public string StreetNumberExtensionAdditional { get; set; }

    [JsonProperty("STREET_AND_STREET_NUMBER_ADDITIONAL")]
    public string StreetAndStreetNumberAdditional { get; set; }

    [JsonProperty("STREET_SUPPLEMENT_ADDITIONAL")]
    public string StreetSupplementAdditional { get; set; }

    [JsonProperty("PO_BOX_ADDITIONAL")] public string PoBoxAdditional { get; set; }

    [JsonProperty("MINOR_TOWN_ADDITIONAL")]
    public string MinorTownAdditional { get; set; }

    [JsonProperty("TRADE_DESCRIPTION_EN")] public string TradeDescriptionEn { get; set; }

    [JsonProperty("TRADE_DESCRIPTION_ORIGINAL")]
    public string TradeDescriptionOriginal { get; set; }

    [JsonProperty("TRADE_DESCRIPTION_LANGUAGE")]
    public string TradeDescriptionLanguage { get; set; }

    [JsonProperty("PRODUCTS_SERVICES")] public string ProductsServices { get; set; }

    [JsonProperty("BVD_SECTOR_CORE_LABEL")]
    public string BvdSectorCoreLabel { get; set; }

    [JsonProperty("INDUSTRY_CLASSIFICATION")]
    public string IndustryClassification { get; set; }

    [JsonProperty("INDUSTRY_PRIMARY_CODE")]
    public string IndustryPrimaryCode { get; set; }

    [JsonProperty("INDUSTRY_PRIMARY_LABEL")]
    public string IndustryPrimaryLabel { get; set; }

    [JsonProperty("INDUSTRY_SECONDARY_CODE")]
    public string IndustrySecondaryCode { get; set; }

    [JsonProperty("INDUSTRY_SECONDARY_LABEL")]
    public string IndustrySecondaryLabel { get; set; }

    [JsonProperty("NACE2_MAIN_SECTION")] public string Nace2MainSection { get; set; }

    [JsonProperty("NACE2_CORE_CODE")] public string Nace2CoreCode { get; set; }

    [JsonProperty("NACE2_CORE_LABEL")] public string Nace2CoreLabel { get; set; }

    [JsonProperty("NACE2_PRIMARY_CODE")] public string Nace2PrimaryCode { get; set; }

    [JsonProperty("NACE2_PRIMARY_LABEL")] public string Nace2PrimaryLabel { get; set; }

    [JsonProperty("NACE2_SECONDARY_CODE")] public string Nace2SecondaryCode { get; set; }

    [JsonProperty("NACE2_SECONDARY_LABEL")]
    public string Nace2SecondaryLabel { get; set; }

    [JsonProperty("NAICS2017_CORE_CODE")] public string Naics2017CoreCode { get; set; }

    [JsonProperty("NAICS2017_CORE_LABEL")] public string Naics2017CoreLabel { get; set; }

    [JsonProperty("NAICS2017_PRIMARY_CODE")]
    public string Naics2017PrimaryCode { get; set; }

    [JsonProperty("NAICS2017_PRIMARY_LABEL")]
    public string Naics2017PrimaryLabel { get; set; }

    [JsonProperty("NAICS2017_SECONDARY_CODE")]
    public string Naics2017SecondaryCode { get; set; }

    [JsonProperty("NAICS2017_SECONDARY_LABEL")]
    public string Naics2017SecondaryLabel { get; set; }

    [JsonProperty("USSIC_CORE_CODE")] public string UssicCoreCode { get; set; }

    [JsonProperty("USSIC_CORE_LABEL")] public string UssicCoreLabel { get; set; }

    [JsonProperty("USSIC_PRIMARY_CODE")] public string UssicPrimaryCode { get; set; }

    [JsonProperty("USSIC_PRIMARY_LABEL")] public string UssicPrimaryLabel { get; set; }

    [JsonProperty("USSIC_SECONDARY_CODE")] public string UssicSecondaryCode { get; set; }

    [JsonProperty("USSIC_SECONDARY_LABEL")]
    public string UssicSecondaryLabel { get; set; }

    [JsonProperty("BVD_ID_NUMBER")] public string BvdIdNumber { get; set; }

    [JsonProperty("BVD_ACCOUNT_NUMBER")] public string BvdAccountNumber { get; set; }

    [JsonProperty("ORBISID")] public string Orbisid { get; set; }

    [JsonProperty("NATIONAL_ID")] public string NationalId { get; set; }

    [JsonProperty("NATIONAL_ID_LABEL")] public string NationalIdLabel { get; set; }

    [JsonProperty("NATIONAL_ID_TYPE")] public string NationalIdType { get; set; }

    [JsonProperty("TRADE_REGISTER_NUMBER")]
    public string TradeRegisterNumber { get; set; }

    [JsonProperty("VAT_NUMBER")] public string VatNumber { get; set; }

    [JsonProperty("EUROPEAN_VAT_NUMBER")] public string EuropeanVatNumber { get; set; }

    [JsonProperty("LEI")] public string Lei { get; set; }

    [JsonProperty("GIIN")] public string Giin { get; set; }

    [JsonProperty("STATISTICAL_NUMBER")] public string StatisticalNumber { get; set; }

    [JsonProperty("COMPANY_ID_NUMBER")] public string CompanyIdNumber { get; set; }

    [JsonProperty("INFORMATION_PROVIDER_ID")]
    public string InformationProviderId { get; set; }

    [JsonProperty("INFORMATION_PROVIDER_ID_LABEL")]
    public string InformationProviderIdLabel { get; set; }

    [JsonProperty("TICKER")] public string Ticker { get; set; }

    [JsonProperty("ISIN")] public string Isin { get; set; }

    [JsonProperty("LEI_STATUS")] public string LeiStatus { get; set; }

    [JsonProperty("LEI_FIRST_ASSIGNMENT_DATE")]
    public string LeiFirstAssignmentDate { get; set; }

    [JsonProperty("LEI_ANNUAL_RENEWAL_DATE")]
    public string LeiAnnualRenewalDate { get; set; }

    [JsonProperty("LEI_MANAGING_LOCAL_OFFICE_UNIT_STR")]
    public string LeiManagingLocalOfficeUnitStr { get; set; }

    [JsonProperty("RELEASE_DATE")] public string ReleaseDate { get; set; }

    [JsonProperty("INFORMATION_PROVIDER")] public string InformationProvider { get; set; }

    [JsonProperty("NAME")] public string Name { get; set; }

    [JsonProperty("PREVIOUS_NAME")] public string PreviousName { get; set; }

    [JsonProperty("PREVIOUS_NAME_DATE")] public string PreviousNameDate { get; set; }

    [JsonProperty("AKA_NAME")] public string AkaName { get; set; }

    [JsonProperty("STATUS")] public string Status { get; set; }

    [JsonProperty("STATUS_DATE")] public string StatusDate { get; set; }

    [JsonProperty("STATUS_CHANGE_DATE")] public string StatusChangeDate { get; set; }

    [JsonProperty("LOCAL_STATUS")] public string LocalStatus { get; set; }

    [JsonProperty("LOCAL_STATUS_DATE")] public string LocalStatusDate { get; set; }

    [JsonProperty("LOCAL_STATUS_CHANGE_DATE")]
    public string LocalStatusChangeDate { get; set; }

    [JsonProperty("STANDARDISED_LEGAL_FORM")]
    public string StandardisedLegalForm { get; set; }

    [JsonProperty("NATIONAL_LEGAL_FORM")] public string NationalLegalForm { get; set; }

    [JsonProperty("INCORPORATION_DATE")] public string IncorporationDate { get; set; }

    [JsonProperty("INCORPORATION_STATE")] public string IncorporationState { get; set; }

    [JsonProperty("ENTITY_TYPE")] public string EntityType { get; set; }

    [JsonProperty("ICIJ_DATA_PRESENCE_INDICATOR")]
    public string IcijDataPresenceIndicator { get; set; }

    [JsonProperty("CONSOLIDATION_CODE")] public string ConsolidationCode { get; set; }

    [JsonProperty("CLOSING_DATE_LAST_ANNUAL_ACCOUNTS")]
    public string ClosingDateLastAnnualAccounts { get; set; }

    [JsonProperty("YEAR_LAST_ACCOUNTS")] public string YearLastAccounts { get; set; }

    [JsonProperty("LIMITED_FINANCIAL_INDICATOR")]
    public string LimitedFinancialIndicator { get; set; }

    [JsonProperty("NO_RECENT_FINANCIAL_INDICATOR")]
    public string NoRecentFinancialIndicator { get; set; }

    [JsonProperty("NUMBER_YEARS")] public string NumberYears { get; set; }
}