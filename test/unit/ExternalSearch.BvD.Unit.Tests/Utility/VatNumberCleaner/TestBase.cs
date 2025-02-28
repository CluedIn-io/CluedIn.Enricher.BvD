namespace CluedIn.ExternalSearch.BvD.Unit.Tests.Utility.BvDNumberCleaner
{
    public abstract class TestBase
    {
        protected readonly Providers.BvD.Utility.BvDNumberCleaner Sut;

        protected TestBase()
        {
            Sut = new Providers.BvD.Utility.BvDNumberCleaner();
        }
    }
}
