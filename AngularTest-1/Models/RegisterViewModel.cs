using System.ComponentModel.DataAnnotations;

namespace AngularTest1.Models
{
    public class RegisterViewModel
    {
        public string UserId { get; set; }
        //[Required]
        //[EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        //[Display(Name = "Role")]
        //public string Role { get; set; }

        ////[Required]
        //[Display(Name = "First Name*")]
        //public string FirstName { get; set; }
        ////[Required]
        //[Display(Name = "Last Name*")]
        //public string LastName { get; set; }
        ////[Required]
        //[Display(Name = "Gender*")]
        //public string Gender { get; set; }
        ////[Required]
        //[Display(Name = "CNIC*")]
        //public string Cnic { get; set; }
        //[Display(Name = "City*")]
        ////[Required]
        //public string City { get; set; }
        //[Display(Name = "Country*")]
        ////[Required]
        //public string Country { get; set; }

    }
}
