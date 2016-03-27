using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils
{
    public interface IConvertible<Input, Output>
    {
        Output Convert(Input input);
    }
}
