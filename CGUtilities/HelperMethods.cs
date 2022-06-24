using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class HelperMethods
    {
        public static Enums.PointInPolygon PointInTriangle(Point p, Point a, Point b, Point c)
        {
            if (a.Equals(b) && b.Equals(c))
            {
                if (p.Equals(a) || p.Equals(b) || p.Equals(c))
                    return Enums.PointInPolygon.OnEdge;
                else
                    return Enums.PointInPolygon.Outside;
            }

            Line ab = new Line(a, b);
            Line bc = new Line(b, c);
            Line ca = new Line(c, a);

            if (GetVector(ab).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(bc).Equals(Point.Identity)) return (PointOnSegment(p, ca.Start, ca.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (GetVector(ca).Equals(Point.Identity)) return (PointOnSegment(p, ab.Start, ab.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == Enums.TurnType.Colinear)
                return PointOnSegment(p, a, b) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(bc, p) == Enums.TurnType.Colinear && PointOnSegment(p, b, c))
                return PointOnSegment(p, b, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            if (CheckTurn(ca, p) == Enums.TurnType.Colinear && PointOnSegment(p, c, a))
                return PointOnSegment(p, a, c) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;

            if (CheckTurn(ab, p) == CheckTurn(bc, p) && CheckTurn(bc, p) == CheckTurn(ca, p))
                return Enums.PointInPolygon.Inside;
            return Enums.PointInPolygon.Outside;
        }
        public static Enums.TurnType CheckTurn(Point vector1, Point vector2)
        {
            double result = CrossProduct(vector1, vector2);
            if (result < 0) return Enums.TurnType.Right;
            else if (result > 0) return Enums.TurnType.Left;
            else return Enums.TurnType.Colinear;
        }
        public static double distance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

        }
        public static double CrossProduct(Point a, Point b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static bool PointOnRay(Point p, Point a, Point b)
        {
            if (a.Equals(b)) return true;
            if (a.Equals(p)) return true;
            var q = a.Vector(p).Normalize();
            var w = a.Vector(b).Normalize();
            return q.Equals(w);
        }
        public static bool PointOnSegment(Point p, Point a, Point b)
        {
            if (a.Equals(b))
                return p.Equals(a);

            if (b.X == a.X)
                return p.X == a.X && (p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y));
            if (b.Y == a.Y)
                return p.Y == a.Y && (p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X));
            double tx = (p.X - a.X) / (b.X - a.X);
            double ty = (p.Y - a.Y) / (b.Y - a.Y);

            return (Math.Abs(tx - ty) <= Constants.Epsilon && tx <= 1 && tx >= 0);
        }
        /// <summary>
        /// Get turn type from cross product between two vectors (l.start -> l.end) and (l.end -> p)
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Enums.TurnType CheckTurn(Line l, Point p)
        {
            Point a = l.Start.Vector(l.End);
            Point b = l.End.Vector(p);
            return HelperMethods.CheckTurn(a, b);
        }
        public static Point GetVector(Line l)
        {
            return l.Start.Vector(l.End);
        }
        public static double DotProduct(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static int MinX(List<Point> points)
        {
            double minX = points[0].X;
            int index = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                    index = i;
                }
            }
            return index;
        }
        public static int MaxX(List<Point> points)
        {
            double maxX = points[0].X;
            int index = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                    index = i;
                }
            }
            return index;
        }
        public static int MinY(List<Point> points)
        {
            double minY = points[0].Y;
            int index = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                    index = i;
                }
            }
            return index;
        }
        public static int MaxY(List<Point> points)
        {
            double maxY = points[0].Y;
            int index = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y > maxY)
                {
                    maxY = points[i].Y;
                    index = i;
                }
            }
            return index;
        }
        public static bool Intersection(Line l1, Line l2)
        {
            if (CheckTurn(l1, l2.Start) != CheckTurn(l1, l2.End) &&
                CheckTurn(l2, l1.Start) != CheckTurn(l2, l1.End))
                return true;
            else
                return false;
        }
        public static double Slope(Line line)
        {
            return (line.End.Y - line.Start.Y) / (line.End.X - line.Start.X);
        }
        public static Point LineLineIntersectionPoint(Line line1, Line line2)
        {
            double m;
            double c;
            double x;
            //y=mx+c
            //m1x+c1 = m2x+c2
            //x = (c2-c1)/(m1-m2)
            if (line1.Start.X != line1.End.X && line2.Start.X != line2.End.X)
            {
                double m1 = Slope(line1);
                double c1 = line1.Start.Y - m1 * line1.Start.X;
                double m2 = Slope(line2);
                double c2 = line2.Start.Y - m2 * line2.Start.X;
                m = m1;
                c = c1;
                x = (c2 - c1) / (m1 - m2);
            }
            else if (line1.Start.X != line1.End.X)
            {
                m = Slope(line2);
                c = line2.Start.Y - m * line2.Start.X;
                x = line1.Start.X;
            }
            else
            {
                m = Slope(line1);
                c = line1.Start.Y - m * line1.Start.X;
                x = line2.Start.X;
            }
            double y = m * x + c;

            return new Point(x, y);
        }

        public static Point IntersectionPoint(Line line1, Line line2)
        {
            double m, c, x;
            //y = m1x+c1, y=m2x+c2
            //y1=y2
            //m1x+c1 = m2x+c2
            //y=mx+c
            //x = (c2-c1)/(m1-m2)
            if (line1.Start.X != line1.End.X && line2.Start.X != line2.End.X)
            {
                double m1 = Slope(line1);
                double c1 = line1.Start.Y - m1 * line1.Start.X;
                double m2 = Slope(line2);
                double c2 = line2.Start.Y - m2 * line2.Start.X;
                m = m1;
                c = c1;
                x = (c2 - c1) / (m1 - m2);
            }
            else if (line1.Start.X != line1.End.X)
            {
                m = Slope(line2);
                c = line2.Start.Y - m * line2.Start.X;
                x = line1.Start.X;
            }
            else
            {
                m = Slope(line1);
                c = line1.Start.Y - m * line1.Start.X;
                x = line2.Start.X;
            }
            double y = m * x + c;

            return new Point(x, y);
        }

        public static bool IS_points_Sorted_CCW(Point Min_X, int ind_OF_Min_X, List<Point> points)
        {
            // next rotational =( i + 1 ) % count ;
            //  pre rotational = ((i-1) + count) % count ;


            Point prev = points[(ind_OF_Min_X - 1 + points.Count()) % points.Count()];
            Point next = points[(ind_OF_Min_X + 1) % points.Count()];

            Line l1 = new Line(prev, next);
            if (HelperMethods.CheckTurn(l1, Min_X) == Enums.TurnType.Right)
            {
                return true;// points are sorted CCW
            }
            else
                return false; // points are sorted CW
        }

        public static double get_angle(Point a, Point b)
        {
            double x = a.X + 100;
            double y = a.Y;
            Point a2 = new Point(x, y);


            Point Vec_1 = a.Vector(a2);
            Point Vec_2 = a.Vector(b);


            double cross = HelperMethods.CrossProduct(Vec_1, Vec_2);
            double dot = HelperMethods.DotProduct(Vec_1, Vec_2);


            double angle = Math.Atan2(cross, dot) * (180 / Math.PI);
            if (angle < 0)
                angle += 360;

            return angle;
        }
    }
}