using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;


namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {

        public Line BaseLine;
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count <= 3)
                outPoints = points;
            else
            {
                //the center point of the first three points:
                double X, Y;
                X = (points[0].X + points[1].X + points[2].X) / 3;
                Y = (points[0].Y + points[1].Y + points[2].Y) / 3;
                Point Base = new Point(X, Y);
                Point NB = new Point(X + 1000, Y);
                BaseLine = new Line(Base, NB);
                OrderedSet<Point> CH = new OrderedSet<Point>(angle_sort);
                CH.Add(points[0]);
                CH.Add(points[1]);
                CH.Add(points[2]);
                Point Prev, next;
                for (int i = 3; i < points.Count(); i++)
                {

                    Prev = CH.DirectRightAndLeftRotational(points[i]).Value;
                    next = CH.DirectRightAndLeftRotational(points[i]).Key;
                    // the point lies outside the polygon
                    if (HelperMethods.CheckTurn(new Line(Prev, next), points[i]) == Enums.TurnType.Right)
                    {

                        Point New_pre = CH.DirectRightAndLeftRotational(Prev).Value;
                        while (HelperMethods.CheckTurn(new Line(points[i], Prev), New_pre) == Enums.TurnType.Left ||
                            HelperMethods.CheckTurn(new Line(points[i], Prev), New_pre) == Enums.TurnType.Colinear)
                        {
                            CH.Remove(Prev);
                            Prev = New_pre;
                            New_pre = CH.DirectRightAndLeftRotational(Prev).Value;
                        }
                        Point New_next = CH.DirectRightAndLeftRotational(next).Key;
                        while (HelperMethods.CheckTurn(new Line(points[i], next), New_next) == Enums.TurnType.Right ||
                            HelperMethods.CheckTurn(new Line(points[i], Prev), New_pre) == Enums.TurnType.Colinear)
                        {
                            CH.Remove(next);
                            next = New_next;
                            New_next = CH.DirectRightAndLeftRotational(next).Key;
                        }
                        CH.Add(points[i]);
                    }
                }
                outPoints = CH.ToList();
            }
        }
        public int angle_sort(Point a, Point b)
        {
            double angle1 = HelperMethods.get_angle(BaseLine.Start, a);
            double angle2 = HelperMethods.get_angle(BaseLine.Start, b);

            if (angle1 < angle2)
                return -1;

            else if (angle1 > angle2)
                return 1;
            //in case two equal angles
            else
            {
                double dis_p1 = HelperMethods.distance(BaseLine.Start, a);
                double dis_p2 = HelperMethods.distance(BaseLine.Start, b);

                if (dis_p1 < dis_p2)
                    return -1;

                else if (dis_p1 > dis_p2)
                    return 1;

                else
                    return 0;

            }
        }
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
