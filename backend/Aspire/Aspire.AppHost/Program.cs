var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Server>("server");

await builder.Build().RunAsync();
