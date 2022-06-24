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
    public class JarvisMarchTest : ConvexHullTest
    {
        [TestMethod]
        public void JarvisMarchTestCase1()
        {
            convexHullTester = new JarvisMarch();
            Case1();
        }
        [TestMethod]
        public void JarvisMarchTestCase2()
        {
            convexHullTester = new JarvisMarch();
            Case2();
        }
        [TestMethod]
        public void JarvisMarchTestCase3()
        {
            convexHullTester = new JarvisMarch();
            Case3();
        }
        [TestMethod]
        public void JarvisMarchTestCase4()
        {
            convexHullTester = new JarvisMarch();
            Case4();
        }
        
        [TestMethod]
        public void JarvisMarchNormalTestCase200Points()
        {
            convexHullTester = new JarvisMarch();
            Case200Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase400Points()
        {
            convexHullTester = new JarvisMarch();
            Case400Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase600Points()
        {
            convexHullTester = new JarvisMarch();
            Case600Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase800Points()
        {
            convexHullTester = new JarvisMarch();
            Case800Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase3000Points()
        {
            convexHullTester = new JarvisMarch();
            Case3000Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase4000Points()
        {
            convexHullTester = new JarvisMarch();
            Case4000Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase5000Points()
        {
            convexHullTester = new JarvisMarch();
            Case5000Points();
        }
        [TestMethod]
        public void JarvisMarchNormalTestCase10000Points()
        {
            convexHullTester = new JarvisMarch();
            Case10000Points();
        }
        [TestMethod]
        public void JarvisMarchSpecialCaseTriangle()
        {
            convexHullTester = new JarvisMarch();
            SpecialCaseTriangle();
        }
        [TestMethod]
        public void JarvisMarchSpecialCaseConvexPolygon()
        {
            convexHullTester = new JarvisMarch();
            SpecialCaseConvexPolygon();
        }
    }
}
