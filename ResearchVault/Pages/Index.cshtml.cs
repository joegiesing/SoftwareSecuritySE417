using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResearchVault.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchVault.Pages
{
    public class IndexModel : PageModel
    {
        public List<SourceModel> lstSourceRecent { get; set; }
        public List<SourceModel> lstSourceFav { get; set; }

        SourceDataAccessLayer factory;


        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new SourceDataAccessLayer(_configuration);
        }


        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}


        public IActionResult OnGet()
        {

            Int32? uID = HttpContext.Session.GetInt32("UserID");

            if (HttpContext.Session.GetInt32("UserID") is null)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                string str = "SELECT * FROM Source WHERE Favorite = 1 AND UserID =" + uID + " ORDER BY DateAdded DESC;";

                lstSourceRecent = factory.ListSources(uID, null).ToList();

                lstSourceFav = factory.ListSources(uID, str).ToList();

                return Page();
            }

        }
    }
}
