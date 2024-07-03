namespace Aspire.Hosting;

static class DistributedApplicationBuilderExtensions
{
    public static IResourceBuilder<SqlServerServerResource> AddSqlServerEdge(this IDistributedApplicationBuilder builder, string name, IResourceBuilder<ParameterResource>? password = null, int? port = null)
    {
        // The password must be at least 8 characters long and contain characters from three of the following four sets: Uppercase letters, Lowercase letters, Base 10 digits, and Symbols
        var passwordParameter = password?.Resource ?? ParameterResourceBuilderExtensions.CreateDefaultPasswordParameter(builder, $"{name}-password", minLower: 1, minUpper: 1, minNumeric: 1);

        var sqlServer = new SqlServerServerResource(name, passwordParameter);

        return builder
            .AddResource(sqlServer)
            .WithEndpoint(port: port, targetPort: 1433, name: "tcp")
            .WithImage("azure-sql-edge", "latest")
            .WithImageRegistry("mcr.microsoft.com")
            .WithEnvironment("ACCEPT_EULA", "Y")
            .WithEnvironment("MSSQL_USER", "sa")
            .WithEnvironment("MSSQL_PID", "Developer")
            .WithEnvironment(context =>
            {
                context.EnvironmentVariables["MSSQL_SA_PASSWORD"] = sqlServer.PasswordParameter;
            });
    }
}
