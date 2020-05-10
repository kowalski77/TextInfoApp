using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TextInfoApp.Pipelines
{
    public static class ChannelExtensions
    {
        public static IList<ChannelReader<T>> Split<T>(this ChannelReader<T> input, int n)
        {
            var outputs = new Channel<T>[n];
            for (var i = 0; i < n; i++)
            {
                outputs[i] = Channel.CreateUnbounded<T>();
            }
            
            Task.Run(async () =>
            {
                var index = 0;
                await foreach (var item in input.ReadAllAsync())
                {
                    await outputs[index].Writer.WriteAsync(item);
                    index = (index + 1) % n;
                }

                foreach (var ch in outputs)
                {
                    ch.Writer.Complete();
                }
            });

            return outputs.Select(ch => ch.Reader).ToArray();
        }
    }
}