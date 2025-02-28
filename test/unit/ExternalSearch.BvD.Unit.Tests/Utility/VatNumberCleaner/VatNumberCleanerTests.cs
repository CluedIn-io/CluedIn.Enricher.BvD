using Xunit;

namespace CluedIn.ExternalSearch.BvD.Unit.Tests.Utility.BvDNumberCleaner
{
    public class BvDNumberCleanerTests : TestBase
    {
        private readonly Providers.BvD.Utility.BvDNumberCleaner _sut;

        public BvDNumberCleanerTests()
        {
            _sut = new Providers.BvD.Utility.BvDNumberCleaner();
        }

        [Theory]
        [InlineData("BE -/0888533955", "BE0888533955")]
        public void TestCheckVatNumber(string vatNumber, string expected)
        {
            Assert.Equal(expected, _sut.CheckBvDNumber(vatNumber));
        }

        [Theory]
        [InlineData("U12345678")]
        public void TestAuCheckDigit(string vatNumber)
        {
            Assert.True(_sut.ATVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("0888533955")]
        public void TestBeCheckDigit(string vatNumber)
        {
            Assert.True(_sut.BEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("10161614")]
        public void TestDenmarkCheckDigit(string vatNumber)
        {
            Assert.True(_sut.DKVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("310031975")]
        public void TestUkCheckDigit(string vatNumber)
        {
            Assert.True(_sut.GBVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("02453829079")]
        public void TestFrCheckDigit(string vatNumber)
        {
            Assert.True(_sut.FRVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("556410328001")]
        public void TestSeCheckDigit(string vatNumber)
        {
            Assert.True(_sut.SEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("802353794B01")]
        public void TestNlCheckDigit(string vatNumber)
        {
            Assert.True(_sut.NLVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("101004508")]
        public void TestBgCheckDigit(string vatNumber)
        {
            Assert.True(_sut.BGVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("100416306IVA")]
        public void TestCheDigitCheck(string vatNumber)
        {
            Assert.True(_sut.CHEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("10111474A")]
        public void TestCyCheckDigit(string vatNumber)
        {
            Assert.True(_sut.CYVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("5511061105")]
        public void TestCzCheckDigit(string vatNumber)
        {
            Assert.True(_sut.CZVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("113891176")]
        public void TestDeCheckDigit(string vatNumber)
        {
            Assert.True(_sut.DEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("100037342")]
        public void TestEeCheckDigit(string vatNumber)
        {
            Assert.True(_sut.EEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("094112730")]
        public void TestElCheckDigit(string vatNumber)
        {
            Assert.True(_sut.ELVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("00000019L")]
        public void TestEsCheckDigit(string vatNumber)
        {
            Assert.True(_sut.ESVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("09853608")]
        public void TestFiCheckDigit(string vatNumber)
        {
            Assert.True(_sut.FIVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("1409095C")]
        public void TestIeCheckDigit(string vatNumber)
        {
            Assert.True(_sut.IEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("00224320234")]
        public void TestItCheckDigit(string vatNumber)
        {
            Assert.True(_sut.ITVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("100000031710")]
        public void TestLtCheckDigit(string vatNumber)
        {
            Assert.True(_sut.LTVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("26375245")]
        public void TestLuCheckDigit(string vatNumber)
        {
            Assert.True(_sut.LUVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("40003032949")]
        public void TestLvCheckDigit(string vatNumber)
        {
            Assert.True(_sut.LVVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("10365719")]
        public void TestMtCheckDigit(string vatNumber)
        {
            Assert.True(_sut.MTVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("864234232")]
        public void TestNoCheckDigit(string vatNumber)
        {
            Assert.True(_sut.NOVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("5210088067")]
        public void TestPlCheckDigit(string vatNumber)
        {
            Assert.True(_sut.PLVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("13182060")]
        public void TestRoCheckDigit(string vatNumber)
        {
            Assert.True(_sut.ROVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("100010812")]
        public void TestRsCheckDigit(string vatNumber)
        {
            Assert.True(_sut.RSVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("2901081545")]
        public void TestRuCheckDigit(string vatNumber)
        {
            Assert.True(_sut.RUVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("10830316")]
        public void TestSiCheckDigit(string vatNumber)
        {
            Assert.True(_sut.SIVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("2020032377")]
        public void TestSkCheckDigit(string vatNumber)
        {
            Assert.True(_sut.SKVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("12385860076")]
        public void TestHrCheckDigit(string vatNumber)
        {
            Assert.True(_sut.HRVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("10766172")]
        public void TestHuCheckDigit(string vatNumber)
        {
            Assert.True(_sut.HUVATCheckDigit(vatNumber));
        }

        /*Example Customer Tests*/

        // If you want to test for the whole VatNumber format(including country code) you call the CheckVATNumber function - returns string
        // If you want to test for only the digit part of VatNumber, you can each individual country check digit function - returns bool

        [Theory]
        [InlineData("CHE-108.315.241", "CHE108315241")]
        public void TestChePattern(string vatNumber, string expected)
        {
            Assert.Equal(expected, _sut.CheckBvDNumber(vatNumber));
        }

        [Theory]
        [InlineData("FR955 4201 6951", "FR95542016951")]
        public void TestFrPattern(string vatNumber, string expected)
        {
            Assert.Equal(expected, _sut.CheckBvDNumber(vatNumber));
        }

        [Theory]
        [InlineData("NL802353794B01", "NL802353794B01")]
        public void TestNlCheckDigit2(string vatNumber, string expected)
        {
            Assert.Equal(expected, _sut.CheckBvDNumber(vatNumber));
        }

        [Theory]
        [InlineData("556410328001")]
        public void TestSeCheckDigit2(string vatNumber)
        {
            Assert.True(_sut.SEVATCheckDigit(vatNumber));
        }

        [Theory]
        [InlineData("B62131180")]
        public void TestEsbCheckDigit2(string vatNumber)
        {
            Assert.True(_sut.ESVATCheckDigit(vatNumber));
        }
    }
}
