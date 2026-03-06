using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ResearchVault.Models;

namespace ResearchVault.Pages.Admin
{
    public class ControlPanelModel : PageModel
    {

        [BindProperty]
        public UserModel rUser { get; set; }

        [BindProperty]
        public List<UserModel> lstUser { get; set; }

        UserDataAccessLayer factory;

        private readonly IConfiguration _configuration;
        private readonly ILogger<ControlPanelModel> _logger;

        public ControlPanelModel(IConfiguration configuration, ILogger<ControlPanelModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            factory = new UserDataAccessLayer(_configuration);
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserID") is null)
            {
                return RedirectToPage("/Index");
            }

            if (HttpContext.Session.GetInt32("Permissions") is null || HttpContext.Session.GetInt32("Permissions") < 2)
            {
                return RedirectToPage("/Index");
            }

            try
            {
                lstUser = factory.ListUsers(null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ControlPanel OnGet");
                lstUser = new List<UserModel>();
                TempData["ErrorMessage"] = "An error occurred loading users. Please try again.";
            }

            return Page();
        }


        public IActionResult OnPostEdit(Int32? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }

            try
            {
                rUser = factory.GetOneUser(id);
                lstUser = factory.ListUsers(null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ControlPanel OnPostEdit for UserID: {id}", id);
                TempData["ErrorMessage"] = "An error occurred loading user data. Please try again.";
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public IActionResult OnPostDelete(Int32? id)
        {
            if (id == null)
            {
                return RedirectToPage("/Error");
            }

            try
            {
                factory.DeleteUser(id);
                lstUser = factory.ListUsers(null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ControlPanel OnPostDelete for UserID: {id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the user. Please try again.";
                lstUser = new List<UserModel>();
            }

            return Page();
        }

        public IActionResult OnPostUpdate()
        {
            if (rUser == null)
            {
                return RedirectToPage("/Error");
            }

            try
            {
                factory.UpdateUser(rUser);
                lstUser = factory.ListUsers(null).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ControlPanel OnPostUpdate for UserID: {UserID}", rUser?.UserID);
                TempData["ErrorMessage"] = "An error occurred while updating the user. Please try again.";
                lstUser = new List<UserModel>();
            }

            return Page();
        }
    }
}