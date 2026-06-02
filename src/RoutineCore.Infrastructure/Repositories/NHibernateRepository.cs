using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;
using RoutineCore.Domain.Repositories;

namespace RoutineCore.Infrastructure.Repositories
{
    public class NHibernateRepository<T> : IRepository<T> where T : class
    {
        protected readonly ISession Session;

        public NHibernateRepository(ISession session)
        {
            Session = session;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await Session.GetAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Session.QueryOver<T>().ListAsync();
        }

        public async Task SaveAsync(T entity)
        {
            using var transaction = Session.BeginTransaction();
            try
            {
                await Session.SaveOrUpdateAsync(entity);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            using var transaction = Session.BeginTransaction();
            try
            {
                await Session.UpdateAsync(entity);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            using var transaction = Session.BeginTransaction();
            try
            {
                await Session.DeleteAsync(entity);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
