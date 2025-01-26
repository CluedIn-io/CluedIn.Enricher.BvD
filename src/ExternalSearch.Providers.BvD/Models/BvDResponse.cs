using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.BvD.Models
{
    public class DatabaseInfo
    {
        [JsonProperty("ReleaseNumber")]
        public string ReleaseNumber { get; set; }

        [JsonProperty("UpdateNumber")]
        public string UpdateNumber { get; set; }

        [JsonProperty("UpdateDate")]
        public DateTime UpdateDate { get; set; }

        [JsonProperty("VersionNumber")]
        public string VersionNumber { get; set; }
    }

    

    public class BvDResponse
    {
        [JsonProperty("SearchSummary")]
        public SearchSummary SearchSummary { get; set; }

        [JsonProperty("Data")]
        public List<Datum> Data { get; set; }
    }

    public class SearchSummary
    {
        [JsonProperty("TotalRecordsFound")]
        public int TotalRecordsFound { get; set; }

        [JsonProperty("Offset")]
        public int Offset { get; set; }

        [JsonProperty("RecordsReturned")]
        public int RecordsReturned { get; set; }

        [JsonProperty("DatabaseInfo")]
        public DatabaseInfo DatabaseInfo { get; set; }

        [JsonProperty("Sort")]
        public Sort Sort { get; set; }
    }

    public class Sort
    {
        [JsonProperty("DESC")]
        public string DESC { get; set; }
    }
    public class Datum
    {
        [JsonProperty("ADDRESS_LINE1")]
        public string ADDRESS_LINE1 { get; set; }

        [JsonProperty("ADDRESS_LINE2")]
        public string ADDRESS_LINE2 { get; set; }

        [JsonProperty("ADDRESS_LINE3")]
        public string ADDRESS_LINE3 { get; set; }

        [JsonProperty("ADDRESS_LINE4")]
        public string ADDRESS_LINE4 { get; set; }

        [JsonProperty("POSTCODE")]
        public string POSTCODE { get; set; }

        [JsonProperty("CITY")]
        public string CITY { get; set; }

        [JsonProperty("CITY_STANDARDIZED")]
        public string CITY_STANDARDIZED { get; set; }

        [JsonProperty("COUNTRY")]
        public string COUNTRY { get; set; }

        [JsonProperty("COUNTRY_ISO_CODE")]
        public string COUNTRY_ISO_CODE { get; set; }

        [JsonProperty("COUNTRY_REGION")]
        public string COUNTRY_REGION { get; set; }

        [JsonProperty("COUNTRY_REGION_TYPE")]
        public string COUNTRY_REGION_TYPE { get; set; }

        [JsonProperty("NUTS1")]
        public string NUTS1 { get; set; }

        [JsonProperty("NUTS2")]
        public string NUTS2 { get; set; }

        [JsonProperty("NUTS3")]
        public string NUTS3 { get; set; }

        [JsonProperty("WORLD_REGION")]
        public string WORLD_REGION { get; set; }

        [JsonProperty("US_STATE")]
        public string US_STATE { get; set; }

        [JsonProperty("ADDRESS_TYPE")]
        public string ADDRESS_TYPE { get; set; }

        [JsonProperty("PHONE_NUMBER")]
        public string PHONE_NUMBER { get; set; }

        [JsonProperty("FAX_NUMBER")]
        public string FAX_NUMBER { get; set; }

        [JsonProperty("DOMAIN")]
        public string DOMAIN { get; set; }

        [JsonProperty("WEBSITE")]
        public string WEBSITE { get; set; }

        [JsonProperty("EMAIL")]
        public string EMAIL { get; set; }

        [JsonProperty("BUILDING")]
        public string BUILDING { get; set; }

        [JsonProperty("STREET")]
        public string STREET { get; set; }

        [JsonProperty("STREET_NUMBER")]
        public string STREET_NUMBER { get; set; }

        [JsonProperty("STREET_NUMBER_EXTENSION")]
        public string STREET_NUMBER_EXTENSION { get; set; }

        [JsonProperty("STREET_AND_STREET_NUMBER")]
        public string STREET_AND_STREET_NUMBER { get; set; }

        [JsonProperty("STREET_SUPPLEMENT")]
        public string STREET_SUPPLEMENT { get; set; }

        [JsonProperty("PO_BOX")]
        public string PO_BOX { get; set; }

        [JsonProperty("MINOR_TOWN")]
        public string MINOR_TOWN { get; set; }

        [JsonProperty("ADDRESS_LINE1_ADDITIONAL")]
        public string ADDRESS_LINE1_ADDITIONAL { get; set; }

        [JsonProperty("ADDRESS_LINE2_ADDITIONAL")]
        public string ADDRESS_LINE2_ADDITIONAL { get; set; }

        [JsonProperty("ADDRESS_LINE3_ADDITIONAL")]
        public string ADDRESS_LINE3_ADDITIONAL { get; set; }

        [JsonProperty("ADDRESS_LINE4_ADDITIONAL")]
        public string ADDRESS_LINE4_ADDITIONAL { get; set; }

        [JsonProperty("POSTCODE_ADDITIONAL")]
        public string POSTCODE_ADDITIONAL { get; set; }

        [JsonProperty("CITY_ADDITIONAL")]
        public string CITY_ADDITIONAL { get; set; }

        [JsonProperty("CITY_STANDARDIZED_ADDITIONAL")]
        public string CITY_STANDARDIZED_ADDITIONAL { get; set; }

        [JsonProperty("COUNTRY_ADDITIONAL")]
        public string COUNTRY_ADDITIONAL { get; set; }

        [JsonProperty("COUNTRY_ISO_CODE_ADDITIONAL")]
        public string COUNTRY_ISO_CODE_ADDITIONAL { get; set; }

        [JsonProperty("LATITUDE_ADDITIONAL")]
        public string LATITUDE_ADDITIONAL { get; set; }

        [JsonProperty("LONGITUDE_ADDITIONAL")]
        public string LONGITUDE_ADDITIONAL { get; set; }

        [JsonProperty("COUNTRY_REGION_ADDITIONAL")]
        public string COUNTRY_REGION_ADDITIONAL { get; set; }

        [JsonProperty("COUNTRY_REGION_TYPE_ADDITIONAL")]
        public string COUNTRY_REGION_TYPE_ADDITIONAL { get; set; }

        [JsonProperty("ADDRESS_TYPE_ADDITIONAL")]
        public string ADDRESS_TYPE_ADDITIONAL { get; set; }

        [JsonProperty("PHONE_NUMBER_ADDITIONAL")]
        public string PHONE_NUMBER_ADDITIONAL { get; set; }

        [JsonProperty("FAX_NUMBER_ADDITIONAL")]
        public string FAX_NUMBER_ADDITIONAL { get; set; }

        [JsonProperty("BUILDING_ADDITIONAL")]
        public string BUILDING_ADDITIONAL { get; set; }

        [JsonProperty("STREET_ADDITIONAL")]
        public string STREET_ADDITIONAL { get; set; }

        [JsonProperty("STREET_NUMBER_ADDITIONAL")]
        public string STREET_NUMBER_ADDITIONAL { get; set; }

        [JsonProperty("STREET_NUMBER_EXTENSION_ADDITIONAL")]
        public string STREET_NUMBER_EXTENSION_ADDITIONAL { get; set; }

        [JsonProperty("STREET_AND_STREET_NUMBER_ADDITIONAL")]
        public string STREET_AND_STREET_NUMBER_ADDITIONAL { get; set; }

        [JsonProperty("STREET_SUPPLEMENT_ADDITIONAL")]
        public string STREET_SUPPLEMENT_ADDITIONAL { get; set; }

        [JsonProperty("PO_BOX_ADDITIONAL")]
        public string PO_BOX_ADDITIONAL { get; set; }

        [JsonProperty("MINOR_TOWN_ADDITIONAL")]
        public string MINOR_TOWN_ADDITIONAL { get; set; }

        [JsonProperty("TRADE_DESCRIPTION_EN")]
        public string TRADE_DESCRIPTION_EN { get; set; }

        [JsonProperty("TRADE_DESCRIPTION_ORIGINAL")]
        public string TRADE_DESCRIPTION_ORIGINAL { get; set; }

        [JsonProperty("TRADE_DESCRIPTION_LANGUAGE")]
        public string TRADE_DESCRIPTION_LANGUAGE { get; set; }

        [JsonProperty("PRODUCTS_SERVICES")]
        public string PRODUCTS_SERVICES { get; set; }

        [JsonProperty("BVD_SECTOR_CORE_LABEL")]
        public string BVD_SECTOR_CORE_LABEL { get; set; }

        [JsonProperty("INDUSTRY_CLASSIFICATION")]
        public string INDUSTRY_CLASSIFICATION { get; set; }

        [JsonProperty("INDUSTRY_PRIMARY_CODE")]
        public string INDUSTRY_PRIMARY_CODE { get; set; }

        [JsonProperty("INDUSTRY_PRIMARY_LABEL")]
        public string INDUSTRY_PRIMARY_LABEL { get; set; }

        [JsonProperty("INDUSTRY_SECONDARY_CODE")]
        public string INDUSTRY_SECONDARY_CODE { get; set; }

        [JsonProperty("INDUSTRY_SECONDARY_LABEL")]
        public string INDUSTRY_SECONDARY_LABEL { get; set; }

        [JsonProperty("NACE2_MAIN_SECTION")]
        public string NACE2_MAIN_SECTION { get; set; }

        [JsonProperty("NACE2_CORE_CODE")]
        public string NACE2_CORE_CODE { get; set; }

        [JsonProperty("NACE2_CORE_LABEL")]
        public string NACE2_CORE_LABEL { get; set; }

        [JsonProperty("NACE2_PRIMARY_CODE")]
        public string NACE2_PRIMARY_CODE { get; set; }

        [JsonProperty("NACE2_PRIMARY_LABEL")]
        public string NACE2_PRIMARY_LABEL { get; set; }

        [JsonProperty("NACE2_SECONDARY_CODE")]
        public string NACE2_SECONDARY_CODE { get; set; }

        [JsonProperty("NACE2_SECONDARY_LABEL")]
        public string NACE2_SECONDARY_LABEL { get; set; }

        [JsonProperty("NAICS2017_CORE_CODE")]
        public string NAICS2017_CORE_CODE { get; set; }

        [JsonProperty("NAICS2017_CORE_LABEL")]
        public string NAICS2017_CORE_LABEL { get; set; }

        [JsonProperty("NAICS2017_PRIMARY_CODE")]
        public string NAICS2017_PRIMARY_CODE { get; set; }

        [JsonProperty("NAICS2017_PRIMARY_LABEL")]
        public string NAICS2017_PRIMARY_LABEL { get; set; }

        [JsonProperty("NAICS2017_SECONDARY_CODE")]
        public string NAICS2017_SECONDARY_CODE { get; set; }

        [JsonProperty("NAICS2017_SECONDARY_LABEL")]
        public string NAICS2017_SECONDARY_LABEL { get; set; }

        [JsonProperty("USSIC_CORE_CODE")]
        public string USSIC_CORE_CODE { get; set; }

        [JsonProperty("USSIC_CORE_LABEL")]
        public string USSIC_CORE_LABEL { get; set; }

        [JsonProperty("USSIC_PRIMARY_CODE")]
        public string USSIC_PRIMARY_CODE { get; set; }

        [JsonProperty("USSIC_PRIMARY_LABEL")]
        public string USSIC_PRIMARY_LABEL { get; set; }

        [JsonProperty("USSIC_SECONDARY_CODE")]
        public string USSIC_SECONDARY_CODE { get; set; }

        [JsonProperty("USSIC_SECONDARY_LABEL")]
        public string USSIC_SECONDARY_LABEL { get; set; }

        [JsonProperty("BVD_ID_NUMBER")]
        public string BVD_ID_NUMBER { get; set; }

        [JsonProperty("BVD_ACCOUNT_NUMBER")]
        public string BVD_ACCOUNT_NUMBER { get; set; }

        [JsonProperty("ORBISID")]
        public string ORBISID { get; set; }

        [JsonProperty("NATIONAL_ID")]
        public string NATIONAL_ID { get; set; }

        [JsonProperty("NATIONAL_ID_LABEL")]
        public string NATIONAL_ID_LABEL { get; set; }

        [JsonProperty("NATIONAL_ID_TYPE")]
        public string NATIONAL_ID_TYPE { get; set; }

        [JsonProperty("TRADE_REGISTER_NUMBER")]
        public string TRADE_REGISTER_NUMBER { get; set; }

        [JsonProperty("VAT_NUMBER")]
        public string VAT_NUMBER { get; set; }

        [JsonProperty("EUROPEAN_VAT_NUMBER")]
        public string EUROPEAN_VAT_NUMBER { get; set; }

        [JsonProperty("LEI")]
        public string LEI { get; set; }

        [JsonProperty("GIIN")]
        public string GIIN { get; set; }

        [JsonProperty("STATISTICAL_NUMBER")]
        public string STATISTICAL_NUMBER { get; set; }

        [JsonProperty("COMPANY_ID_NUMBER")]
        public string COMPANY_ID_NUMBER { get; set; }

        [JsonProperty("INFORMATION_PROVIDER_ID")]
        public string INFORMATION_PROVIDER_ID { get; set; }

        [JsonProperty("INFORMATION_PROVIDER_ID_LABEL")]
        public string INFORMATION_PROVIDER_ID_LABEL { get; set; }

        [JsonProperty("TICKER")]
        public string TICKER { get; set; }

        [JsonProperty("ISIN")]
        public string ISIN { get; set; }

        [JsonProperty("LEI_STATUS")]
        public string LEI_STATUS { get; set; }

        [JsonProperty("LEI_FIRST_ASSIGNMENT_DATE")]
        public string LEI_FIRST_ASSIGNMENT_DATE { get; set; }

        [JsonProperty("LEI_ANNUAL_RENEWAL_DATE")]
        public string LEI_ANNUAL_RENEWAL_DATE { get; set; }

        [JsonProperty("LEI_MANAGING_LOCAL_OFFICE_UNIT_STR")]
        public string LEI_MANAGING_LOCAL_OFFICE_UNIT_STR { get; set; }

        [JsonProperty("RELEASE_DATE")]
        public string RELEASE_DATE { get; set; }

        [JsonProperty("INFORMATION_PROVIDER")]
        public string INFORMATION_PROVIDER { get; set; }

        [JsonProperty("NAME")]
        public string NAME { get; set; }

        [JsonProperty("PREVIOUS_NAME")]
        public string PREVIOUS_NAME { get; set; }

        [JsonProperty("PREVIOUS_NAME_DATE")]
        public string PREVIOUS_NAME_DATE { get; set; }

        [JsonProperty("AKA_NAME")]
        public string AKA_NAME { get; set; }

        [JsonProperty("STATUS")]
        public string STATUS { get; set; }

        [JsonProperty("STATUS_DATE")]
        public string STATUS_DATE { get; set; }

        [JsonProperty("STATUS_CHANGE_DATE")]
        public string STATUS_CHANGE_DATE { get; set; }

        [JsonProperty("LOCAL_STATUS")]
        public string LOCAL_STATUS { get; set; }

        [JsonProperty("LOCAL_STATUS_DATE")]
        public string LOCAL_STATUS_DATE { get; set; }

        [JsonProperty("LOCAL_STATUS_CHANGE_DATE")]
        public string LOCAL_STATUS_CHANGE_DATE { get; set; }

        [JsonProperty("STANDARDISED_LEGAL_FORM")]
        public string STANDARDISED_LEGAL_FORM { get; set; }

        [JsonProperty("NATIONAL_LEGAL_FORM")]
        public string NATIONAL_LEGAL_FORM { get; set; }

        [JsonProperty("INCORPORATION_DATE")]
        public string INCORPORATION_DATE { get; set; }

        [JsonProperty("INCORPORATION_STATE")]
        public string INCORPORATION_STATE { get; set; }

        [JsonProperty("ENTITY_TYPE")]
        public string ENTITY_TYPE { get; set; }

        [JsonProperty("ICIJ_DATA_PRESENCE_INDICATOR")]
        public string ICIJ_DATA_PRESENCE_INDICATOR { get; set; }

        [JsonProperty("CONSOLIDATION_CODE")]
        public string CONSOLIDATION_CODE { get; set; }

        [JsonProperty("CLOSING_DATE_LAST_ANNUAL_ACCOUNTS")]
        public string CLOSING_DATE_LAST_ANNUAL_ACCOUNTS { get; set; }

        [JsonProperty("YEAR_LAST_ACCOUNTS")]
        public string YEAR_LAST_ACCOUNTS { get; set; }

        [JsonProperty("LIMITED_FINANCIAL_INDICATOR")]
        public string LIMITED_FINANCIAL_INDICATOR { get; set; }

        [JsonProperty("NO_RECENT_FINANCIAL_INDICATOR")]
        public string NO_RECENT_FINANCIAL_INDICATOR { get; set; }

        [JsonProperty("NUMBER_YEARS")]
        public string NUMBER_YEARS { get; set; }
    }



}


