var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("fitcal-db");
builder.AddProject<Projects.FitCal_WebAPI>("fitcal-api").WithReference("fitcal-db").WaitFor(postgres);

builder.Build().Run();