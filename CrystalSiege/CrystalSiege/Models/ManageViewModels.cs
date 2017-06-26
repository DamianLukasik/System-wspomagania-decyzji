using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;

namespace CrystalSiege.Models
{
    //
    public class LocalizedDisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
    {
        public LocalizedDisplayNameAttribute(string resourceId)
            : base(GetMessageFromResource(resourceId))
        {   }

        private static string GetMessageFromResource(string resourceId)
        {
            string str = "";
            switch (resourceId)
            {
                case "SelectGraphic":
                    str = Resources.HomeTexts.SelectGraphic;
                    break;
                case "NewPassword":
                    str = Resources.HomeTexts.NewPassword;
                    break;
                case "ConfirmNewPassword":
                    str = Resources.HomeTexts.ConfirmNewPassword;
                    break;
                case "NewLogin":
                    str = Resources.HomeTexts.NewLogin;
                    break;
                case "NewEmail":
                    str = Resources.HomeTexts.NewEmail;
                    break;
                case "ActualPassword":
                    str = Resources.HomeTexts.ActualPassword;
                    break;
                case "Code":
                    str = Resources.HomeTexts.Code;
                    break;
                case "PhoneNumber":
                    str = Resources.HomeTexts.PhoneNumber;
                    break;
            }
            return str;
        }
    }
    //
    public class FileUploadModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "~/Resources/Image")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string file { get; set; }
    }
    public class BaseModel
    {
        [Required]
        [LocalizedDisplayName("SelectGraphic")]
        public string img { get; set; }        
    }
    public class ChangeAvatarViewModel : BaseModel
    {
        
    }
    public class TranslateViewModel
    {
        public List<RawTranslateViewModel> Collection { get; set; }
    }
    public class RawTranslateViewModel
    {
        public string Name_Oryginal { get; set; }
        public string Name_ang { get; set; }
        public string Name_pl { get; set; }
    }

    public class SlideViewModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class SectionViewModel
    {
        public int Id { get; set; }
        public string title { get; set; }
    }

    public class SubSectionViewModel : SectionViewModel
    {
        [AllowHtml]
        public string HtmlContent { get; set; }
    }

    public class NewsViewModel : BaseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Tags { get; set; }
        //public List<System.String> Tags { get; set; }
    }

    ///////////////////
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać conajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("ConfirmNewPassword")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeLoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("NewLogin")]
        public string NewLogin { get; set; }     
    }
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [LocalizedDisplayName("NewEmail")]
        public string NewEmail { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("ActualPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać conajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [LocalizedDisplayName("NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [LocalizedDisplayName("ConfirmNewPassword")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [LocalizedDisplayName("PhoneNumber")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [LocalizedDisplayName("Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [LocalizedDisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}