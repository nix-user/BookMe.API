using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Models;

namespace BookMe.Data.Context
{
    public class AppContext : DbContext
    {
        public AppContext()
            : base("DBConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
                .Property(e => e.UserName).HasColumnType("NVARCHAR").HasMaxLength(128);

            modelBuilder.Entity<UserProfile>()
                .Property(x => x.UserName)
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));
        }
    }
}
