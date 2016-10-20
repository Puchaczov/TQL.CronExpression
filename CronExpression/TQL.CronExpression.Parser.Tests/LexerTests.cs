﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TQL.CronExpression.Parser;

namespace TQL.CronExpression.Parser.Tests
{
    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void Lexer_ReturnAppropiatePosition()
        {
            var lexer = new Lexer("1-2,5-6#4");

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
            var lexer = new Lexer("1-2,5-6#4");

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

        [TestMethod]
        public void Lexer_ReturnLWAppropiateToken()
        {
            var lexer = new Lexer("LW 1");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.LW, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }

        [TestMethod]
        public void Lexer_SurrendedByOtherOptions()
        {
            var lexer = new Lexer("125,LW,3#4");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Comma, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.LW, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Comma, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Hash, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }

        [TestMethod]
        public void Lexer_WithMonthsOptions()
        {
            var lexer = new Lexer("MON,WED,125LW");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Name, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Comma, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Name, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Comma, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.LW, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }

        [TestMethod]
        public void Lexer_ShouldReturnMultipleWhiteSpaces()
        {
            var lexer = new Lexer($"* {Environment.NewLine} *");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Star, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Star, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }

        [TestMethod]
        public void Lexer_ShouldReturnNameToken()
        {
            var lexer = new Lexer("*a 1");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Name, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }

        [TestMethod]
        public void Lexer_ShouldTokenize()
        {
            var lexer = new Lexer("0 0 0 L 2 * 2015-2150");
            var token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.L, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Star, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.WhiteSpace, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Range, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Integer, token.TokenType);
            token = lexer.NextToken();
            Assert.AreEqual(Enums.TokenType.Eof, token.TokenType);
        }
    }
}