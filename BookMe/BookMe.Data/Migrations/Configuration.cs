using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Data.Context;

namespace BookMe.Data.Migrations
{
    public class Configuration : DbMigrationsConfiguration<AppContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }
    }
}