namespace Cron.Parser.Tokens
{
    public class MissingToken : Token
    {
        public MissingToken(TextSpan span)
            : base("_", Enums.TokenType.Missing, span)
        { }

        public override Token Clone() => new MissingToken(Span.Clone());
    }
}
