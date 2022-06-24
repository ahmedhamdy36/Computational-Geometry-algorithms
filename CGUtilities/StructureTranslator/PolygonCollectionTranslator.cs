using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CGUtilities.StructureTranslator
{
    public class PolygonCollectionTranslator : ICollectionTranslate
    {
        public string Encode<T>(List<T> elements)
        {
            StringBuilder _builder = new StringBuilder();
            _builder.AppendLine("BeginSection:Polygons");
            foreach (var singlePolygon in elements)
            {
                Polygon candidate = singlePolygon as Polygon;
                foreach (Line singleLine in candidate.lines)
                {
                    _builder.Append(singleLine.Start.X + "," + singleLine.Start.Y + "," + singleLine.End.X + "," + singleLine.End.Y);
                    if (!candidate.lines[candidate.lines.Count - 1].Equals(singleLine)) _builder.Append(",");
                }
                _builder.AppendLine();
            }
            _builder.AppendLine("EndSection:Polygons");
            return _builder.ToString();
        }

        public List<object> Decode(string content)
        {
            List<object> result = new List<object>();
            string[] tokens = content.Split(new[] { "BeginSection:Polygons", "EndSection:Polygons" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 1) return null;
            StringReader stringReader = new StringReader(tokens[1].Trim());
            while (stringReader.Peek() != -1)
            {
                Polygon candidate = new Polygon();
                string[] lineTokens = stringReader.ReadLine().Split(',');
                for (int linePointIndex = 0; linePointIndex < lineTokens.Length; linePointIndex+=4)
                {
                    candidate.lines.Add(new Line(new Point(double.Parse(lineTokens[linePointIndex]), double.Parse(lineTokens[linePointIndex+1])), new Point(double.Parse(lineTokens[linePointIndex+2]), double.Parse(lineTokens[linePointIndex+3]))));
                }
                result.Add(candidate);
            }
            stringReader.Close();
            return result;
        }
    }
}