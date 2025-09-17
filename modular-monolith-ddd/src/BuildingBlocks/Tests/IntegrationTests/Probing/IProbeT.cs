// File: BuildingBlocks/Tests/IntegrationTests/Probing/IProbeT.cs
// Summary: Generic probing contract returning samples and satisfaction
// predicate—used by `Poller` to await eventual outcomes.
namespace ModularMonolithDDD.BuildingBlocks.Tests.IntegrationTests.Probing
{
    public interface IProbe<T>
    {
        bool IsSatisfied(T sample);

        Task<T> GetSampleAsync();

        string DescribeFailureTo();
    }
}
