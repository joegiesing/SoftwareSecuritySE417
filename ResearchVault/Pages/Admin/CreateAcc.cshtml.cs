using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ResearchVault.Models;

namespace ResearchVault.Pages.Admin
{
    public class CreateAccModel : PageModel
    {
        [BindProperty]
        public UserModel rUser { get; set; }

        //private readonly IConfiguration _configuration;

        private readonly UserDataAccessLayer accessLayerUser;

        public CreateAccModel(IConfiguration configuration)
        {
            //_configuration = configuration;
            accessLayerUser = new UserDataAccessLayer(configuration);
        }



        public void OnGet()
        {

        }


        public IActionResult OnPost()
        {
            IActionResult temp;

            if (ModelState.IsValid == false)
            {
                temp = Page();

            }
            else if (accessLayerUser.ifEmailExists(rUser.Email)){
                temp = Page();
            }
            else
            {
                if (rUser is null == false)
                {
                    rUser.Permissions = 0;

                    accessLayerUser.AddUser(rUser);

                    temp = RedirectToPage("/Admin/Index");
                }
                else
                {
                    temp = Page();
                }

            }

            return temp;
        }
    }
}
