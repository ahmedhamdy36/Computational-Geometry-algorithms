using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class smart_point_Monotone_Partion
    {
        public int id;
        public Point current_point;
        public string Type;
        public Line prev_line;
        public Line next_line;

        public smart_point_Monotone_Partion()
        {
            this.id = 0;
            this.current_point = new Point(0, 0);
            this.Type = "start";
        }

        public smart_point_Monotone_Partion(int num, Point postion, string kind, Line pre, Line next)
        {
            this.id = num;
            this.current_point = postion;
            this.Type = kind;
            this.prev_line = pre;
            this.next_line = next;
        }
    }

    class smart_edge
    {
        public Line edge_values;
        public int id;
        public smart_point_Monotone_Partion helper;

        public smart_edge()
        {
            this.id = 0;
            this.helper = new smart_point_Monotone_Partion();
        }
        public smart_edge(Line postion, int num, smart_point_Monotone_Partion p_helper)
        {
            this.id = num;
            this.edge_values = postion;
            this.helper = p_helper;
        }
    }


    class MonotonePartitioning : Algorithm
    {
        OrderedSet<smart_point_Monotone_Partion> Sorted_AND_Calssified_points;
        Point mini_x = new Point(double.MaxValue, double.MaxValue);
        int index_of_mini_x;

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            #region Sort_Polygon_CCW
            /// First at all we must check That polygon is sorted CCW
            List<Point> poly_input = new List<Point>();
            int size = polygons[0].lines.Count;
            for (int i = 0; i < size; i++)
            {
                Point p = polygons[0].lines[i].Start;
                if (p.X <= mini_x.X)
                {
                    if (p.X < mini_x.X)
                    {
                        mini_x = p;
                        index_of_mini_x = i;
                    }

                    else if (p.Y < mini_x.Y)
                    {
                        mini_x = p;
                        index_of_mini_x = i;
                    }
                }
                poly_input.Add(p);
            }

            bool Points_sorted_CCW = HelperMethods.IS_points_Sorted_CCW(mini_x, index_of_mini_x, poly_input);

            // if points were sorted CC then Resort them to be CCW
            if (Points_sorted_CCW == false)
            {
                /*  for (int i = 0; i < polygons[0].lines.Count; i++)
                    {
                        Point temp = polygons[0].lines[i].Start;

                        polygons[0].lines[i].Start = polygons[0].lines[i].End;
                        polygons[0].lines[i].End = temp;

                    }
                  */
                polygons[0].lines.Reverse();
                poly_input.Reverse();
            }

            #endregion

            #region Sorted_AND_Calssified
            // Step 1 : Sort and Calssify each point , and fill list Sorted_AND_Calssified_points 
            Comparison<smart_point_Monotone_Partion> comp = new Comparison<smart_point_Monotone_Partion>(Sort_Polygon);
            Sorted_AND_Calssified_points = new OrderedSet<smart_point_Monotone_Partion>(comp);

            // Step 1.1 :: sort points based on y_axis 
            for (int i = 0; i < size; i++)
            {
                Point p = polygons[0].lines[i].Start;
                Line pre = polygons[0].lines[getPrevIndex(i, size)];
                Line next = polygons[0].lines[getNextIndex(i, size)];

                Sorted_AND_Calssified_points.Add(new smart_point_Monotone_Partion(i, p, "", pre, next));
            } //end of for loop 

            int index = Sorted_AND_Calssified_points[0].id;
            // Step 1.2 :: classify each point 
            for (int i = 0; i < size; i++)
            {
                Point p = Sorted_AND_Calssified_points[i].current_point;
                int id = Sorted_AND_Calssified_points[i].id;
                Sorted_AND_Calssified_points[i].Type = Classify_point(p, id, poly_input);
            }
            #endregion

            #region Monotone_partitioning_Algorithm

            OrderedSet<smart_edge> SweepLine = new OrderedSet<smart_edge>(new Comparison<smart_edge>(Sort_Sweep_line));
            for (int i = 0; i < size; i++)
            {
                smart_point_Monotone_Partion V = Sorted_AND_Calssified_points[i];
                #region HandleStartVertex 
                if (V.Type == "start")
                    SweepLine.Add(new smart_edge(V.next_line, V.id, V));
                #endregion

                #region HandleEndVertex
                else if (V.Type == "end")
                {
                    Line prev = V.prev_line;
                    for (int e = 0; e < SweepLine.Count; e++)
                    {
                        if (SweepLine[e].edge_values.Equals(prev))
                        {
                            if (SweepLine[e].helper.Equals("merge"))
                                outLines.Add(new Line(SweepLine[e].helper.current_point, V.current_point));

                            SweepLine.Remove(SweepLine[e]);
                        }
                    }
                }

                #endregion

                #region HandleSplitVertex
                else if (V.Type.Equals("split"))
                {
                    smart_edge most_left_adge = SweepLine.GetFirst();
                    outLines.Add(new Line(V.current_point, most_left_adge.helper.current_point));
                    most_left_adge.helper = V;
                    SweepLine.Add(new smart_edge(V.next_line, V.id, V));
                }
                #endregion

                #region HandleMergeVertex
                else if (V.Type.Equals("merge"))
                {
                    Line prev = V.prev_line;
                    for (int e = 0; e < SweepLine.Count; e++)
                    {
                        if (SweepLine[e].edge_values.Equals(prev))
                        {
                            if (SweepLine[e].helper.Type.Equals("merge"))
                                outLines.Add(new Line(V.current_point, SweepLine[e].helper.current_point));

                            SweepLine.Remove(SweepLine[e]);
                        }
                    }

                    smart_edge most_left = SweepLine.GetFirst();
                    if (most_left.helper.Type.Equals("merge"))
                        outLines.Add(new Line(V.current_point, most_left.helper.current_point));

                    most_left.helper = V;
                }
                #endregion

                #region HandleRegularVertex
                else
                {
                    #region HandleRegularLeft
                    if (V.Type.Equals("regular_left"))
                    {
                        Line prev = V.prev_line;
                        for (int e = 0; e < SweepLine.Count; e++)
                        {
                            if (SweepLine[e].edge_values.Equals(prev))
                            {
                                if (SweepLine[e].helper.Type.Equals("merge"))
                                    outLines.Add(new Line(V.current_point, SweepLine[e].helper.current_point));

                                SweepLine.Remove(SweepLine[e]);
                                SweepLine.Add(new smart_edge(V.next_line, V.id, V));
                            }

                        }
                    }
                    #endregion

                    #region HnadleRegularRight
                    else if (V.Type.Equals("regular_right"))
                    {
                        smart_edge most_left = SweepLine.GetFirst();
                        if (most_left.helper.Type.Equals("merge"))
                            outLines.Add(new Line(V.current_point, most_left.helper.current_point));

                        most_left.helper = V;
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
        } //end of run 

        private int Sort_Polygon(smart_point_Monotone_Partion p1, smart_point_Monotone_Partion p2)
        {
            if (p1.current_point.Y < p2.current_point.Y)
            {
                return 1;
            }
            else if (p1.current_point.Y > p2.current_point.Y)
            {
                return -1;
            }
            else
            {
                if (p1.current_point.X < p2.current_point.X)
                {
                    return 1;
                }
                else if (p1.current_point.X > p2.current_point.X)
                {
                    return -1;
                }
                else
                    return 0;
            }
        } //end of Sort_Polygon

        private int Sort_Sweep_line(smart_edge e1, smart_edge e2)
        {
            if (e1.edge_values.Start.X < e2.edge_values.Start.X && e1.edge_values.End.X < e2.edge_values.End.X)
            {
                return -1;
            }
            else if (e1.edge_values.Start.X > e2.edge_values.Start.X && e1.edge_values.End.X > e2.edge_values.End.X)
            {
                return 1;
            }
            else
                return 0;
        }
        private string Classify_point(Point p, int index, List<Point> _polygon)
        {
            string type = "";
            int Size = _polygon.Count();


            Point prev = _polygon[getPrevIndex(index, Size)];
            Point next = _polygon[getNextIndex(index, Size)];

            //point is Start Point
            if (HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Left
                && (p.Y > prev.Y && p.Y > next.Y))
            {
                type = "start";
            }

            //Point is End Point
            else if (HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Left
                && (p.Y < prev.Y && p.Y < next.Y))
            {
                type = "end";
            }


            //Point is Merge Vertix
            else if (HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Right
                && (p.Y < prev.Y && p.Y < next.Y))
            {
                type = "merge";
            }

            //Point is Split Vertex
            else if (HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Right
                && (p.Y > prev.Y && p.Y > next.Y))
            {
                type = "split";
            }

            //point is Regular Left
            else if ((HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Right ||
                          HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Left)
                           && (p.Y < prev.Y && p.Y > next.Y))
            {
                type = "regular_left";
            }


            //point is Regular Right
            if ((HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Right ||
                        HelperMethods.CheckTurn(new Line(prev, p), next) == Enums.TurnType.Left)
                        && (p.Y > prev.Y && p.Y < next.Y)) // point is convex or concave
            {
                type = "regular_right";
            }

            return type;
        } //end of classify function 

        /// <summary>
        /// Get Prev index point
        private int getPrevIndex(int index, int polygonSize)
        {
            return (index - 1 + polygonSize) % polygonSize;
        }

        /// <summary>
        /// Get next index point
        private int getNextIndex(int index, int polygonSize)
        {
            return (index + 1) % polygonSize;
        }


        public override string ToString()
        {
            return "Monotone Partitioning";
        }
    }
}