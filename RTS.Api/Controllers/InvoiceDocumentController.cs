using Application.Models.Requests;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using AutoWrapper.Wrappers;
using static Microsoft.AspNetCore.Http.StatusCodes;
using AutoWrapper.Models;
using Application.Models.Responses;
using Application.Contracts;
using Domain.Enums;

namespace RTS.Api.Controllers
{
    [Route("api/v1/invoice-document/")]
    public class InvoiceDocumentController : ControllerBase
    {
        private readonly ILogger<InvoiceDocument> _logger;
        private readonly IInvoiceDocumentService _invoiceDocument;
        private readonly IDependentCreditNoteService _dependentNoteService;
        private readonly IMapper _mapper;


        public InvoiceDocumentController(ILogger<InvoiceDocument> logger,
            IInvoiceDocumentService invoiceDocument, IDependentCreditNoteService dependentNoteService,IMapper mapper)
        {
            _logger = logger;
            _invoiceDocument = invoiceDocument;
            _mapper = mapper;
            _dependentNoteService = dependentNoteService;
        }

        [Route("{docNumber}")]
        [HttpGet]
        public async Task<ApiResponse> GetInvoiceDocument([FromRoute] long docNumber)
        {
            if (docNumber == null || docNumber.ToString().Length != 10)
            {
                return new ApiResponse("Document Number is not valid or empty",
                    Status406NotAcceptable);
            }


            try
            {
                var invoiceDocument = await _invoiceDocument.GetByIdAsync(docNumber);
               
                if (invoiceDocument == null)
                    return new ApiResponse("InvoiceDocument",
                        Status404NotFound);
                var dependentNote = await _dependentNoteService.FindAsync(i => i.ParentInvoiceNumber == docNumber);

                var mappedEntity = _mapper.Map<InvoiceDocument, InvoiceDocumentResponse>(invoiceDocument);

                if (dependentNote != null)
                {
                    foreach (var item in dependentNote)
                    {
                        item.InvoiceDocument = null;
                    }
                   mappedEntity.DependentCreditNote= dependentNote;
                }

                return new ApiResponse("InvoiceDocument", mappedEntity, Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse("InvoiceDocument", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "GetInvoiceDocument",
                    Status = Status500InternalServerError,
                    Title = "Error in loading Data",
                    Type = "Error"
                });
            }
        }

        [Route("total")]
        [HttpGet]
        public async Task<ApiResponse> GetAllInvoiceDocument()
        {
            try
            {
                var totalDocument = await _invoiceDocument.GetAllAsync(CancellationToken.None);
                if (totalDocument == null)
                {
                    return new ApiResponse("InvoiceDocument has not found",
                        Status404NotFound);
                }

                foreach (var item in totalDocument)
                {
                    var dependentNote = await _dependentNoteService.FindAsync(i => i.ParentInvoiceNumber == item.InvoiceNumber);
                    foreach (var dep in dependentNote)
                    {
                        dep.InvoiceDocument = null;
                    }
                    item.DependentCreditNote = dependentNote.ToList();
                   
                }
              
               

                var mappedEntity =
                    _mapper.Map<List<InvoiceDocument>, List<InvoiceDocumentResponse>>(totalDocument.ToList());

                return new ApiResponse("InvoiceDocument", mappedEntity, Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse("InvoiceDocument", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "GetAllInvoiceDocument",
                    Status = Status500InternalServerError,
                    Title = "Error in loading Data",
                    Type = "Error"
                });
            }
        }


        [HttpPost]
        public async Task<ApiResponse> CreateInvoiceDocument([FromBody] CreateInvoiceDocumentReq invoiceDocument)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            try
            {
                var mappedEntity = _mapper.Map<CreateInvoiceDocumentReq, InvoiceDocument>(invoiceDocument);

                var result = await _invoiceDocument.Create(mappedEntity);
                return new ApiResponse("Created", mappedEntity, Status201Created);
            }

            catch (Exception ex)
            {
                return new ApiResponse("InvoiceDocument", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "CreateInvoiceDocument",
                    Status = Status500InternalServerError,
                    Title = "Error in Creating Data",
                    Type = "Error"
                });
            }
        }


        [HttpPut]
        [Route("{docNumber}")]
        public async Task<ApiResponse> UpdateInvoiceDocument([FromRoute] long docNumber,
            [FromBody] UpdateInvoiceDocumentReq invoiceDocument)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (docNumber.ToString().Length != 10)
            {
                return new ApiResponse("Document Number is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                

                var currentEntity = await _invoiceDocument.GetByIdAsync(docNumber);
                if (currentEntity is null)
                    return new ApiResponse("Document Number is not exists",
                        Status404NotFound);

                var dependentInvoiceDocument = await _dependentNoteService.FindAsync(x=>x.ParentInvoiceNumber== docNumber);

               
                var mappedEntity = _mapper.Map<UpdateInvoiceDocumentReq, InvoiceDocument>(invoiceDocument);
                mappedEntity.InvoiceNumber = currentEntity.InvoiceNumber;
                mappedEntity.Id = currentEntity.Id;

                //To Approved when InvoceDocument Change To approved
                if (invoiceDocument.InvoiceStatus == SubmitStatus.Approved)
                {
                    foreach (var dependentCreditNote in dependentInvoiceDocument.ToList())
                    { dependentCreditNote.CreditStatus = SubmitStatus.Approved;
                        _dependentNoteService.Update(dependentCreditNote);
                    }
                }
            
                    _invoiceDocument.Update(mappedEntity);
                     

                return new ApiResponse("Updated", currentEntity, Status200OK);
            }

            catch (Exception ex)
            {
                return new ApiResponse("InvoiceDocument", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "UpdateInvoiceDocument",
                    Status = Status500InternalServerError,
                    Title = "Error in updating Data",
                    Type = "Error"
                });
            }
        }


        [HttpDelete]
        [Route("{docNumber}")]
        public async Task<ApiResponse> DeleteInvoiceDocument([FromRoute] long docNumber)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (docNumber.ToString().Length != 10)
            {
                return new ApiResponse("Document Number is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                var currentEntity = await _invoiceDocument.GetByIdAsync(docNumber);
                if (currentEntity is null)
                    return new ApiResponse("Document Number is not exists",
                        Status404NotFound);

                var dependentInvoiceDocument = await _dependentNoteService.FindAsync(x => x.ParentInvoiceNumber == docNumber);

                foreach (var dependentCreditNote in dependentInvoiceDocument.ToList())
                    {
                       
                        _dependentNoteService.Delete(dependentCreditNote);
                    }
                

                _invoiceDocument.Delete(currentEntity);
                return new ApiResponse("Deleted", currentEntity, Status200OK);
            }

            catch (Exception ex)
            {
                return new ApiResponse("InvoiceDocument", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "DeleteInvoice",
                    Status = Status500InternalServerError,
                    Title = "Error in updating Data",
                    Type = "Error"
                });
            }
        }
    }
}