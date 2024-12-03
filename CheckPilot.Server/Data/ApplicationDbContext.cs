using CheckPilot.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CheckPilot.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region User
        public DbSet<User> UserCheckPilot { get; set; }
        public DbSet<InvoicePhoto> InvoicePhoto { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .OwnsOne(p => p.RecordLog);

            builder.Entity<InvoicePhoto>()
                .OwnsOne(p => p.RecordLog);
        }
    }
}
