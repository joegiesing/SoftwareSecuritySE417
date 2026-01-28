using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ResearchVault.Models
{
    public class UserPassModel
    {
        public Int32 UserID { get; set; }


        [Required(ErrorMessage = "Email required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String Email { get; set; }


        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Invalid Password")]
        public String Password { get; set; }


        [Required]
        public Int32 Permissions { get; set; }


        public String Feedback { get; set; }

    }
}
