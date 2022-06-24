using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class InsertingDiagonals : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            Polygon polygon = new Polygon(lines);
            polygon = CounterClockwise(polygon);
            outLines = Inserting_Diagonals(polygon);
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

        //Inserte Diagonals
        public List<Line> Inserting_Diagonals(Polygon Polygon)
        {
            //lines in triangels
            List<Line> outputDiagonals = new List<Line>();

            if (Polygon.lines.Count <= 3)
                return outputDiagonals;

            int Current = 0;
            while (true)
            {
                if (IsConvex(Polygon, Current) == true)
                    break;
                else
                    Current++;
            }

            int previous = ((Current - 1) + Polygon.lines.Count) % Polygon.lines.Count;
            int next = (Current + 1) % Polygon.lines.Count;

            Point p1 = Polygon.lines[previous].Start;
            Point p2 = Polygon.lines[Current].Start;
            Point p3 = Polygon.lines[next].Start;

            //to calculate the number of the points inside the triangle (cprev, c, cnext)
            List<Point> points_in_triangle = new List<Point>();
            for (int i = 0; i < Polygon.lines.Count; i++)
            {
                Point po = Polygon.lines[i].Start;
                if (HelperMethods.PointInTriangle(po, p1, p2, p3) == Enums.PointInPolygon.Inside)
                    points_in_triangle.Add(po);
            }

            //no points in the triangle
            Line l;
            if (points_in_triangle.Count == 0)
            {
                l = new Line(p1, p3);
                outputDiagonals.Add(l);
            }

            else
            {
                //to calculate the max distance between the inside point in the triangle and the line (c.prev, c.next) 
                List<double> distances = new List<double>();
                for (int i = 0; i < points_in_triangle.Count; i++)
                    distances.Add(distance(p1, p3, points_in_triangle[i]));

                int MaxDistantPoint = distances.IndexOf(distances.Max());
                l = new Line(p2, points_in_triangle[MaxDistantPoint]);
                outputDiagonals.Add(l);
            }

            //Divide the polygon to 2 subpolygons
            Polygon po1, po2;
            List<Line> lines1 = new List<Line>();
            List<Line> lines2 = new List<Line>();
            int start = 0, end = 0;
            for (int i = 0; i < Polygon.lines.Count; i++)
            {
                if (Polygon.lines[i].Start.Equals(l.Start))
                    start = i;
                if (Polygon.lines[i].Start.Equals(l.End))
                    end = i;
            }

            int go = start;
            while (true)
            {
                if (go == end)
                    break;
                lines1.Add(Polygon.lines[go]);
            }
            lines1.Add(new Line(l.End, l.Start));
            po1 = new Polygon(lines1);

            go = end;
            while (true)
            {
                if (go == start)
                    break;
                lines2.Add(Polygon.lines[go]);
            }
            lines2.Add(new Line(l.Start, l.End));
            po2 = new Polygon(lines2);

            List<Line> Result1 = new List<Line>();
            List<Line> Result2 = new List<Line>();

            Result1 = Inserting_Diagonals(po1);
            Result2 = Inserting_Diagonals(po2);

            List<Line> Result = new List<Line>();
            Result = Result1.Concat(Result2).ToList();
            return Result;
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

        //Calculate the distance between line(p1,p2) and point p0
        public double distance(Point p1, Point p2, Point p0)
        {
            double result;
            result = Math.Abs(((p2.X - p1.X) * (p1.Y - p0.Y)) - ((p1.X - p0.X) * (p2.Y - p1.Y)));
            result /= Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            return result;
        }
        
        public override string ToString()
        {
            return "Inserting Diagonals";
        }
    }
}
