using DictionaryLoader;

namespace UnitTests
{
    public class DictionaryCompressorOptimizedTests : DictionaryCompressorTestsBase
    {
         public DictionaryCompressorOptimizedTests() : base()
         {
             Sut = new WordDictionaryCompressorOptimized();
         }
    }
}