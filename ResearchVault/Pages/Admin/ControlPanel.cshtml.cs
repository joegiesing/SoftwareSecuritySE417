using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
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

        public ControlPanelModel(IConfiguration configuration)
        {
            _configuration = configuration;
            factory = new UserDataAccessLayer(_configuration);
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserID") is null)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                //secondary and primary admins can view
                if (HttpContext.Session.GetInt32("Permissions") is null || HttpContext.Session.GetInt32("Permissions") < 1)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    lstUser = factory.ListUsers(null).ToList();
                    return Page();
                }
            }
        }

        public IActionResult OnPostEdit(Int32? id)
        {
            //Only primary admins can edit
            if (HttpContext.Session.GetInt32("Permissions") < 2)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit users.";
                lstUser = factory.ListUsers(null).ToList();
                return Page();
            }

            if (id == null)
            {
                return RedirectToPage("/Error");
            }
            else
            {
                rUser = factory.GetOneUser(id);
                lstUser = factory.ListUsers(null).ToList();
                return Page();
            }
        }

        public IActionResult OnPostDelete(Int32? id)
        {
            //Only primary admin can delete users
            if (HttpContext.Session.GetInt32("Permissions") < 2)
            {
                TempData["ErrorMessage"] = "You do not have permission to delete users.";
                lstUser = factory.ListUsers(null).ToList();
                return Page();
            }

            if (id == null)
            {
                return RedirectToPage("/Error");
            }
            else
            {
                factory.DeleteUser(id);
                lstUser = factory.ListUsers(null).ToList();

                return Page();
            }
        }

        public IActionResult OnPostUpdate()
        {
            //Only primary admin can update users
            if (HttpContext.Session.GetInt32("Permissions") < 2)
            {
                TempData["ErrorMessage"] = "You do not have permission to update users.";
                lstUser = factory.ListUsers(null).ToList();
                return Page();
            }

            if (rUser == null)
            {
                return RedirectToPage("/Error");
            }
            else
            {
                factory.UpdateUser(rUser);
                lstUser = factory.ListUsers(null).ToList();

                return Page();
            }
        }
    }
}