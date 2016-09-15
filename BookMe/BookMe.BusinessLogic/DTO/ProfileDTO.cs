using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.DTO
{
    public class ProfileDTO : BaseDTO
    {
        public string UserName { get; set; }

        public int Floor { get; set; }

        public string FavouriteRoom { get; set; }
    }
}