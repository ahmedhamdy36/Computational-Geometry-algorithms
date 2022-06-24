using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            points.Sort((a, b) => {
                if (a.X == b.X) return a.Y.CompareTo(b.Y);
                return a.X.CompareTo(b.X);
            });
            outPoints = GetConvexHull(points);
        }

        List<Point> GetConvexHull(List<Point> points)
        {
            if (points.Count <= 3)
                return SortAntiClockwise(points);

            double sum = 0;
            foreach (Point p in points)
                sum += p.X;

            double mean = sum / points.Count;
            List<Point> L = new List<Point>();
            List<Point> R = new List<Point>();

            foreach (Point p in points)
            {
                if (p.X < mean)
                    L.Add(p);
                else
                    R.Add(p);
            }

            if (L.Count == 0)
                return new List<Point> { R[0], R[R.Count - 1] };
            if (R.Count == 0)
                return new List<Point> { L[0], L[L.Count - 1] };

            List<Point> LHull = GetConvexHull(L);
            List<Point> RHull = GetConvexHull(R);

            return Merge(LHull, RHull);
        }

        List<Point> Merge(List<Point> LHull, List<Point> RHull)
        {
            int l = HelperMethods.MaxX(LHull);
            int r = HelperMethods.MinX(RHull);
            Tuple<int, int> lower = GetTangent(LHull, RHull, l, r);
            Tuple<int, int> upper = GetTangent(RHull, LHull, r, l);

            List<Point> merged = new List<Point>();
            for (int i = lower.Item2; i != upper.Item1; i = (i + 1) % RHull.Count)
                merged.Add(RHull[i]);

            merged.Add(RHull[upper.Item1]);
            for (int i = upper.Item2; i != lower.Item1; i = (i + 1) % LHull.Count)
                merged.Add(LHull[i]);

            merged.Add(LHull[lower.Item1]);
            for (int i = 0; i < merged.Count; i++)
                if (HelperMethods.CheckTurn(new Line(merged[(i - 1 + merged.Count) % merged.Count], merged[i]),
                                            merged[(i + 1) % merged.Count]) == Enums.TurnType.Colinear)
                    merged.RemoveAt(i);

            return merged;
        }

        Tuple<int, int> GetTangent(List<Point> LHull, List<Point> RHull, int l, int r)
        {
            Line tangent = new Line(LHull[l], RHull[r]);
            while (true)
            {
                while (HelperMethods.CheckTurn(tangent, LHull[(l + 1) % LHull.Count]) == Enums.TurnType.Right ||
                       HelperMethods.CheckTurn(tangent, LHull[(l - 1 + LHull.Count) % LHull.Count]) == Enums.TurnType.Right)
                {
                    l = (l - 1 + LHull.Count) % LHull.Count;
                    tangent = new Line(LHull[l], RHull[r]);
                }
                while (HelperMethods.CheckTurn(tangent, RHull[(r + 1) % RHull.Count]) == Enums.TurnType.Right ||
                       HelperMethods.CheckTurn(tangent, RHull[(r - 1 + RHull.Count) % RHull.Count]) == Enums.TurnType.Right)
                {
                    r = (r + 1) % RHull.Count;
                    tangent = new Line(LHull[l], RHull[r]);
                }
                if (HelperMethods.CheckTurn(tangent, LHull[(l + 1) % LHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(tangent, LHull[(l - 1 + LHull.Count) % LHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(tangent, RHull[(r + 1) % RHull.Count]) != Enums.TurnType.Right &&
                    HelperMethods.CheckTurn(tangent, RHull[(r - 1 + RHull.Count) % RHull.Count]) != Enums.TurnType.Right)
                    break;
            }
            return Tuple.Create(l, r);
        }

        List<Point> SortAntiClockwise(List<Point> points)
        {
            int minY = HelperMethods.MinY(points);

            Line l = new Line(points[minY], new Point(points[minY].X + 1000.0, points[minY].Y));
            List<Tuple<double, int>> list = new List<Tuple<double, int>>();

            for (int i = 0; i < points.Count; i++)
            {
                if (i == minY) continue;
                Point v1 = l.Start.Vector(l.End);
                Point v2 = l.Start.Vector(points[i]);

                double cros = HelperMethods.CrossProduct(v1, v2);
                double dot = HelperMethods.DotProduct(v1, v2);
                double angle = Math.Atan2(cros, dot) * (180.00 / Math.PI);

                if (angle < 0)
                    angle += 360;

                list.Add(Tuple.Create(angle, i));
            }

            list.Sort((a, b) => {
                if (a.Item1 == b.Item1) return a.Item2.CompareTo(b.Item2);
                return a.Item1.CompareTo(b.Item1);
            });

            List<Point> sortedPoints = new List<Point>();
            sortedPoints.Add(points[minY]);

            foreach (var pair in list)
                sortedPoints.Add(points[pair.Item2]);

            return sortedPoints;
        }

        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }
    }
}