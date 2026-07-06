namespace ModularMonolithDDD.BuildingBlocks.Infrastructure
{
	/// <summary>
	/// Sql connection factory.
	/// </summary>
	public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
	{
		private readonly string _connectionString;
		private IDbConnection _connection = default!;

		/// <summary>
		/// Initializes a new instance of the <see cref="SqlConnectionFactory"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		public SqlConnectionFactory(string connectionString)
		{			
			this._connectionString = connectionString;
		}

		/// <summary>
		/// Get an open connection.
		/// </summary>
		/// <returns>The open connection.</returns>
		public IDbConnection GetOpenConnection()
		{			
			if (this._connection == null || this._connection.State != ConnectionState.Open)
			{
				this._connection = new SqlConnection(_connectionString);
				this._connection.Open();
			}
			return this._connection;
		}

		/// <summary>
		/// Create a new connection.
		/// </summary>
		/// <returns>The new connection.</returns>
		public IDbConnection CreateNewConnection()
		{
			var connection = new SqlConnection(_connectionString);
			connection.Open();

			return connection;
		}

		/// <summary>
		/// Get the connection string.
		/// </summary>
		/// <returns>The connection string.</returns>
		public string GetConnectionString()
		{
			return _connectionString;
		}

		/// <summary>
		/// Dispose the connection.
		/// </summary>
		public void Dispose()
		{
			if (this._connection != null && this._connection.State == ConnectionState.Open)
			{
				this._connection.Dispose();
			}
		}
	}
}
