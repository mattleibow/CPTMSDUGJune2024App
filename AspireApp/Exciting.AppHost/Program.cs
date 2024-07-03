var builder = DistributedApplication.CreateBuilder(args);

// Add a SQL Server database
var sqlpassword = builder.AddParameter("sqlpassword", secret: true);
var db = builder
    .AddSqlServerEdge("sqlserver", port: 1433, password: sqlpassword) // use the same port as DB tools
    .AddDatabase("excitingdb")
    .WithEndpoint(port: 14330);

// Add a database worker to prepare and migrate the database
// when new deployments are made to the application
builder.AddProject<Projects.Exciting_DatabaseWorker>("dbworker")
    .WithReference(db);

// Add the team management REST API
var teamApi = builder
    .AddProject<Projects.Exciting_TeamApi>("teamapi")
    .WithReference(db);

// Add the user-facing website
var website = builder
    .AddProject<Projects.Exciting_Website>("website")
    .WithExternalHttpEndpoints()
    .WithReference(teamApi);

// Add the mobile app
if (builder.ExecutionContext.IsRunMode)
{
    builder.AddMobileProject("mobile", "../Exciting.Mobile")
        .WithReference(teamApi);
}

var app = builder.Build();

app.Run();
