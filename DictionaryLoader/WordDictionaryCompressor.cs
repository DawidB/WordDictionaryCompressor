using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DictionaryLoader
{
    public class WordDictionaryCompressor : IWordDictionaryCompressor
    {
        private const char NewLineSeparator = '\n';
        
        public string Compress(string input)
        {
            return string.Join(NewLineSeparator,
                Compress(input.Split(NewLineSeparator, StringSplitOptions.RemoveEmptyEntries)));
        }

        public virtual string[] Compress(string[] input)
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
                
                if (currentNestingLevel < nestingLevels.Count - 1)
                {
                    nestingLevels = nestingLevels
                        .TakeWhile(nl => nl.Key <= currentNestingLevel)
                        .ToDictionary(kv => kv.Key, kv=> kv.Value);
                }

                if (!entry.StartsWith(previousEntry)) continue;
                
                var suffix = entry.Remove(0, previousEntry.Length);
                result[i] = currentNestingLevel + suffix;

                var nextEntry = i < input.Length - 1
                    ? input[i + 1]
                    : null;
                if (nextEntry != null)
                    nestingLevels.Add(currentNestingLevel + 1, entry);
            }

            return result;
        }

        public virtual string[] Decompress(string[] input)
        {
            var nestingLevelIds = new Dictionary<int, string>();
            var result = new string[input.Length];
            
            var entry = input[0];
            result[0] = entry;
            nestingLevelIds.Add(0, entry);

            for (var i = 1; i < input.Length; i++)
            {
                entry = input[i];

                var match = Regex.Match(entry, "\\d+");
                if (!match.Success)
                {
                    result[i] = entry;
                    nestingLevelIds.Clear();
                    nestingLevelIds.Add(0, entry);
                    continue;
                }

                var previousNestingLevelId = int.Parse(match.Value);
                var prefix = nestingLevelIds[previousNestingLevelId];
                result[i] = prefix + entry.Remove(0, match.Value.Length);

                var nextNestingLevel = previousNestingLevelId + 1;
                if (nestingLevelIds.ContainsKey(nextNestingLevel))
                {
                    nestingLevelIds = nestingLevelIds.Where(nl => nl.Key < nextNestingLevel)
                        .ToDictionary(nl => nl.Key, nl => nl.Value);
                }
                
                nestingLevelIds.Add(nextNestingLevel, result[i]);
            }

            return result;
        }

        public virtual string Decompress(string input)
        {
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

                var match = Regex.Match(entry, "\\d+");
                if (!match.Success)
                {
                    result.Append('\n').Append(entry);
                    nestingLevelIds.Clear();
                    nestingLevelIds.Add(0, entry);
                    continue;
                }

                var previousNestingLevel = int.Parse(match.Value);
                var prefix = nestingLevelIds[previousNestingLevel];
                newEntry = prefix + entry.Remove(0, match.Value.Length);
                result.Append('\n').Append(newEntry);

                var nextNestingLevelId = previousNestingLevel + 1;
                if (nestingLevelIds.ContainsKey(nextNestingLevelId))
                {
                    nestingLevelIds = nestingLevelIds.Where(nl => nl.Key < nextNestingLevelId)
                        .ToDictionary(nl => nl.Key, nl => nl.Value);
                }
                
                nestingLevelIds.Add(nextNestingLevelId, newEntry);
            }

            return result.ToString();
        }
        
        protected string GetNextString(in string text, ref int index)
        {
            var nextIndex = text.IndexOf('\n', index);
            if (nextIndex == -1)
            {
                var lastResult = text[index..];
                index = text.Length;
                return lastResult;
            }

            var currentResult = text.Substring(index, nextIndex - index);
            index = nextIndex + 1;
            return currentResult;
        }
    }
}