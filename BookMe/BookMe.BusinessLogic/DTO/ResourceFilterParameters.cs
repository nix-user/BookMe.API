using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.BusinessLogic.DTO
{
    public class ResourceFilterParameters
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public bool HasPolycom { get; set; }

        public RoomSizeDTO? RoomSize { get; set; }
    }
}