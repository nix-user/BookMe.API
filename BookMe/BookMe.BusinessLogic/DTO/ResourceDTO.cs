using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.DTO
{
    public class ResourceDTO : BaseDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool HasTv { get; set; }

        public bool HasPolycom { get; set; }

        public RoomSize? RoomSize { get; set; }
    }
}
