using System.IO;
using DictionaryLoader;
using FluentAssertions;
using Xunit;

namespace UnitTests;

public abstract class DictionaryCompressorTestsBase
{
    private static string _input = string.Empty;
    
    protected IWordDictionaryCompressor Sut = new WordDictionaryCompressor();

    protected DictionaryCompressorTestsBase()
    {
        if (string.IsNullOrEmpty(_input))
            _input = File.ReadAllText("Data/sowpods.txt");
    }
        
    [Fact]
    public void Compress_GivenSimpleText_OutputsCompressedData()
    {
        //Arrange
        var input = @"AA
AAH
AAHED
AAHING
AAHS
AAL
AALII
AALIIS
AALS
AARDVARK
AARDVARKS
AARDWOLF
AARDWOLVES".ReplaceLineEndings("\n");
        var expectedOutput = @"AA
0H
1ED
1ING
1S
0L
1II
2S
1S
0RDVARK
1S
0RDWOLF
0RDWOLVES".ReplaceLineEndings("\n");

        //Act
        var output = Sut.Compress(input);

        //Assert
        output.Should().Be(expectedOutput);
    }

    [Fact]
    public void Compress_GivenFullData_CompressionRatioIdCorrect()
    {
        //Arrange
        var zipContents = File.ReadAllText("Data/sowpods.zip");

        //Act
        var compressedInput = Sut.Compress(_input);

        //Assert
        compressedInput.Length.Should().BeLessThan(_input.Length);
        compressedInput.Length.Should().BeGreaterOrEqualTo(zipContents.Length);
    }
        
    [Fact]
    public void Decompress_GivenCompressedData_OutputsCorrectText()
    {
        //Arrange
        var compressedInput = Sut.Compress(_input);

        //Act
        var output = Sut.Decompress(compressedInput);

        //Assert
        output.Length.Should().Be(_input.Length);
        output.Should().Be(_input);
    }
}