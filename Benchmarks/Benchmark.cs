using BenchmarkDotNet.Attributes;
using FluentAssertions;

namespace Benchmarks;

[MemoryDiagnoser]
public class DictionaryLoaderBenchmarks
{
    private readonly DataLoader _sut = new();
    private readonly string[] _expectedOutput;
        
    public DictionaryLoaderBenchmarks()
    {
        _expectedOutput = _sut.LoadLines();
    }
    
    [Benchmark]
    public void LoadText()
    {
        _sut.LoadText();
    }
       
    [Benchmark(Baseline = true)]
    public void LoadLines()
    {
        _sut.LoadLines();
    }

    [Benchmark]
    public void LoadZipText()
    {
        _sut.LoadTextFromZip();
    }
    
    [Benchmark]
    public void LoadZipLines()
    {
        _sut.LoadLinesFromZip();
    }

    [Benchmark]
    public void LoadCwdText()
    {
        _sut.LoadTextFromCwd();
    }

    [Benchmark]
    public void LoadCwdLines()
    {
        _sut.LoadLinesFromCwd();
    }

    [Benchmark]
    public void LoadOptimizedCwdText()
    {
        _sut.LoadTextFromOptimizedCwd();
    }

    [Benchmark]
    public void LoadOptimizedCwdLines()
    {
        _sut.LoadLinesFromOptimizedCwd();
    }

    private void AssertEqualToInput(string[] output)
    {
        output.Should().BeEquivalentTo(_expectedOutput, "Expected output to be the same as the input.");
    }
}