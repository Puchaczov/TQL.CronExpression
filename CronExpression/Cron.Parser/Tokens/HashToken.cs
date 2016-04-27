namespace Cron.Parser.Tokens
{
    public class HashToken : Token
    {
        public HashToken(TextSpan span)
            : base("#", Enums.TokenType.Hash, span)
        { }

        public override Token Clone() => new HashToken(Span.Clone());
    }
}
