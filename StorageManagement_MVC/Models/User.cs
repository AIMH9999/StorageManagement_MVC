using System.ComponentModel.DataAnnotations;

namespace StorageManagement_MVC.Models
{
    public class User
    {
        [Required(ErrorMessage = "User ID is required")]
        public string userId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string passWord { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string? userName { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Birthday is required")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone Number must be exactly 9 digits")]
        public int? phoneNumber { get; set; }

        public int Status { get; set; }


        [Required(ErrorMessage = "Role is required")]
        public string? Role { get; set; }

    }
}
