using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Zach.MinimalValidators.Core.Extensions.AspNetCore;
using Zach.MinimalValidators.Core.Implementation.DI;
using Zach.MinimalValidators.Library;
using Zach.MinimalValidators.Library.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddContainer<DIContainer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/note/{id}", ([Invalid][FromRoute] string id) =>
{
    return Results.Ok(id);
}).WithValidationFilter()
.Produces<string>(StatusCodes.Status200OK)
.Produces<List<ValidationResult>>(StatusCodes.Status400BadRequest);

app.Run();
