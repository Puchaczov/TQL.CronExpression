using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Converter
{
    public interface IConverter<in TRequest, out TResponse>
    {
        TResponse Convert(TRequest request);
    }
}
