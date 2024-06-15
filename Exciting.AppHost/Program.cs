var builder = DistributedApplication.CreateBuilder(args);

// TODO
var sqlpassword = builder.AddParameter("sqlpassword", secret: true);
var db = builder
    .AddSqlServerEdge("sqlserver", port: 1433, password: sqlpassword) // use the same port as DB tools
    .AddDatabase("excitingdb")
    .WithEndpoint(port: 14330);

// TODO
builder.AddProject<Projects.Exciting_DatabaseWorker>("dbworker")
    .WithReference(db);

// TODO
var teamApi = builder
    .AddProject<Projects.Exciting_TeamApi>("teamapi")
    .WithReference(db);

// TODO: 2. WEB - Add the web resource to the host builder
var website = builder
    .AddProject<Projects.Exciting_Website>("website")
    .WithExternalHttpEndpoints()
    .WithReference(teamApi);

builder.AddMobileProject("mobile", "../Exciting.Mobile")
    .WithReference(teamApi);

var app = builder.Build();

app.Run();
