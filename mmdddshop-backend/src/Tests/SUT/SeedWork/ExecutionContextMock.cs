// File: Tests/SUT/SeedWork/ExecutionContextMock.cs
// Purpose: Lightweight implementation of `IExecutionContextAccessor`
// for tests. Allows setting a synthetic `UserId` and is safe to
// use across test runs.
namespace ModularMonolithDDD.Tests.SUT.SeedWork
{
    public class ExecutionContextMock : IExecutionContextAccessor
    {
        public ExecutionContextMock(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }

        public Guid CorrelationId { get; }

        public bool IsAvailable { get; }

        public void SetUserId(Guid userId)
        {
            this.UserId = userId;
        }
    }
}
