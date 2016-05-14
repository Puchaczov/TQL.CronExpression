using Cron.Core.Tokens;
using System;

namespace Cron.Core.Syntax
{
    public interface ILexer<TTokenType> where TTokenType: struct, IComparable, IFormattable
    {
        GenericToken<TTokenType> NextToken();
    }
}
