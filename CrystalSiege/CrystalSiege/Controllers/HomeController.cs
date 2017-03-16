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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Secure()
        {
            return View("Home/Index");
        }


        public ActionResult Secure(string id)
        {
            if (id != null)
            {
                using (ContentsEntities contents = new ContentsEntities())
                {
                    id = CoderUTF8.Encode(id);
                    Secure idx = contents.Secure.Where(u => u.link == id).FirstOrDefault();
                    Person per = contents.Person.Where(u => u.access == 1).FirstOrDefault();
                    if (idx != null)
                    {
                        ViewBag.Title = "Twoje dane logowania";
                        ViewBag.Message = "<div><dl class='dl-horizontal'></dl><dt>Login:</dt><dd>" + per.username + "</dd><dt>Hasło:</dt><dd>" + per.password + "</dd></div>";
                    }
                    else
                    {
                        ViewBag.Title = "Link do podstrony odzyskiwania hasła nieistnieje";
                        ViewBag.Message = "<div class='container body-content'><br><img class='media-object img-rounded' src='../Resources/Image/haha_zecora.png' width='210px' height='210px'><br>";
                    }
                    var t = contents.Secure.Where(u => u.link == id).First();
                    contents.Secure.Remove(t);
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