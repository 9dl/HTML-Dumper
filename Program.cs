using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HTML_Dumper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Start
            Console.Clear();
            Console.Title = "HTML Dumper";
            Console.WriteLine();
            Console.Write(" URL -> ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            string str = Console.ReadLine();

            Console.Clear();
            Console.WriteLine();

            Index(str);
            CSS(str);
            JS(str);

            Console.WriteLine();
            Print(" Done!");
            Console.ReadKey();
        }

        public static void Index(string URL)
        {
            if (!Directory.Exists("Dumped")) { Directory.CreateDirectory("Dumped"); }

            var httpRequest = new Leaf.xNet.HttpRequest();
            httpRequest.AllowAutoRedirect = false;
            httpRequest.UserAgentRandomize();
            var src = httpRequest.Get(URL).ToString();

            File.WriteAllText("Dumped/index.html", src);

            Print(" Index: " + URL);
        }

        public static void CSS(string URL)
        {
            List<string> CSS_Files = new List<string>();

            var httpRequest = new Leaf.xNet.HttpRequest();
            httpRequest.AllowAutoRedirect = false;
            httpRequest.UserAgentRandomize();
            var src = httpRequest.Get(URL).ToString();
            string[] lines = src.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var item in lines)
            {
                if (item.ToString().Contains("<link rel=\"stylesheet\" href=\""))
                {
                    var CSS_URL = Parse(item.ToString(), "<link rel=\"stylesheet\" href=\"", "\"/>");
                    CSS_Files.Add(CSS_URL);
                    Print(" CSS: " + CSS_URL);
                }
            }

            foreach (var item in CSS_Files)
            {
                if (item.Contains("http"))
                {
                    var CSS_Source = httpRequest.Get(item);
                    var item2 = item.Replace("http://", "").Replace("https://", "").Replace("//", "/").Split('/')[0];
                    File.WriteAllText(item2, CSS_Source.ToString());
                }
                else
                {
                    var CSS_Url_ = URL + item;
                    CSS_Url_ = CSS_Url_.Replace("http://", "").Replace("https://", "").Replace("//", "/").Split('/')[0];
                    var CSS_Source = httpRequest.Get(CSS_Url_ + "/" + item);
                    File.WriteAllText("Dumped/" + item.Replace("_", "").Replace("/", ""), CSS_Source.ToString());
                }
            }
        }

        public static void JS(string URL)
        {
            List<string> JS_Files = new List<string>();

            var httpRequest = new Leaf.xNet.HttpRequest();
            httpRequest.AllowAutoRedirect = false;
            httpRequest.UserAgentRandomize();
            var src = httpRequest.Get(URL).ToString();
            string[] lines = src.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var item in lines)
            {
                if (item.ToString().Contains("<script") && item.EndsWith("</script>"))
                {
                    var JS_URL = Parse(item.ToString(), "src=\"", "\"></script>");
                    JS_Files.Add(JS_URL);
                    Print(" JS: " + JS_URL);
                }
            }

            foreach (var item in JS_Files)
            {
                if (item.Contains("http"))
                {
                    var CSS_Source = httpRequest.Get(item).ToString();
                    var item2 = item.Replace("http://", "").Replace("https://", "").Replace("//", "/").Split('/')[0];
                    File.WriteAllText(item2, CSS_Source.ToString());
                }
                else
                {
                    var CSS_Url_ = URL + item;
                    CSS_Url_ = CSS_Url_.Replace("http://", "").Replace("https://", "").Replace("//", "/").Split('/')[0];
                    var CSS_Source = httpRequest.Get(CSS_Url_ + "/" + item);
                    File.WriteAllText("Dumped/" + item.Replace("_", "").Replace("/", ""), CSS_Source.ToString());
                }
            }
        }

        private static string Parse(string input, string beginning, string end)
        {
            int num = input.IndexOf(beginning) + beginning.Length;
            int num2 = input.IndexOf(end, num);
            return (end == "") ? input.Substring(num) : input.Substring(num, num2 - num);
        }

        private static void Print(string input)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" [");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("DUMPER");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("]");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(input);
        }
    }
}