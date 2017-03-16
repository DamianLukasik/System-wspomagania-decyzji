using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;

namespace CrystalSiege.Models
{
    public class BaseModel
    {
        [Display(Name = "Wybierz grafikę")]
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
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeLoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nowy login")]
        public string NewLogin { get; set; }        
    }
    public class ChangeEmailViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Nowy Adres E-mail")]
        public string NewEmail { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Aktualne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać conajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessage = "Hasła muszą być takie same")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}