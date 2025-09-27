var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PatientHealthRecord_Web>("web");

builder.Build().Run();
