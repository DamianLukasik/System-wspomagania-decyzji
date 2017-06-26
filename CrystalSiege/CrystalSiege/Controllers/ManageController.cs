using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CrystalSiege.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Hosting;

namespace CrystalSiege.Controllers
{
  //  [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //
        //News
        public ActionResult DeleteNews(int id)
        {
            if (Request.Cookies["Session"] != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    News news = contents.News.Where(u => u.Id == id).First();
                    List<News_Tags> newsList = contents.News_Tags.Where(u => u.NewsID == news.Id).ToList();
                    foreach (News_Tags n in newsList)
                    {
                        contents.News_Tags.Remove(n);
                    }

                    contents.News.Remove(news);
                    contents.SaveChanges();
                }
                ViewNews();
                return View("ViewNews");
            }
            return View("../Home/Index");
        }
        public ActionResult ViewNews()
        {
            if (Request.Cookies["Session"] != null)
            {
                List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags_pl + "</span> ";
                    }
                    String absolutePath = FileIsExists(wart.image);

                    String[] str = {
                        CoderUTF8.Decode(wart.title),
                        absolutePath,
                        new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                        wart.author,
                        wart.Id.ToString()                        
                    };
                    dane.Add(str);
                }
            }
            ViewBag.Message = dane;
            return View();
            }
            return View("../Home/Index");
        }

        public String FileIsExists(string img)
        {
            var absolutePath = HttpContext.Server.MapPath("~/Resources/Image/" + img);
            if (System.IO.File.Exists(absolutePath))
            {
                absolutePath = img;
            }
            else
            {
                absolutePath = "haha_zecora.png";
            }
            return absolutePath;
        }

        public ActionResult AddNews()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
           return View("../Home/Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNews(NewsViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            if (Request.Cookies["Session"] != null)
            {
                string filename = UploadImage(file, model.img);
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    string id_ = Request.Cookies["Session"].Value;
                    Person user = contents.People.Where(u => u.Id == id_).FirstOrDefault();
                    //
                    int idx = 1;                    
                    if (contents.News.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.News.ToList().LastOrDefault().Id + 1);
                    }
                    //
                    int i_ = 1;
                    int idxt = 1;                    
                    //
                    //Tags
                    List<News_Tags> news_tags = new List<News_Tags>();
                    //Walidacja
                    string[] tab_tags;
                    if (model.Tags != null)
                    {
                        tab_tags = model.Tags.Split('|');
                        foreach (string tab_tag in tab_tags)
                        {
                            if (tab_tag == "") { break; }
                            if (contents.News_Tags.ToList().Count != 0)
                            {
                                idxt = Convert.ToInt32(contents.News_Tags.ToList().LastOrDefault().Id + i_);
                            }
                            else
                            {
                                idxt += i_ - 1;
                            }
                            News_Tags t = new News_Tags();
                            t.Id = idxt;
                            t.NewsID = idx;
                            var id_tag = contents.Tags.Where(u => u.tags_pl == tab_tag).FirstOrDefault();
                            t.TagsID = id_tag.Id.ToString();
                            news_tags.Add(t);
                            i_++;
                        }
                        var customers = contents.Set<News>();
                        customers.Add(new News
                        {
                            Id =  idx,                        
                            title = CoderUTF8.Encode(model.Title),
                            description = CoderUTF8.Encode(model.Description),
                            image = filename,//zrobić wybór, albo upload obrazka
                            date = DateTime.Now,
                            author = user.username,
                            News_Tags = news_tags
                        });
                    }
                    else
                    {
                        ViewBag.Message = "Zapomniałeś tagów";
                        var customers = contents.Set<News>();
                        customers.Add(new News
                        {
                            Id = idx,
                            title = CoderUTF8.Encode(model.Title),
                            description = CoderUTF8.Encode(model.Description),
                            image = filename,//zrobić wybór, albo upload obrazka
                            date = DateTime.Now,
                            author = user.username,
                        });
                    }
                    
                    //News
                    
                    contents.SaveChanges();
                    /*
                    //Tags
                    
                    var tab_tag_ = contents.Set<News_Tags>();
                    foreach (string tab_tag in tab_tags)
                    {
                        if (tab_tag == "") { break; }
                        var id_tag = contents.Tags.Where(u => u.tags_pl == tab_tag).FirstOrDefault();
                        tab_tag_.Add(new News_Tags
                        {
                            Id = Guid.NewGuid().ToString(),
                            NewsID = idx,
                            TagsID = id_tag.ToString()
                        });
                    }

                    contents.SaveChanges();*/
                    ViewNews();
                    return View("ViewNews");
                }
            }
            return View("Content");
            }
            return View("../Home/Index");
        }
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditNews(NewsViewModel viewModel, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                News news;
            using (var ctx = new damianlukasik3612_crystalsiegeEntities())
            {
                news = ctx.News.Where(s => s.Id == viewModel.Id).FirstOrDefault<News>();

                //problem z kodowaniem
                List<News_Tags> newsList = ctx.News_Tags.Where(u => u.NewsID == news.Id).ToList();
                foreach (News_Tags n in newsList)
                {
                    ctx.News_Tags.Remove(n);
                }
                //
                //
                List<News_Tags> news_tags = new List<News_Tags>();

                int idx = viewModel.Id;//Convert.ToInt32(ctx.News.ToList().LastOrDefault().Id);                

                string[] tab_tags;
                if (viewModel.Tags != null)
                { 
                    tab_tags = viewModel.Tags.Split('|');                
                    int i_ = 1;
                    foreach (string tab_tag in tab_tags)
                    {
                        if (tab_tag == "") { break; }
                        int idxt = 1;
                        if (ctx.News_Tags.ToList().Count != 0)
                        {
                            idxt = Convert.ToInt32(ctx.News_Tags.ToList().LastOrDefault().Id + i_);
                        }

                        News_Tags t = new News_Tags();
                        t.Id = idxt;
                        t.NewsID = idx;
                        var id_tag = ctx.Tags.Where(u => u.tags_pl == tab_tag).FirstOrDefault();
                        t.TagsID = id_tag.Id.ToString();
                        news_tags.Add(t);
                        i_++;
                    }
                    news.News_Tags = news_tags;
                }           
                //
                switch (lang)
                {
                    case "pl":
                        if (news != null)
                        {
                            news.title = CoderUTF8.Encode(viewModel.Title);
                            news.description = CoderUTF8.Encode(viewModel.Description);
                            news.image = FileIsExists(viewModel.img);
                         //   news.News_Tags = news_tags;
                        }
                        break;
                    case "en":
                        if (news != null)
                        {
                            news.title_eng = CoderUTF8.Encode(viewModel.Title);
                            news.description_eng = CoderUTF8.Encode(viewModel.Description);
                            news.image = FileIsExists(viewModel.img);
                        //    news.News_Tags = news_tags;
                        }
                        break;
                }                
                ctx.Entry(news).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
            ViewNews();
            return View("ViewNews");
            }
            return View("../Home/Index");
        }
        public ActionResult EditNews(int id, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                News news = contents.News
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                string tags = "";
                List<News_Tags> news_tags = news.News_Tags.ToList();
                foreach (News_Tags nt in news_tags)
                {
                    tags += nt.Tag.tags_pl+"?"+nt.Tag.color+"|";
                }

                String[] dane = { };
                switch (lang)
                {
                    case "pl":
                        String[] dane1 = {
                            CoderUTF8.Decode(news.title),//[0]
                            CoderUTF8.Decode(news.description),//[1]
                            new ContentsController().DecodeDate(news.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),//[2]
                            news.image,//[3]
                            news.Id.ToString(),//[4]
                            tags,//[5]
                            "pl"//[6]
                        };
                        dane = dane1;
                        break;
                    case "en":
                        String[] dane2 = {
                            CoderUTF8.Decode(news.title_eng),//[0]
                            CoderUTF8.Decode(news.description_eng),//[1]
                            new ContentsController().DecodeDate(news.date.Value.ToString("G",
                                System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),//[2]
                            news.image,//[3]
                            news.Id.ToString(),//[4]
                            tags,//[5]
                            "en"//[6]
                        };
                        dane = dane2;
                        break;
                }              
                ViewBag.Message = dane;
            }
            return View();
            }
            return View("../Home/Index");
        }
        
        //
        //Translate Manager
        public ActionResult ViewTranslate()
        {
            if (Request.Cookies["Session"] != null)
            {
                TranslateViewModel model = new TranslateViewModel();
            model.Collection = new List<RawTranslateViewModel>();

            List<String> en = LoadAllResources("en");
            List<String> pl = LoadAllResources("pl");
            for (int i = 0; i<pl.Count; i++)
            {
                String[] str = { en[i], pl[i] };

                model.Collection.Add(new RawTranslateViewModel {
                    Name_Oryginal = en[i],
                    Name_ang = en[i],
                    Name_pl = pl[i]
                });
            }
            ///

            return View(model);
            }
            return View("../Home/Index");
        }
        public ActionResult UpdateTranslate(TranslateViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                var ctr = model.Collection.Count(x => x.Name_Oryginal != null);

            var fullPath = Path.Combine(Server.MapPath("~/Resources"), "HomeTexts.resx");

            XmlDocument doc = new XmlDocument();//przerobić
            doc.Load(fullPath);

            // XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            // manager.AddNamespace("bk", "http://www.contoso.com/books");

            XmlNodeList aNodes = doc.SelectNodes("/root/data");
            model.Collection.Reverse();
            for (int i = 0; i < model.Collection.Count; i++)
            {
                XmlNode Node = aNodes[i].SelectSingleNode("value");
                Node.InnerText = model.Collection[i].Name_ang;
            }

            //dokończyć
            /*
                        if (System.IO.File.Exists(fullPath))
                        {
                         //   XmlDocument doc = new XmlDocument();
                         //   doc.Load(fullPath);
                          //  XmlNodeList aNodes = doc.SelectNodes("/root/data");
                            //foreach (XmlNode aNode in aNodes)
                            for(int i=0; i<model.Collection.Count; i++)
                            {
                               // XElement aNode = new XElement(aNodes[i].Name, aNodes[i].InnerXml);
                                XElement t = XElement.Load(fullPath).Element("root").Elements("data").ElementAt(i);
                                // grab the "id" attribute
                                t.Element("value").Value = model.Collection[i].Name_ang;
                                t.Save(fullPath);
                            }
                            // save the XmlDocument back to disk
                         //   doc.Save(fullPath);
                        }
                        /*
                        Hashtable resourceEntries = new Hashtable();

                        string path = Path.Combine(Server.MapPath("~/Resources"),Path.GetFileName("ContentTexts.resx"));


                        RexResourceReader reader = new ResourceReader(path);

                            if (reader != null)
                            {
                                IDictionaryEnumerator id = reader.GetEnumerator();
                                foreach (DictionaryEntry d in reader)
                                {
                                    if (d.Value == null)
                                        resourceEntries.Add(d.Key.ToString(), "");
                                    else
                                        resourceEntries.Add(d.Key.ToString(), d.Value.ToString());
                                }
                                reader.Close();
                            }

                        //Modify resources here...
                        for (int i = 0; i < model.Collection.Count(); ++i)
                        {
                            string key = model.Collection[i].Name_Oryginal;
                            if (!resourceEntries.ContainsKey(key))
                            {
                                String value = model.Collection[i].Name_ang.ToString();
                                if (value == null) value = "";

                                resourceEntries.Add(key, value);
                            }
                        }

                        //Write the combined resource file
                        ResourceWriter resourceWriter = new ResourceWriter(path);

                        foreach (String key in resourceEntries.Keys)
                        {
                            resourceWriter.AddResource(key, resourceEntries[key]);
                        }
                        resourceWriter.Generate();
                        resourceWriter.Close();
                        */

            return View("Content");
            }
            return View("../Home/Index");
        }
        private List<String> LoadAllResources(string lang)
        {
            ResourceSet resx_ = Resources.HomeTexts.ResourceManager.GetResourceSet(
                System.Globalization.CultureInfo.CreateSpecificCulture(lang), true, true);
            List<String> dane = new List<String>();
            foreach (DictionaryEntry entry in resx_)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                //
                dane.Add(entry.Value.ToString());
            }
            return dane;
        }
        //
        //Image from site
        public ActionResult ViewImage()
        {
            if (Request.Cookies["Session"] != null)
            {
                List<String> dane = new List<String>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {//pobierz obrazki z Resources
                string directory = AppDomain.CurrentDomain.BaseDirectory+"\\Resources\\Image";
                IEnumerable<string> filenames = System.IO.Directory.EnumerateFiles(directory, "*.*");

              //  String[] filenames = Server.MapPath("/MyWebSite");
              // String[] filenames = CrystalSiege.Resources.
              
                //List<CarouselInfo> slider = contents.CarouselInfo.ToList();
                foreach (String filename in filenames)
                {                    
                    String[] tab = filename.Split('\\');
                    string s = tab[tab.Length - 1];
                    if (s == "haha_zecora.png") { continue; }
                    dane.Add(s);
                }
                ViewBag.Message = dane;
            }
            return View();
            }
            return View("../Home/Index");
        }
        public ActionResult DeleteImage(string name)
        {
            if (Request.Cookies["Session"] != null)
            {
                ViewBag.deleteSuccess = "false";
            var photoName = "";
            photoName = name;
            var fullPath = Path.Combine(Server.MapPath("~/Resources/Image"), photoName);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            ViewImage();
            return View("ViewImage");
        }
           return View("../Home/Index");
    }


        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase file)
        {
          //  tbl_details tbl = new tbl_details();
           // var allowedExtensions = new[] {
          //      ".Jpg", ".png", ".jpg", "jpeg"
          //  };

            if (Request.Cookies["Session"] != null && file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = "";
                    if (Path.GetExtension(file.FileName).ToLower() == ".jpg" ||
                    Path.GetExtension(file.FileName).ToLower() == ".gif" ||
                    Path.GetExtension(file.FileName).ToLower() == ".png" ||
                    Path.GetExtension(file.FileName).ToLower() == ".jpeg")
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        path = Path.Combine(HostingEnvironment.MapPath("~/Resources/Image/"), fileName);
                        file.SaveAs(path);
                        ViewBag.Message = "Załadowano zdjęcie";
                        /*
                        string ImageName = Path.GetFileName(file.FileName);
                        string physicalPath = Server.MapPath("~/Resources/Image/" + ImageName);
                        file.SaveAs(physicalPath);*/
                        /*
                        //   System.IO.File.WriteAllText(path, "Hello World");

                        // extract only the filename
                        var fileName = Path.GetFileName(file.FileName);
                        // store the file inside ~/App_Data/uploads folder
                        var path = Path.Combine(Server.MapPath("~/Resources/Image"), fileName);
                        //  var path = Server.MapPath("~/Resources/Image" + fileName);
                        file.SaveAs(path);//?
                        string fullSavePath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/Platypus{0}.csv", dbContextAsInt));
                        */
                    }
                    else
                    {
                        ViewBag.Message = "nieprawidłowe rozszerzenie";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Błąd:  Data:" + ex.Data.ToString() + "   HelpLink:" + ex.HResult.ToString() + "   Message:" + ex.Message.ToString() + "  Source" + ex.Source.ToString() + "   StackTrace:" + ex.StackTrace.ToString() + "  TargetSite:" + ex.TargetSite.ToString() + " ";
                }
                   
                }
                else
                {
                    ViewBag.Message = "Nie wybrano pliku.";
                }
                // redirect back to the index action to show the form once again             
            // return View("../Home/Index");
            return View();
        }
        public ActionResult AddImage()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
               return View("../Home/Index");
        }
        //
        //Slide from Carousel
        public ActionResult DeleteSlide(int id)
        {
            if (Request.Cookies["Session"] != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                var slide = contents.CarouselInfoes.Where(u => u.Id == id).First();
                contents.CarouselInfoes.Remove(slide);
                contents.SaveChanges();
            }
            ViewSlide();
            return View("ViewSlide");
        }
           return View("../Home/Index");
    }

        public string UploadImage(HttpPostedFileBase file, string img)
        {
                //Upload Image
                if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Resources/Image"),
                                                   Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    ViewBag.Message = "Plik został pomyślnie załadowany";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Błąd:" + ex.Message.ToString();
                }
            String filename = "";
            if (file == null)
            {
                if (img != null)
                {
                    filename = img;
                }
                else
                {
                    filename = "haha_zecora.png";
                }
            }
            else
            {
                filename = file.FileName;
            }
            return filename;
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSlide(SlideViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            if (Request.Cookies["Session"] != null)
            {
                string filename = UploadImage(file,model.img);
            
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    //int idx = contents.CarouselInfo.Count() + 1;
                    int idx = 1;
                    if (contents.CarouselInfoes.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.CarouselInfoes.ToList().LastOrDefault().Id + 1);
                    }
                    string link_ = model.Link;
                    if (link_ != "")
                    {
                        link_ = CoderUTF8.Encode(model.Link);
                    }
                    var customers = contents.Set<CarouselInfo>();
                    customers.Add(new CarouselInfo {
                        Id = idx,
                        Title = CoderUTF8.Encode(model.Title),
                        Description = CoderUTF8.Encode(model.Description),
                        image = filename,//zrobić wybór, albo upload obrazka
                        Link = link_
                    });
                    contents.SaveChanges();
                    ViewSlide();
                    return View("ViewSlide");
                }
            }
            return View("Content");
            }
            return View("../Home/Index");
        }        
        public ActionResult AddSlide()
        {
            if (Request.Cookies["Session"] != null)
            {
                ViewImage();
            return View();
            }
            return View("../Home/Index");
        }
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSlide(SlideViewModel viewModel, HttpPostedFileBase file, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                CarouselInfo slide;
            string filename = UploadImage(file, viewModel.img);
            using (var ctx = new damianlukasik3612_crystalsiegeEntities())
            {
                slide = ctx.CarouselInfoes.Where(s => s.Id == viewModel.Id).FirstOrDefault<CarouselInfo>();
            }
            //problem z kodowaniem
            //
            
            if (slide != null)
            {
                switch (lang)
                {
                    case "pl":
                        slide.Title = CoderUTF8.Encode(viewModel.Title);
                        slide.Description = CoderUTF8.Encode(viewModel.Description);
                        slide.Link = CoderUTF8.Encode(viewModel.Link);
                        break;
                    case "eng":
                        slide.Title_ang = CoderUTF8.Encode(viewModel.Title);
                        slide.Description_ang = CoderUTF8.Encode(viewModel.Description);
                        slide.Link = CoderUTF8.Encode(viewModel.Link);
                        break;
                }
                slide.image = filename;
            }

            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                contents.Entry(slide).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSlide();
            return View("ViewSlide");
            }
            return View("../Home/Index");
        }
        public ActionResult EditSlide(int id, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                CarouselInfo slide = contents.CarouselInfoes
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                switch (lang)
                {
                    case "pl":
                        String[] dane1 = {
                            CoderUTF8.Decode(slide.Title),
                            CoderUTF8.Decode(slide.Description),
                            CoderUTF8.Decode(slide.Link),//[2]
                            slide.image,//[3]
                            slide.Id.ToString(),//[4]
                            lang,//[5]
                        };
                        ViewBag.Message = dane1;
                        break;
                    case "eng":
                        String[] dane2 = {
                            CoderUTF8.Decode(slide.Title_ang),
                            CoderUTF8.Decode(slide.Description_ang),
                            CoderUTF8.Decode(slide.Link),//[2]
                            slide.image,//[3]
                            slide.Id.ToString(),//[4]
                            lang,//[5]
                        };
                        ViewBag.Message = dane2;
                        break;
                }
            }
            return View();
            }
            return View("../Home/Index");
        }
        public ActionResult ViewSlide()
        {
            if (Request.Cookies["Session"] != null)
            {
                List<List<String[]>> dane_ = new List<List<String[]>>();
            List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<CarouselInfo> slider = contents.CarouselInfoes.ToList();
                string[] lang_tab = { "pl", "ang"};
                foreach(string lang in lang_tab)
                {
                    foreach (CarouselInfo slide in slider)
                    {
                        var url_img = FileIsExists(slide.image);
                        switch (lang)
                        {
                            case "pl":
                                String[] str1 = {
                                    url_img,
                                    CoderUTF8.Decode(slide.Title),
                                    slide.Id.ToString()
                                };
                                dane.Add(str1);
                                break;
                            case "ang":
                                String[] str2 = {
                                    url_img,
                                    CoderUTF8.Decode(slide.Title_ang),
                                    slide.Id.ToString()
                                };
                                dane.Add(str2);
                                break;
                        }
                    }
                    dane_.Add(dane);
                    dane = new List<String[]>();
                }   
                ViewBag.Message = dane_;
            }
            return View();
        }
           return View("../Home/Index");
    }
        //
        //Sections
        public ActionResult AddSection()
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                return View();
            }
            return View("../Home/Index");
        }
        public ActionResult AddSubSection(string id)
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                ViewBag.Message = id;
                return View();
            }
               return View("../Home/Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSection(SectionViewModel model, string returnUrl)
        {
            if (Request.Cookies["Session"] != null)
            {
                //
                if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    int idx = 1;
                    if (contents.Sections.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.Sections.ToList().LastOrDefault().Id + 1);
                    }
                    int idx_ = 1;
                    if (contents.Subsections.ToList().Count != 0)
                    {
                        idx_ = Convert.ToInt32(contents.Subsections.ToList().LastOrDefault().Id + 1);
                    }
                    String str_nazwa = "brak nazwy";
                    if (model.title != null)
                    {
                        str_nazwa = model.title;
                    }
                    //   string idx = (contents.Sections.Count()+1)+""+Guid.NewGuid().GetHashCode().ToString();
                    var customers = contents.Set<Section>();
                    customers.Add(new Section
                    {
                        Id = idx,
                        title = CoderUTF8.Encode(str_nazwa),
                        title_ang = CoderUTF8.Encode(str_nazwa)
                    });
                    var customers_ = contents.Set<Subsection>();
                    customers_.Add(new Subsection
                    {
                        Id = idx_,
                        SectionsId = idx,
                        title = CoderUTF8.Encode("nowa podsekcja"),
                        title_ang = CoderUTF8.Encode("new subsection"),
                        content = CoderUTF8.Encode(" "),
                        content_ang = CoderUTF8.Encode(" ")
                    });
                    contents.SaveChanges();
                    ViewSection();
                    return View("ViewSection");
                }
            }
            return View("Content");
            }
            return View("../Home/Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubSection(SubSectionViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            if (Request.Cookies["Session"] != null)
            {
                //
                if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    int idx = 1;
                    if (contents.Subsections.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.Subsections.ToList().LastOrDefault().Id + 1);
                    }
                    String str_nazwa = "brak nazwy";
                    if (model.title != null)
                    {
                        str_nazwa = model.title;
                    }
                    var customers = contents.Set<Subsection>();
                    customers.Add(new Subsection
                    {
                        Id = idx,
                        SectionsId = model.Id,
                        title = CoderUTF8.Encode(model.title),
                        title_ang = CoderUTF8.Encode(model.title),
                        content = CoderUTF8.Encode(model.HtmlContent),
                        content_ang = CoderUTF8.Encode(model.HtmlContent),
                    });
                    contents.SaveChanges();
                    ViewSection();
                    return View("ViewSection");
                }
            }
            return View("Content");
            }
           return View("../Home/Index");
        }
        public ActionResult ViewSection()
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                return View();
            }
            return View("../Home/Index");
        }
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSection(SectionViewModel viewModel, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                Section sec;
            using (var ctx = new damianlukasik3612_crystalsiegeEntities())
            {
                sec = ctx.Sections.Where(s => s.Id == viewModel.Id).FirstOrDefault<Section>();
            }
            if (sec != null)
            {
                switch (lang)
                {
                    case "pl":
                        sec.title = CoderUTF8.Encode(viewModel.title);
                        break;
                    case "en":
                        sec.title_ang = CoderUTF8.Encode(viewModel.title);
                        break;
                }                
            }
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                contents.Entry(sec).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
            }
            return View("../Home/Index");
        }        
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSubSection(SubSectionViewModel viewModel, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                Subsection sec;
            using (var ctx = new damianlukasik3612_crystalsiegeEntities())
            {
                sec = ctx.Subsections.Where(s => s.Id == viewModel.Id).FirstOrDefault<Subsection>();
            }
            if (sec != null)
            {
                switch (lang)
                {
                    case "pl":
                        sec.title = CoderUTF8.Encode(viewModel.title);
                        sec.content = CoderUTF8.Encode(viewModel.HtmlContent);
                        break;
                    case "en":
                        sec.title_ang = CoderUTF8.Encode(viewModel.title);
                        sec.content_ang = CoderUTF8.Encode(viewModel.HtmlContent);
                        break;
                }
            }
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                contents.Entry(sec).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
            }
            return View("../Home/Index");
        }
        public ActionResult EditSection(int id, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                Section sec = contents.Sections
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                switch (lang)
                {
                    case "pl":
                        String[] dane1 = {
                            sec.Id.ToString(),
                            CoderUTF8.Decode(sec.title),
                            "pl"
                        };
                        ViewBag.Message = dane1;
                        break;
                    case "en":
                        String[] dane2 = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title_ang),//[1]
                            "en"//[2]
                        };
                        ViewBag.Message = dane2;
                        break;
                }                
            }
            return View();
            }
            return View("../Home/Index");
        }
        public ActionResult EditSubSection(int id, string lang)
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                Subsection sec = contents.Subsections
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                switch (lang)
                {
                    case "pl":
                        String[] dane1 = {
                            sec.Id.ToString(),
                            CoderUTF8.Decode(sec.title),
                            CoderUTF8.Decode(sec.content),
                            "pl"
                        };
                        ViewBag.Message = dane1;
                        break;
                    case "en":
                        String[] dane2 = {
                            sec.Id.ToString(),//[0]
                            CoderUTF8.Decode(sec.title_ang),//[1]
                            CoderUTF8.Decode(sec.content_ang),//[2]
                            "en"//[3]
                        };
                        ViewBag.Message = dane2;
                        break;
                }
            }
            return View();
            }
            return View("../Home/Index");
        }        
        public ActionResult DeleteSection(int id)
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                Section sec = contents.Sections.Where(u => u.Id == id).First();
                List<Subsection> subsectionList = contents.Subsections.Where(u => u.SectionsId == sec.Id).ToList();
                foreach (Subsection s in subsectionList)
                {
                    contents.Subsections.Remove(s);
                }
                contents.Sections.Remove(sec);
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
            }
            return View("../Home/Index");
        }
        //Subsection
        public ActionResult DeleteSubSection(int id)
        {
            if (Request.Cookies["Session"] != null)
            {
                //ViewSection();
                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                Subsection sec = contents.Subsections.Where(u => u.Id == id).First();                
                contents.Subsections.Remove(sec);
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
            }
            return View("../Home/Index");
        }
                
        ////////////////
        public ActionResult ChangeLogin()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
            return View("../Home/Index");
        }
        public ActionResult ChangeEmail()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
            return View("../Home/Index");
        }
        public ActionResult ChangeAvatar()
        {
            if (Request.Cookies["Session"] != null)
            {
                ViewImage();
                return View();
            }
            return View("../Home/Index");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(ChangeAvatarViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            if (Request.Cookies["Session"] != null)
            {
                string filename = UploadImage(file, model.img);

            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
                {
                    var idek = Request.Cookies["Session"].Value;
                    //
                    // string user = contents.People.Where(u => u.Id == Request.Cookies["Session"].Value).FirstOrDefault().username;
                    Person per = contents.People.Where(u => u.Id == idek).FirstOrDefault<Person>();

                    per.awatar = filename;
                    
                    contents.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    contents.SaveChanges();                    
                }
            }
            ViewImage();
            return View();
            }
            return View("../Home/Index");
        }
        //
        //Get Image Awatar
        public static string Get_Awatar(string n)
        {
            string awatar = "";
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                string idek = n;
                awatar = contents.People.Where(x => x.Id == idek).FirstOrDefault().awatar;
            }
            return awatar;
        }
        //
        //Get Username
        public static string Get_Username(string n)
        {
            string name = "";
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                string idek = n;
                name = contents.People.Where(x => x.Id == idek).FirstOrDefault().username;
            }
            return name;
        }
        //
        //Get Images
        public static List<String> Get_ImagesList()
        {
            List<String> dane = new List<String>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {//pobierz obrazki z Resources
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
        //
        //Get Tags
        public static List<String[]> Get_TagsList()
        {
            List<String[]> dane = new List<String[]>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {//pobierz obrazki z Resources
                List<Tag> tagi = contents.Tags.ToList();

                foreach (Tag tag in tagi)
                {
                    String[] s = { tag.tags_pl , tag.color };
                    dane.Add(s);
                }
            }
            return dane;
        }
        //
        //Get Tags
        public static List<List<String[]>> Get_NewsList()
        {
            List<List<String[]>> dane = new List<List<String[]>>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();
                                
                String[] lang_tab = { "pl", "en" };                
                foreach (String lang in lang_tab)
                {
                    List<String[]> dane_ = new List<String[]>();
                    foreach (News wart in news)
                    {
                        List<News_Tags> news_tags = wart.News_Tags.ToList();
                        String tagi = "";
                        foreach (News_Tags nt in news_tags)
                        {
                            tagi += " <span class='label label-success' style='background-color: " + nt.Tag.color + "'>" + nt.Tag.tags_pl + "</span> ";
                        }
                        //  String url_ = //metoda statyczna sprawdzająca czy plik istnieje
                        switch (lang)
                        {
                            case "pl":
                                String[] str1 = {
                                    CoderUTF8.Decode(wart.title),
                                    wart.image,
                                    new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                    System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                                    wart.author,
                                    wart.Id.ToString(),
                                    "pl"
                                };
                                dane_.Add(str1);
                                break;
                            case "en":
                                String[] str2 = {
                                    CoderUTF8.Decode(wart.title_eng),
                                    wart.image,
                                    new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                                    System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                                    wart.author,
                                    wart.Id.ToString(),
                                    "en"
                                };
                                dane_.Add(str2);
                                break;
                        }                         
                    }
                    dane.Add(dane_);
                }
            }
            return dane;
        }
        //
        //Get Sections and subsections
        public static List<List<List<String[]>>> Get_Sections_and_subsections()
        {
            List<List<List<String[]>>> dane__ = new List<List<List<String[]>>>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                string[] lang_tab = { "pl", "en" };
                foreach (string lang in lang_tab)
                { 
                    List<List<String[]>> dane = new List<List<String[]>>();
                    List<Section> sect_list = contents.Sections.ToList();
                    foreach (Section sect in sect_list)
                    {
                        List<String[]> dane_ = new List<String[]>();
                        if (sect.Subsections.Count != 0)
                        {
                            List<Subsection> subsect_list = sect.Subsections.ToList();
                            foreach (Subsection subsect in subsect_list)
                            {
                                switch (lang)
                                {
                                    case "pl":
                                        String[] str1 = {
                                            CoderUTF8.Decode(sect.title),//[0]
                                            CoderUTF8.Decode(subsect.title),//[1]
                                            CoderUTF8.Decode(subsect.content),//[2]
                                            subsect.Id.ToString(),//[3]
                                            subsect.SectionsId.ToString(),//[4]
                                            lang,//[5]
                                            sect.Id.ToString(),//[6]
                                            subsect.Id.ToString()//[7]
                                        };
                                        dane_.Add(str1);
                                        break;
                                    case "en":
                                        String[] str2 = {
                                            CoderUTF8.Decode(sect.title_ang),//[0]
                                            CoderUTF8.Decode(subsect.title_ang),//[1]
                                            CoderUTF8.Decode(subsect.content_ang),//[2]
                                            subsect.Id.ToString(),//[3]
                                            subsect.SectionsId.ToString(),//[4]
                                            lang,//[5]
                                            sect.Id.ToString(),//[6]
                                            subsect.Id.ToString()//[7]
                                        };
                                        dane_.Add(str2);
                                        break;
                                }                                
                            }
                        }
                        else
                        {
                            switch (lang)
                            {
                                case "pl":
                                    String[] str3 = {
                                        CoderUTF8.Decode(sect.title),//[0]
                                        lang,//[1]
                                        sect.Id.ToString()//[2]
                                    };
                                    dane_.Add(str3);
                                    break;
                                case "en":
                                    String[] str4 = {
                                        CoderUTF8.Decode(sect.title_ang),//[0]
                                        lang,//[1]
                                        sect.Id.ToString()//[2]
                                    };
                                    dane_.Add(str4);
                                    break;
                            }           
                        }
                        dane.Add(dane_);
                    }
                    dane__.Add(dane);
                }
            }
            return dane__;
        }
        public static List<List<String[]>> Get_Sections()
        {
            string[] lang_tab = { "pl", "en" };
            List<List<String[]>> dane_ = new List<List<String[]>>();
            using (damianlukasik3612_crystalsiegeEntities contents = new damianlukasik3612_crystalsiegeEntities())
            {
                List<Section> sect_list = contents.Sections.ToList();
                foreach (string lang in lang_tab)
                {
                    List<String[]> dane = new List<String[]>();
                    foreach (Section sect in sect_list)
                    {
                        switch (lang)
                        {
                            case "pl":
                                String[] str1 = {
                                    CoderUTF8.Decode(sect.title),//[0]
                                    sect.Id.ToString(),//[1]
                                    lang,//[2]
                                    sect.Id.ToString()//[3]
                                };
                                dane.Add(str1);
                                break;
                            case "en":
                                String[] str2 = {
                                    CoderUTF8.Decode(sect.title_ang),//[0]
                                    sect.Id.ToString(),//[1]
                                    lang,//[2]
                                    sect.Id.ToString()//[3]
                                };
                                dane.Add(str2);
                                break;
                        }
                    }
                    dane_.Add(dane);
                }
            }
            return dane_;
        }
        //
        public ActionResult Content()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
               return View("../Home/Index");
        }

        //
        public static ResourceManager Get_Resources()
        {
            ResourceManager rm = new ResourceManager("CrystalSiege.Resources.HomeTexts",
                Assembly.GetExecutingAssembly());
         //   ResourceSet temp = rm.GetResourceSet(new System.Globalization.CultureInfo(lang), true, false);
            return rm;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            if (Request.Cookies["Session"] != null)
            {
                ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";
                /*
                var userId = User.Identity.GetUserId();
                var model = new IndexViewModel
                {
                    HasPassword = HasPassword(),
                    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                    Logins = await UserManager.GetLoginsAsync(userId),
                    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
                };*/

                var model = new IndexViewModel
                {
                    HasPassword = HasPassword()
                };

                switch (Resources.HomeTexts.lang_set)
                {
                    case "pl":
                        ViewBag.Title = "Profil";
                        break;
                    case "en":
                        ViewBag.Title = "Profile";
                        break;
                }               
                return View(model);
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            if (Request.Cookies["Session"] != null)
            {
                ManageMessageId? message;
                var result = await UserManager.RemoveLoginAsync(Request.Cookies["Session"].Value, new UserLoginInfo(loginProvider, providerKey));
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    message = ManageMessageId.RemoveLoginSuccess;
                }
                else
                {
                    message = ManageMessageId.Error;
                }
                return RedirectToAction("ManageLogins", new { Message = message });
                }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {           
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(Request.Cookies["Session"].Value, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            if (Request.Cookies["Session"] != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(Request.Cookies["Session"].Value, true);
                var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Manage");
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            if (Request.Cookies["Session"] != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(Request.Cookies["Session"].Value, false);
            var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
            }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            if (Request.Cookies["Session"] != null)
            {
                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(Request.Cookies["Session"].Value, phoneNumber);
                // Send an SMS through the SMS provider to verify the phone number
                return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var result = await UserManager.ChangePhoneNumberAsync(Request.Cookies["Session"].Value,model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
                }
                // If we got this far, something failed, redisplay form
                ModelState.AddModelError("", "Failed to verify phone");
                return View(model);
            }
           return View("../Home/Index");
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            if (Request.Cookies["Session"] != null)
            {
                var result = await UserManager.SetPhoneNumberAsync(Request.Cookies["Session"].Value, null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
            }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
               return View("../Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                /*
                var user = UserManager.FindById(Request.Cookies["Session"].Value);
                user.Email = model.NewEmail;
                var updateResult = await UserManager.UpdateAsync(user);
                if (updateResult.Succeeded)
                {
                    ViewBag.Message = "Adres e-mail został zmieniony";
                }
                else
                {
                    ViewBag.Message = "Adres e-mail nie został zmieniony";
                    return View();
                }*/
                using (var ctx = new damianlukasik3612_crystalsiegeEntities())
                {
                    var idek = Request.Cookies["Session"].Value;
                   // var user = UserManager.FindById(idek);
                    Person per = ctx.People.Where(s => s.Id == idek).FirstOrDefault<Person>();
                    if (per != null)
                    {
                        per.email = model.NewEmail;
                        ViewBag.Message = "Adres e-mail został zmieniony";
                    }
                    else
                    {
                        ViewBag.Message = "Adres e-mail nie został zmieniony";
                    }
                    ctx.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                return View(model);
            }
            return View("../Home/Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeLogin(ChangeLoginViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                string idek = Request.Cookies["Session"].Value;
                using (damianlukasik3612_crystalsiegeEntities ctx = new damianlukasik3612_crystalsiegeEntities())
                {
                    Person per = ctx.People.Where(s => s.Id == idek).FirstOrDefault<Person>();
                    if (per != null)
                    {
                        List<News> nes = ctx.News.Where(s => s.author == per.username).ToList();
                        per.username = model.NewLogin;
                        foreach (News ne in nes)
                        {
                            ne.author = per.username;
                            ViewBag.Message = "Login został zmieniony";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Login nie został zmieniony";
                        return View();
                    }
                    ctx.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();                    
                    return View(model);                         
                }                
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                // model.OldPassword = AccountController.Decrypt(model.OldPassword);
                if (!ModelState.IsValid)
                {
                    return View(model);
                }               
                using (var ctx = new damianlukasik3612_crystalsiegeEntities())
                {
                    var idek = Request.Cookies["Session"].Value;
                   // var user = UserManager.FindById(idek);
                    Person per = ctx.People.Where(s => s.Id == idek).FirstOrDefault<Person>();
                    if (per != null)
                    {
                        per.password = model.ConfirmPassword;
                        ViewBag.Message = "Hasło zostało zmienione";
                    }
                    else
                    {
                        ViewBag.Message = "Hasło nie zostało zmienione";
                    }
                    ctx.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
              //  AddErrors(result);
                return View(model);
            }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            if (Request.Cookies["Session"] != null)
            {
                return View();
            }
               return View("../Home/Index");
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (Request.Cookies["Session"] != null)
            {
                if (ModelState.IsValid)
                {
                   /* var result = await UserManager.AddPasswordAsync(Request.Cookies["Session"].Value, model.NewPassword);
                    if (result.Succeeded)
                    {*/
                        var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
                        if (user != null)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        }
                        return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                  //  }
                  //  AddErrors(result);
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {

            if (Request.Cookies["Session"] != null)
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                    : message == ManageMessageId.Error ? "An error has occurred."
                    : "";
                var user = await UserManager.FindByIdAsync(Request.Cookies["Session"].Value);
                if (user == null)
                {
                    return View("Error");
                }
                var userLogins = await UserManager.GetLoginsAsync(Request.Cookies["Session"].Value);
                var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
                ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
                return View(new ManageLoginsViewModel
                {
                    CurrentLogins = userLogins,
                    OtherLogins = otherLogins
                });
            }
            return View("../Home/Index");
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {

            if (Request.Cookies["Session"] != null)
            {
                // Request a redirect to the external login provider to link a login for the current user
                return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), Request.Cookies["Session"].Value);
            }
            return View("../Home/Index");
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            if (Request.Cookies["Session"] != null)
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, Request.Cookies["Session"].Value);
                if (loginInfo == null)
                {
                    return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
                }
                var result = await UserManager.AddLoginAsync(Request.Cookies["Session"].Value, loginInfo.Login);
                return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            return View("../Home/Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;               
            }

            base.Dispose(disposing);
           // View("../Shared/Error");
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            /*
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }*/
            if (Request.Cookies["Session"] != null)
            {
                string id = Request.Cookies["Session"].Value;
                using (damianlukasik3612_crystalsiegeEntities content = new damianlukasik3612_crystalsiegeEntities())
                {
                    Person per = content.People.Where(u => u.Id == id).FirstOrDefault<Person>();
                    if (per != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(Request.Cookies["Session"].Value);
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}