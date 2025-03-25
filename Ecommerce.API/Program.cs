
using Ecommerce.API;
using Ecommerce.API.Extensions;
using Ecommerce.Application;
using Ecommerce.Application.Abstractions.Infrastructure;
using Ecommerce.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.RegisterMediator();
    builder.Services.RegisterAPIServices();
    builder.Services.RegisterDatabase(builder.Configuration);

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
catch (Exception)
{

	throw;
}
finally
{

}

