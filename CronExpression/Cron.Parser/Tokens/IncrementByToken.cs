namespace Cron.Parser.Tokens
{
    public class IncrementByToken : Token
    {
        public IncrementByToken(TextSpan span)
            : base("/", Enums.TokenType.Inc, span)
        { }

        public override Token Clone() => new IncrementByToken(Span.Clone());
    }
}
