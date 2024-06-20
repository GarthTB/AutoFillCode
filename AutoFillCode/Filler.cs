namespace AutoFillCode
{
    internal class Filler
    {
        public Filler(string path, byte limit)
        {
            _path = path;
            _limit = limit;
        }

        private readonly string _path = string.Empty;
        private readonly byte _limit = 1;

        private readonly HashSet<Entry> origins = new();
        private readonly HashSet<Entry> simpler = new();
        private readonly HashSet<Entry> remains = new();

        public void Fill()
        {
            Console.Clear();
            Console.WriteLine($"文件路径：{_path}");
            Console.WriteLine($"最短码长：{_limit}");
            try
            {
                Console.WriteLine("开始读取文件...");
                ReadFile();
                Console.WriteLine($"读取完成，共有{origins.Count}个条目。");
                Console.WriteLine("开始装填...");
                FillCode();
                Console.WriteLine($"装填完成，共有{simpler.Count}个简码；\n{remains.Count}个条目没有简码。");
                Console.WriteLine("开始写入文件...");
                WriteFileAsync();
                Console.WriteLine("写入完毕，装填结束。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n程序已中止！");
            }
        }

        private void ReadFile()
        {
            using StreamReader reader = new(_path, System.Text.Encoding.UTF8);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split('\t');
                if (parts.Length != 3)
                    throw new Exception("文件中有无法识别的行！");
                origins.Add(new Entry(parts[0], parts[1], parts[2]));
            }
            if (origins.Count == 0)
                throw new Exception("文件为空！");
            if (origins.All(e => e.Code.Length < _limit))
                throw new Exception("最短码长大于所有码长！");
        }

        private void FillCode()
        {
            var sortedEntries = origins.OrderByDescending(e => e.Priority);
            HashSet<string> used = new();
            foreach (var se in sortedEntries)
                if (Remains(used, se))
                    remains.Add(se);
            if (used.Count == 0)
                throw new Exception("没有找到任何简码！");
        }

        private bool Remains(HashSet<string> used, Entry se)
        {
            var span = se.Code.AsSpan();
            for (int length = _limit; length < span.Length; length++)
            {
                string code = new(span[..length]);
                if (!used.Contains(code))
                {
                    simpler.Add(new Entry(se.Word, code, se.Priority));
                    used.Add(code);
                    return false;
                }
            }
            return true;
        }

        private void WriteFileAsync()
        {
            WriteSimplifiedOnly();
            if (remains.Count != 0)
                WriteRemains();
            WriteCombined();
        }

        private void WriteSimplifiedOnly()
        {
            string writepath = _path.Replace(Path.GetExtension(_path), "_SimplifiedOnly.txt");
            using StreamWriter sw = new(writepath, false, System.Text.Encoding.UTF8);
            var sortedSimpler = simpler.OrderBy(e => e.Code);
            foreach (var se in sortedSimpler)
                sw.WriteLine($"{se.Word}\t{se.Code}\t{se.Priority}");
            Console.WriteLine($"仅含简码的文件已写入：{writepath}");
        }

        private void WriteRemains()
        {
            string writepath = _path.Replace(Path.GetExtension(_path), "_RemainsOnly.txt");
            using StreamWriter sw = new(writepath, false, System.Text.Encoding.UTF8);
            var sortedRemains = remains.OrderBy(e => e.Code);
            foreach (var sr in sortedRemains)
                sw.WriteLine($"{sr.Word}\t{sr.Code}\t{sr.Priority}");
            Console.WriteLine($"没有简码的条目已写入：{writepath}");
        }

        private void WriteCombined()
        {
            string writepath = _path.Replace(Path.GetExtension(_path), "_Full.txt");
            using StreamWriter sw = new(writepath, false, System.Text.Encoding.UTF8);
            HashSet<Entry> full = new(origins);
            full.UnionWith(simpler);
            var sortedFull = full.OrderBy(e => e.Code);
            foreach (var sf in sortedFull)
                sw.WriteLine($"{sf.Word}\t{sf.Code}\t{sf.Priority}");
            Console.WriteLine($"包含全码和简码的文件已写入：{writepath}");
        }
    }
}
