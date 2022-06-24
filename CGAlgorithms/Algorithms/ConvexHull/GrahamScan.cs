using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort(com);
            double mny = double.MaxValue;
            double x = 0;
            int ind = 0;
            bool[] vis = new bool[points.Count];

            for (int i = 0; i < points.Count; i++)
                vis[i] = false;
            
            for (int i = 0; i < points.Count; i++)
            {
                if (mny > points[i].Y)
                {
                    mny = points[i].Y;
                    x = points[i].X;
                    ind = i;
                }
            }

            Line l = new Line(new Point(x, mny), new Point(x + 1000.0, mny));
            List<KeyValuePair<double, int>> list = new List<KeyValuePair<double, int>>();
            
            for (int i = 0; i < points.Count; i++)
            {
                if (i == ind) continue;
                Point v1 = tovector(l.Start, l.End);
                Point v2 = tovector(l.Start, points[i]);
                double cros = HelperMethods.CrossProduct(v1, v2);
                double dot = HelperMethods.DotProduct(v1, v2);
                double angle = Math.Atan2(cros, dot) * (180.00 / Math.PI); ;
                if (angle < 0)
                    angle += 360;
                list.Add(new KeyValuePair<double, int>(angle, i));
            }

            list.Sort(Compare1);
            Stack<int> st = new Stack<int>();
            st.Push(ind);
            
            if (list.Count > 0)
                st.Push(list[0].Value);
            
            for (int i = 1; i < points.Count - 1 && st.Count >= 2; i++)
            {
                int pp1 = st.Peek();
                Point p1 = points[pp1];
                st.Pop();
                int pp2 = st.Peek();
                Point p2 = points[pp2];
                st.Push(pp1);
                l = new Line(p2, p1);
                if (HelperMethods.CheckTurn(l, points[list[i].Value]) == Enums.TurnType.Left)
                {
                    st.Push(list[i].Value);
                }
                else if (HelperMethods.CheckTurn(l, points[list[i].Value]) == Enums.TurnType.Colinear)
                {
                    st.Pop();
                    st.Push(list[i].Value);
                }
                else
                {
                    st.Pop();
                    i--;
                }
            }
            while (st.Count > 0)
            {
                outPoints.Add(points[st.Peek()]);
                st.Pop();
            }
        }

        Point tovector(Point a, Point b)
        {
            return new Point(b.X - a.X, b.Y - a.Y);
        }
        static int com(Point a, Point b)
        {
            if (a.X == b.X) return a.Y.CompareTo(b.Y);
            return a.X.CompareTo(b.X);
        }
        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
        static int Compare1(KeyValuePair<double, int> a, KeyValuePair<double, int> b)
        {
            if (a.Key == b.Key) return a.Value.CompareTo(b.Value);
            return a.Key.CompareTo(b.Key);
        }
    }
}