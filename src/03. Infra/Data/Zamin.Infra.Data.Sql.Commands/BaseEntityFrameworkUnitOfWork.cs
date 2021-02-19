﻿using Zamin.Core.Domain.Data;
using Zamin.Utilities;
using System.Threading.Tasks;

namespace Zamin.Infra.Data.Sql.Commands
{
    public abstract class BaseEntityFrameworkUnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : BaseCommandDbContext
    {
        protected readonly TDbContext _dbContext;
        protected readonly ZaminServices _hamoonApplicationContext;

        public BaseEntityFrameworkUnitOfWork(TDbContext dbContext, ZaminServices hamoonApplicationContext)
        {
            _dbContext = dbContext;
            _hamoonApplicationContext = hamoonApplicationContext;
        }

        public void BeginTransaction()
        {
            _dbContext.BeginTransaction();
        }

        public int Commit()
        {
            var result = _dbContext.SaveChanges();
            return result;
        }

        public async Task<int> CommitAsync()
        {
            var result = await _dbContext.SaveChangesAsync();
            return result;
        }

        public void CommitTransaction()
        {
            _dbContext.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _dbContext.RollbackTransaction();
        }
    }
}
