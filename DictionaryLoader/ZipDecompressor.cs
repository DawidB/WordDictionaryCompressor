using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace DictionaryLoader
{
    public class ZipDecompressor
    {
        public string DecompressText(string path)
        {
            using var archive = ZipFile.OpenRead(path);
            foreach (var entry in archive.Entries)
            {
                if (!entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) continue;
                
                using var stream = entry.Open();
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            return string.Empty;
        }
        
        public string[] Decompress(string path)
        {
            var output = new string[267_751];
            var index = 0;
            string? line;
            using var archive = ZipFile.OpenRead(path);
            foreach (var entry in archive.Entries)
            {
                if (!entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)) continue;
                
                using var stream = entry.Open();
                using var reader = new StreamReader(stream);
                while ((line = reader.ReadLine()) != null)
                {
                    output[index++] = line;
                }
            }

            return output;
        }
    }
}