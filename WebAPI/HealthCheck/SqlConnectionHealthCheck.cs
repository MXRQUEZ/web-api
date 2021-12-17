using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.HealthCheck
{
    public class SqlConnectionHealthCheck : IHealthCheck
    {
        private const string DefaultTestQuery = "Select 1";

        public SqlConnectionHealthCheck(string connectionString)
            : this(connectionString, DefaultTestQuery)
        {
        }

        public SqlConnectionHealthCheck(string connectionString, string testQuery)
        {
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            TestQuery = testQuery;
        }

        public string ConnectionString { get; }

        public string TestQuery { get; }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    if (TestQuery != null)
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = TestQuery;

                        await command.ExecuteNonQueryAsync(cancellationToken);
                    }
                }
                catch (DbException ex)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
                }
            }

            return HealthCheckResult.Healthy();
        }
    }
}