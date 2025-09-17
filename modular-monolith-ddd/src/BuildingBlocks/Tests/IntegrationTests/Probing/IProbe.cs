// File: BuildingBlocks/Tests/IntegrationTests/Probing/IProbe.cs
// Summary: Basic probing contract for eventual consistency tests.
namespace ModularMonolithDDD.BuildingBlocks.Tests.IntegrationTests.Probing
{
    public interface IProbe
    {
        bool IsSatisfied();

        Task SampleAsync();

        string DescribeFailureTo();
    }    
}
