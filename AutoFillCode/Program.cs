namespace AutoFillCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2) RunWithArgs(args);
            else RunWithoutArgs();
        }

        private static void RunWithArgs(string[] args)
        {
            try
            {
                if (!File.Exists(args[0]))
                    throw new FileNotFoundException("指定的文件不存在！");
                if (!byte.TryParse(args[1], out byte limit) && limit != 0)
                    throw new ArgumentException("无法识别的最短编码长度！");
                Filler filler = new(args[0], limit);
                filler.Fill();
            }
            catch (Exception ex) when (ex is ArgumentException or FileNotFoundException)
            {
                Console.WriteLine($"{ex.Message}程序已中止！");
            }
        }

        private static void RunWithoutArgs()
        {
            for (; ; )
            {
                Console.WriteLine("请指定一个要自动装填简码的全码文件，\n每行格式如下（RIME格式，UTF-8编码）");
                Console.WriteLine("字词\t全码\t优先级（词频）");
                string path = GetPath();

                Console.WriteLine("请指定最短的编码长度，留空则默认为1：");
                byte limit = GetLimit();

                Filler filler = new(path, limit);
                filler.Fill();

                Console.WriteLine("是否再次执行？输入c继续，其他任何键退出。");
                if (Console.ReadKey().KeyChar != 'c') break;
            }
        }

        private static string GetPath()
        {
            string path = string.Empty;
            do
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input) || !File.Exists(input))
                    Console.WriteLine("文件不存在，请重新指定：");
                else path = input;
            } while (path.Length == 0);
            return path;
        }

        private static byte GetLimit()
        {
            byte limit = 1;
            bool valid = false;
            do
            {
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)
                    || (byte.TryParse(input, out limit) && limit != 0))
                    valid = true;
                else Console.WriteLine("无法识别的编码长度，请重新输入：");
            } while (!valid);
            return limit;
        }
    }
}
