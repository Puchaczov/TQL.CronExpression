namespace Cron.Parser.Tokens
{
    public class NoneToken : Token
    {
        public NoneToken(TextSpan span)
            : base(string.Empty, Enums.TokenType.None, span)
        { }

        public override Token Clone() => new NoneToken(Span.Clone());
    }
}
