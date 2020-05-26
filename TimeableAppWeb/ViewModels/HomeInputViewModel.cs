using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeableAppWeb.ViewModels
{
    public class HomeInputViewModel
    {
        public bool NoActiveScreen { get; set; }
        public bool PasswordChanged { get; set; }
        public bool UserActivated { get; set; }
        public bool ShowUserNotActive { get; set; }

        [EmailAddress]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Email), Prompt = nameof(Email), ResourceType = typeof(Resources.Views.Account.Account))]
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Password), Prompt = nameof(Password), ResourceType = typeof(Resources.Views.Account.Account))]
        public string Password { get; set; } = default!;
    }
}
