using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CGUtilities.StructureTranslator
{
    public class PointCollectionTranslator : ICollectionTranslate
    {
        public string Encode<T>(List<T> elements)
        {
            StringBuilder _builder = new StringBuilder();
            _builder.AppendLine("BeginSection:Points");
            foreach (var singlePoint in elements)
            {
                _builder.AppendLine((singlePoint as Point).X + "," + (singlePoint as Point).Y);
            }
            _builder.AppendLine("EndSection:Points");
            return _builder.ToString();
        }

        public List<object> Decode(string content)
        {
            List<object> result = new List<object>();
            string[] tokens = content.Split(new[] { "BeginSection:Points", "EndSection:Points" }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 1) return null;
            StringReader stringReader = new StringReader(tokens[0].Trim());
            while (stringReader.Peek() != -1)
            {
                string[] pointTokens = stringReader.ReadLine().Split(',');
                result.Add(new Point(double.Parse(pointTokens[0]), double.Parse(pointTokens[1])));
            }
            stringReader.Close();
            return result;
        }
    }
}
