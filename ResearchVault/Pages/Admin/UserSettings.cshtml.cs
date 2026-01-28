using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ResearchVault.Models;
using System.Collections.Generic;
using System.Linq;

namespace ResearchVault.Pages.Admin
{
    public class UserSettingsModel : PageModel
    {

        [BindProperty]
        public UserModel rUser { get; set; }

        public List<UserModel> lstUser { get; set; }

        UserDataAccessLayer factory;


        private readonly IConfiguration _configuration;

        public UserSettingsModel(IConfiguration configuration)
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

            return Page();
        }

        public IActionResult OnPostEdit(int? id)
        {
            if (id == null)
            {
                //return Page();
                return RedirectToPage("/Error");
            }
            else
            {
                rUser = factory.GetOneUser(id);
                
                return Page();
            }
        }
    }
}
