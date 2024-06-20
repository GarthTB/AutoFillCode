namespace AutoFillCode
{
    internal class Entry
    {
        public Entry(string word, string code, string priority)
        {
            Word = word;
            Code = code;
            if (!int.TryParse(priority, out Priority))
                throw new ArgumentException("遇到了无法识别的优先级！");
        }

        public Entry(string word, string code, int priority)
        {
            Word = word;
            Code = code;
            Priority = priority;
        }

        public readonly string Word = string.Empty;
        public readonly string Code = string.Empty;
        public readonly int Priority;
    }
}
