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
    public class GrahamScanTest : ConvexHullTest
    {
        [TestMethod]
        public void GrahamScanTestCase1()
        {
            convexHullTester = new GrahamScan();
            Case1();
        }
        [TestMethod]
        public void GrahamScanTestCase2()
        {
            convexHullTester = new GrahamScan();
            Case2();
        }
        [TestMethod]
        public void GrahamScanTestCase3()
        {
            convexHullTester = new GrahamScan();
            Case3();
        }
        [TestMethod]
        public void GrahamScanTestCase4()
        {
            convexHullTester = new GrahamScan();
            Case4();
        }
        
        [TestMethod]
        public void GrahamScanTestCase8()
        {
            convexHullTester = new GrahamScan();
            Case8();
        }
        [TestMethod]
        public void GrahamScanTestCase9()
        {
            convexHullTester = new GrahamScan();
            Case9();
        }
        [TestMethod]
        public void GrahamScanTestCase10()
        {
            convexHullTester = new GrahamScan();
            Case10();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase1000Points()
        {
            convexHullTester = new GrahamScan();
            Case1000Points();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase2000Points()
        {
            convexHullTester = new GrahamScan();
            Case2000Points();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase3000Points()
        {
            convexHullTester = new GrahamScan();
            Case3000Points();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase4000Points()
        {
            convexHullTester = new GrahamScan();
            Case4000Points();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase5000Points()
        {
            convexHullTester = new GrahamScan();
            Case5000Points();
        }
        [TestMethod]
        public void GrahamScanNormalTestCase10000Points()
        {
            convexHullTester = new GrahamScan();
            Case10000Points();
        }
        [TestMethod]
        public void GrahamScanSpecialCaseTriangle()
        {
            convexHullTester = new GrahamScan();
            SpecialCaseTriangle();
        }
        [TestMethod]
        public void GrahamScanSpecialCaseConvexPolygon()
        {
            convexHullTester = new GrahamScan();
            SpecialCaseConvexPolygon();
        }
    }
}
