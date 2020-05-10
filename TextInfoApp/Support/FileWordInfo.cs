using System.Collections.Generic;

namespace TextInfoApp.Support
{
    public class FileWordInfo
    {
        public FileWordInfo(
            string source,
            IReadOnlyList<Word> words)
        {
            this.Source = source;
            this.Words = words;
        }

        public string Source { get; }
        public IReadOnlyList<Word> Words { get; }
    }
}