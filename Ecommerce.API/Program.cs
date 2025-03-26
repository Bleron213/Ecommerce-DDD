
using Admin.API.Handlers;
using Ecommerce.API;
using Ecommerce.API.Extensions;
using Ecommerce.Application;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");
Log.Information("Launching: Ecommerce Api");

var builder = WebApplication.CreateBuilder(args);

try
{

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
            .WithDefaultDestructurers()
            .WithDestructurers([new DbUpdateExceptionDestructurer()])
        )
        .Enrich.FromLogContext()
        .Enrich.With<ActivityEnricher>()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddControllers()
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    
    builder.Services.AddOpenApi();
    builder.Services.RegisterMediator();
    builder.Services.RegisterAPIServices();
    builder.Services.RegisterDatabase(builder.Configuration);
    builder.Services.RegisterRepositories();

    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services
    .AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    var app = builder.Build();

    app.UseExceptionHandler();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.Title = "Ecommerce API";
        });
    }


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();


    using var scope = app.Services.CreateScope();

    Seeder seeder = new Seeder(scope.ServiceProvider.GetRequiredService<IEcommerceDbContext>(), scope.ServiceProvider.GetRequiredService<ICurrentUserService>());
    seeder.Seed();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception while bootstrapping application");
}
finally
{
    Log.Information("Shutting down...");
    Log.CloseAndFlush();
}

