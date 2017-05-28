using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace CrystalSiege.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Change(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            cookie.Expires = DateTime.Now.AddDays(2d);
            Response.SetCookie(cookie);
            //    Response.Redirect(Request.RawUrl);

            //   string a = Thread.CurrentThread.CurrentCulture.Name.Substring(0,2);


        //    Models.ChangeLoginViewModel.SetLogin(); na później

/*
            Models.BaseModel.img = Resources.HomeTexts.SelectGraphic;
            Models.SetPasswordViewModel.ConfirmPassword = Resources.HomeTexts.ConfirmNewPassword;
            Models.SetPasswordViewModel.NewPassword = Resources.HomeTexts.NewPassword;
            Models.ChangeLoginViewModel.NewLogin = Resources.HomeTexts.NewLogin;
            Models.ChangeEmailViewModel.NewEmail = Resources.HomeTexts.NewEmail;
            Models.ChangePasswordViewModel.NewPassword = Resources.HomeTexts.NewPassword;
            Models.ChangePasswordViewModel.ConfirmPassword = Resources.HomeTexts.ConfirmNewPassword;
            Models.ChangePasswordViewModel.OldPassword = Resources.HomeTexts.ActualPassword;
            Models.AddPhoneNumberViewModel.Number = Resources.HomeTexts.PhoneNumber;
            Models.VerifyPhoneNumberViewModel.PhoneNumber = Resources.HomeTexts.PhoneNumber;
            Models.VerifyPhoneNumberViewModel.Code = Resources.HomeTexts.Code;*/

            return View("~/Views/Home/Index.cshtml");
        }        
    }
}