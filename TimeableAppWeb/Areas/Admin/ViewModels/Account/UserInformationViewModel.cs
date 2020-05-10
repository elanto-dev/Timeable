using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.Account
{
    public class UserInformationViewModel
    {
        [EmailAddress]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Email), Prompt = nameof(Email), ResourceType = typeof(Resources.Views.Account.Account))]
        public string Email { get; set; } = default!;

        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(FirstName), Prompt = nameof(FirstName), ResourceType = typeof(Resources.Views.Account.Account))]
        public string FirstName { get; set; } = default!;

        [MinLength(1, ErrorMessageResourceName = "ErrorMessage_MinLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [MaxLength(100, ErrorMessageResourceName = "ErrorMessage_MaxLength", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(LastName), Prompt = nameof(LastName), ResourceType = typeof(Resources.Views.Account.Account))]
        public string LastName { get; set; } = default!;

        [Display(Name = nameof(InformationSaved), Prompt = nameof(InformationSaved), ResourceType = typeof(Resources.Views.Account.Account))]
        public bool InformationSaved { get; set; }
    }
}
