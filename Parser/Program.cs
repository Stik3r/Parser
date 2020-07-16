using System;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество картинок");
            int count = int.Parse(Console.ReadLine());
            WebClient client = new WebClient();
            client.Headers["User-Agent"] = "Mozilla/5.0";
            client.Encoding = System.Text.Encoding.UTF8;
            string name;
            for (int i = 0; i < count; i++)
            {
                name = "";
                try
                {
                    string pageurl = RandomURL(ref name);
                    string picurl = GetUrl(new HtmlWeb().Load(pageurl));
                    SavePicture(client, picurl, name);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Download picture " + name + " success");
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Download picture " + name + " faled");
                    continue;
                }
            }
        }

        public static string GetUrl(HtmlDocument doc)
        {
            return doc.DocumentNode.LastChild.LastChild.SelectSingleNode("//img[@class='no-click screenshot-image']").GetAttributeValue("src", "");
        }
        public static string RandomURL(ref string name)
        {
            string[] symbol = "a b c d e f g h i j k l m n o p q  r s t u v w x y z 0 1 2 3 4 5 6 7 8 9".Split();
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
            string dirPath = @"d:\picture";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            client.DownloadFile(path, dirPath + "\\" + name + ".png");
        }
    }
}
