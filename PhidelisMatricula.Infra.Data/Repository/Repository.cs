using System;
using System.Linq;
using System.Threading.Tasks;
using PhidelisMatricula.Domain.Core.Interfaces;
using PhidelisMatricula.Domain.Core.Models;
using PhidelisMatricula.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace PhidelisMatricula.Infra.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(ApplicationDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task Add(TEntity obj)
        {
            await DbSet.AddAsync(obj);
        }

        public virtual async Task<TEntity> GetByIdAsync(long id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }

        public virtual async Task RemoveAsync(long id)
        {
            var obj = await DbSet.FindAsync(id);
            DbSet.Remove(obj);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
