using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrustructure.Swagger;

internal class RegisterSwagger

{
    public void RegisterAppServices(IServiceCollection services, IConfiguration config)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        });


        services.AddSwaggerGen(options =>
        {
            options.DocInclusionPredicate((docname, apidesc) =>
            {
                var assemblyname = ((ControllerActionDescriptor) apidesc.ActionDescriptor).ControllerTypeInfo
                    .Assembly.GetName().Name;
                var currentassemblyname = GetType().Assembly.GetType().Name;
                return currentassemblyname == assemblyname;
            });

            options.SwaggerDoc("v1",
                new OpenApiInfo {Title = "RTS Apis", Version = "v1"});
        });
    }
}