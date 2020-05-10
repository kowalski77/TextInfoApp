using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TextInfoApp.Support;

namespace TextInfoApp.Pipelines
{
    public class ChannelTextSeeker : IRequest<bool, (ChannelReader<SourceText>, ChannelReader<string>)>
    {
        private readonly string[] webApis;

        public ChannelTextSeeker(string[] webApis)
        {
            this.webApis = webApis;
        }

        public (ChannelReader<SourceText>, ChannelReader<string>) Invoke(bool input)
        {
            var output = Channel.CreateUnbounded<SourceText>();
            var errors = Channel.CreateUnbounded<string>();

            async Task RetrieveTextAsync(params string[] sources)
            {
                foreach (var source in sources)
                {
                    try
                    {
                        var text = await TextReader.ReadWebApiAsync(source, CancellationToken.None);
                        await output.Writer.WriteAsync(new SourceText(source, text), CancellationToken.None);
                    }
                    catch (Exception e)
                    {
                        if (e is InvalidOperationException || e is OperationCanceledException)
                        {
                            await errors.Writer.WriteAsync(e.Message);
                        }
                    }
                }
            }

            Task.Run(async () =>
            {
                try
                {
                    await RetrieveTextAsync(this.webApis);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Cancelled.");
                }
                finally
                {
                    output.Writer.Complete();
                    errors.Writer.Complete();
                }
            });

            return (output, errors);
        }
    }
}