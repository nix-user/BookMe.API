using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.Core.Models
{
    public class Resource : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool HasTv { get; set; }

        public bool HasPolycom { get; set; }

        public RoomSize? RoomSize { get; set; }
    }
}
