using Cron.Core.Tokens;
using System;

namespace Cron.Core.Syntax
{
    public interface ILexer<TToken>
    {
        TToken NextToken();
    }
}
