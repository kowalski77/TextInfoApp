using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TextInfoApp.Support;

namespace TextInfoApp.Regular
{
    public class TextSeeker
    {
        private readonly string[] webApis;

        public TextSeeker(string[] webApis)
        {
            this.webApis = webApis;
        }

        public async Task<IReadOnlyList<SourceText>> Invoke()
        {
            var list = new List<SourceText>();
            foreach (var webApi in this.webApis)
            {
                var source = await TextReader.ReadWebApiAsync(webApi, CancellationToken.None);
                list.Add(new SourceText(webApi, source));
            }

            return list.ToList();
        }
    }
}