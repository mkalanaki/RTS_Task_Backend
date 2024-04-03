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
    [Route("api/v1/independent-credit/")]
    public class InDependentCreditNoteController : ControllerBase
    {
        private readonly ILogger<InDependentCreditNote> _logger;
        private readonly IInDependentCreditNoteService _independentCreateService;
        private readonly IMapper _mapper;


        public InDependentCreditNoteController(ILogger<InDependentCreditNote> logger,
            IInDependentCreditNoteService inDependentCreditNoteService, IMapper mapper)
        {
            _logger = logger;
            _independentCreateService = inDependentCreditNoteService;
            _mapper = mapper;
        }

        [Route("{creditNumber}")]
        [HttpGet]
        public async Task<ApiResponse> GetIndependentCreditNoteById([FromRoute] long creditNumber)
        {
            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("creditNumber is not valid or empty",
                    Status406NotAcceptable);
            }


            try
            {
                var inDependentCreditNote = await _independentCreateService.GetByIdAsync(creditNumber);
                if (inDependentCreditNote is null)
                    return new ApiResponse("IndependentCreditNote",
                        Status404NotFound);

                var mappedEntity = _mapper.Map<InDependentCreditNote, InDependentCreditNoteResponse>(inDependentCreditNote);

                return new ApiResponse("IndependentCredi", mappedEntity, Status200OK);
            }
            catch (Exception ex)
            {
                return new ApiResponse("IndependentCredi", new ProblemDetails
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
        public async Task<ApiResponse> GetAllIndependentCreditNotes()
        {
            try
            {
                var totalDocument = await _independentCreateService.GetAllAsync(CancellationToken.None);
                if (totalDocument is null)
                {
                    return new ApiResponse("IndependentCreditNote has not found",
                        Status404NotFound);
                }


                var mappedEntity =
                    _mapper.Map<List<InDependentCreditNote>, List<InDependentCreditNoteResponse>>(totalDocument.ToList());

                return new ApiResponse("IndependentCreditNote", mappedEntity, Status200OK);
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
        public async Task<ApiResponse> CreateInependentCredit([FromBody] CreateInDependentCreditNoteReq indpependentReq)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            try
            {
                var mappedEntity = _mapper.Map<CreateInDependentCreditNoteReq, InDependentCreditNote>(indpependentReq);
               


                 var result = await _independentCreateService.Create(mappedEntity);
                return new ApiResponse("Created", mappedEntity, Status201Created);
            }

            catch (Exception ex)
            {
                return new ApiResponse("IndependentCreditNote", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "IndependentCreditNote",
                    Status = Status500InternalServerError,
                    Title = "Error in Creating Data",
                    Type = "Error"
                });
            }
        }


        [HttpPut]
        [Route("{creditNumber}")]
        public async Task<ApiResponse> UpdateIndependentCreditNote([FromRoute] long creditNumber,
            [FromBody] UpdateInDependentCreditNoteReq DependentCreditNoteReq)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("creditNumber  is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                var currentEntity = await _independentCreateService.GetByIdAsync(creditNumber);
                if (currentEntity is null)
                    return new ApiResponse("creditNumber is not exists",
                        Status404NotFound);

                
                var mappedEntity = _mapper.Map<UpdateInDependentCreditNoteReq, InDependentCreditNote>(DependentCreditNoteReq);
                mappedEntity.Id = currentEntity.Id;
                mappedEntity.CreditNumber = currentEntity.CreditNumber;

                _independentCreateService.Update(mappedEntity);
                return new ApiResponse("Updated", mappedEntity, Status200OK);
            }

            catch (Exception ex)
            {
                return new ApiResponse("IndependentCreditNotete", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "UpdateIndependentCreditNote",
                    Status = Status500InternalServerError,
                    Title = "Error in updating Data",
                    Type = "Error"
                });
            }
        }


        [HttpDelete]
        [Route("{creditNumber}")]
        public async Task<ApiResponse> DeleteIndependentCreditNote([FromRoute] long creditNumber)
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = (ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage));
                throw new ApiProblemDetailsException(ModelState);
            }

            if (creditNumber.ToString().Length != 10)
            {
                return new ApiResponse("IndependentCreditNote is not valid or empty",
                    Status406NotAcceptable);
            }

            try
            {
                var currentEntity = await _independentCreateService.GetByIdAsync(creditNumber);
                if (currentEntity is null)
                    return new ApiResponse("IndependentCreditNote is not exists",
                        Status404NotFound);


                _independentCreateService.Delete(currentEntity);
                return new ApiResponse("Deleted", currentEntity, Status200OK);
            }

            catch (Exception ex)
            {
                return new ApiResponse("IndependentCreditNote", new ProblemDetails
                {
                    Detail = ex.ToString(),
                    Instance = "DeleteIndependentCreditNote",
                    Status = Status500InternalServerError,
                    Title = "Error in updating Data",
                    Type = "Error"
                });
            }
        }
    }
}