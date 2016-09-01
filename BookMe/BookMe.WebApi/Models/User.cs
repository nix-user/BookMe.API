using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookMe.WebApi.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FavoriteRoom { get; set; }

        public string MyRoom { get; set; }
    }
}