using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public DbSet<Profile> Records { get; set; }
    }
}
