using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StorageManagement_MVC.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter username")]
        public string Username {  get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password {  get; set; }
    }
}
