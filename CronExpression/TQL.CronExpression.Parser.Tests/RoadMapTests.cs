using Cron.Parser.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tests
{
    [TestClass]
    public class RoadMapTests
    {
        [TestMethod]
        public void CheckRoadMap_SimpleExpression_ShouldPass()
        {
            var ast = "* * * * * * *".Parse(false);

            Assert.AreEqual(ast.Path, "0");

            var descendants = ast.Desecendants;
            Assert.AreEqual(descendants[0].Path, "00");
            Assert.AreEqual(descendants[1].Path, "01");
            Assert.AreEqual(descendants[2].Path, "02");
            Assert.AreEqual(descendants[3].Path, "03");
            Assert.AreEqual(descendants[4].Path, "04");
            Assert.AreEqual(descendants[5].Path, "05");
            Assert.AreEqual(descendants[6].Path, "06");

            Assert.AreEqual(descendants[0].Desecendants[0].Path, "000");
            Assert.AreEqual(descendants[1].Desecendants[0].Path, "010");
            Assert.AreEqual(descendants[2].Desecendants[0].Path, "020");
            Assert.AreEqual(descendants[3].Desecendants[0].Path, "030");
            Assert.AreEqual(descendants[4].Desecendants[0].Path, "040");
            Assert.AreEqual(descendants[5].Desecendants[0].Path, "050");
            Assert.AreEqual(descendants[6].Desecendants[0].Path, "060");
        }

        [TestMethod]
        public void CheckRoadMap_NestedExpression_ShouldPass()
        {
            var ast = "1-2 4-7/2 abc-cde#45 1,2,3,4,4-5,1-3/2 1#5 * *".Parse(false);

            var descendants = ast.Desecendants;
            //1-2
            Assert.AreEqual(descendants[0].Desecendants[0].Path, "000"); /*-*/
            Assert.AreEqual(descendants[0].Desecendants[0].Desecendants[0].Path, "0000"); /*1*/
            Assert.AreEqual(descendants[0].Desecendants[0].Desecendants[1].Path, "0001"); /*2*/

            Assert.AreEqual(descendants[1].Desecendants[0].Path, "010"); /*/*/
            Assert.AreEqual(descendants[1].Desecendants[0].Desecendants[0].Path, "0100"); /*4-7*/
            Assert.AreEqual(descendants[1].Desecendants[0].Desecendants[0].Desecendants[0].Path, "01000"); /*-*/
            Assert.AreEqual(descendants[1].Desecendants[0].Desecendants[0].Desecendants[0].Desecendants[0], "010000"); /*4*/
            Assert.AreEqual(descendants[1].Desecendants[0].Desecendants[0].Desecendants[0].Desecendants[1], "010001"); /*7*/
            Assert.AreEqual(descendants[1].Desecendants[0].Desecendants[1].Path, "0101"); /*2*/
        }
    }
}
