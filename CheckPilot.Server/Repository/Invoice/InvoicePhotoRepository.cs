using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


using CheckPilot.Server.Data;
using CheckPilot.Server.Repository;
using CheckPilot.Models;
using System.Linq.Expressions;

namespace CheckPilot.Server.Repository
{
    public class InvoicePhotoRepository : IPhotoRepository<InvoicePhoto, int>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        public InvoicePhotoRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public InvoicePhoto Add(InvoicePhoto entity)
        {
            var db = _factory.CreateDbContext();
            entity.Initialize();
            db.InvoicePhoto.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public async Task<InvoicePhoto> AddAsync(InvoicePhoto entity)
        {
            var db = _factory.CreateDbContext();
            entity.Initialize();
            db.InvoicePhoto.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public void Delete(int key)
        {
            var db = _factory.CreateDbContext();
            InvoicePhoto entity = db.InvoicePhoto.Find(key);
            db.InvoicePhoto.Remove(entity);
            db.SaveChanges();
        }

        public async Task DeleteAsync(int key)
        {
            var db = _factory.CreateDbContext();
            InvoicePhoto entity = await db.InvoicePhoto.FindAsync(key);
            db.InvoicePhoto.Remove(entity);
            await db.SaveChangesAsync();
        }

        public InvoicePhoto GetByKey(int key)
        {
            return GetByKey(key, true);
        }

        public InvoicePhoto GetByKey(int key, bool tracking = false)
        {
            var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.InvoicePhoto.Find(key);
            }
            else
            {
                return db.InvoicePhoto.AsNoTracking().FirstOrDefault(item => item.InvoicePhotoId == key);
            }
        }

        public async Task<InvoicePhoto> GetByKeyAsync(int key)
        {
            return await GetByKeyAsync(key, true);
        }

        public async Task<InvoicePhoto> GetByKeyAsync(int key, bool tracking = false)
        {
            var db = _factory.CreateDbContext();
            if (tracking)
            {
                return await db.InvoicePhoto.FindAsync(key);
            }
            else
            {
                return await db.InvoicePhoto.AsNoTracking().FirstOrDefaultAsync(item => item.InvoicePhotoId == key);
            }
        }

        public IList<InvoicePhoto> GetList()
        {
            var db = _factory.CreateDbContext();
            return db.InvoicePhoto.ToList();
        }

        public async Task<IList<InvoicePhoto>> GetListAsync()
        {
            var db = _factory.CreateDbContext();
            return await db.InvoicePhoto.ToListAsync();
        }

        public InvoicePhoto Update(InvoicePhoto entity)
        {
            var db = _factory.CreateDbContext();
            entity.Updated();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            return entity;
        }

        public async Task<InvoicePhoto> UpdateAsync(InvoicePhoto entity)
        {
            var db = _factory.CreateDbContext();
            entity.Updated();
            db.Entry(entity).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<IList<InvoicePhoto>> FindAsync(Expression<Func<InvoicePhoto, bool>> predicate)
        {
            var db = _factory.CreateDbContext();
            return await db.InvoicePhoto.AsNoTracking().Where(predicate).ToListAsync();
        }

    }
}
