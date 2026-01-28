using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ResearchVault.Models;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System;
using System.Threading.Tasks;

namespace ResearchVault.Pages
{
    public class SearchSourceModel : PageModel
    {

        //public SourceModel rSource { get; set; }
        public List<SourceModel> lstSource { get; set; }

        SourceDataAccessLayer factory;


        //very convoluted
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



    private readonly IConfiguration _configuration;

        public SearchSourceModel(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new SourceDataAccessLayer(_configuration);
        }


        public IActionResult OnGet()
        {
            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (HttpContext.Session.GetInt32("UserID") is null)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                lstSource = factory.ListSources(uID,null).ToList();
                return Page();
            }
            

            //SqlStr = "yes";
            //return Page();

        }


        //search bar
        public IActionResult OnPostSearch(string? strTitle)
        {
            strTitle.Trim();
            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (strTitle != null)
            {
                string str = "SELECT * FROM Source WHERE Title LIKE '%" + strTitle + "%'";
                lstSource = factory.ListSources(uID, str).ToList();

                SqlStr = str;
                return Page();
                
            }
            else
            {
                return RedirectToPage("/Error");
            }
        }


        public IActionResult OnPostDelete(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }
            else
            {


                factory.DeleteSource(id);
                //return Page();
                return RedirectToPage("/SearchSource");

            }
        }

        //public void OnPostDelete(int? id)
        //{

        //    factory.DeleteSource(id);
        //}

        
        public IActionResult OnPostFilter()
        {
            SqlStr = "SELECT * FROM Source";

            Int32? uID = HttpContext.Session.GetInt32("UserID");


            if (SqlStrTitleAZ == true || SqlStrTitleZA == true)
            {
                SqlStr += " ORDER BY";

                if (SqlStrTitleAZ == true)
                {
                    SqlStr += " Title ASC";
                }
                else if (SqlStrTitleZA == true)
                {
                    SqlStr += " Title DESC";
                }


                if (SqlStrCategoryAZ == true)
                {
                    SqlStr += " Category ASC,";
                }
                else if (SqlStrCategoryZA == true)
                {
                    SqlStr += " Category DESC,";
                }
            }
            else 
            {
                SqlStr += "dedsed";
            }


            lstSource = factory.ListSources(uID, SqlStr).ToList();

            return Page();
        }

    }
}
