using Application.Models.Requests;
using Domain.Entities;

namespace Infrustructure.Profile;

using Application.Models.Responses;
using AutoMapper;

public class MappedProfile : Profile
{
    public MappedProfile()
    {
        CreateMap<DependentCreditNote, CreateDependentCreditNoteReq>().ReverseMap();
        CreateMap<InvoiceDocument, CreateInvoiceDocumentReq>().ReverseMap();
        CreateMap<InvoiceDocument, UpdateInvoiceDocumentReq>().ReverseMap();
        CreateMap<InvoiceDocument, InvoiceDocumentResponse>().ReverseMap();
        CreateMap<InDependentCreditNote, CreateInDependentCreditNoteReq>().ReverseMap();
        CreateMap<InDependentCreditNote, InDependentCreditNoteResponse>().ReverseMap();
        CreateMap<UpdateInDependentCreditNoteReq, InDependentCreditNote>().ReverseMap();
        CreateMap<DependentCreditNote, DependentCreditNoteResponse>().ReverseMap();
        
    }
}