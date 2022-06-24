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
    public class DivideAndConquerTest : ConvexHullTest
    {
        [TestMethod]
        public void DivideAndConquerTestCase1()
        {
            convexHullTester = new DivideAndConquer();
            Case1();
        }
        [TestMethod]
        public void DivideAndConquerTestCase2()
        {
            convexHullTester = new DivideAndConquer();
            Case2();
        }
        [TestMethod]
        public void DivideAndConquerTestCase3()
        {
            convexHullTester = new DivideAndConquer();
            Case3();
        }
        [TestMethod]
        public void DivideAndConquerTestCase4()
        {
            convexHullTester = new DivideAndConquer();
            Case4();
        }
        
        [TestMethod]
        public void DivideAndConquerTestCase8()
        {
            convexHullTester = new DivideAndConquer();
            Case8();
        }
        [TestMethod]
        public void DivideAndConquerTestCase9()
        {
            convexHullTester = new DivideAndConquer();
            Case9();
        }
        [TestMethod]
        public void DivideAndConquerTestCase10()
        {
            convexHullTester = new DivideAndConquer();
            Case10();
        }

        [TestMethod]
        public void DivideAndConquerSpecialCaseConvexPolygon()
        {
            convexHullTester = new DivideAndConquer();
            SpecialCaseConvexPolygon();
        }
        [TestMethod]
        public void DivideAndConquerSpecialCaseTriangle()
        {
            convexHullTester = new DivideAndConquer();
            SpecialCaseTriangle();
        }

        [TestMethod]
        public void DivideAndConquerNormalTestCase1000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case1000Points();
        }
        [TestMethod]
        public void DivideAndConquerNormalTestCase2000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case2000Points();
        }
        [TestMethod]
        public void DivideAndConquerNormalTestCase3000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case3000Points();
        }
        [TestMethod]
        public void DivideAndConquerNormalTestCase4000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case4000Points();
        }
        [TestMethod]
        public void DivideAndConquerNormalTestCase5000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case5000Points();
        }
        [TestMethod]
        public void DivideAndConquerNormalTestCase10000Points()
        {
            convexHullTester = new DivideAndConquer();
            Case10000Points();
        }

    }
}
