using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            if (points.Count <= 3)
                outPoints = points;

            else
            {
                double minX = points[0].X;
                int indmin_x = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].X < minX)
                    {
                        minX = points[i].X;
                        indmin_x = i;
                    }
                }

                double maxX = points[0].X;
                int indmax_x = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].X > maxX)
                    {
                        maxX = points[i].X;
                        indmax_x = i;
                    }
                }

                double minY = points[0].Y;
                int indmin_y = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Y < minY)
                    {
                        minY = points[i].Y;
                        indmin_y = i;
                    }
                }
                double maxY = points[0].Y;
                int indmax_y = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Y > maxY)
                    {
                        maxY = points[i].Y;
                        indmax_y = i;
                    }
                }

                outPoints.Add(points[indmax_x]);
                outPoints.Add(points[indmin_x]);
                outPoints.Add(points[indmax_y]);
                outPoints.Add(points[indmin_y]);

                Line l1 = new Line(points[indmin_x], points[indmin_y]);
                Line l2 = new Line(points[indmin_y], points[indmax_x]);
                Line l3 = new Line(points[indmax_x], points[indmax_y]);
                Line l4 = new Line(points[indmax_y], points[indmin_x]);

                List<Point> d1 = new List<Point>();
                List<Point> d2 = new List<Point>();
                List<Point> d3 = new List<Point>();
                List<Point> d4 = new List<Point>();

                List<Point> Res1 = new List<Point>();
                List<Point> Res2 = new List<Point>();
                List<Point> Res3 = new List<Point>();
                List<Point> Res4 = new List<Point>();
                for (int i = 0; i < points.Count; i++)
                {
                    int count = 0;

                    Enums.TurnType type1 = HelperMethods.CheckTurn(l1, points[i]);

                    if (type1 == Enums.TurnType.Left)
                        count++;

                    Enums.TurnType type2 = HelperMethods.CheckTurn(l2, points[i]);

                    if (type2 == Enums.TurnType.Left)
                        count++;


                    Enums.TurnType type3 = HelperMethods.CheckTurn(l3, points[i]);

                    if (type3 == Enums.TurnType.Left)
                        count++;

                    Enums.TurnType type4 = HelperMethods.CheckTurn(l4, points[i]);

                    if (type4 == Enums.TurnType.Left)
                        count++;
                    if (count == 0 || count == 4)
                        points.Remove(points[i]);
                    if (type1 == Enums.TurnType.Right)
                        d1.Add(points[i]);
                    if (type2 == Enums.TurnType.Right)
                        d2.Add(points[i]);
                    if (type3 == Enums.TurnType.Right)
                        d3.Add(points[i]);
                    if (type4 == Enums.TurnType.Right)
                        d4.Add(points[i]);
                }

                if (d1.Count > 0)
                    Res1 = Quick_Hull(d1, l1);
                if (d2.Count > 0)
                    Res2 = Quick_Hull(d2, l2);
                if (d3.Count > 0)
                    Res3 = Quick_Hull(d3, l3);
                if (d4.Count > 0)
                    Res4 = Quick_Hull(d4, l4);

                if (Res1.Count > 0)
                    outPoints = outPoints.Concat(Res1).ToList();
                if (Res2.Count > 0)
                    outPoints = outPoints.Concat(Res2).ToList();
                if (Res3.Count > 0)
                    outPoints = outPoints.Concat(Res3).ToList();
                if (Res4.Count > 0)
                    outPoints = outPoints.Concat(Res4).ToList();

                outPoints.Sort(same_line);
                List<Point> l = new List<Point>();
                l.Add(outPoints[0]);
                for (int i = 1; i < outPoints.Count; i++)
                {
                    if (outPoints[i].Equals(outPoints[i - 1]))
                        continue;
                    l.Add(outPoints[i]);
                }
                outPoints = l;
            }
        }

        public static List<Point> Quick_Hull(List<Point> points, Line l)
        {
            if (points.Count == 0)
                return new List<Point>();

            Point farthest_point = max_distance(points, l.Start, l.End);

            List<Point> External_1 = new List<Point>();
            List<Point> External_2 = new List<Point>();

            Line l1 = new Line(l.Start, farthest_point);
            Line l2 = new Line(farthest_point, l.End);
            for (int i = 0; i < points.Count; i++)
            {
                if (HelperMethods.CheckTurn(l1, points[i]) == Enums.TurnType.Right)
                    External_1.Add(points[i]);
                if (HelperMethods.CheckTurn(l2, points[i]) == Enums.TurnType.Right)
                    External_2.Add(points[i]);
            }

            List<Point> Res1 = new List<Point>();
            List<Point> Res2 = new List<Point>();

            if (External_1.Count > 0)
                Res1 = Quick_Hull(External_1, l1);
            if (External_2.Count > 0)
                Res2 = Quick_Hull(External_2, l2);

            List<Point> Res = new List<Point>();
            if (Res1.Count > 0)
                Res = Res.Concat(Res1).ToList();
            if (Res2.Count > 0)
                Res = Res.Concat(Res2).ToList();
            Res.Add(farthest_point);

            return Res;
        }

        public static Point max_distance(List<Point> points, Point s, Point e)
        {
            double m = ((e.Y - s.Y) / (e.X - s.X));
            double c = (e.Y - m * e.X);

            double m2 = -1 / m;

            double max_dist = int.MinValue;
            Point max_point = new Point(int.MaxValue, int.MaxValue);
            foreach (Point p in points)
            {
                double x = ((-m2 * p.X) + p.Y - c) / (m - m2);
                double Y = (m * x) + c;

                double dist = Math.Sqrt(Math.Pow(p.X - x, 2) + Math.Pow(p.Y - Y, 2));

                if (dist > max_dist)
                {
                    max_dist = dist;
                    max_point = p;
                }
            }
            return max_point;
        }

        static int same_line(Point a, Point b)
        {
            if (a.X == b.X) return a.Y.CompareTo(b.Y);
            return a.X.CompareTo(b.X);
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}