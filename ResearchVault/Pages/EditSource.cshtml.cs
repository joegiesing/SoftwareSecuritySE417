using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ResearchVault.Models;

namespace ResearchVault.Pages
{
    public class EditSourceModel : PageModel
    {

        [BindProperty]
        public SourceModel rSource { get; set; }


        //create data access layer as factory
        SourceDataAccessLayer factory;


        //var for setting status of forms
        [BindProperty(SupportsGet = true)]
        public String ValidStatus { get; set; }


        [BindProperty]
        public Int32 sourceID { get; set; }



        private readonly IConfiguration _configuration;

        public EditSourceModel(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new SourceDataAccessLayer(_configuration);
        }



        public IActionResult OnGet(int? id)
        {
            //sourceID = Convert.ToInt32(id);

            if (id == null)
            {
                return RedirectToPage("/Error");
            }
            else
            {
                rSource = factory.GetOneSource(id);
            }
            return Page();
        }


        //edit post
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else //if (HttpContext.Session.GetInt32("UserID") == rSource.UserID)
            {
                factory.UpdateSource(rSource);
            }

            return Page();
        }


        //delete post
        public IActionResult OnPostDelete(int? id)
        {
            if (id == null)
            {
                return Page();
            }
            else
            {
                factory.DeleteSource(id);
                return RedirectToPage("/SearchSource");

            }
        }



        //public IActionResult OnPost()
        //{
            
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var modelStateEntry in ModelState.Values)
        //        {
        //            foreach (var error in modelStateEntry.Errors)
        //            {
        //                var errorMessage = error.ErrorMessage;

        //                ErrorMessage = errorMessage;

        //            }
        //        }
        //        //return RedirectToPage("/index");
        //        return Page();
        //    }

            
        //    return Page();
        //}

    }
}
