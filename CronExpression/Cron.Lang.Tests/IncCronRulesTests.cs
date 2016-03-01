using Cron.Visitors.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Tests
{
    [TestClass]
    public class IncCronRulesTests
    {
        [TestMethod]
        public void CheckInc_IncInSeconds_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * * *");
        }
        
        [TestMethod]
        public void CheckInc_IncInMinutes_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* 1/5 * * * * *");
        }

        [TestMethod]
        public void CheckInc_IncInHours_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * 1/5 * * * *");
        }

        [TestMethod]
        public void CheckInc_IncInDayOfMonth_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * 1/5 * * *");
        }

        [TestMethod]
        public void CheckInc_IncInMonths_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * 1/5 * *");
        }

        [TestMethod]
        public void CheckInc_IncInDayOfWeek_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * 1/5 *");
        }

        [TestMethod]
        public void CheckInc_IncInYears_ShouldPass()
        {
            TestsHelper.CheckExpressionDidNotReturnsValidationErrors("* * * * * * 200/5");
        }
    }
}
