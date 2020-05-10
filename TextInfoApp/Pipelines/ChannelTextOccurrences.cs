using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using TextInfoApp.Support;

namespace TextInfoApp.Pipelines
{
    public class ChannelTextOccurrences : IRequest<ChannelReader<FileWordInfo>, ChannelReader<(FileWordInfo, int ocurrencies)>>
    {
        private readonly char target;

        public ChannelTextOccurrences(char target)
        {
            this.target = target;
        }

        public ChannelReader<(FileWordInfo, int ocurrencies)> Invoke(ChannelReader<FileWordInfo> input)
        {
            var output = Channel.CreateUnbounded<(FileWordInfo, int)>();

            Task.Run(async () =>
            {
                await foreach (var words in input.ReadAllAsync())
                {
                    var count = CountChar(words.Words, this.target);
                    await output.Writer.WriteAsync((words, count));
                }

                output.Writer.Complete();
            });

            return output;
        }

        private static int CountChar(IEnumerable<Word> words, char target)
        {
            return words.Count(x => x.Value.Contains(target));
        }
    }
}