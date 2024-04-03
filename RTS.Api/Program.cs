using FluentValidation.AspNetCore;
using Infrustructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.ImplicitlyValidateChildProperties = true;
        v.ImplicitlyValidateRootCollectionElements = true;
        v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    });


Application.AppInjections.ConfigureServices(builder.Services);


var appSettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();


DbInjection.ConfigureServices(builder.Services, appSettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseSwagger();

app.UseSwaggerUI();
app.UseCors("CorsPolicy");

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "RTS APIs Test");
    options.RoutePrefix = string.Empty; // Set the Swagger UI at the root URL
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    DbInjection.MigrateDatabase(scope.ServiceProvider);
}

app.UseAuthorization();

app.Run();