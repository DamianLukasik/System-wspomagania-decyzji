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

namespace CrystalSiege.Controllers
{
    [Authorize]
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
            using (ContentsEntities contents = new ContentsEntities())
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
        public ActionResult ViewNews()
        {
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();
                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: " + nt.Tags.color + "'>" + nt.Tags.tags_pl + "</span> ";
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
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNews(NewsViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            string filename = UploadImage(file, model.img);
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (ContentsEntities contents = new ContentsEntities())
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
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
                            author = user.UserName,
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
                            author = user.UserName,
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
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditNews(NewsViewModel viewModel)
        {
            News news;
            using (var ctx = new ContentsEntities())
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
                
                string[] tab_tags = viewModel.Tags.Split('|');
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
                //
                if (news != null)
                {
                    news.title = CoderUTF8.Encode(viewModel.Title);
                    news.description = CoderUTF8.Encode(viewModel.Description);
                    news.image = FileIsExists(viewModel.img);
                    news.News_Tags = news_tags;
                }
                ctx.Entry(news).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
            ViewNews();
            return View("ViewNews");
        }
        public ActionResult EditNews(int id)
        {
            using (ContentsEntities contents = new ContentsEntities())
            {
                News news = contents.News
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                string tags = "";
                List<News_Tags> news_tags = news.News_Tags.ToList();
                foreach (News_Tags nt in news_tags)
                {
                    tags += nt.Tags.tags_pl+"?"+nt.Tags.color+"|";
                }

                String[] dane = {
                    CoderUTF8.Decode(news.title),//[0]
                    CoderUTF8.Decode(news.description),//[1]
                    new ContentsController().DecodeDate(news.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),//[2]
                    news.image,//[3]
                    news.Id.ToString(),//[4]
                    tags//[5]
                };
                ViewBag.Message = dane;
            }
            return View();
        }
        
        //
        //Translate Manager
        public ActionResult ViewTranslate()
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
        public ActionResult UpdateTranslate(TranslateViewModel model)
        {
            var ctr = model.Collection.Count(x => x.Name_Oryginal != null);

            var fullPath = Path.Combine(Server.MapPath("~/Resources"), "ContentTexts.resx");

            XmlDocument doc = new XmlDocument();
            doc.Load(fullPath);

            // XmlNamespaceManager manager = new XmlNamespaceManager(navigator.NameTable);
            // manager.AddNamespace("bk", "http://www.contoso.com/books");

            XmlNodeList aNodes = doc.SelectNodes("/root/data");

            for (int i = 0; i < model.Collection.Count; i++)
            {
                XmlNode Node = aNodes[i].SelectSingleNode("value");
                Node.InnerText = model.Collection[i].Name_ang;
            }


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
        private List<String> LoadAllResources(string lang)
        {
            ResourceSet resx_ = CrystalSiege.Resources.ContentTexts.ResourceManager.GetResourceSet(
                System.Globalization.CultureInfo.CreateSpecificCulture(lang), true, true);
            List<String> dane = new List<String>();
            foreach (System.Collections.DictionaryEntry entry in resx_)
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
            List<String> dane = new List<String>();
            using (ContentsEntities contents = new ContentsEntities())
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
        public ActionResult DeleteImage(string name)
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
        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase file)
        {
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
            else
            {
                ViewBag.Message = "Nie wybrano pliku.";
            }
            return View();
        }
        public ActionResult AddImage()
        {
            return View();
        }
        //
        //Slide from Carousel
        public ActionResult DeleteSlide(int id)
        {
            using (ContentsEntities contents = new ContentsEntities())
            {
                var slide = contents.CarouselInfo.Where(u => u.Id == id).First();
                contents.CarouselInfo.Remove(slide);
                contents.SaveChanges();
            }
            ViewSlide();
            return View("ViewSlide");
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
            string filename = UploadImage(file,model.img);
            
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (ContentsEntities contents = new ContentsEntities())
                {
                    //int idx = contents.CarouselInfo.Count() + 1;
                    int idx = 1;
                    if (contents.CarouselInfo.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.CarouselInfo.ToList().LastOrDefault().Id + 1);
                    }
                    var customers = contents.Set<CarouselInfo>();
                    customers.Add(new CarouselInfo {
                        Id = idx,
                        Title = CoderUTF8.Encode(model.Title),
                        Description = CoderUTF8.Encode(model.Description),
                        image = filename,//zrobić wybór, albo upload obrazka
                        Link = ""
                    });
                    contents.SaveChanges();
                    ViewSlide();
                    return View("ViewSlide");
                }
            }
            return View("Content");
        }        
        public ActionResult AddSlide()
        {
            ViewImage();
            return View();
        }
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSlide(SlideViewModel viewModel, HttpPostedFileBase file)
        {
            CarouselInfo slide;
            string filename = UploadImage(file, viewModel.img);
            using (var ctx = new ContentsEntities())
            {
                slide = ctx.CarouselInfo.Where(s => s.Id == viewModel.Id).FirstOrDefault<CarouselInfo>();
            }
            //problem z kodowaniem
            //
            
            if (slide != null)
            {
                slide.Title = CoderUTF8.Encode(viewModel.Title);
                slide.Description = CoderUTF8.Encode(viewModel.Description);

                slide.image = filename;
            }

            using (ContentsEntities contents = new ContentsEntities())
            {
                contents.Entry(slide).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSlide();
            return View("ViewSlide");
        }
        public ActionResult EditSlide(int id)
        {            
            using (ContentsEntities contents = new ContentsEntities())
            {
                CarouselInfo slide = contents.CarouselInfo
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                String[] dane = {
                    CoderUTF8.Decode(slide.Title),
                    CoderUTF8.Decode(slide.Description),
                    slide.Link,//[2]
                    slide.image,//[3]
                    slide.Id.ToString()//[4]
                };
                ViewBag.Message = dane;
            }
            return View();
        }
        public ActionResult ViewSlide()
        {
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<CarouselInfo> slider = contents.CarouselInfo.ToList();               
                foreach (CarouselInfo slide in slider)
                {
                    var url_img = FileIsExists(slide.image);
                    String[] str = {
                        url_img,
                        CoderUTF8.Decode(slide.Title),
                        slide.Id.ToString()
                    };
                    dane.Add(str);
                }
                ViewBag.Message = dane;
            }
            return View();
        }
        //
        //Sections
        public ActionResult AddSection()
        {
            //ViewSection();
            return View();
        }
        public ActionResult AddSubSection(string id)
        {
            //ViewSection();
            ViewBag.Message = id;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSection(SectionViewModel model, string returnUrl)
        {            
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (ContentsEntities contents = new ContentsEntities())
                {
                    int idx = 1;
                    if (contents.Sections.ToList().Count != 0)
                    {
                        idx = Convert.ToInt32(contents.Sections.ToList().LastOrDefault().Id + 1);
                    }
                    String str_nazwa = "brak nazwy";
                    if (model.title != null)
                    {
                        str_nazwa = model.title;
                    }
                    //   string idx = (contents.Sections.Count()+1)+""+Guid.NewGuid().GetHashCode().ToString();
                    var customers = contents.Set<Sections>();
                    customers.Add(new Sections
                    {
                        Id = idx,
                        title = CoderUTF8.Encode(str_nazwa),
                    });
                    contents.SaveChanges();
                    ViewSection();
                    return View("ViewSection");
                }
            }
            return View("Content");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddSubSection(SubSectionViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (ContentsEntities contents = new ContentsEntities())
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
                    var customers = contents.Set<Subsections>();
                    customers.Add(new Subsections
                    {
                        Id = idx,
                        SectionsId = model.Id,
                        title = CoderUTF8.Encode(str_nazwa),
                        content = CoderUTF8.Encode(model.HtmlContent),
                    });
                    contents.SaveChanges();
                    ViewSection();
                    return View("ViewSection");
                }
            }
            return View("Content");
        }
        public ActionResult ViewSection()
        {
            //ViewSection();
            return View();
        }
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSection(SectionViewModel viewModel)
        {
            Sections sec;
            using (var ctx = new ContentsEntities())
            {
                sec = ctx.Sections.Where(s => s.Id == viewModel.Id).FirstOrDefault<Sections>();
            }
            if (sec != null)
            {
                sec.title = CoderUTF8.Encode(viewModel.title);
            }
            using (ContentsEntities contents = new ContentsEntities())
            {
                contents.Entry(sec).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
        }        
        [HttpPost] // this action takes the viewModel from the modal
        public ActionResult EditSubSection(SubSectionViewModel viewModel)
        {
            Subsections sec;
            using (var ctx = new ContentsEntities())
            {
                sec = ctx.Subsections.Where(s => s.Id == viewModel.Id).FirstOrDefault<Subsections>();
            }
            if (sec != null)
            {
                sec.title = CoderUTF8.Encode(viewModel.title);
                sec.content = CoderUTF8.Encode(viewModel.HtmlContent);
            }
            using (ContentsEntities contents = new ContentsEntities())
            {
                contents.Entry(sec).State = System.Data.Entity.EntityState.Modified;
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
        }
        public ActionResult EditSection(int id)
        {
            //ViewSection();
            using (ContentsEntities contents = new ContentsEntities())
            {
                Sections sec = contents.Sections
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                String[] dane = {
                    sec.Id.ToString(),
                    CoderUTF8.Decode(sec.title)
                };
                ViewBag.Message = dane;
            }
            return View();
        }
        public ActionResult EditSubSection(int id)
        {
            //ViewSection();
            using (ContentsEntities contents = new ContentsEntities())
            {
                Subsections sec = contents.Subsections
                         .Where(u => u.Id == id)
                         .FirstOrDefault();
                String[] dane = {
                    sec.Id.ToString(),
                    CoderUTF8.Decode(sec.title),
                    CoderUTF8.Decode(sec.content)
                };
                ViewBag.Message = dane;
            }
            return View();
        }        
        public ActionResult DeleteSection(int id)
        {
            //ViewSection();
            using (ContentsEntities contents = new ContentsEntities())
            {
                Sections sec = contents.Sections.Where(u => u.Id == id).First();
                List<Subsections> subsectionList = contents.Subsections.Where(u => u.SectionsId == sec.Id).ToList();
                foreach (Subsections s in subsectionList)
                {
                    contents.Subsections.Remove(s);
                }
                contents.Sections.Remove(sec);
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
        }
        //Subsection
        public ActionResult DeleteSubSection(int id)
        {
            //ViewSection();
            using (ContentsEntities contents = new ContentsEntities())
            {
                Subsections sec = contents.Subsections.Where(u => u.Id == id).First();                
                contents.Subsections.Remove(sec);
                contents.SaveChanges();
            }
            ViewSection();
            return View("ViewSection");
        }
                
        ////////////////
        public ActionResult ChangeLogin()
        {
            return View();
        }
        public ActionResult ChangeEmail()
        {
            return View();
        }
        public ActionResult ChangeAvatar()
        {
            ViewImage();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(ChangeAvatarViewModel model, string returnUrl, HttpPostedFileBase file)
        {
            string filename = UploadImage(file, model.img);

            //
            if (model != null)//Walidacja && ModelState.IsValid)
            {
                int id = 0;

                using (ContentsEntities contents = new ContentsEntities())
                {
                    //

                    string user = UserManager.FindById(User.Identity.GetUserId()).UserName;
                    Person per = contents.Person.Where(u => u.username == user).FirstOrDefault<Person>();

                    per.awatar = filename;
                    
                    contents.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    contents.SaveChanges();                    
                }
            }
            ViewImage();
            return View();
        }
        //
        //Get Image Awatar
        public static string Get_Awatar(string n)
        {
            string awatar = "";
            using (ContentsEntities contents = new ContentsEntities())
            {
                string idek = n;
                awatar = contents.Person.Where(x => x.Id == idek).FirstOrDefault().awatar;
            }
            return awatar;
        }
        //
        //Get Username
        public static string Get_Username(string n)
        {
            string name = "";
            using (ContentsEntities contents = new ContentsEntities())
            {
                string idek = n;
                name = contents.Person.Where(x => x.Id == idek).FirstOrDefault().username;
            }
            return name;
        }
        //
        //Get Images
        public static List<String> Get_ImagesList()
        {
            List<String> dane = new List<String>();
            using (ContentsEntities contents = new ContentsEntities())
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
            using (ContentsEntities contents = new ContentsEntities())
            {//pobierz obrazki z Resources
                List<Tags> tagi = contents.Tags.ToList();

                foreach (Tags tag in tagi)
                {
                    String[] s = { tag.tags_pl , tag.color };
                    dane.Add(s);
                }
            }
            return dane;
        }
        //
        //Get Tags
        public static List<String[]> Get_NewsList()
        {
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<News> news = contents.News.OrderByDescending(s => s.date).ToList();

                foreach (News wart in news)
                {
                    List<News_Tags> news_tags = wart.News_Tags.ToList();
                    String tagi = "";
                    foreach (News_Tags nt in news_tags)
                    {
                        tagi += " <span class='label label-success' style='background-color: " + nt.Tags.color + "'>" + nt.Tags.tags_pl + "</span> ";
                    }

                  //  String url_ = //metoda statyczna sprawdzająca czy plik istnieje

                    String[] str = {
                        CoderUTF8.Decode(wart.title),
                        wart.image,
                        new ContentsController().DecodeDate(wart.date.Value.ToString("G",
                        System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))),
                        wart.author,
                        wart.Id.ToString()
                    };
                    dane.Add(str);
                }
            }
            return dane;
        }
        //
        //Get Sections and subsections
        public static List<List<String[]>> Get_Sections_and_subsections()
        {
            List<List<String[]>> dane = new List<List<String[]>>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<Sections> sect_list = contents.Sections.ToList();                
                foreach (Sections sect in sect_list)
                {
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
                                subsect.Id.ToString(),//[3]
                                subsect.SectionsId.ToString()//[4]
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
                    }                    
                    dane.Add(dane_);           
                }
            }
            return dane;
        }
        public static List<String[]> Get_Sections()
        {
            List<String[]> dane = new List<String[]>();
            using (ContentsEntities contents = new ContentsEntities())
            {
                List<Sections> sect_list = contents.Sections.ToList();
                foreach (Sections sect in sect_list)
                {
                    String[] str = {
                        CoderUTF8.Decode(sect.title),//[0]
                        sect.Id.ToString()//[1]
                    };
                    dane.Add(str);
                }
            }
            return dane;
        }
        //
        public ActionResult Content()
        {
            return View();
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
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

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
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

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
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

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
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
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeLogin(ChangeLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string idek = User.Identity.GetUserId();
            var user = UserManager.FindById(idek);
            user.UserName = model.NewLogin;
            var updateResult = await UserManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                using (var ctx = new ContentsEntities())
                {
                    Person per = ctx.Person.Where(s => s.Id == idek ).FirstOrDefault<Person>();                
                    if (per != null)
                    {
                        per.username = model.NewLogin;
                    }                
                    ctx.Entry(per).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                }
                ViewBag.Message = "Login został zmieniony";
            }
            else
            {
                ViewBag.Message = "Login nie został zmieniony";
                return View();
            }
            return View(model);
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
           // model.OldPassword = AccountController.Decrypt(model.OldPassword);

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            using (var ctx = new ContentsEntities())
            {
                var idek = User.Identity.GetUserId();
                var user = UserManager.FindById(idek);
                Person per = ctx.Person.Where(s => s.Id == idek).FirstOrDefault<Person>();
                if (per != null)
                {
                    per.password = model.ConfirmPassword;
                }
                ctx.Entry(per).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;               
            }

            base.Dispose(disposing);
           // View("~/Shared/Error");
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
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