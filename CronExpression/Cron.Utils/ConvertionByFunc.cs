using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils
{
    public class ConvertionByFunc<Input, Output> : IConvertible<Input, Output>
    {
        private Func<Input, Output> converter;

        public ConvertionByFunc(Func<Input, Output> converter)
        {
            this.converter = converter;
        }

        public Output Convert(Input input)
        {
            return converter(input);
        }
    }
}
