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
            string[] searchs = search.Split(' ');//słowa kluczowe 
            
            List<List<string[]>> result = new List<List<string[]>>();
            List<string[]> wyniki_news = new List<string[]>();
            List<string[]> wyniki_cont = new List<string[]>();

            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {//sprawdzać njusy
                List<News> news_list = contents.News.ToList();
                Boolean log, log_title, log_desc, log_author;
                foreach (News news in news_list)
                {
                    log = false;
                    log_title = false;
                    log_desc = false;
                    log_author = false;
                    string str_title_desc = "";

                    List<News_Tags> news_tags = news.News_Tags.ToList();
                    foreach (News_Tags tg in news_tags)
                    {//po wszystkich tagach
                        string title_ = "";
                        string tags_ = "";
                        switch (Resources.HomeTexts.lang_set)
                        {
                            case "pl":
                                if (Search(tg.Tag.tags_pl, searchs))
                                {
                                    title_ = news.title;
                                    tags_ = tg.Tag.tags_pl.ToString();
                                    log = true;
                                }
                                break;
                            case "en":
                                if (Search(tg.Tag.tags, searchs))
                                {
                                    title_ = news.title_eng;
                                    tags_ = tg.Tag.tags.ToString();
                                    log = true;
                                }
                                break;
                        }
                        if (log)
                        {
                            string[] tab_s = {
                                news.Id.ToString(),//[0]
                                CoderUTF8.Decode(title_) ,//[1]
                                "tag " + tg.Tag.color + " " + tags_ + "",//[2]
                                new ContentsController().DecodeDate(news.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                                news.author //[4]
                            };
                            wyniki_news.Add(tab_s);
                            break;
                        }
                    }
                    if (log) { continue; }
                    if (Search(CoderUTF8.Decode(news.title), searchs))
                    {//po tytule
                        log_title = true;
                        str_title_desc = Resources.HomeTexts.InTitle;
                    }
                    else if (Search(CoderUTF8.Decode(news.description), searchs))
                    {//po opisie
                        log_desc = true;
                        str_title_desc = Resources.HomeTexts.InContentNews;
                    }
                    else if (Search(CoderUTF8.Decode(news.author), searchs))
                    {//po nazwie autora
                        log_author = true;
                        str_title_desc = Resources.HomeTexts.InNameAuthor;
                    };
                    if (log_desc || log_title || log_author)
                    {
                        string title_ = "";
                        switch (Resources.HomeTexts.lang_set)
                        {
                            case "pl":
                                title_ = news.title;
                                break;
                            case "en":
                                title_ = news.title_eng;
                                break;
                        }
                        log = true;
                        string[] tab_s = {
                            news.Id.ToString(),//[0]
                            CoderUTF8.Decode(title_) ,//[1]
                            str_title_desc,//Tytuł - \" " + CoderUTF8.Decode(wart.title) + "\"",//[2]
                            new ContentsController().DecodeDate(news.date.Value.ToString("G",
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) ,//[3]
                            news.author //[4]
                        };
                        wyniki_news.Add(tab_s);
                        continue;
                    }
                }
                //sprawdzać content
                List<Subsection> subs = contents.Subsections.ToList();
                foreach (Subsection sec in subs)
                {
                    switch (Resources.HomeTexts.lang_set)
                    {
                        case "pl":
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
                            break;
                        case "en":
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
                            break;
                    }                    
                }
            }

            //////
            List<List<string[]>> list = new List<List<string[]>>();
            List<string[]> a = new List<string[]>();
            string[] b = { };
            int con;
            if (wyniki_news.Count != 0)
            {
                con = wyniki_news.Count;
                if (wyniki_news.Count == 1)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " news pasujący do kryteriów", "news" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteries", "news" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                if (wyniki_news.Count >= 2 && wyniki_news.Count <= 4)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " newsy pasujące do kryteriów", "news" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteries", "news" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                if (wyniki_news.Count >= 5)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " newsów pasujące do kryteriów", "news" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " news matched to the criteries", "news" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                result.Add(a);
                result.Add(wyniki_news);
            }
            else
            {
                List<string[]> cc = new List<string[]>();
                string[] c = { "nic", "news" };
                cc.Add(c);
                result.Add(cc);
                result.Add(cc);
            }
            //
            if (wyniki_cont.Count != 0)
            {
                con = wyniki_cont.Count;
                if (wyniki_cont.Count == 1)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcje pasującą do kryteriów", "content" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteries", "content" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                if (wyniki_cont.Count >= 2 && wyniki_cont.Count <= 4)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcje pasujące do kryteriów", "content" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteries", "content" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                if (wyniki_cont.Count >= 5)
                {
                    switch (CrystalSiege.Resources.HomeTexts.lang_set)
                    {
                        case "pl":
                            string[] b1 = { "Znaleziono " + con + " subsekcji pasujące do kryteriów", "content" };
                            b = b1;
                            break;
                        case "en":
                            string[] b2 = { "Found " + con + " subsections matched to the criteries", "content" };
                            b = b2;
                            break;
                    }
                    a.Add(b);
                }
                result.Add(a);
                result.Add(wyniki_cont);
            }
            else
            {
                List<string[]> cc = new List<string[]>();
                string[] c = { "nic", "content" };
                cc.Add(c);
                result.Add(cc);
                result.Add(cc);
            }
            ViewBag.Message = result;

            if ((wyniki_news.Count + wyniki_cont.Count) != 0)
            {
                ViewBag.Title = Resources.HomeTexts.SearchResults;
            }
            else
            { 
                ViewBag.Title = Resources.HomeTexts.NothingFound;
            }
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