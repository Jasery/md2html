using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Cache;
using System.Configuration;

namespace md2html
{
    public class AppConfig
    {
        public static string TemplateHTML
        {
            get
            {
                return ConfigurationManager.AppSettings["TemplateHTML"];
            }
        }

        public static string Style
        {
            get
            {
                return ConfigurationManager.AppSettings["Style"];
            }
        }
    }
}
