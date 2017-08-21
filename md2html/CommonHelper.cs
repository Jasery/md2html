using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace md2html
{
    public class CommonHelper
    {
        public static bool IsAbsolutePath(string path)
        {
            return Regex.IsMatch(path, @"^[CDEFGHIJKLMNOPQ]:.*");
        }

        public static string GetAbsolutePath(string path)
        {
            path = path.Replace("\\", "/");
            if (IsAbsolutePath(path))
            {
                return path;
            }
            var environmentDir = Environment.CurrentDirectory;
            var tempDir = environmentDir;
            while (!IsAbsolutePath(path))
            {
                if (path.StartsWith(".."))
                {
                    tempDir = Directory.GetParent(tempDir).FullName;
                    path = Regex.Replace(path, @"^\.\./", "");
                }
                else if (path.StartsWith("."))
                {
                    path = Regex.Replace(path, @"^\./", "");
                }
                else
                {
                    path = System.IO.Path.Combine(tempDir, path);
                }
            }
            return path;
        }
    }    
}
