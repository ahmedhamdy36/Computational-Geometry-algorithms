using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> P = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                if (!P.Contains(points[i]))
                    P.Add(points[i]);
            }
            points = P;

            Point minimumPoint;
            double minimumY = 9999999999, minimumX = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y < minimumY)
                {
                    minimumY = points[i].Y;
                    minimumX = points[i].X;
                    minimumPoint = points[i];
                }
            }

            Point minimumPoint2 = new Point(minimumX, minimumY);
            outPoints.Add(minimumPoint2);

            Point ex = new Point(minimumX - 20, minimumY);
            Point start = minimumPoint2;

            while (true)
            {
                double largest_angle = 0;
                Point next = minimumPoint2;
                double dist = 0;
                double largest_dist = 0;

                for (int i = 0; i < points.Count; i++)
                {
                    Point ab = new Point(minimumPoint2.X - ex.X, minimumPoint2.Y - ex.Y);
                    Point ac = new Point(points[i].X - minimumPoint2.X, points[i].Y - minimumPoint2.Y);

                    double cross = HelperMethods.CrossProduct(ab, ac);
                    double dot = (ab.X * ac.X) + (ab.Y * ac.Y);
                    
                    double angle = Math.Atan2(cross, dot);
                    if (angle < 0)
                        angle = angle + (2 * Math.PI);

                    dist = Math.Sqrt((minimumPoint2.X - points[i].X) + (minimumPoint2.Y - points[i].Y));
                    if (angle > largest_angle)
                    {
                        largest_angle = angle;
                        largest_dist = dist;
                        next = points[i];
                    }
                    else if (angle == largest_angle && dist > largest_dist)
                    {
                        largest_dist = dist;
                        next = points[i];
                    }
                }   
                    
                outLines.Add(new Line(next, minimumPoint2));
                if (start.X == next.X && start.Y == next.Y)
                    break;

                outPoints.Add(next);

                ex = minimumPoint2;
                minimumPoint2 = next;
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
