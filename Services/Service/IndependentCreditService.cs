using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Contracts;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class InDependentCreditNoteService : IInDependentCreditNoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        private ILogger<InDependentCreditNoteService> _logger;


        public InDependentCreditNoteService(IUnitOfWork unitOfWork,
            ILogger<InDependentCreditNoteService> logger)
        {
            _unitOfWork = unitOfWork;

            _logger = logger;
        }


        public async Task<InDependentCreditNote> Create(InDependentCreditNote entity,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.Create is running query");

                var user = await _unitOfWork.GetRepository<InDependentCreditNote>().Create(entity);

                await _unitOfWork.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"InvoiceDocumentService.Create Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }


        public async Task<IQueryable<InDependentCreditNote>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"InDependentCreditNoteService.GetAllAsync is running query");

                var inDependentCreditNotes = await _unitOfWork.GetRepository<InDependentCreditNote>().GetAllAsync();
                return inDependentCreditNotes;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"InDependentCreditNoteService.GetAllAsync Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public async Task<InDependentCreditNote> GetByIdAsync(long docNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"InDependentCreditNoteService.GetByIdAsync is running query");

                var inDependentCreditNote = await _unitOfWork.GetRepository<InDependentCreditNote>().GetByIdAsync(docNumber);

                return inDependentCreditNote;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"InDependentCreditNoteService.GetByIdAsync Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public void Update(InDependentCreditNote entity)
        {
            try
            {
                _logger.LogInformation($"InDependentCreditNoteService.Update is running query");


                Expression<Func<InDependentCreditNote, bool>> predicate = e => e.Id == entity.Id;

                _unitOfWork.GetRepository<InDependentCreditNote>().Update(predicate, entity);

                _unitOfWork.SaveChangesAsync();
               
            }
            catch (Exception ex)
            {
                 _unitOfWork.Rollback();
                _logger.LogError($"InDependentCreditNoteService.Update Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public void Delete(InDependentCreditNote entity)
        {
            try
            {
                _logger.LogInformation($"InDependentCreditNoteService.Delete is running query");

                _unitOfWork.GetRepository<InDependentCreditNote>().Delete(entity);

                _unitOfWork.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                 _unitOfWork.Rollback();
                _logger.LogError($"InDependentCreditNoteService.Delete Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

      
       
        

        public async Task<IEnumerable<InDependentCreditNote>> FindAsync(Expression<Func<InDependentCreditNote, bool>> predicate)
        {
            return null;
        }
    }
}