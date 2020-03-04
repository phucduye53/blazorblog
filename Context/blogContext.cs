using System;
using System.Linq;
using blazorblog.Entity;
using blazorblog.Entity.AbstractClass;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace blazorblog.Context
{
    public partial class blogContext :  IdentityDbContext<User>
    {
        public blogContext(DbContextOptions<blogContext> options)
       : base(options)
        {
        }


        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == Microsoft.EntityFrameworkCore.EntityState.Added || x.State == Microsoft.EntityFrameworkCore.EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    //   string identityName = Thread.CurrentPrincipal.Identity.Name; HANDLE LATER
                    string identityName = "admin";
                    DateTime now = DateTime.UtcNow;

                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                        .ToTable("Users", "dbo");
        }
    }
}