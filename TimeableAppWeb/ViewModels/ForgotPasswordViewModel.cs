using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(Email), Prompt = nameof(Email), ResourceType = typeof(Resources.Views.Account.Account))]
        public string Email { get; set; } = default!;

        public bool LinkSent { get; set; }
    }
}
