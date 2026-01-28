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
    public class ViewSourceModel : PageModel
    {

        [BindProperty]
        public SourceModel rSource { get; set; }


        //create data access layer as factory
        SourceDataAccessLayer factory;


        //var for setting status of forms
        [BindProperty(SupportsGet = true)]
        public String ValidStatus { get; set; }

        public String ErrorMessage { get; set; }

        [BindProperty]
        public String Stat { get; set; }



        private readonly IConfiguration _configuration;

        public ViewSourceModel(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new SourceDataAccessLayer(_configuration);
        }



        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserID") is null)
            {
                return RedirectToPage("/Admin/Index");
            }
            else
            {
                return Page();
            }
            //IActionResult temp;
            //if (HttpContext.Session.GetString("FormStatus") == "Edit")
            //if (id.HasValue)
            //{

            //    rSource = factory.GetOneSource(id);
            //}
            //else
            //{
            //    return RedirectToPage("/Error");
            //}

            //if (rSource == null)
            //{
            //    return RedirectToPage("/Error");
            //}
            //else if (rSource != null)
            //{
            //    return RedirectToPage("/Admin/ControlPanel");

            //}

            //else
            //{
            //    return RedirectToPage("/index");
            //}

            //id = HttpContext.Session.GetInt32("SourceID");
            //HttpContext.Session.GetString("FormStatus");

        }



        public IActionResult OnPost()
        {
            Int32? uID = HttpContext.Session.GetInt32("UserID");


            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                if (rSource != null) //HttpContext.Session.GetInt32("UserID") == rSource.UserID
                {
                    factory.AddSource(rSource, uID);
                }
                else
                {
                    return RedirectToPage("/Error");
                }

                return Page();
            }
        }



    }
}
