using System;
using System.IO;
using DictionaryLoader;
using FluentAssertions;
using Xunit;

namespace UnitTests;

public class ZipDecompressorTests
{
    private static string[] _input = Array.Empty<string>();
    private static string _inputText = string.Empty;

    private readonly ZipDecompressor _sut = new();

    public ZipDecompressorTests()
    {
        if (_inputText.Length == 0)
            _inputText = File.ReadAllText("Data/sowpods.txt");
        
        _input = _inputText.Split("\n");
    }

    [Fact]
    public void Decompress_GivenCompressedData_OutputsCorrectText()
    {
        //Arrange & Act
        var output = _sut.DecompressText("Data/sowpods.zip");

        //Assert
        output.Should().Be(_inputText);
    }

    [Fact]
    public void Decompress_GivenCompressedData_OutputsCorrectLines()
    {
        //Arrange & Act
        var output = _sut.Decompress("Data/sowpods.zip");

        //Assert
        output.Length.Should().Be(_input.Length);
        output.Should().BeEquivalentTo(_input);
    }
}