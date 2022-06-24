using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities.StructureTranslator
{
    public class LineCollectionTranslator : ICollectionTranslate
    {
        public string Encode<T>(List<T> elements)
        {
            StringBuilder _builder = new StringBuilder();
            _builder.AppendLine("BeginSection:Lines");
            foreach (var singleLine in elements)
            {
                Line candidate = singleLine as Line;
                _builder.AppendLine(candidate.Start.X + "," + candidate.Start.Y + "," + candidate.End.X + "," + candidate.End.Y);
            }
            _builder.AppendLine("EndSection:Lines");
            return _builder.ToString();
        }

        public List<object> Decode(string content)
        {
            List<object> result = new List<object>();
            string[] tokens = content.Split(new[] { "BeginSection:Lines", "EndSection:Lines" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 1) return null;
            StringReader stringReader = new StringReader(tokens[1].Trim());
            while (stringReader.Peek() != -1)
            {
                string[] lineTokens = stringReader.ReadLine().Split(',');
                result.Add(new Line(new Point(double.Parse(lineTokens[0]), double.Parse(lineTokens[1])), new Point(double.Parse(lineTokens[2]), double.Parse(lineTokens[3]))));
            }
            stringReader.Close();
            return result;
        }
    }
}
