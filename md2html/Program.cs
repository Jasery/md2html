using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeyRed.MarkdownSharp;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;

namespace md2html
{
    class Program
    {
        static void Main(string[] args)
        {
            string originFilePath = string.Empty,
                targetFilePath = string.Empty,
                styleFilePath = string.Empty;
            args = new string[] { "base.md", "target.html" };

            GetArgs(args, out originFilePath, out targetFilePath, out styleFilePath);

            if (string.IsNullOrEmpty(originFilePath))
            {
                System.Environment.Exit(0);
            }
            
            Markdown markDown = new Markdown();
            HtmlDocument htmlDoc = new HtmlDocument();

            htmlDoc.Load(AppConfig.TemplateHTML);

            var mdTexts = ReadFileText(originFilePath);
            var toc = mdTexts.Where(md => md.StartsWith("#")).ToList();

            var mdText = string.Join("\n", mdTexts);
            var tocText = string.Join("\n", toc);

            var mdHtml = markDown.Transform(mdText);
            var tocHtml = markDown.Transform(tocText);

            var contentDoc = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='content']");
            var tocDoc = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='toc']");
            var headDoc = htmlDoc.DocumentNode.SelectSingleNode("//head");

            contentDoc.InnerHtml = mdHtml;
            //tocDoc.InnerHtml = tocHtml;
            //var targetFilePath = CommonHelper.GetAbsolutePath(targetHTMLFile);

            var styleText = string.Join("\n", ReadFileText(styleFilePath));
            var style = string.Format("<style>{0}</style>", styleText);
            var styleNode = HtmlNode.CreateNode(style);
            headDoc.AppendChild(styleNode);

            using (var fs = new FileStream(targetFilePath, FileMode.OpenOrCreate))
            {
                var buffer = System.Text.Encoding.UTF8.GetBytes(htmlDoc.DocumentNode.InnerHtml);
                fs.Write(buffer, 0, buffer.Length);

            }
            
            Console.ReadKey();
        }

        static string[] ReadFileText(string path)
        {
            return File.ReadAllLines(path).Where(s => !string.IsNullOrEmpty(s)).ToArray();
        }

        /// <summary>
        /// 处理参数
        /// </summary>
        /// <param name="args">原始参数数组</param>
        /// <param name="originFilePath">源文件路径</param>
        /// <param name="targetFilePath">目标文件路径</param>
        /// <param name="styleFilePath">样式文件路径</param>
        static void GetArgs(string[] args, out string originFilePath, out string targetFilePath, out string styleFilePath)
        {
            var environmentPath = Environment.CurrentDirectory;
            if (args.Length == 0)
            {
                originFilePath = targetFilePath = styleFilePath = string.Empty;
                return;
            }
            originFilePath = CommonHelper.GetAbsolutePath(args[0]);
            if (args.Length == 1)
            {
                var fileName = System.IO.Path.GetFileNameWithoutExtension(originFilePath);
                var originDir = System.IO.Path.GetDirectoryName(originFilePath);
                targetFilePath = System.IO.Path.Combine(originDir, fileName + ".html");
                styleFilePath = string.Empty;
            }
            else if (args.Length == 2)
            {
                targetFilePath = CommonHelper.GetAbsolutePath(args[1]);
                styleFilePath = string.Empty;
            }
            else
            {
                targetFilePath = CommonHelper.GetAbsolutePath(args[1]);
                styleFilePath = CommonHelper.GetAbsolutePath(args[2]);
            }
            if (string.IsNullOrEmpty(styleFilePath))
            {
                styleFilePath = CommonHelper.GetAbsolutePath(AppConfig.Style);
            }
        }
    }
}
