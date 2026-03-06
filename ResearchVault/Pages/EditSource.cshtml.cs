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
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EditSourceModel> _logger;

        public EditSourceModel(IConfiguration configuration, ILogger<EditSourceModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            factory = new SourceDataAccessLayer(_configuration);
        }


        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }

            try
            {
                rSource = factory.GetOneSource(id);

                if (rSource == null || rSource.SourceID == 0)
                {
                    return RedirectToPage("/Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in EditSource OnGet for SourceID: {id}", id);
                return RedirectToPage("/Error");
            }

            return Page();
        }


        // edit post
        public IActionResult OnPostEdit()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                factory.UpdateSource(rSource);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in EditSource OnPostEdit for SourceID: {SourceID}", rSource?.SourceID);
                rSource.Feedback = "An error occurred while saving. Please try again.";
                return Page();
            }

            return Page();
        }


        // delete post
        public IActionResult OnPostDelete(int? id)
        {
            if (id == null)
            {
                return Page();
            }

            try
            {
                factory.DeleteSource(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in EditSource OnPostDelete for SourceID: {id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting. Please try again.";
                return Page();
            }

            return RedirectToPage("/SearchSource");
        }

    }
}