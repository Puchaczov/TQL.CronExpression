using Cron.Tokens;

namespace Cron.Core.Tokens
{
    public class EndOfFileToken : GenericToken<TokenTypeBase>
    {
        protected EndOfFileToken(string value, TokenTypeBase type, TextSpan span) 
            : base(value, type, span)
        { }

        public EndOfFileToken(TextSpan span)
            : base(string.Empty, TokenTypeBase.Eof, span)
        { }

        public override GenericToken<TokenTypeBase> Clone() => new EndOfFileToken(Value, Type, Span);
    }
}
