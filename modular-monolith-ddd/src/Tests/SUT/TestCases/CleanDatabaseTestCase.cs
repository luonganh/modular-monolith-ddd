namespace ModularMonolithDDD.Tests.SUT.TestCases
{
    public class CleanDatabaseTestCase : TestBase
    {
        protected override bool PerformDatabaseCleanup => true;
    }
}
