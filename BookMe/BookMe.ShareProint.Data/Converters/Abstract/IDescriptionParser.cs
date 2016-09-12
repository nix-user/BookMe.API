using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMe.Core.Enums;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IDescriptionParser
    {
        bool HasPolycom(string description);

        bool HasTv(string description);

        RoomSize? ParseRoomSize(string description);
    }
}