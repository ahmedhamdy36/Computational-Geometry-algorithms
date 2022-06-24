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
    public class IncrementalTest : ConvexHullTest
    {
        [TestMethod]
        public void IncrementalTestCase1()
        {
            convexHullTester = new Incremental();
            Case1();
        }
        [TestMethod]
        public void IncrementalTestCase2()
        {
            convexHullTester = new Incremental();
            Case2();
        }
        [TestMethod]
        public void IncrementalTestCase3()
        {
            convexHullTester = new Incremental();
            Case3();
        }
        [TestMethod]
        public void IncrementalTestCase4()
        {
            convexHullTester = new Incremental();
            Case4();
        }
        [TestMethod]
        public void IncrementalTestCase8()
        {
            convexHullTester = new Incremental();
            Case8();
        }
        [TestMethod]
        public void IncrementalTestCase9()
        {
            convexHullTester = new Incremental();
            Case9();
        }
        [TestMethod]
        public void IncrementalTestCase10()
        {
            convexHullTester = new Incremental();
            Case10();
        }
        [TestMethod]
        public void IncrementalTestCase11()
        {
            convexHullTester = new Incremental();
            Case11();
        }

        [TestMethod]
        public void IncrementalNormalTestCase1000Points()
        {
            convexHullTester = new Incremental();
            Case1000Points();
        }
        [TestMethod]
        public void IncrementalNormalTestCase2000Points()
        {
            convexHullTester = new Incremental();
            Case2000Points();
        }
        [TestMethod]
        public void IncrementalNormalTestCase3000Points()
        {
            convexHullTester = new Incremental();
            Case3000Points();
        }
        [TestMethod]
        public void IncrementalNormalTestCase4000Points()
        {
            convexHullTester = new Incremental();
            Case4000Points();
        }
        [TestMethod]
        public void IncrementalNormalTestCase5000Points()
        {
            convexHullTester = new Incremental();
            Case5000Points();
        }
        [TestMethod]
        public void IncrementalNormalTestCase10000Points()
        {
            convexHullTester = new Incremental();
            Case10000Points();
        }

        [TestMethod]
        public void IncrementalSpecialCaseTriangle()
        {
            convexHullTester = new Incremental();
            SpecialCaseTriangle();
        }
        [TestMethod]
        public void IncrementalSpecialCaseConvexPolygon()
        {
            convexHullTester = new Incremental();
            SpecialCaseConvexPolygon();
        }
    }
}
