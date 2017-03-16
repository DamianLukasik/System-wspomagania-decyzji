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
            using (ContentsEntities contents = new ContentsEntities())
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

        // GET: Contents
        public ActionResult Index(int id)
        {
            using (ContentsEntities contents = new ContentsEntities())
            {                
                string tab = contents.Subsections
                         .Where(u => u.Id == id)
                         .FirstOrDefault().content;
                String str = Server.HtmlDecode(tab);

                ViewBag.Message = CoderUTF8.Decode(str);

            }
            return View();
        }

        public ActionResult News(int news)
        {
            if (news != null)
            {
                using (ContentsEntities contents = new ContentsEntities())
                {
                    News news_ = contents.News.Where(u => u.Id == news).FirstOrDefault();
                    List<String> str = new List<String>();
                    str.Add(CoderUTF8.Decode(news_.title));//[0]
                    str.Add(CoderUTF8.Decode(news_.description));//[1]
                    str.Add(news_.author);//[2]
                    str.Add(DecodeDate(news_.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))));//[3]
                    str.Add(Server.HtmlDecode(news_.image));//[4]
                    List<News_Tags> news_tags = news_.News_Tags.ToList();
                    String tagi = "";
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: " + nt.Tags.color + "'>" + nt.Tags.tags_pl + "</span> ";
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
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                int m = new ContentsController().DecodeDateMonth(month);
                List<News> news = contents.News.Where(u => u.date.Value.Month == m).OrderByDescending(s => s.date).ToList();
               // News.ToList();
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: " + nt.Tags.color + "'>" + nt.Tags.tags_pl + "</span> ";
                    }

                    String[] str = {
                        CoderUTF8.Decode(wart.title),
                        CoderUTF8.Decode(wart.description),
                        new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                        wart.author,
                        tagi,
                        wart.image,
                        wart.Id.ToString()
                    };
                    dane.Add(str);
                }
            }
            ViewBag.Message = dane;

            return View();
        }

        public List<String[]> ReadCarouselInfo()
        {
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<CarouselInfo> tab = contents.CarouselInfo.ToList();

                foreach (CarouselInfo wart in tab)
                {
                    String[] str = {
                        CoderUTF8.Decode(wart.Title),
                        CoderUTF8.Decode(wart.Description),
                        wart.image,
                        wart.Link
                    };
                    dane.Add(str);
                }
                return dane;
            }
            return dane;
        }

        public Dictionary<String, int> ReadAllMonthsNews()
        {
            Dictionary<String, int> result = new Dictionary<String, int>();
            List<String> months = new List<String>();
            using (ContentsEntities contents = new ContentsEntities())
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
            using (ContentsEntities contents = new ContentsEntities())
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
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: "+nt.Tags.color+"'>"+ nt.Tags.tags_pl + "</span> ";
                    }

                    String[] str = {
                        CoderUTF8.Decode(wart.title),
                        CoderUTF8.Decode(wart.description),
                        DecodeDate(wart.date.Value.ToString("G",
                        CultureInfo.CreateSpecificCulture("en-US"))),
                        wart.author,
                        tagi,
                        wart.image,
                        wart.Id.ToString()
                    };
                    dane.Add(str);
                }
                return dane;
            }
            return dane;
        }

        public String DecodeDateMonths(String month)
        {
            String result ="";
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
            return result;
        }
        public int DecodeDateMonth(String month)
        {
            int result = 0;
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
            return result;
        }

        public String DecodeDate(String data)
        {
            String result = "";
            //3/14/2017 11:58:00 PM
            //"{dt:M/d/yyyy HH:mm:ss}"
            String[] tab = data.Split('/');
            result += tab[1] + " ";
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
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<Sections> sections = contents.Sections.ToList();                
                foreach (Sections sec in sections)
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
                   

                    List<String> subsections = new List<string>();
                    List<String[]> tab = new List<String[]>();
                    String[] t = {
                           sec.Id.ToString(),
                            CoderUTF8.Decode(sec.title),
                            CoderUTF8.Decode(sec.title_ang)
                        };
                    tab.Add(t);

                    if (sec.Subsections.Count != 0)
                    {
                        List<Subsections> subsec = sec.Subsections.ToList();                 
                        foreach (Subsections ss in subsec)
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