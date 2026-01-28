using System;
using Microsoft.AspNetCore.Mvc;

namespace ResearchVault.Models
{
    public class FilterModel
    {
        public String SqlStr { get; set; }

        [BindProperty]
        public Boolean SqlStrTitleAZ { get; set; }

        [BindProperty]
        public Boolean SqlStrTitleZA { get; set; }

        [BindProperty]
        public Boolean SqlStrCategoryAZ { get; set; }

        [BindProperty]
        public Boolean SqlStrCategoryZA { get; set; }

        [BindProperty]
        public String SqlStrAuthor { get; set; }

        [BindProperty]
        public String SqlStrType { get; set; }

        [BindProperty]
        public String SqlStrTags { get; set; }

        [BindProperty]
        public String SqlStrDate { get; set; }
    }
}
