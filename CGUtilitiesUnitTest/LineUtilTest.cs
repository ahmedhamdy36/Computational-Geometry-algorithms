using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGUtilities;

namespace CGUtilitiesUnitTest
{
    /// <summary>
    /// Unit Test for Line Utility
    /// </summary>
    [TestClass]
    public class LineUtilTest
    {
        [TestMethod]
        public void CheckTurnLeftIfLeftInRange()
        {
            Line l = new Line(new Point(4, 5), new Point(2,2));
            Assert.AreEqual(HelperMethods.CheckTurn(l, new Point(3, 3)), Enums.TurnType.Left);
        }
        [TestMethod]
        public void CheckTurnLeftIfLeftOutRange()
        {
            Line l = new Line(new Point(4, 5), new Point(2, 2));
            Assert.AreEqual(HelperMethods.CheckTurn(l, new Point(1, -2)), Enums.TurnType.Left);
        }
        [TestMethod]
        public void CheckTurnRightIfRightInRange()
        {
            Line l = new Line(new Point(4, 5), new Point(2, 2));
            Assert.AreEqual(HelperMethods.CheckTurn(l, new Point(2, 3)), Enums.TurnType.Right);
        }
        [TestMethod]
        public void CheckTurnRightIfRightOutRange()
        {
            Line l = new Line(new Point(4, 5), new Point(2, 2));
            Assert.AreEqual(HelperMethods.CheckTurn(l, new Point(4, 10)), Enums.TurnType.Right);
        }
        //Todo Add Colinear Cases;
    }
}
