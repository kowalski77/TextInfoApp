using System.Collections.Generic;
using System.Linq;
using TextInfoApp.Support;

namespace TextInfoApp.Regular
{
    public class TextOccurrences
    {
        private readonly char target;

        public TextOccurrences(char target)
        {
            this.target = target;
        }

        public IReadOnlyList<(FileWordInfo, int)> Invoke(IReadOnlyList<FileWordInfo> input)
        {
            return (from fileWordInfo in input let count = 
                CountChar(fileWordInfo.Words, this.target) 
                select (fileWordInfo, count))
                .ToList();
        }

        private static int CountChar(IEnumerable<Word> words, char target)
        {
            return words.Count(x => x.Value.Contains(target));
        }
    }
}