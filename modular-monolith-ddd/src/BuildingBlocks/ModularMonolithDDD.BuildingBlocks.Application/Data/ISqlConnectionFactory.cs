namespace ModularMonolithDDD.BuildingBlocks.Application.Data
{
	/// <summary>
	/// Interface for creating SQL connections.
	/// Connection pooling.
	/// Transaction management.
	/// Configuration.
	/// </summary>
    public interface ISqlConnectionFactory
    {
		/// <summary>
		/// Get an open connection.
		/// </summary>
		/// <returns>The open connection.</returns>
		IDbConnection GetOpenConnection();

		/// <summary>
		/// Create a new connection.
		/// </summary>
		/// <returns>The new connection.</returns>
		IDbConnection CreateNewConnection();

		/// <summary>
		/// Get the connection string.
		/// </summary>
		/// <returns>The connection string.</returns>
		string GetConnectionString();
	}
}
