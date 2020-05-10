namespace TextInfoApp.Support
{
    public class SourceText
    {
        public SourceText(
            string source, 
            string text)
        {
            this.Source = source;
            this.Text = text;
        }

        public string Source { get; }
        public string Text { get; }
    }
}