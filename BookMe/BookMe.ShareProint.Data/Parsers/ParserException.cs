using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.ShareProint.Data.Parsers
{
    internal class ParserException : Exception
    {
        public ParserException(string message) : base(message)
        {
        }
    }
}