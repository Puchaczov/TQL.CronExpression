using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void Lexer_ReturnAppropiatePosition()
        {
            Lexer lexer = new Lexer("1-2,5-6#4");

            Assert.AreEqual(0, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(1, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(2, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(3, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(4, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(5, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(6, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(7, lexer.Position);
            lexer.NextToken();
            Assert.AreEqual(8, lexer.Position);
            lexer.NextToken();
        }

        [TestMethod]
        public void Lexer_ReturnAppropiateToken()
        {
            Lexer lexer = new Lexer("1-2,5-6#4");

            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Range, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Comma, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Range, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Hash, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }
    }
}
