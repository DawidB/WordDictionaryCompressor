namespace DictionaryLoader
{
    public interface IWordDictionaryCompressor
    {
        string Compress(string input);
        string[] Compress(string[] input);
        string Decompress(string compressedInput);
        string[] Decompress(string[] compressedInput);
    }
}