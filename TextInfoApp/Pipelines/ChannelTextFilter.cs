using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TextInfoApp.Support;

namespace TextInfoApp.Pipelines
{
    public class ChannelTextFilter : IRequest<ChannelReader<SourceText>, ChannelReader<FileWordInfo>>
    {
        private readonly int length;

        public ChannelTextFilter(int length)
        {
            this.length = length;
        }

        public ChannelReader<FileWordInfo> Invoke(ChannelReader<SourceText> input)
        {
            var inputs = input.Split(3);
            var output = Channel.CreateUnbounded<FileWordInfo>();

            Task.Run(async () =>
            {
                async Task Redirect(ChannelReader<SourceText> channel)
                {
                    await foreach (var item in channel.ReadAllAsync())
                    {
                        var words = GetWords(item.Text, this.length);

                        await output.Writer.WriteAsync(new FileWordInfo(item.Source, words));
                    }
                }

                await Task.WhenAll(inputs.Select(Redirect).ToArray());
                output.Writer.Complete();
            });

            return output;
        }

        private static IReadOnlyList<Word> GetWords(string input, int length)
        {
            Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            Task.Delay(5000).Wait();

            var matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches
                where !string.IsNullOrEmpty(m.Value) && m.Value.Length >= length
                select new Word(m.Value);

            return words.ToList();
        }
    }
}