using DictionaryLoader;

namespace Benchmarks;

public class DataLoader
{
    private const string TextFilePath = "Data/sowpods.txt",
        ZipFilePath = "Data/sowpods.zip",
        CwdFilePath = "Data/sowpods.cwd";

    public string LoadText()
    {
        return File.ReadAllText(TextFilePath);
    }

    public string[] LoadLines()
    {
        return File.ReadAllLines(TextFilePath);
    }

    public string LoadTextFromZip()
    {
        var compressor = new ZipDecompressor();
        return compressor.DecompressText(ZipFilePath);
    }

    public string[] LoadLinesFromZip()
    {
        var compressor = new ZipDecompressor();
        return compressor.Decompress(ZipFilePath);
    } 
    
    public string LoadTextFromCwd()
    {
        var compressor = new WordDictionaryCompressor();
        var data = File.ReadAllText(CwdFilePath);
        return compressor.Decompress(data);
    }
        
    public string[] LoadLinesFromCwd()
    {
        var compressor = new WordDictionaryCompressor();
        var data = File.ReadAllLines(CwdFilePath);
        return compressor.Decompress(data);
    }
    
    public string LoadTextFromOptimizedCwd()
    {
        var compressor = new WordDictionaryCompressorOptimized();
        var data = File.ReadAllText(CwdFilePath);
        return compressor.Decompress(data);
    }
        
    public string[] LoadLinesFromOptimizedCwd()
    {
        var compressor = new WordDictionaryCompressorOptimized();
        var data = File.ReadAllLines(CwdFilePath);
        return compressor.Decompress(data);
    }
    
    //TODO: add LoadTextFromCwd and LoadTextFromOptimizedCwd methods for testing
}