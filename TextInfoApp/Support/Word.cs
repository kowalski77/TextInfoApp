namespace TextInfoApp.Support
{
    public class Word
    {
        public string Value { get; }

        public Word(string value)
        {
            this.Value = value;
        }

        public int Length => this.Value.Length;
    }
}