var builder = DistributedApplication.CreateBuilder(args);

var sqlpassword = builder.AddParameter("sqlpassword", secret: true);
var db = builder
    .AddSqlServerEdge("sqlserver", port: 1433, password: sqlpassword) // use the same port as DB tools
    .AddDatabase("excitingdb")
    .WithEndpoint(port: 14330);

builder.AddProject<Projects.Exciting_DatabaseWorker>("dbworker")
    .WithReference(db);

var teamApi = builder
    .AddProject<Projects.Exciting_TeamApi>("teamapi")
    .WithReference(db);

var website = builder
    .AddProject<Projects.Exciting_Website>("website")
    .WithExternalHttpEndpoints()
    .WithReference(teamApi);

if (builder.ExecutionContext.IsRunMode)
{
    builder.AddMobileProject("mobile", "../Exciting.Mobile")
        .WithReference(teamApi);
}

var app = builder.Build();

app.Run();
