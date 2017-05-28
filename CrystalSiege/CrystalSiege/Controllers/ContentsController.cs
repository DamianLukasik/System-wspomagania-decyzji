using CrystalSiege.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;

namespace CrystalSiege.Controllers
{
    public class ContentsController : Controller
    {  
        public static List<String> GetImage()
        {
            List<String> dane = new List<String>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                string directory = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Image";
                IEnumerable<string> filenames = System.IO.Directory.EnumerateFiles(directory, "*.*");
                
                foreach (String filename in filenames)
                {
                    String[] tab = filename.Split('\\');
                    string s = tab[tab.Length - 1];
                    if (s == "haha_zecora.png") { continue; }
                    dane.Add(s);
                }
            }
            return dane;
        }
        private string GetCookie()
        {
            var myCookie = CrystalSiege.Resources.HomeTexts.lang_set;
         /*   if (System.Web.HttpContext.Current.Request.Cookies["Language"] != null && System.Web.HttpContext.Current.Request.Cookies["Language"].Value != "")
            {
                myCookie = System.Web.HttpContext.Current.Response.Cookies["Language"].Value;
            }     */     
            return myCookie;
        }
  
        // GET: Contents
        public ActionResult Index(int id)
        {
            var myCookie = GetCookie();

            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                string tab = "";
                if (myCookie != null)
                {
                    switch (myCookie)
                    {
                        case "pl":
                            tab = contents.Subsections
                                .Where(u => u.Id == id)
                                .FirstOrDefault().content;
                            break;
                        case "en":
                            tab = contents.Subsections
                                .Where(u => u.Id == id)
                                .FirstOrDefault().content_ang;
                            break;
                    }
                }
                else
                {
                    tab = contents.Subsections
                        .Where(u => u.Id == id)
                        .FirstOrDefault().content;
                }           
                String str = Server.HtmlDecode(tab);
                ViewBag.Message = CoderUTF8.Decode(str);
            }
            return View();
        }
        public ActionResult News(int news)
        {
            var myCookie = GetCookie();

            if (news != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    News news_ = contents.News.Where(u => u.Id == news).FirstOrDefault();
                    List<String> str = new List<String>();
                    switch (myCookie)
                    {
                        case "pl":
                            str.Add(CoderUTF8.Decode(news_.title));//[0]
                            str.Add(CoderUTF8.Decode(news_.description));//[1]
                            break;
                        case "en":
                            str.Add(CoderUTF8.Decode(news_.title_eng));//[0]
                            str.Add(CoderUTF8.Decode(news_.description_eng));//[1]
                            break;
                    }
                    str.Add(news_.author);//[2]
                    str.Add(DecodeDate(news_.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))));//[3]
                    str.Add(Server.HtmlDecode(news_.image));//[4]
                    List<News_Tags> news_tags = news_.News_Tags.ToList();
                    String tagi = "";
                    switch (myCookie)
                    {
                        case "pl":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags_pl + "</span> ";
                            }
                            break;
                        case "en":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags + "</span> ";
                            }
                            break;
                    }
                    str.Add(tagi);//[5]
                    ViewBag.Message = str;
                }
                return View();
            }
            return View("~/Views/Home/Index.cshtml");
        }   
        public ActionResult NewsList(String month)
        {
            var myCookie = GetCookie();

            List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                int m = new ContentsController().DecodeDateMonth(month);
                List<News> news = contents.News.Where(u => u.date.Value.Month == m).OrderByDescending(s => s.date).ToList();
               // News.ToList();
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                    switch (myCookie)
                    {
                        case "pl":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags_pl + "</span> ";
                            }
                            String[] str1 = {
                                CoderUTF8.Decode(wart.title),
                                CoderUTF8.Decode(wart.description),
                                new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                                wart.author,
                                tagi,
                                wart.image,
                                wart.Id.ToString()
                            };
                            dane.Add(str1);
                            break;
                        case "en":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags + "</span> ";
                            }
                            String[] str2 = {
                                CoderUTF8.Decode(wart.title),
                                CoderUTF8.Decode(wart.description),
                                new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                                wart.author,
                                tagi,
                                wart.image,
                                wart.Id.ToString()
                            };
                            dane.Add(str2);
                            break;
                    }
                }
            }
            ViewBag.Message = dane;

            return View();
        }
        public List<String[]> ReadCarouselInfo()
        {
            var myCookie = GetCookie();
            
            List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<CarouselInfo> tab = contents.CarouselInfoes.ToList();

                foreach (CarouselInfo wart in tab)
                {
                    switch (myCookie)
                    {                       
                        case "en":
                            String[] str2 = {
                                CoderUTF8.Decode(wart.Title_ang),
                                CoderUTF8.Decode(wart.Description_ang),
                                wart.image,
                                wart.Link
                            };
                            dane.Add(str2);
                            break;                        
                        case "pl":
                            String[] str1 = {
                                CoderUTF8.Decode(wart.Title),
                                CoderUTF8.Decode(wart.Description),
                                wart.image,
                                wart.Link
                            };
                            dane.Add(str1);
                            break;
                    }
                }
                return dane;
            }
            return dane;
        }  
        public Dictionary<String, int> ReadAllMonthsNews()
        {
            Dictionary<String, int> result = new Dictionary<String, int>();
            List<String> months = new List<String>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();
                if (news.Count == 0)
                {
                    return null;
                }
                foreach (News wart in news)
                {
                    DateTime wart_date = wart.date.Value;
                    months.Add(DecodeDateMonths(wart_date.Month.ToString()));
                }
                //-----
                result.Add(months[0], 1);
                for(int i=1; i<months.Count; i++)
                {
                    String month = months[i];

                    if (result.ContainsKey(month))
                    {
                        result[month] = result[month] + 1;
                    }
                    else
                    {
                        result.Add(month, 1);
                    }
                }
                return result;
            }
            return result;
        }      
        public List<String[]> ReadAllNews()
        {
            List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();
                if (news.Count == 0)
                {
                    return null;
                }
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                                        
                    var myCookie = GetCookie();

                    switch (myCookie)
                    {
                        case "pl":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags_pl + "</span> ";
                            }
                            String[] str1 = {
                                CoderUTF8.Decode(wart.title),
                                CoderUTF8.Decode(wart.description),
                                DecodeDate(wart.date.Value.ToString("G",
                                CultureInfo.CreateSpecificCulture("en-US"))),
                                wart.author,
                                tagi,
                                wart.image,
                                wart.Id.ToString()
                            };
                            dane.Add(str1);
                            break;
                        case "en":
                            foreach (News_Tags nt in news_tags)
                            {
                                tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags + "</span> ";
                            }
                            String[] str2 = {
                                CoderUTF8.Decode(wart.title_eng),
                                CoderUTF8.Decode(wart.description_eng),
                                DecodeDate(wart.date.Value.ToString("G",
                                CultureInfo.CreateSpecificCulture("en-US"))),
                                wart.author,
                                tagi,
                                wart.image,
                                wart.Id.ToString()
                            };
                            dane.Add(str2);
                            break;
                    }
                }
                return dane;
            }
            return dane;
        }   
        public String DecodeDateMonths(String month)
        {
            string myCookie = GetCookie();
            String result = "";

            switch (myCookie)
            {
                case "pl":
                    switch (month)
                    {
                        case "1":
                            result = "Styczeń";
                            break;
                        case "2":
                            result = "Luty";
                            break;
                        case "3":
                            result = "Marzec";
                            break;
                        case "4":
                            result = "Kwiecień";
                            break;
                        case "5":
                            result = "Maj";
                            break;
                        case "6":
                            result = "Czerwiec";
                            break;
                        case "7":
                            result = "Lipiec";
                            break;
                        case "8":
                            result = "Sierpień";
                            break;
                        case "9":
                            result = "Wrzesień";
                            break;
                        case "10":
                            result = "Październik";
                            break;
                        case "11":
                            result = "Listopad";
                            break;
                        case "12":
                            result = "Grudzień";
                            break;
                        default:
                            break;
                    }
                    break;
                case "en":
                    switch (month)
                    {
                        case "1":
                            result = "January";
                            break;
                        case "2":
                            result = "February";
                            break;
                        case "3":
                            result = "March";
                            break;
                        case "4":
                            result = "April";
                            break;
                        case "5":
                            result = "May";
                            break;
                        case "6":
                            result = "June";
                            break;
                        case "7":
                            result = "July";
                            break;
                        case "8":
                            result = "August";
                            break;
                        case "9":
                            result = "Septembrr";
                            break;
                        case "10":
                            result = "October";
                            break;
                        case "11":
                            result = "November";
                            break;
                        case "12":
                            result = "December";
                            break;
                        default:
                            break;
                    }
                    break;
            }
            
            return result;
        } 
        public int DecodeDateMonth(String month)
        {
            int result = 0;
            string myCookie = GetCookie();

            switch (myCookie)
            {
                case "pl":
                    switch (month)
                    {
                        case "Styczeń":
                            result = 1;
                            break;
                        case "Luty":
                            result = 2;
                            break;
                        case "Marzec":
                            result = 3;
                            break;
                        case "Kwiecień":
                            result = 4;
                            break;
                        case "Maj":
                            result = 5;
                            break;
                        case "Czerwiec":
                            result = 6;
                            break;
                        case "Lipiec":
                            result = 7;
                            break;
                        case "Sierpień":
                            result = 8;
                            break;
                        case "Wrzesień":
                            result = 9;
                            break;
                        case "Październik":
                            result = 10;
                            break;
                        case "Listopad":
                            result = 11;
                            break;
                        case "Grudzień":
                            result = 12;
                            break;
                        default:
                            break;
                    }
                    break;
                case "en":
                    switch (month)
                    {
                        case "January":
                            result = 1;
                            break;
                        case "February":
                            result = 2;
                            break;
                        case "March":
                            result = 3;
                            break;
                        case "April":
                            result = 4;
                            break;
                        case "May":
                            result = 5;
                            break;
                        case "June":
                            result = 6;
                            break;
                        case "July":
                            result = 7;
                            break;
                        case "August":
                            result = 8;
                            break;
                        case "September":
                            result = 9;
                            break;
                        case "October":
                            result = 10;
                            break;
                        case "November":
                            result = 11;
                            break;
                        case "December":
                            result = 12;
                            break;
                        default:
                            break;
                    }
                    break;
            }
            
            return result;
        }
        public String DecodeDate(String data)
        {
            var myCookie = GetCookie();

            String result = "";
            //3/14/2017 11:58:00 PM
            //"{dt:M/d/yyyy HH:mm:ss}"
            String[] tab = data.Split('/');
            result += tab[1] + " ";
            switch (myCookie)
            {
                case "pl":
                    switch (tab[0])
                    {
                        case "1":
                            result += "Stycznia";
                            break;
                        case "2":
                            result += "Lutego";
                            break;
                        case "3":
                            result += "Marca";
                            break;
                        case "4":
                            result += "Kwietnia";
                            break;
                        case "5":
                            result += "Maja";
                            break;
                        case "6":
                            result += "Czerwca";
                            break;
                        case "7":
                            result += "Lipca";
                            break;
                        case "8":
                            result += "Sierpnia";
                            break;
                        case "9":
                            result += "Września";
                            break;
                        case "10":
                            result += "Października";
                            break;
                        case "11":
                            result += "Listopada";
                            break;
                        case "12":
                            result += "Grudnia";
                            break;
                        default:
                            break;
                    }
                    break;
                case "en":
                    switch (tab[0])
                    {
                        case "1":
                            result += "January";
                            break;
                        case "2":
                            result += "February";
                            break;
                        case "3":
                            result += "March";
                            break;
                        case "4":
                            result += "April";
                            break;
                        case "5":
                            result += "May";
                            break;
                        case "6":
                            result += "June";
                            break;
                        case "7":
                            result += "July";
                            break;
                        case "8":
                            result += "August";
                            break;
                        case "9":
                            result += "September";
                            break;
                        case "10":
                            result += "October";
                            break;
                        case "11":
                            result += "November";
                            break;
                        case "12":
                            result += "December";
                            break;
                        default:
                            break;
                    }
                    break;
            }            
            result += " " + tab[2].Split(':')[0] + ":" + tab[2].Split(':')[1] + " ";

            return result;
        }  
        public List<String> ReadResources()
        {
        //    ResourceManager res = Resources.HomeTexts.ResourceManager;
            List<String> s = new List<String>();
            ResourceManager q = Resources.ContentIds.ResourceManager;
            ResourceSet resourceSet = q.GetResourceSet(CultureInfo.CurrentUICulture, true, true);      
            foreach (DictionaryEntry entry in resourceSet)
            {//Wczytuje wszystkie wiersze z zasobu
             //   String[] str = { "", "" };
             //   str[0] = entry.Key.ToString();
             //   str[1] = entry.Value.ToString();
                s.Add(entry.Key.ToString());       
            }
            return s;
        }     
        public static List<List<String[]>> Get_Sections()
        {
            List<List<String[]>> dane = new List<List<String[]>>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<Section> sections = contents.Sections.ToList();                
                foreach (Section sec in sections)
                {/*
                    List<String[]> dane_ = new List<String[]>();
                    if (sect.Subsections.Count != 0)
                    {
                        List<Subsections> subsect_list = sect.Subsections.ToList();                        
                        foreach (Subsections subsect in subsect_list)
                        {
                            String[] str = {
                                CoderUTF8.Decode(sect.title),//[0]
                                CoderUTF8.Decode(subsect.title),//[1]
                                CoderUTF8.Decode(subsect.content),//[2]
                                subsect.Id,//[3]
                                subsect.SectionsId//[4]
                            };
                            dane_.Add(str);
                        }
                    }
                    else
                    {
                        String[] str = {
                                CoderUTF8.Decode(sect.title),//[0]
                            };
                        dane_.Add(str);
                    }  */

                    //
                    String[] t = {
                        sec.Id.ToString(),
                        CoderUTF8.Decode(sec.title),
                        CoderUTF8.Decode(sec.title_ang)
                    };

                    List<String> subsections = new List<string>();
                    List<String[]> tab = new List<String[]>();                   
                    tab.Add(t);

                    if (sec.Subsections.Count != 0)
                    {
                        List<Subsection> subsec = sec.Subsections.ToList();                 
                        foreach (Subsection ss in subsec)
                        {
                            String[] y = {
                            ss.Id.ToString(),
                            CoderUTF8.Decode(ss.title),
                            CoderUTF8.Decode(ss.title_ang)
                        };
                            tab.Add(y);
                            //  subsections += "<li><a href='~/Contents/Index'>"+ss.title+"</a></li>";
                            //<li>" + @Html.ActionLink(ss.title, "Index", "Contents", new { id = sekcja }, null) + "</li>                     
                        }
                        
                    }
                    dane.Add(tab);
                }
            }
            return dane;
        }
    }
}