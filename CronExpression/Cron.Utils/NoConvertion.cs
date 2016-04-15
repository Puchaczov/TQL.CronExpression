using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils
{
    public class NoConvertion<T> : IConvertible<T, T>
    {
        public T Convert(T input) => input;
    }
}
