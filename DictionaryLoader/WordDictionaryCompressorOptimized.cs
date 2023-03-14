using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DictionaryLoader
{
    public class WordDictionaryCompressorOptimized : WordDictionaryCompressor
    {
        public override string[] Compress(string[] input)
        {
            var nestingLevels = new Dictionary<int, string>();
            var result = new string[input.Length];
            
            var entry = input[0];
            result[0] = entry;
            nestingLevels.Add(0, entry);
            
            for (var i = 1; i < input.Length; i++)
            {
                entry = input[i];

                var (currentNestingLevel, previousEntry) = nestingLevels.LastOrDefault(nl => entry.StartsWith(nl.Value));
                if (previousEntry == null)
                {
                    result[i] = entry;
                    nestingLevels.Clear();
                    nestingLevels.Add(0, entry);
                    continue;
                }
                
                for (var j = currentNestingLevel + 1; j < nestingLevels.Count; j++)
                    nestingLevels.Remove(j);

                if (!entry.StartsWith(previousEntry)) continue;
                
                result[i] = currentNestingLevel + entry.Remove(0, previousEntry.Length);;

                if(i < input.Length - 1)
                    nestingLevels.Add(currentNestingLevel + 1, entry);
            }

            return result;
        }

        public override string[] Decompress(string[] input)
        {
            int previousNestingLevelId, nextNestingLevelId;
            var regex = new Regex("\\d+", RegexOptions.Compiled); 
            var nestingLevelIds = new Dictionary<int, string>();
            var result = new string[input.Length];
            
            result[0] = input[0];
            nestingLevelIds.Add(0, input[0]);

            for (var i = 1; i < input.Length; i++)
            {
                var entry = input[i];
                var isNewPrefix = !regex.IsMatch(entry);
                if (isNewPrefix)
                {
                    result[i] = entry;
                    nestingLevelIds.Clear();
                    nestingLevelIds.Add(0, entry);
                    continue;
                }
                
                previousNestingLevelId = entry[0] - 48;
                
                //concatenate prefix and suffix
                result[i] = nestingLevelIds[previousNestingLevelId] + entry.Remove(0, 1);
                
                nextNestingLevelId = previousNestingLevelId + 1;
                
                //clear obsolete nesting levels
                for (var j = nextNestingLevelId; j < nestingLevelIds.Count; j++)
                    nestingLevelIds.Remove(j);
                
                nestingLevelIds.Add(nextNestingLevelId, result[i]);
            }

            return result;
        }

        public override string Decompress(string input)
        {
            int previousNestingLevelId, nextNestingLevelId;
            var regex = new Regex("\\d+", RegexOptions.Compiled); 
            var nestingLevelIds = new Dictionary<int, string>();
            var result = new StringBuilder(input.Length);
            string newEntry;
            var i = 0;
            var entry = GetNextString(input, ref i);
            
            result.Append(entry);
            nestingLevelIds.Add(0, entry);

            for (; i < input.Length;)
            {
                entry = GetNextString(input, ref i);

                var isNewPrefix = !regex.IsMatch(entry);
                if (isNewPrefix)
                {
                    result.Append('\n').Append(entry);
                    nestingLevelIds.Clear();
                    nestingLevelIds.Add(0, entry);
                    continue;
                }

                previousNestingLevelId = entry[0] - 48;

                //concatenate prefix and suffix
                newEntry = nestingLevelIds[previousNestingLevelId] + entry.Remove(0, 1);
                result.Append('\n').Append(newEntry);

                nextNestingLevelId = previousNestingLevelId + 1;
                for (var j = nextNestingLevelId; j < nestingLevelIds.Count; j++)
                    nestingLevelIds.Remove(j);
                
                nestingLevelIds.Add(nextNestingLevelId, newEntry);
            }

            return result.ToString();
        }
    }
}