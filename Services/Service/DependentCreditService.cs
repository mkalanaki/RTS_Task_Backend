using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
    public class DependentCreditNoteService : IDependentCreditNoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<DependentCreditNoteService> _logger;

        public DependentCreditNoteService(IUnitOfWork unitOfWork,
            ILogger<DependentCreditNoteService> logger, IRepositoryBase<DependentCreditNote> DependentCreditNote)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DependentCreditNote> Create(DependentCreditNote entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.Create is running query");

                var user = await _unitOfWork.GetRepository<DependentCreditNote>().Create(entity);

                await _unitOfWork.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"DependentCreditNoteService.Create Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }


        public async Task<IQueryable<DependentCreditNote>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.GetAllAsync is running query");

                var inDependentCreditNotes = await _unitOfWork.GetRepository<DependentCreditNote>().GetAllAsync();
                return inDependentCreditNotes;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"DependentCreditNoteService.GetAllAsync Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public async Task<DependentCreditNote> GetByIdAsync(long docNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.GetByIdAsync is running query");

                var inDependentCreditNote = await _unitOfWork.GetRepository<DependentCreditNote>().GetByIdAsync(docNumber);

                return inDependentCreditNote;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError($"DependentCreditNoteService.GetByIdAsync Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public void Update(DependentCreditNote entity)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.Update is running query");


                Expression<Func<DependentCreditNote, bool>> predicate = e => e.Id == entity.Id;

                _unitOfWork.GetRepository<DependentCreditNote>().Update(predicate, entity);

                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 _unitOfWork.Rollback();
                _logger.LogError($"DependentCreditNoteService.Update Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public void Delete(DependentCreditNote entity)
        {
            try
            {
                _logger.LogInformation($"DependentCreditNoteService.Delete is running query");

                _unitOfWork.GetRepository<DependentCreditNote>().Delete(entity);
                _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 _unitOfWork.Rollback();
                _logger.LogError($"DependentCreditNoteService.Delete Exception: {ex}");
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IEnumerable<DependentCreditNote>> FindAsync(Expression<Func<DependentCreditNote, bool>> predicate)
        {
            return await _unitOfWork.GetRepository<DependentCreditNote>().FindAsync(predicate);
        }
    }
}