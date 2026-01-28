using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ResearchVault.Models
{
    public class UserModel : UserPassModel
    {
        //variables
        [Required(ErrorMessage = "Name required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Invalid characters used")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 20 characters long")]
        public String FName { get; set; }


        [Required(ErrorMessage = "Name required")]
        [RegularExpression(@"[^0123456789/*-+.,:;!?%&½$#£><|(){}-]+$", ErrorMessage = "Invalid characters used")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 20 characters long")]
        public String LName { get; set; }


        //[Required(ErrorMessage = "Email required")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //public String Email { get; set; }


        [Required(ErrorMessage = "Username required")]
        public String Username { get; set; }


        //[Required(ErrorMessage = "Password required")]
        //[DataType(DataType.Password)]
        //[StringLength(30, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters long.")]
        //public String Password { get; set; }


    }
}
