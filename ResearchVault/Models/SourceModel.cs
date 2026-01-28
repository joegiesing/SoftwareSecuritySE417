using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static ResearchVault.Models.ValidationLibrary;
using Microsoft.VisualBasic;
using System.Globalization;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResearchVault.Models
{
    public class SourceModel
    {

        [Required]
        public Int32 SourceID { get; set; }


        [Required(ErrorMessage = "Title required")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 255 characters long.")]
        public String Title { get; set; }


        //[StringLength(30, MinimumLength = 4, ErrorMessage = "Authors name must be between 4 and 30 characters long.")]
        [Required(ErrorMessage = "Author's name required")]
        [RegularExpression(@"[^0123456789/*-+.,:;!?%&½$#£><|(){}-]+$", ErrorMessage = "Invalid characters used")]
        public String Author { get; set; }


        public String Publisher { get; set; }


        [Required(ErrorMessage = "Link required")]
        public String Link { get; set; }


        [Required(ErrorMessage = "Date created required")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        //[DataType(DataType.Date)]
        [MyDate(ErrorMessage = "Invalid date")]
        public DateTime? DateCreated { get; set; }


        //date added will be automatically generated when its is first added to database
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }


        //[StringOptionsValidate(Allowed = new String[] { "Book", "Website", "Video" }, ErrorMessage = "Category is invalid.")]
        [Required(ErrorMessage = "Type required")]
        public String Type { get; set; }


        [RegularExpression(@"[^0123456789/*-+.,:;!?%&½$#£><|(){}-]+$", ErrorMessage = "Invalid characters used")]
        public String Category { get; set; }


        [RegularExpression(@"[^0123456789/*-+.,:;!?%&½$#£><|(){}-]+$", ErrorMessage = "Invalid characters used")]
        public String Tags { get; set; }


        [Required]
        public Boolean Favorite { get; set; }


        public String Notes { get; set; }

        [ForeignKey("UserID")]
        public Int32 UserID { get; set;  }

        public String Feedback { get; set; }
    }
}
