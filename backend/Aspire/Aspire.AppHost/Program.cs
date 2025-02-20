var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Server>("server");

builder.Build().Run();
