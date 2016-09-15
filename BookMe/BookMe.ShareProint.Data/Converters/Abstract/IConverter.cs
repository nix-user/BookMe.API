using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.ShareProint.Data.Converters.Abstract
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(TInput value);

        IEnumerable<TOutput> Convert(IEnumerable<TInput> values);

        TInput ConvertBack(TOutput value);

        IEnumerable<TInput> ConvertBack(IEnumerable<TOutput> values);
    }
}