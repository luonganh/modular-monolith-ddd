// File: BuildingBlocks/Tests/IntegrationTests/Probing/IProbe.cs
// Summary: Basic probing contract for eventual consistency tests.
// AssertEventually: Check multiple times within a period of time. Only fail if it is still wrong after the timeout.

namespace ModularMonolithDDD.BuildingBlocks.Tests.IntegrationTests.Probing
{
    /// <summary>
    /// A class that describes “what condition I need to wait for.”
    /// </summary>
    public interface IProbe
    {
        /// <summary>
        /// check whether that data is correct yet
        /// </summary>        
        /// <returns></returns>
        bool IsSatisfied();

        /// <summary>
        /// fetch the latest data
        /// </summary>
        /// <returns></returns>
        Task SampleAsync();

        /// <summary>
        /// if it still isn't correct after waiting, print a clear error message
        /// </summary>
        /// <returns></returns>
        string DescribeFailureTo();
    }    
}
