using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IConverter<in TInput, out TOutput>
    {
        TOutput Convert(TInput value);

        IEnumerable<TOutput> Convert(IEnumerable<TInput> values);
    }
}