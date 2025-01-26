using Xunit;

namespace CluedIn.ExternalSearch.BvD.Unit.Tests.Utility.BvDNumberCleaner.Customer.ISS
{
    /// <summary>
    /// Checks ISS VAT numbers
    /// Developers can use http://www.vatcheck.eu/vatcheck.php to validate EU VAT numbers
    /// </summary>
    public class CheckVatNumberTests : TestBase
    {
        [Theory]
        [InlineData("GB222 5607 89")]
        [InlineData("GB776 5310 13")]
        [InlineData("BE0888533955")]
        [InlineData("GB417 1157 75")]
        [InlineData("GB702 4610 79")]
        [InlineData("GB272 7221 11")]
        [InlineData("GB735 1406 54")]
        [InlineData("GB168 8372 61")]
        [InlineData("GB747 8798 54")]
        [InlineData("SE556410328001")]
        [InlineData("GB108 3046 44")]
        [InlineData("DK10161614")]
        [InlineData("GB945 7518 87")]
        [InlineData("FR02453829079")]
        [InlineData("GB503 1018 15")]
        [InlineData("NL802353794B01")]
        [InlineData("GB923 4281 36")]
        [InlineData("GB731 7507 42")]
        [InlineData("GB883 8175 78")]
        [InlineData("GB597 2654 89")]
        [InlineData("GB704 2101 01")]
        [InlineData("GB108 2409 39")]
        [InlineData("GB703 5643 53")]
        [InlineData("GB202 1441 76")]
        [InlineData("GB724 6179 27")]
        [InlineData("GB439 4758 08")]
        [InlineData("GB336 3492 51")]
        [InlineData("GB135 5557 06")]
        [InlineData("GB559 0978 89")]
        [InlineData("GB506 6026 70")]
        [InlineData("GB814 2577 34")]
        [InlineData("GB108 2470 36")]
        [InlineData("GB243 1640 91")]
        [InlineData("GB997 3273 64")]
        [InlineData("GB919 2934 95")]
        [InlineData("GB892 1637 03")]
        [InlineData("GB160 0904 45")]
        [InlineData("GB297 9364 87")]
        [InlineData("GB434 9713 35")]
        [InlineData("GB494 5524 16")]
        [InlineData("GB223 7100 10")]
        [InlineData("GB804 1163 73")]
        [InlineData("GB394 1212 63")]
        [InlineData("GB178 3069 80")]
        [InlineData("GB239 2074 64")]
        [InlineData("GB801 7806 45")]
        [InlineData("GB732 3615 53")]
        [InlineData("GB298 4484 96")]
        [InlineData("GB694 1366 12")]
        [InlineData("GB223 4728 76")]
        [InlineData("GB670 7557 13")]
        [InlineData("GB989 1122 90")]
        [InlineData("GB795 9294 54")]
        [InlineData("GB746 6441 14")]
        [InlineData("GB823 8535 19")]
        [InlineData("GB772 1252 45")]
        [InlineData("GB310 0319 75")]
        public void ReturnsValidVatNumber(string vatNumber)
        {
            Assert.NotEmpty(
                Sut.CheckBvDNumber(vatNumber));
        }

        [Theory]
        [InlineData("GB000 0000 11")]
        [InlineData("GB000 0000 12")]
        [InlineData("GB000 0000 01")]
        [InlineData("GB000 0000 05")]
        [InlineData("GB000 0000 23")]
        [InlineData("GB000 0000 31")]
        [InlineData("GB000 0000 17")]
        [InlineData("GB000 0000 02")]
        [InlineData("GB000 0000 06")]
        [InlineData("GB000 0000 27")]
        [InlineData("GB000 0000 00")]
        [InlineData("GB000 0000 03")]
        [InlineData("GB000 0000 25")]
        [InlineData("GB000 0000 08")]
        [InlineData("GB000 0000 09")]
        [InlineData("GB000 0000 07")]
        [InlineData("GB000 0000 04")]
        [InlineData("GB000 0000 10")]
        [InlineData("GB000 0000 21")]
        [InlineData("GB000 0000 14")]
        [InlineData("GB000 0000 15")]
        [InlineData("GB000 0000 28")]
        [InlineData("GB000 0000 19")]
        [InlineData("GB000 0000 24")]
        [InlineData("GB000 0000 26")]
        [InlineData("GB000 0000 13")]
        [InlineData("GB000 0000 16")]
        [InlineData("GB000 0000 18")]
        [InlineData("GB000 0000 20")]
        [InlineData("GB000 0000 22")]
        [InlineData("GB000 0000 29")]
        [InlineData("GB000 0000 54")]
        [InlineData("GB505 4652 55")]
        public void ReturnsEmptyStringForInvalidVatNumber(string vatNumber)
        {
            Assert.Empty(
                Sut.CheckBvDNumber(vatNumber));
        }
    }
}
