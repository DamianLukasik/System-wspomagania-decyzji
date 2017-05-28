using CrystalSiege.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrystalSiege.Controllers
{
    public class HomeController : Controller
    {
    
        public ActionResult Index()
        {
            return View();
        }

        private Boolean Search (String res, string[] searchs)
        {
            foreach (string ss in searchs)
            {
                bool i = res.ToLower().Contains(ss.ToLower());
                if (i)
                {
                    return true;
                }
            }
            return false;
        }
      
        public ActionResult SearchResult(string search)
        {
            string[] searchs = search.Split(' ');                      
                        
            List<List<String>> njusy = new List<List<String>>();
            List<string[]> wyniki_news = new List<string[]>();
            List<string[]> wyniki_cont = new List<string[]>();
            
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<News> news = contents.News.ToList();
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    bool log = false;
                    foreach (News_Tags tg in news_tags)
                    {
                        if (Search(tg.Tag.tags_pl, searchs) && CrystalSiege.Resources.HomeTexts.lang_set == "pl")
                        {
                            string title_ = "";
                            switch (Resources.HomeTexts.lang_set)
                            {
                                case "pl":
                                    title_ = wart.title;
                                    break;
                                case "en":
                                    title_ = wart.title_eng;
                                    break;
                            }
                            string[] tab_s = {
                                wart.Id.ToString(),//[0]
                                CoderUTF8.Decode(title_) ,//[1]
                                "tag " + tg.Tag.color + " " + tg.Tag.tags_pl + "",//[2]
                                new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                                wart.author //[4]
                            };
                            wyniki_news.Add(tab_s);
                            // wyniki.Add(wart.Id);
                            log = true;
                            continue;
                        }
                        if (Search(tg.Tag.tags, searchs) && CrystalSiege.Resources.HomeTexts.lang_set == "en")
                        {
                            string title_ = "";
                            switch (Resources.HomeTexts.lang_set)
                            {
                                case "pl":
                                    title_ = wart.title;
                                    break;
                                case "en":
                                    title_ = wart.title_eng;
                                    break;
                            }
                            string[] tab_s = {
                                wart.Id.ToString(),//[0]
                                CoderUTF8.Decode(title_) ,//[1]
                                "tag " + tg.Tag.color + " " + tg.Tag.tags + "",//[2]
                                new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                                wart.author //[4]
                            };
                            wyniki_news.Add(tab_s);
                            // wyniki.Add(wart.Id);
                            log = true;
                            continue;
                        }
                    }
                    if (log) { log = false; break; }
                    if (Search(CoderUTF8.Decode(wart.title), searchs))
                    {
                        string title_ = "";
                        switch (Resources.HomeTexts.lang_set)
                        {
                            case "pl":
                                title_ = wart.title;
                                break;
                            case "en":
                                title_ = wart.title_eng;
                                break;
                        }

                        string[] tab_s = {
                            wart.Id.ToString(),//[0]
                            CoderUTF8.Decode(title_) ,//[1]
                            Resources.HomeTexts.InTitle,//Tytuł - \" " + CoderUTF8.Decode(wart.title) + "\"",//[2]
                            new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                            wart.author //[4]
                        };
                        wyniki_news.Add(tab_s);
                        // wyniki.Add(wart.Id);
                        continue;
                    }
                    if (Search(CoderUTF8.Decode(wart.description), searchs))
                    {
                        string title_ = "";
                        switch (Resources.HomeTexts.lang_set)
                        {
                            case "pl":
                                title_ = wart.title;
                                break;
                            case "en":
                                title_ = wart.title_eng;
                                break;
                        }

                        string[] tab_s = {
                            wart.Id.ToString(),//[0]
                            CoderUTF8.Decode(title_) ,//[1]
                            Resources.HomeTexts.InContentNews,//[2]
                            new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                            wart.author //[4]
                        };
                        wyniki_news.Add(tab_s);
                        // wyniki.Add(wart.Id);
                        continue;
                    }
                    if (Search(wart.author, searchs))
                    {
                        string title_ = "";
                        switch (Resources.HomeTexts.lang_set)
                        {
                            case "pl":
                                title_ = wart.title;
                                break;
                            case "en":
                                title_ = wart.title_eng;
                                break;
                        }
                        string[] tab_s = {
                            wart.Id.ToString(),//[0]
                            CoderUTF8.Decode(title_) ,//[1]
                            Resources.HomeTexts.InNameAuthor,// + wart.author,//[2]
                            new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                            wart.author //[4]
                        };
                        wyniki_news.Add(tab_s);
                        // wyniki.Add(wart.Id);
                        continue;
                    }  
                }
                
                List<Subsection> subs = contents.Subsections.ToList();
                foreach (Subsection sec in subs)
                {
                    if (Search(CoderUTF8.Decode(sec.title), searchs))
                    {
                        string[] tab_s = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title),//[1]
                            Resources.HomeTexts.InNameSubsection,//- \"" + CoderUTF8.Decode(sec.title) +"\"" ,//[2]
                        };
                        wyniki_cont.Add(tab_s);
                        continue;
                    }
                    if (Search(CoderUTF8.Decode(sec.title_ang), searchs))
                    {
                        string[] tab_s = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title_ang),//[1]
                            Resources.HomeTexts.InNameSubsection,//- \"" + CoderUTF8.Decode(sec.title_ang) +"\"" ,//[2]
                        };
                        wyniki_cont.Add(tab_s);
                        continue;
                    }
                    if (Search(CoderUTF8.Decode(sec.content), searchs))
                    {
                        string[] tab_s = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title),//[1]
                            Resources.HomeTexts.InContentSubsection,//- \"" + CoderUTF8.Decode(sec.title) +"\"" ,//[2]
                        };
                        wyniki_cont.Add(tab_s);
                        continue;
                    }
                    if (Search(CoderUTF8.Decode(sec.content_ang), searchs))
                    {
                        string[] tab_s = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title_ang),//[1]
                            Resources.HomeTexts.InContentSubsection,//- \"" + CoderUTF8.Decode(sec.title_ang) +"\"" ,//[2]
                        };
                        wyniki_cont.Add(tab_s);
                        continue;
                    }
                }
            }

            List<List<string[]>> list = new List<List<string[]>>();

            if ((wyniki_news.Count + wyniki_cont.Count) != 0)
            {
                int con;
                ViewBag.Title = Resources.HomeTexts.SearchResults; 

                list.Add(wyniki_news);
               // ViewBag.List_news = wyniki_news;
                con = wyniki_news.Count;
                if (wyniki_news.Count == 1)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " news pasujący do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                  //  ViewBag.Message_news = "Znaleziono " + con + " news pasujący do kryteriów";
                }
                if (wyniki_news.Count >= 2 && wyniki_news.Count <= 4)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " newsy pasujące do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                    //   ViewBag.Message_news = "Znaleziono " + con + " newsy pasujące do kryteriów";
                }
                if (wyniki_news.Count >= 5)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " newsów pasujące do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                    //   ViewBag.Message_news = "Znaleziono " + con + " newsów pasujące do kryteriów";
                }

                list.Add(wyniki_cont);
             //   ViewBag.List_cont = wyniki_cont;
                con = wyniki_cont.Count;
                if (wyniki_cont.Count == 1)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcje pasującą do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                    //    ViewBag.Message_cont = "Znaleziono " + con + " subsekcje pasującą do kryteriów";
                }
                if (wyniki_cont.Count >= 2 && wyniki_cont.Count <= 4)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcje pasujące do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                    //    ViewBag.Message_cont = "Znaleziono " + con + " subsekcje pasujące do kryteriów";
                }
                if (wyniki_cont.Count >= 5)
                {
                    List<string[]> a = new List<string[]>();
                    string[] b = { };
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcji pasujące do kryteriów" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteria" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                    list.Add(a);
                    //   ViewBag.Message_cont = "Znaleziono " + con + " subsekcji pasujące do kryteriów";
                }
                ViewBag.Message = list;
            }
            else
            {
                 ViewBag.Title = Resources.HomeTexts.NothingFound;
            }           
            //
            return View();
        }
     
        public ActionResult Secure(string id)
        {
            if (id != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    id = CoderUTF8.Encode(id);
                    Secure idx = contents.Secures.Where(u => u.link == id).FirstOrDefault();
                    Person per = contents.People.Where(u => u.access == 1).FirstOrDefault();
                    if (idx != null)
                    {
                        ViewBag.Title = Resources.HomeTexts.YourLoginDate;
                        ViewBag.Message = "<div><dl class='dl-horizontal'></dl><dt>"+Resources.HomeTexts.Login+":</dt><dd>" + per.username + "</dd><dt>"+Resources.HomeTexts.password+":</dt><dd>" + per.password + "</dd></div>";
                    }
                    else
                    {
                        ViewBag.Title = Resources.HomeTexts.ThePasswordRecoverySitelinkDoesNotExist;
                        ViewBag.Message = "<div class='container body-content'><br><img class='media-object img-rounded' src='../Resources/Image/haha_zecora.png' width='210px' height='210px'><br>";
                    }
                    var t = contents.Secures.Where(u => u.link == id).First();
                    contents.Secures.Remove(t);
                    contents.SaveChanges();
                }
                return View();
            }
            else
            {
                return View("Home/Index");
            }            
        }
    }
}