using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine:Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            // The events OrderedSet from minimum x to maximum x: 
            OrderedSet<Event> Q = new OrderedSet<Event>(
                (a, b) =>
                {
                    if (a.point.X == b.point.X)
                        return a.point.Y.CompareTo(b.point.Y);
                    else
                        return a.point.X.CompareTo(b.point.X);
                });

            // The Sweep Line OrderedSet from the maximum y to minimum y:
            OrderedSet<CGUtilities.Line> L = new OrderedSet<CGUtilities.Line>(
                (a, b) =>
                {
                    return -a.Start.Y.CompareTo(b.Start.Y);
                });

            //Assume s is lines list
            for (int i = 0; i < lines.Count; i++)
            {
                Q.Add(new Event(lines[i].Start, "StartPoint", i));
                Q.Add(new Event(lines[i].End, "EndPoint", i));
            }

            //HandleEvent:
            while (Q.Count != 0)
            {
                //min E 
                Event E = Q.First();

                if (E.type.Equals("StartPoint"))
                {
                    //add the segment to the sweep line
                    CGUtilities.Line segment = lines[E.segment1];
                    L.Add(segment);

                    // calculate s+ and s-
                    var UpperAndLower = L.DirectUpperAndLower(segment);
                    CGUtilities.Line uper_seg = UpperAndLower.Key;
                    CGUtilities.Line lower_seg = UpperAndLower.Value;

                    //If there is intersectionPoint between segment and s+ add it
                    if (uper_seg != null && CGUtilities.HelperMethods.Intersection(uper_seg, segment))
                    {
                        CGUtilities.Point intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(uper_seg, segment);
                        if (!outPoints.Contains(intersectionPoint))
                            Q.Add(new Event(intersectionPoint, "IntersectionPoint", lines.IndexOf(uper_seg), lines.IndexOf(segment)));
                    }

                    //If there is intersectionPoint between segment and s- add it
                    if (lower_seg != null && CGUtilities.HelperMethods.Intersection(segment, lower_seg))
                    {
                        CGUtilities.Point intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(segment, lower_seg);
                        if (!outPoints.Contains(intersectionPoint))
                            Q.Add(new Event(intersectionPoint, "IntersectionPoint", lines.IndexOf(segment), lines.IndexOf(lower_seg)));
                    }
                }
                else if (E.type.Equals("EndPoint"))
                {
                    //Remove the segment from the sweep line
                    CGUtilities.Line segment = lines[E.segment1];
                    var UpperAndLower = L.DirectUpperAndLower(segment);
                    CGUtilities.Line uper_seg = UpperAndLower.Key;
                    CGUtilities.Line lower_seg = UpperAndLower.Value;
                    L.Remove(segment);

                    if (uper_seg != null && lower_seg != null && CGUtilities.HelperMethods.Intersection(uper_seg, lower_seg))
                    {
                        CGUtilities.Point intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(uper_seg, lower_seg);
                        if (!outPoints.Contains(intersectionPoint))
                            Q.Add(new Event(intersectionPoint, "IntersectionPoint", lines.IndexOf(uper_seg), lines.IndexOf(lower_seg)));
                    }
                }
                else
                {
                    outPoints.Add(E.point);
                    CGUtilities.Line segment1 = lines[E.segment1];
                    CGUtilities.Line segment2 = lines[E.segment2];

                    //Get  L.prev(s1) and  L.next(s2)
                    var seg1UpperAndLower = L.DirectUpperAndLower(segment1);
                    CGUtilities.Line uper_seg = seg1UpperAndLower.Key;
                    var seg2UpperAndLower = L.DirectUpperAndLower(segment2);
                    CGUtilities.Line lower_seg = seg2UpperAndLower.Value;

                    if (uper_seg != null && CGUtilities.HelperMethods.Intersection(uper_seg, segment2))
                    {
                        CGUtilities.Point intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(uper_seg, segment2);
                        if (!outPoints.Contains(intersectionPoint))
                            Q.Add(new Event(intersectionPoint, "IntersectionPoint", lines.IndexOf(uper_seg), lines.IndexOf(segment2)));
                    }

                    if (lower_seg != null && CGUtilities.HelperMethods.Intersection(lower_seg, segment1))
                    {
                        CGUtilities.Point intersectionPoint = CGUtilities.HelperMethods.IntersectionPoint(lower_seg, segment1);
                        if (!outPoints.Contains(intersectionPoint))
                            Q.Add(new Event(intersectionPoint, "IntersectionPoint", lines.IndexOf(lower_seg), lines.IndexOf(segment1)));
                    }

                    //Swap segment1 and segment2 in L
                    L.Remove(segment1);
                    L.Remove(segment2);
                    CGUtilities.Line Swap = segment1;
                    segment1 = segment2;
                    segment2 = Swap;
                    L.Add(segment1);
                    L.Add(segment2);
                }

                Q.RemoveFirst();
            }
        }

        public class Event
        {
            public CGUtilities.Point point { get; set; }
            public int segment1 { get; set; }
            public int segment2 { get; set; }
            public string type { get; set; }
            public Event(CGUtilities.Point point_p, string type_t, int segment_s)
            {
                point = point_p;
                type = type_t;
                segment1 = segment_s;
            }
            public Event(CGUtilities.Point point_p, string type_t, int segment_s1, int segment_s2)
            {
                point = point_p;
                type = type_t;
                segment1 = segment_s1;
                segment2 = segment_s2;
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
