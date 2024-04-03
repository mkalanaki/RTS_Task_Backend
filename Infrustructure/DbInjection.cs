using Application.UnitOfWork;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using Infrustructure.Swagger;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using Infrustructure.Contracts;

namespace Infrustructure;

public class DbInjection
{
    private readonly IConfiguration configuration;

    private IConfiguration GetConfiguration()
    {
        return configuration;
    }

    public DbInjection(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public static void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
    {
        services.AddAutoMapper(typeof(StartupBase));
        services.AddDbContext<RTSDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly("Infrustructure")));

        services.AddDbContext<RTSDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
       
    });

        services.Configure<CorsSettings>(Configuration.GetSection("CorsSettings"));



        services.AddCors(option =>
        {
            option.AddPolicy("CorsPolicy",
                builder =>
                {
                    var corsSettings = Configuration.GetSection("CorsSettings").Get<CorsSettings>();
                    builder.WithOrigins(corsSettings.AllowedOrigins.ToArray())
                           .WithHeaders(corsSettings.AllowedHeaders.ToArray())
                           .WithMethods(corsSettings.AllowedMethods.ToArray());
                });

        });

        services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; });


        services.AddScoped<Func<RTSDbContext>>((provider) => () => provider.GetService<RTSDbContext>());

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryManager<>));
       


        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });


        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "RTS TASK API",
                Description = "An ASP.NET Core Web APIs ",
                TermsOfService = new Uri("https://example.com/terms"),
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        });
    }

    public void Onconfiguration(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        //optionsBuilder.UseSqlServer("Data Source=DESKTOP-73KNC10\\SQLEXPRESS;Database=RTSBD;User ID=sa;Password=123456");
    }

    public static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<RTSDbContext>>();

        using (var dbContext = new RTSDbContext(dbContextOptions))
        {
            dbContext.Database.Migrate();
        }
    }
}