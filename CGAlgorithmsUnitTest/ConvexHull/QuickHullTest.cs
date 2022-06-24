using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGAlgorithms.Algorithms.ConvexHull;
using CGAlgorithms;
using CGUtilities;
using System.Collections.Generic;

namespace CGAlgorithmsUnitTest
{
    /// <summary>
    /// Unit Test for Convex Hull
    /// </summary>
    [TestClass]
    public class QuickHullTest : ConvexHullTest
    {
        [TestMethod]
        public void QuickHullTestCase1()
        {
            convexHullTester = new QuickHull();
            Case1();
        }
        [TestMethod]
        public void QuickHullTestCase2()
        {
            convexHullTester = new QuickHull();
            Case2();
        }
        [TestMethod]
        public void QuickHullTestCase3()
        {
            convexHullTester = new QuickHull();
            Case3();
        }
        [TestMethod]
        public void QuickHullTestCase4()
        {
            convexHullTester = new QuickHull();
            Case4();
        }
        [TestMethod]
        public void QuickHullTestCase8()
        {
            convexHullTester = new QuickHull();
            Case8();
        }
        [TestMethod]
        public void QuickHullTestCase9()
        {
            convexHullTester = new QuickHull();
            Case9();
        }
        [TestMethod]
        public void QuickHullNormalTestCase3000Points()
        {
            convexHullTester = new QuickHull();
            Case3000Points();
        }
        [TestMethod]
        public void QuickHullNormalTestCase4000Points()
        {
            convexHullTester = new QuickHull();
            Case4000Points();
        }
        [TestMethod]
        public void QuickHullNormalTestCase5000Points()
        {
            convexHullTester = new QuickHull();
            Case5000Points();
        }
        [TestMethod]
        public void QuickHullNormalTestCase10000Points()
        {
            convexHullTester = new QuickHull();
            Case10000Points();
        }
        [TestMethod]
        public void QuickHullSpecialCaseTriangle()
        {
            convexHullTester = new QuickHull();
            SpecialCaseTriangle();
        }
        
        [TestMethod]
        public void QuickHullSpecialCaseConvexPolygon()
        {
            convexHullTester = new QuickHull();
            SpecialCaseConvexPolygon();
        }
    }
}
