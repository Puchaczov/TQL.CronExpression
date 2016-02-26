using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cron.Parser.Visitors;
using Cron.Visitors;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class CronLang
    {
        [TestMethod]
        public void TestMethod1()
        {
            Lexer lexer = new Lexer("0/5 14,18,3-39,52 * ? JAN,MAR,SEP MON-FRI 2002-2010");
            Parser parser = new Parser(lexer);

            INodeVisitor visitor = new CronNodeVisitorBase();

            var tree = parser.ComposeRootComponents();
            tree.Accept(visitor);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Lexer lexer = new Lexer("0,3#2,1-5,2-6,6,1,0 0/5 14,18-20,25 * FEB-MAY,5/2,JANUARY,FEBRUARY MON-FRI,1W,1545L,6#3 ?");
            Parser parser = new Parser(lexer);

            CronNodeVisitorBase visitor = new CronNodeVisitorBase();

            var tree = parser.ComposeRootComponents();
            tree.Accept(visitor);
        }

        [TestMethod]
        public void TestMethod3()
        {
            Lexer lexer = new Lexer("* * * * * MON#5,6#3 ?");
            Parser parser = new Parser(lexer);

            CronNodeVisitorBase visitor = new CronNodeVisitorBase();

            var tree = parser.ComposeRootComponents();
            tree.Accept(visitor);
        }
    }

}
