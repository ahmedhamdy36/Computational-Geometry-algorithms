using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class MonotoneTriangulation  :Algorithm
    {
        public override void Run(System.Collections.Generic.List<CGUtilities.Point> points, System.Collections.Generic.List<CGUtilities.Line> lines, System.Collections.Generic.List<CGUtilities.Polygon> polygons, ref System.Collections.Generic.List<CGUtilities.Point> outPoints, ref System.Collections.Generic.List<CGUtilities.Line> outLines, ref System.Collections.Generic.List<CGUtilities.Polygon> outPolygons)
        {
            Polygon pol = new Polygon(lines);
            pol = CounterClockwise(pol); //take O(N)
            bool check = CheckMonotone(pol); //take O(N)

            for (int i = 0; i < pol.lines.Count; i++)
                points.Add(pol.lines[i].Start);

            //sort the points on max Y and max X on tie O(n)
            List<Point> E = new List<Point>();
            for (int i = 0; i < pol.lines.Count; i++)
                E.Add(pol.lines[i].Start);
            E.Sort(point_sort);

            Stack<Point> S = new Stack<Point>();
            S.Push(E[0]);
            S.Push(E[1]);
            int current = 2;

            while (current != pol.lines.Count)
            {
                Point p = E[current];
                Point top = S.Peek();
                //Assume E[0].X this is the value which detemines the left and the right side
                // E[current].X < E[0].X the current point lies in the left side
                // E[current].X > E[0].X the current point lies in the right side
                bool same_side = false;
                if (p.X < E[0].X && top.X < E[0].X)
                    same_side = true;
                else if (p.X > E[0].X && top.X > E[0].X)
                    same_side = true;

                // P and Top on the same side 
                if (same_side == true)
                {
                    S.Pop();
                    Point top2 = S.Peek();

                    //Check the top point is convex or not
                    int index = points.IndexOf(top);
                    if (IsConvex(pol, index) == true)
                    {
                        outLines.Add(new Line(p, top2));
                        if (S.Count == 1)
                        {
                            S.Push(p);
                            current++;
                        }
                    }
                    else
                    {
                        S.Push(top);
                        S.Push(p);
                        current++;
                    }
                }
                //P and Top on different side 
                else
                {
                    while (S.Count != 1)
                    {
                        Point top2 = S.Pop();
                        outLines.Add(new Line(p, top2));
                    }
                    S.Pop();
                    S.Push(top);
                    S.Push(p);
                }
            }
        }

        //Check the orientation of the polygon
        public Polygon CounterClockwise(Polygon pol)
        {
            double signed_area = 0;
            for (int i = 0; i < pol.lines.Count; i++)
                signed_area += (pol.lines[i].End.X - pol.lines[i].Start.X) * (pol.lines[i].End.Y + pol.lines[i].Start.Y);
            signed_area /= 2;

            //signed_area > 0 : clockwise
            if (signed_area > 0)
            {
                //convert the sort from CW to CCW
                pol.lines.Reverse();
                for (int i = 0; i < pol.lines.Count; i++)
                {
                    Point replace = pol.lines[i].Start;
                    pol.lines[i].Start = pol.lines[i].End;
                    pol.lines[i].End = replace;
                }
            }
            return pol;
        }

        //Check Monotone: this function return true if and only if no cusp points
        public bool CheckMonotone(Polygon pol)
        {
            // the count to determine the number of cusp points
            int count = 0;

            for (int i = 0; i < pol.lines.Count; i++)
            {

                int previous = ((i - 1) + pol.lines.Count) % pol.lines.Count;
                int next = (i + 1) % pol.lines.Count;

                Point p = pol.lines[i].Start;
                Point prev_p = pol.lines[previous].Start;
                Point next_p = pol.lines[next].Start;

                //the two edges lie in the same side and the angle > 180
                if (next_p.Y < p.Y && prev_p.Y < p.Y && !IsConvex(pol, i))
                    count++;
                else if (next_p.Y > p.Y && prev_p.Y > p.Y && !IsConvex(pol, i))
                    count++;
            }

            if (count == 0)
                return true;

            return false;
        }

        //Check Convex point 
        public bool IsConvex(Polygon p, int Current)
        {
            int previous = ((Current - 1) + p.lines.Count) % p.lines.Count;
            int next = (Current + 1) % p.lines.Count;

            Point p1 = p.lines[previous].Start;
            Point p2 = p.lines[Current].Start;
            Point p3 = p.lines[next].Start;
            Line l = new Line(p1, p2);
            if (HelperMethods.CheckTurn(l, p3) == Enums.TurnType.Left)
                return true;
            return false;
        }

        //sort the points on max Y and max X on tie O(n)
        public static int point_sort(Point a, Point b)
        {
            if (a.Y == b.Y) 
                return -a.X.CompareTo(b.X);
            else
                return -a.Y.CompareTo(b.Y);
        }

        public override string ToString()
        {
            return "Monotone Triangulation";
        }
    }
}
