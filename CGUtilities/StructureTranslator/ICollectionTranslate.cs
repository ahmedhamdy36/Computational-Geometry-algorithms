using System.Collections.Generic;
using System.Text;

namespace CGUtilities.StructureTranslator
{
    public interface ICollectionTranslate
    {
        string Encode<T>(List<T> elements);
        List<object> Decode(string content);
    }
}
