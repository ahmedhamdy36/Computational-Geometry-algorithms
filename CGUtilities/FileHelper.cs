using System;
using System.IO;

namespace CGUtilities
{
    public class FileHelper
    {
        private static FileStream _localstream;
        private static StreamReader _localReader;
        private static StreamWriter _localWriter;

        public static string Load(string filePath)
        {
            string result = String.Empty;
            if (Path.GetExtension(filePath) != ".cgds") return result;
            _localstream = new FileStream(filePath, FileMode.Open);
            _localReader = new StreamReader(_localstream);
            result = _localReader.ReadToEnd();
            _localReader.Dispose();
            _localstream.Dispose();
            return result;
        }

        public static void Save(string filePath, string content)
        {
            if (filePath == "*.cgds") return;
            _localstream = new FileStream(filePath, FileMode.Create);
            _localWriter = new StreamWriter(_localstream);
            if (filePath == "*.cgds") return;
            _localWriter.Write(content);
            _localWriter.Flush();
            _localWriter.Dispose();
            _localstream.Dispose();
        }
    }
}
