using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models
{
    public class UtilClass
    {
        public static string ParseHtml(string html)
        {
            var newJsonDesc = html.Replace("<", "&lt;");
            newJsonDesc = newJsonDesc.Replace(">", "&gt;");
            return newJsonDesc;
        }
    }
}
