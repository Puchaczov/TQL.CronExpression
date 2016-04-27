namespace Cron.Parser.Tokens
{
    public class EndOfFileToken : Token
    {
        public EndOfFileToken(TextSpan span)
            :base(string.Empty, Enums.TokenType.Eof, span)
        { }

        public override Token Clone() => new EndOfFileToken(Span.Clone());
    }
}
