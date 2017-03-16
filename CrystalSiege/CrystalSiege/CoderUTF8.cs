using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrystalSiege
{
    public class CoderUTF8
    {
        public static string Encode(string str)
        {
            if (str == null)
            {
                return "";
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(str);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static string Decode(string base64)
        {
            if (base64 == null)
            {
                return "";
            }
            var sa = Convert.FromBase64String(base64);
            string str = System.Text.Encoding.UTF8.GetString(sa);
            return str;
        }
    }
}