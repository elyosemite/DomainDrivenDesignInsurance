using DomainDrivenDesignInsurance.API;
using DomainDrivenDesignInsurance.Application;
using DomainDrivenDesignInsurance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});

app.MapPolicyEndpointMap();

app.Run();
