using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ResearchVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Runtime.ExceptionServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResearchVault.Pages.Admin
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public UserPassModel rUserLogin { get; set; }

        //public String isValid = "";

        private readonly IConfiguration _configuration;


        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void OnGet()
        {
            HttpContext.Session.Clear();
        }


        public IActionResult OnPost()
        {
            IActionResult temp;
            List<UserPassModel> lstUserModel = new List<UserPassModel>();

            if (ModelState.IsValid == false)
            {
                //rUserLogin.Feedback += "Login Failed: model state invalid";

                temp = Page();
            }
            else
            {
                if (rUserLogin != null)
                {

                    UserPassDataAccessLayer factory = new UserPassDataAccessLayer(_configuration);

                    lstUserModel = factory.GetUserLogin(rUserLogin).ToList();

                    if (lstUserModel.Count > 0)
                    {
                        HttpContext.Session.SetInt32("UserID", lstUserModel[0].UserID);
                        HttpContext.Session.SetString("Email", lstUserModel[0].Email);
                        HttpContext.Session.SetInt32("Permissions", lstUserModel[0].Permissions);

                        //isValid = "is-valid";

                        temp = Redirect("/Index");

                    }
                    else
                    {
                        //rUserLogin.Feedback = "Login Failed.";
                        temp = Page();
                    }
                }
                else
                {
                    //rUserLogin.Feedback = "Login Failed.";
                    temp = Page();
                }

            }

            return temp;
        }
    }
}
