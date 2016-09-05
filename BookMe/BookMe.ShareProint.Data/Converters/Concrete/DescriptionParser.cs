using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookMe.Core.Enums;
using BookMe.ShareProint.Data.Converters.Abstract;

namespace BookMe.ShareProint.Data.Converters.Concrete
{
    public class DescriptionParser : IDescriptionParser
    {
        private const string PolycomToken = "Polycom";
        private const string TvToken = "TV";
        private const string SizeRegEx = "(?<=\\Size:)[^\\]]+";

        private readonly IDictionary<string, RoomSize> roomSizeByString = new Dictionary<string, RoomSize>()
        {
            { "S", RoomSize.Small },
            { "M", RoomSize.Middle },
            { "L", RoomSize.Large },
        };

        public bool HasPolycom(string description)
        {
            return this.CheckIfResourceHasProperty(description, PolycomToken);
        }

        public bool HasTv(string description)
        {
            return this.CheckIfResourceHasProperty(description, TvToken);
        }

        public RoomSize? ParseRoomSize(string description)
        {
            if (description == null)
            {
                return null;
            }

            var match = Regex.Match(description, SizeRegEx);
            if (match.Success)
            {
                var sizeString = match.Groups[0].Value[0].ToString();
                return this.roomSizeByString[sizeString];
            }
            else
            {
                return null;
            }
        }

        private bool CheckIfResourceHasProperty(string description, string properyToken)
        {
            return description != null && description.Contains(properyToken);
        }
    }
}
