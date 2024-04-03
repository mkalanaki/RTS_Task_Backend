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

namespace RTS.Api.Controllers
{
    [Route("api/v1/dependent-credit/")]
    public class DependentCreditNoteController : ControllerBase
    {
        private readonly ILogger<DependentCreditNote> _logger;
        private readonly IDependentCreditNoteService _DependentCreditNoteService;
        private readonly IInvoiceDocumentService _invoiceDocumentService;
        private readonly IMapper _mapper;


        public DependentCreditNoteController(ILogger<DependentCreditNote> logger,
            IDependentCreditNoteService DependentCreditNoteService, IMapper mapper,
            IInvoiceDocumentService invoiceDocumentService)
        {
            _logger = logger;
            _DependentCreditNoteService = DependentCreditNoteService;
            _mapper = mapper;
            _invoiceDocumentService = invoiceDocumentService;
        }

        [Route("{creditNumber}")]
        [HttpGet]
        public async Task<ApiResponse> GetDependentById([FromRoute] long creditNumber)
        {
            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("creditNumbe is not valid or empty",
                    Status406NotAcceptable);
            }


            try
            {
                var dependetCreditNote = await _DependentCreditNoteService.GetByIdAsync(creditNumber);
                
                if (dependetCreditNote is null)
                    return new ApiResponse("DependentCredi",
                        Status404NotFound);
               
                  var mappedEntity = _mapper.Map<DependentCreditNote, DependentCreditNoteResponse>(dependetCreditNote);

                return new ApiResponse("DependentCredi", mappedEntity, Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse("DependentCredi", new ProblemDetails
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
        public async Task<ApiResponse> GetAllDependentCreditNotes()
        {
            try
            {
                var totalDocument = await _DependentCreditNoteService.GetAllAsync(CancellationToken.None);
                if (totalDocument is null)
                {
                    return new ApiResponse("DependentCredi has not found",
                        Status404NotFound);
                }


                var mappedEntity =
                    _mapper.Map<List<DependentCreditNote>, List<DependentCreditNoteResponse>>(totalDocument.ToList());

                return new ApiResponse("DependentCredi", mappedEntity.ToArray(), Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse("DependentCredi", new ProblemDetails
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
        public async Task<ApiResponse> CreateDependentCreditNote([FromBody] CreateDependentCreditNoteReq createDependentCreditNote)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            try
            {
                if (createDependentCreditNote.ParentInvoiceNumber <= 0)
                {
                    return new ApiResponse("DependentCredi has not valid Input in ParentInvoiceNumber ",
                        Status404NotFound);
                }

                var parentInvoceDocument =
                    await _invoiceDocumentService.GetByIdAsync(createDependentCreditNote.ParentInvoiceNumber);
                if(parentInvoceDocument is null)
                {
                    return new ApiResponse(" ParentInvoiceNumber dosen't exist",
                        Status404NotFound);
                }

                var mappedEntity = _mapper.Map<CreateDependentCreditNoteReq, DependentCreditNote>(createDependentCreditNote);
                mappedEntity.InvoiceDocument = parentInvoceDocument;

                var result = await _DependentCreditNoteService.Create(mappedEntity);
                return new ApiResponse("Created", mappedEntity, Status201Created);
            }

            catch (Exception ex)
            {
                return new ApiResponse("DependentCreditNote", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "CreateDependentCreditNote",
                    Status = Status500InternalServerError,
                    Title = "Error in Creating Data",
                    Type = "Error"
                });
            }
        }


        [HttpPut]
        [Route("{creditNumber}")]
        public async Task<ApiResponse> UpdateDependentCreditNote([FromRoute] long creditNumber,
            [FromBody] UpdateDependentCreditNoteReq DependentCreditNoteReq)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("credit Number is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                var currentEntity = await _DependentCreditNoteService.GetByIdAsync(creditNumber);
                if (currentEntity is null)
                    return new ApiResponse("creditNumbe is not exists",
                        Status404NotFound);

                currentEntity.CreditNumber = DependentCreditNoteReq.CreditNumber;
                currentEntity.ExternalCreditNumber = DependentCreditNoteReq.ExternalCreditNumber;
                currentEntity.TotalAmount = DependentCreditNoteReq.TotalAmount;
                currentEntity.CreditStatus = DependentCreditNoteReq.CreditStatus;

                var checkInvoceDocument =
                    await _invoiceDocumentService.GetByIdAsync(DependentCreditNoteReq.ParentInvoiceNumber);

                if (checkInvoceDocument is null)
                    return new ApiResponse("ParentInvoiceNumber  is not valid or empty",
                        Status406NotAcceptable);

                currentEntity.ParentInvoiceNumber = DependentCreditNoteReq.ParentInvoiceNumber;
                currentEntity.InvoiceDocument = checkInvoceDocument;

                _DependentCreditNoteService.Update(currentEntity);
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
        [Route("{creditNumber}")]
        public async Task<ApiResponse> DeleteDependentCreditNote([FromRoute] long creditNumber)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("creditNumber is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                var currentEntity = await _DependentCreditNoteService.GetByIdAsync(creditNumber);
                if (currentEntity is null)
                    return new ApiResponse("creditNumber is not exists",
                        Status404NotFound);


                _DependentCreditNoteService.Delete(currentEntity);
                return new ApiResponse("Deleted", currentEntity, Status200OK);
            }

            catch (Exception ex)
            {
                return new ApiResponse("DependentCreditNote", new ProblemDetails
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