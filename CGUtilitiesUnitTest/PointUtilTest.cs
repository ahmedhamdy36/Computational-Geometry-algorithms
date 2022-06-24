using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CGUtilities;

namespace CGUtilitiesUnitTest
{
    /// <summary>
    /// Unit Test for Point Utility
    /// </summary>
    [TestClass]
    public class PointUtilTest
    {
        [TestMethod]
        public void CheckCrossProduct()
        {
            Point a = new Point(3, 5);
            Point b = new Point(4, 12);

            Assert.AreEqual(16, HelperMethods.CrossProduct(a, b));
        }
        #region In Triangle Tests
        [TestMethod]
        public void InsideTriangleClockwise()
        {
            Point a = new Point(2, 2);
            Point b = new Point(4, 5);
            Point c = new Point(6, 1);

            Point p = new Point(3, 3);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Inside);
        }
        [TestMethod]
        public void InsideTriangleCounterClockwise()
        {
            Point a = new Point(6, 1);
            Point b = new Point(4, 5);
            Point c = new Point(2, 2);

            Point p = new Point(3, 3);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Inside);
        }
        [TestMethod]
        public void OnTriangleBorder()
        {
            Point a = new Point(3, 4);
            Point b = new Point(6, 6);
            Point c = new Point(3, 8);

            Point p = new Point(3, 6);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.OnEdge);
        }
        [TestMethod]
        public void OnBorderIfOnHead()
        {
            Point a = new Point(3, 4);
            Point b = new Point(6, 6);
            Point c = new Point(3, 8);

            Point p = new Point(3, 4);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.OnEdge);
        }
        [TestMethod]
        public void OutsideTriangle()
        {
            Point a = new Point(3, 4);
            Point b = new Point(6, 6);
            Point c = new Point(3, 8);

            Point p = new Point(1, 1);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Outside);
        }
        [TestMethod]
        public void OnBorderIfOnBorderAndTriangleIsSegment()
        {
            Point a = new Point(1, 1);
            Point b = new Point(3, 3);
            Point c = new Point(3, 3);

            Point p = new Point(2, 2);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.OnEdge);
        }
        [TestMethod]
        public void OutsideIfTriangleIsSegmentAndPointIsCollinearAfterEnd()
        {
            Point a = new Point(1, 1);
            Point b = new Point(3, 3);
            Point c = new Point(3, 3);

            Point p = new Point(4, 4);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Outside);
        }
        [TestMethod]
        public void OutsideIfTriangleIsSegmentAndPointIsCollinearBeforeStart()
        {
            Point a = new Point(1, 1);
            Point b = new Point(3, 3);
            Point c = new Point(3, 3);

            Point p = new Point(0, 0);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Outside);
        }
        [TestMethod]
        public void OnBorderIfTriangleIsThePoint()
        {
            Point a = new Point(3, 3);
            Point b = new Point(3, 3);
            Point c = new Point(3, 3);

            Point p = new Point(3, 3);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.OnEdge);
        }
        [TestMethod]
        public void OutsideIfTriangleIsPointNotEqualThePoint()
        {
            Point a = new Point(3, 3);
            Point b = new Point(3, 3);
            Point c = new Point(3, 3);

            Point p = new Point(2, 1);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Outside);
        }
        [TestMethod]
        public void OutsideIfPointIsColinearButNotOnEdge()
        {
            Point a = new Point(1, 0);
            Point b = new Point(2, 0);
            Point c = new Point(3, 0);

            Point p = new Point(0, 0);
            var res = HelperMethods.PointInTriangle(p, a, b, c);
            Assert.AreEqual(res, Enums.PointInPolygon.Outside);
        }
        #endregion
        #region Point On Ray
        [TestMethod]
        public void PointOnRayReturnsTrueIfExistsOnSegment()
        {
            var p = new Point(2, 2);
            var onRay = HelperMethods.PointOnRay(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onRay, true);
        }
        [TestMethod]
        public void PointOnRayReturnsTrueIfAfterEnd()
        {
            var p = new Point(4, 4);
            var onRay = HelperMethods.PointOnRay(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onRay, true);
        }
        [TestMethod]
        public void PointOnRayReturnsTrueIfExistsOnStart()
        {
            var p = new Point(1, 1);
            var onRay = HelperMethods.PointOnRay(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onRay, true);
        }
        [TestMethod]
        public void PointOnRayReturnsTrueIfExistsOnEnd()
        {
            var p = new Point(3, 3);
            var onRay = HelperMethods.PointOnRay(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onRay, true);
        }
        [TestMethod]
        public void PointOnRayReturnsTrueIfStartEqualsEnd()
        {
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var p = new Point(random.Next(), random.Next());
                var onRay = HelperMethods.PointOnRay(p, new Point(1, 1), new Point(1, 1));
                Assert.AreEqual(onRay, true);
            }
        }
        [TestMethod]
        public void PointOnRayReturnsFalseIfPointBeforeStart()
        {
            var p = new Point(1, 1);
            var onRay = HelperMethods.PointOnRay(p, new Point(2, 2), new Point(3, 3));
            Assert.AreEqual(onRay, false);
        }
        [TestMethod]
        public void PointOnRayReturnsFalseIfPointNotColinear()
        {
            var p = new Point(2, 3);
            var onRay = HelperMethods.PointOnRay(p, new Point(2, 2), new Point(3, 3));
            Assert.AreEqual(onRay, false);
        }
        #endregion
        #region Point On Segment
        [TestMethod]
        public void PointOnSegmentReturnsTrueIfExistsOnSegment()
        {
            var p = new Point(2, 2);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentReturnsTrueIfExistsOnSegmentStart()
        {
            var p = new Point(1, 1);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentReturnsTrueIfExistsOnSegmentEnd()
        {
            var p = new Point(3, 3);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentReturnsFalseIfExistsBeforeSegmentStart()
        {
            var p = new Point(0, 0);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentReturnsFalseIfExistsAfterSegmentEnd()
        {
            var p = new Point(4, 4);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentReturnsFalseIfDoesnotExistsOnSegment()
        {
            var p = new Point(3, 4);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(3, 3));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentReturnsTrueIfStartEqualsEndEqualsPoint()
        {
            var p = new Point(1, 1);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(1, 1));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentReturnsFalseIfStartEqualsEndDoesnotEqualPoint()
        {
            var p = new Point(1, 1);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 1), new Point(1, 1));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentHorizontalNotOnSegmentColinear()
        {
            var p = new Point(0, 2);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 2), new Point(3, 2));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentHorizontalNotOnSegmentNotColinear()
        {
            var p = new Point(-1, 9);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 2), new Point(3, 2));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentHorizontalOnSegment()
        {
            var p = new Point(2, 2);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(1, 2), new Point(3, 2));
            Assert.AreEqual(onSegment, true);
        }
        [TestMethod]
        public void PointOnSegmentVerticalNotOnSegmentColinear()
        {
            var p = new Point(0, 1);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(0, 2), new Point(0, 4));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentVerticalNotOnSegmentNotColinear()
        {
            var p = new Point(3, 1);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(0, 2), new Point(0, 4));
            Assert.AreEqual(onSegment, false);
        }
        [TestMethod]
        public void PointOnSegmentVerticalOnSegment()
        {
            var p = new Point(0, 3);
            var onSegment = HelperMethods.PointOnSegment(p, new Point(0, 2), new Point(0, 4));
            Assert.AreEqual(onSegment, true);
        }
        #endregion
    }
}
