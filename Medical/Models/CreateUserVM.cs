using System.ComponentModel.DataAnnotations;

namespace Medical.Models
{

    public enum UserRole
    {
        Admin,
        Doctor,
        Patient
    }
    public class CreateUserVM
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(14,MinimumLength =14)]
        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }
    }
    public class EditUserVM
    {

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(14,MinimumLength =14)]
        [Display(Name = "National ID")]
        public string NationalId { get; set; }

        [Required]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        [Display(Name = "Role")]
        public UserRole Role { get; set; }
    }
}
