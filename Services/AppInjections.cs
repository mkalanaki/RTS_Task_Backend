using Application.Contracts;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Repositories;
using FluentValidation;
using Infrustructure.Profile;
using Microsoft.Extensions.DependencyInjection;
using Services;
using System.Text.Json.Serialization;

namespace Application;

public class AppInjections
{
    public static void ConfigureServices(IServiceCollection services)
    {

        // services.AddScoped(typeof(IBaseRepositoryAsync<>), typeof(BaseRepositoryAsync<>));

      
        //Register DTO Validators
        services.AddTransient<IValidator<CreateInDependentCreditNoteReq>, CreateInDependentCreditNoteReqValidator>();
        services.AddTransient<IValidator<CreateDependentCreditNoteReq>, CreateDependentCreditNoteReqValidator>();
        services.AddTransient<IValidator<CreateInvoiceDocumentReq>, CreateInvoiceDocumentReqValidator>();
        services.AddTransient<IValidator<UpdateInvoiceDocumentReq>, UpdateInvoiceDocumentReqValidator>();
        services.AddTransient<IValidator<UpdateDependentCreditNoteReq>, UpdateDependentCreditNoteReqValidator>();
        services.AddTransient<IValidator<UpdateInDependentCreditNoteReq>, UpdateInDependentCreditNoteReqValidator>();


        services.AddTransient<IInvoiceDocumentService, InvoiceDocumentService>();
        services.AddTransient<IInDependentCreditNoteService, InDependentCreditNoteService>();
        services.AddTransient<IDependentCreditNoteService, DependentCreditNoteService>();


        services.AddAutoMapper(typeof(MappedProfile));
        services.AddControllers()
    .AddJsonOptions(opt => { opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


        //Disable Automatic Model State Validation built-in to ASP.NET Core
        // services.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = true; });
    }
}