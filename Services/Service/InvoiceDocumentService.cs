using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts;
using Application.Helper;
using Application.Models.Requests;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class InvoiceDocumentService : IInvoiceDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private ILogger<InvoiceDocumentService> _logger;


        public InvoiceDocumentService(IUnitOfWork unitOfWork,
            ILogger<InvoiceDocumentService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }


        public async Task<InvoiceDocument> Create(InvoiceDocument entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"InvoiceDocumentService.Create is running query");

                var user = await _unitOfWork.GetRepository<InvoiceDocument>().Create(entity);

                await _unitOfWork.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"InvoiceDocumentService.Create Exception: {ex}");
                await _unitOfWork.Rollback();
                throw new Exception(ex.ToString());
            }
        }


        public async Task<IQueryable<InvoiceDocument>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"DependentCreditNoteService.Create is running query");

            var invDocument = await _unitOfWork.GetRepository<InvoiceDocument>().GetAllAsync();

            return invDocument;
        }

        public async Task<InvoiceDocument> GetByIdAsync(long docNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"InvoiceDocumentService.GetAllAsync is running query");

                var invDocument = await _unitOfWork.GetRepository<InvoiceDocument>().GetByIdAsync(docNumber, cancellationToken);

                return invDocument;
            }
            catch (Exception ex)
            {

                _logger.LogError($"InvoiceDocumentService.GetAllAsync Exception: {ex}");
                await _unitOfWork.Rollback();
                throw new Exception(ex.ToString());
            }
        }

        public void Update(InvoiceDocument entity)
        {
            try
            {
                _logger.LogInformation($"InvoiceDocumentService.Update is running query");


                Expression<Func<InvoiceDocument, bool>> predicate = e => e.Id == entity.Id;

                _unitOfWork.GetRepository<InvoiceDocument>().Update(predicate, entity);

                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InvoiceDocumentService.Update Exception: {ex}");
                 _unitOfWork.Rollback();
                throw new Exception(ex.ToString());
            }
        }

        public void Delete(InvoiceDocument entity)
        {
            try
            {
                _logger.LogInformation($"InvoiceDocumentService.Delete is running query");


                _unitOfWork.GetRepository<InvoiceDocument>().Delete(entity);
                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 _unitOfWork.Rollback();
                _logger.LogError($"InvoiceDocumentService.Delete Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public Task<InvoiceDocument> GetByIdAsync(long id, params Expression<Func<InvoiceDocument, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InvoiceDocument>> FindAsync(Expression<Func<InvoiceDocument, bool>> predicate)
        {
           return await _unitOfWork.GetRepository<InvoiceDocument>().FindAsync(predicate);
              
        }
    }
}