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
                if (HttpContext.Session.GetInt32("Permissions") is null || HttpContext.Session.GetInt32("Permissions") < 2)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    lstUser = factory.ListUsers(null).ToList();
                    return Page();
                }
                //else
                //{
                //    return RedirectToPage("/Index");
                //}
            }
            

        }

        //[HttpGet("/Admin/ControlPanel/{id}")]
        //public IActionResult OnGetEdit(int id)
        //{
        //    //lstUser = factory.ListUsers(null).ToList();
        //    rUser = factory.GetOneUser(id);
        //    if (rUser == null)
        //    {
        //        return RedirectToPage("/Error");
        //    }
        //    else
        //    {
        //        return Page();
        //    }

        //    //if (id == null)
        //    //{
        //    //    return Page();
        //    //}
        //    //else
        //    //{
        //    //    //lstUser = factory.ListUsers(null).ToList();
        //    //    rUser = factory.GetOneUser(id);
        //    //    return RedirectToPage("/Error");
        //    //}
        //}

        public IActionResult OnPostEdit(Int32? id)
        {
            if (id == null)
            {
                //return Page();
                return RedirectToPage("/Error");
            }
            else
            {
                rUser = factory.GetOneUser(id);
                lstUser = factory.ListUsers(null).ToList();
                //int id = rUser.UserID; // Get the ID from the rUser object
                //return RedirectToPage("/Admin/ControlPanel", new { id });
                //return RedirectToPage("/Admin/ControlPanel");
                return Page();
            }
        }

        public IActionResult OnPostDelete(Int32? id)
        {
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
