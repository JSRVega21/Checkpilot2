using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using CheckPilot.Models;
using CheckPilot.Server.Data;

namespace CheckPilot.Server.Repository
{
    public class UserRepository : IUserRepository<User, int>
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        public UserRepository(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public User Add(User entity)
        {
            var db = _factory.CreateDbContext();
            entity.UserPassword = BCrypt.Net.BCrypt.HashPassword(entity.UserPassword);
            entity.Initialize();
            db.UserCheckPilot.Add(entity);
            db.SaveChanges();
            return entity;
        }

        public async Task<User> AddAsync(User entity)
        {
            var db = _factory.CreateDbContext();
            entity.UserPassword = BCrypt.Net.BCrypt.HashPassword(entity.UserPassword);
            entity.Initialize();
            db.UserCheckPilot.Add(entity);
            await db.SaveChangesAsync();
            return entity;
        }

        public void Delete(int key)
        {
            var db = _factory.CreateDbContext();
            User entity = db.UserCheckPilot.Find(key);
            db.UserCheckPilot.Remove(entity);
            db.SaveChanges();
        }

        public async Task DeleteAsync(int key)
        {
            var db = _factory.CreateDbContext();
            User entity = await db.UserCheckPilot.FindAsync(key);
            db.UserCheckPilot.Remove(entity);
            await db.SaveChangesAsync();
        }

        public User GetByKey(int key)
        {
            return GetByKey(key, true);
        }

        public User GetByKey(int key, bool tracking = false)
        {
            var db = _factory.CreateDbContext();
            return tracking
                ? db.UserCheckPilot.Find(key)
                : db.UserCheckPilot.AsNoTracking().FirstOrDefault(u => u.UserId == key);
        }

        public async Task<User> GetByKeyAsync(int key)
        {
            return await GetByKeyAsync(key, true);
        }

        public async Task<User> GetByKeyAsync(int key, bool tracking = false)
        {
            var db = _factory.CreateDbContext();
            if (tracking)
            {
                return await db.UserCheckPilot.FindAsync(key);
            }
            else
            {
                return await db.UserCheckPilot.AsNoTracking().FirstOrDefaultAsync(item => item.UserId == key);
            }
        }

        public IList<User> GetList()
        {
            var db = _factory.CreateDbContext();
            return db.UserCheckPilot.ToList();
        }

        public async Task<IList<User>> GetListAsync()
        {
            var db = _factory.CreateDbContext();
            return await db.UserCheckPilot.ToListAsync();
        }

        public User Update(User entity)
        {
            var db = _factory.CreateDbContext();
            entity.UserPassword = BCrypt.Net.BCrypt.HashPassword(entity.UserPassword);
            entity.Updated();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var db = _factory.CreateDbContext();
            var existingUser = await db.UserCheckPilot.FindAsync(entity.UserId);

            if (existingUser == null)
                throw new Exception("Usuario no encontrado");

            if (!string.IsNullOrWhiteSpace(entity.UserPassword) &&
                entity.UserPassword != existingUser.UserPassword)
            {
                existingUser.UserPassword = BCrypt.Net.BCrypt.HashPassword(entity.UserPassword);
            }

            existingUser.UserName = entity.UserName;
            existingUser.UserEmail = entity.UserEmail;
            existingUser.UserPhone = entity.UserPhone;
            existingUser.UserRoleId = entity.UserRoleId;
            existingUser.UserRole = entity.UserRole;

            await db.SaveChangesAsync();
            return existingUser;
        }

        public ApplicationDbContext GetDbContext()
        {
            return _factory.CreateDbContext();
        }

    }
}
