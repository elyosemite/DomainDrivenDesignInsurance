using DomainDrivenDesignInsurance.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApi();

var app = builder.Build();

app.UseApi();

app.Run();
