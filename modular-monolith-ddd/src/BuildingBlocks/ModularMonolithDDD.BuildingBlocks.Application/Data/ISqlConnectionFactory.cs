namespace ModularMonolithDDD.BuildingBlocks.Application.Data
{
    public interface ISqlConnectionFactory
    {
		IDbConnection GetOpenConnection();

		IDbConnection CreateNewConnection();

		string GetConnectionString();
	}
}
