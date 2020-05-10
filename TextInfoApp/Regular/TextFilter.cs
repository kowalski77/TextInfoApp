using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TextInfoApp.Support;

namespace TextInfoApp.Regular
{
    public class TextFilter
    {
        private readonly int length;

        public TextFilter(int length)
        {
            this.length = length;
        }

        public IReadOnlyList<FileWordInfo> Invoke(IReadOnlyList<SourceText> inputs)
        {
            return (from sourceText in inputs
                let words = GetWords(sourceText.Text, this.length) 
                select new FileWordInfo(sourceText.Source, words))
                .ToList();
        }

        private static IReadOnlyList<Word> GetWords(string input, int length)
        {
            Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Task.Delay(3000).Wait();

            var matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches
                where !string.IsNullOrEmpty(m.Value) && m.Value.Length >= length
                select new Word(m.Value);

            return words.ToList();
        }
    }
}