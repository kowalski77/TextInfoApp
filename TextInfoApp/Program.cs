using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TextInfoApp.Pipelines;

namespace TextInfoApp
{
    internal class Program
    {
        private static async Task Main()
        {
            var sw = new Stopwatch();
            sw.Start();

            #region Channels
            //Seek
            var textSeeker = new ChannelTextSeeker(new[] { "webapi1", "webapi2", "webapi3" });
            var textFilter = new ChannelTextFilter(3);
            var textOccurrences = new ChannelTextOccurrences('a');

            var (channelReader, errors) = textSeeker.Invoke(true);
            var filteredWords = textFilter.Invoke(channelReader);
            var counter = textOccurrences.Invoke(filteredWords);

            //Results
            var totalOccurrences = 0;
            await foreach (var (fileWordInfo, occur) in counter.ReadAllAsync())
            {
                Console.WriteLine($"In document from source: {fileWordInfo.Source} there are {occur} occurrences of the char 'a' in words with length greater than 3");
                Console.WriteLine();
                totalOccurrences += occur;
            }

            Console.WriteLine($"Total occurrences: {totalOccurrences}");
            Console.WriteLine();

            //Errors
            await foreach (var errMessage in errors.ReadAllAsync())
            {
                Console.WriteLine(errMessage);
            }
            #endregion

            #region Regular
            //var textSeeker = new TextSeeker(new[] { "webapi1", "webapi2", "webapi3" });
            //var textFilter = new TextFilter(3);
            //var textOccurrences = new TextOccurrences('a');

            //var source = await textSeeker.Invoke();
            //var filtered = textFilter.Invoke(source);
            //var occurrences = textOccurrences.Invoke(filtered);

            //var totalOccurrences = 0;
            //foreach (var (fileWordInfo, occurs) in occurrences)
            //{
            //    Console.WriteLine($"In document from source: {fileWordInfo} there are {occurs} occurrences of the char 'a' in words with length greater than 3");
            //    Console.WriteLine();
            //    totalOccurrences += occurs;
            //}

            //Console.WriteLine($"Total occurrences: {totalOccurrences}");
            //Console.WriteLine();
            #endregion

            sw.Stop();
            Console.WriteLine($"Elapsed milliseconds: {sw.Elapsed.TotalMilliseconds}");
        }
    }
}