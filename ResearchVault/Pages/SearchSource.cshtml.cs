using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public List<SourceModel> lstSource { get; set; }

        SourceDataAccessLayer factory;

        public String SqlStr { get; set; }

        [BindProperty]
        public Boolean SqlStrTitleAZ { get; set; }

        [BindProperty]
        public Boolean SqlStrTitleZA { get; set; }

        [BindProperty]
        public Boolean SqlStrCategoryAZ { get; set; }

        [BindProperty]
        public Boolean SqlStrCategoryZA { get; set; }

        //[BindProperty]
        //public String SqlStrAuthor { get; set; }

        //[BindProperty]
        //public String SqlStrType { get; set; }

        //[BindProperty]
        //public String SqlStrTags { get; set; }

        //[BindProperty]
        //public String SqlStrDate { get; set; }


        private readonly IConfiguration _configuration;
        private readonly ILogger<SearchSourceModel> _logger;

        public SearchSourceModel(IConfiguration configuration, ILogger<SearchSourceModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            factory = new SourceDataAccessLayer(_configuration);
        }


        public IActionResult OnGet()
        {
            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (uID is null)
            {
                return RedirectToPage("/Admin/Index");
            }

            try
            {
                lstSource = factory.ListSources(uID, null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchSource OnGet for UserID: {uID}", uID);
                lstSource = new List<SourceModel>();
                TempData["ErrorMessage"] = "An error occurred loading sources. Please try again.";
            }

            return Page();
        }

        #nullable enable
        //search bar

        public IActionResult OnPostSearch(string? strTitle)
        {
            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (string.IsNullOrWhiteSpace(strTitle))
            {
                return RedirectToPage("/Error");
            }

            try
            {
                strTitle = strTitle.Trim();
                string str = "SELECT * FROM Source WHERE Title LIKE '%" + strTitle + "%'";
                lstSource = factory.ListSources(uID, str).ToList();
                SqlStr = str;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchSource OnPostSearch for UserID: {uID}", uID);
                lstSource = new List<SourceModel>();
                TempData["ErrorMessage"] = "An error occurred during search. Please try again.";
            }

            return Page();
        }


        public IActionResult OnPostDelete(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }

            try
            {
                factory.DeleteSource(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchSource OnPostDelete for SourceID: {id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting. Please try again.";
                return Page();
            }

            return RedirectToPage("/SearchSource");
        }


        public IActionResult OnPostFilter()
        {
            SqlStr = "SELECT * FROM Source";

            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (SqlStrTitleAZ == true || SqlStrTitleZA == true)
            {
                SqlStr += " ORDER BY";

                if (SqlStrTitleAZ == true)
                    SqlStr += " Title ASC";
                else if (SqlStrTitleZA == true)
                    SqlStr += " Title DESC";

                if (SqlStrCategoryAZ == true)
                    SqlStr += " Category ASC,";
                else if (SqlStrCategoryZA == true)
                    SqlStr += " Category DESC,";
            }
            else
            {
                SqlStr += "dedsed";
            }

            try
            {
                lstSource = factory.ListSources(uID, SqlStr).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in SearchSource OnPostFilter for UserID: {uID}", uID);
                lstSource = new List<SourceModel>();
                TempData["ErrorMessage"] = "An error occurred applying filters. Please try again.";
            }

            return Page();
        }

    }
}