using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;

namespace Parser
{
    class Program
    {
        static string YorN;
        static void Main(string[] args)
        {
             Console.ForegroundColor = ConsoleColor.Yellow;
             Console.WriteLine("Input count of picture");
             int count = int.Parse(Console.ReadLine());
             DownloadAsync(count);
             Console.ReadLine();
        }

        public static async void DownloadAsync(int count)
        {
            int succsesDowload = 0;
            while(succsesDowload < count)
            {
                Task<int> t1 = Task.Run(() => Download());
                Thread.Sleep(50);
                Task<int> t2 = Task.Run(() => Download());
                Thread.Sleep(50);
                Task<int> t3 = Task.Run(() => Download());
                await Task.WhenAll(new[] { t1, t2, t3 });
                succsesDowload += t1.Result + t2.Result + t3.Result;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(succsesDowload.ToString() + " files was download");
        }

        public static int Download()
        {
            WebClient client = new WebClient();
            client.Headers["User-Agent"] = "Mozilla/5.0";
            client.Encoding = System.Text.Encoding.UTF8;
            string name = "";
            try
            {
                string pageurl = RandomURL(ref name);
                string picurl = GetUrl(new HtmlWeb().Load(pageurl));
                SavePicture(client, picurl, name);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Download picture " + name + " success");
                return 1;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Download picture " + name + " faled");
                return 0;
            }
        }

        public static string GetUrl(HtmlDocument doc)
        {
            return doc.DocumentNode.LastChild.SelectSingleNode(@"//img[@id='screenshot-image']").GetAttributeValue("src", "");
        }
        public static string RandomURL(ref string name)
        {
            string[] symbol = "a b c d e f g h i j k l m n o p q r s t u v w x y z 0 1 2 3 4 5 6 7 8 9".Split();
            string url = @"https://prnt.sc/";
            Random rand = new Random();
            int randIndex;
            for (int i = 0; i < 6; i++)
            {
                randIndex = rand.Next(0, symbol.Length);
                url += symbol[randIndex];
                name += symbol[randIndex];
            }
            return url;
        }
        public static void SavePicture(WebClient client, string path, string name)
        {
            string dirPath = Environment.CurrentDirectory + "\\" + DateTime.Today.ToShortDateString();
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            client.DownloadFile(path, dirPath + "\\" + name + ".png");
        }
    }
}
