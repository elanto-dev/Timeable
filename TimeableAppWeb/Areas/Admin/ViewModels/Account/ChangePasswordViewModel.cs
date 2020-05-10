using System.ComponentModel.DataAnnotations;

namespace TimeableAppWeb.Areas.Admin.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [StringLength(100, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Resources.Views.Common), MinimumLength = 10)]
        [Display(Name = nameof(OldPassword), Prompt = nameof(OldPassword), ResourceType = typeof(Resources.Views.Account.Account))]
        public string OldPassword { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [StringLength(100, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Resources.Views.Common), MinimumLength = 10)]
        [Display(Name = nameof(Password), Prompt = nameof(Password), ResourceType = typeof(Resources.Views.Account.Account))]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Compare("Password", ErrorMessageResourceName = "ErrorMessage_PasswordDontMatch", ErrorMessageResourceType = typeof(Resources.Views.Common))]
        [Display(Name = nameof(ConfirmPassword), Prompt = nameof(ConfirmPassword), ResourceType = typeof(Resources.Views.Account.Account))]
        public string ConfirmPassword { get; set; } = default!;

        [Display(Name = nameof(PasswordSuccessfullyChanged), Prompt = nameof(PasswordSuccessfullyChanged), ResourceType = typeof(Resources.Views.Account.Account))]
        public bool PasswordSuccessfullyChanged { get; set; }
    }
}
