using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ResearchVault.Models
{
    class ValidationLibrary
    {
        public static bool IsValidEmail(string temp) //tests if email is valid length
        {
            bool bln = true;

            int atLocation = temp.IndexOf("@");
            //int NextatLocation = temp.IndexOf("@", atLocation + 1);
            int periodLocation = temp.LastIndexOf(".");

            if (temp.Length < 8)
            {
                bln = false;
            }
            else if (atLocation < 2)
            {
                bln = false;
            }
            else if (!temp.Contains("."))
            {
                bln = false;
            }
            else if ((periodLocation + 2) > (temp.Length))
            {
                bln = false;
            }

            return bln;
        }

        public static bool HasStrValue(string str) //tests if there is string input
        {
            bool bln = false;

            if (str.Length > 0)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsValidLength(string str, int len) //tests if string is valid length
        {
            bool bln = false;

            if (str.Length == len)
            {
                bln = true;
            }

            return bln;
        }



        /// <summary>
        /// ---------------------------------------
        /// ----------------Numbers---------------
        /// ---------------------------------------
        /// </summary>
        public static bool IsMinNum(int temp, int num) //tests if input int is greater than minimum number
        {
            bool bln = false;

            if (temp >= num)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsMinNum(double temp, double num) //tests if input double is greater than minimum number
        {
            bool bln = false;

            if (temp >= num)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsMaxNum(int temp, int num) //tests if input int is less than maximun number
        {
            bool bln = false;

            if (temp <= num)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsMaxNum(double temp, double num) //tests if input double is less than maximum number
        {
            bool bln = false;

            if (temp <= num)
            {
                bln = true;
            }

            return bln;
        }



        /// <summary>
        /// ---------------------------------------
        /// ----------------DateTime---------------
        /// ---------------------------------------
        /// </summary>
        public static bool IsFutureDate(DateTime date) //tests if DateTime is future date
        {
            bool bln = false;

            if (date > DateTime.Now)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsFutureOrPresDate(DateTime date) //tests if DateTime is future or present date
        {
            bool bln = false;

            if (date >= DateTime.Now)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsPastDate(DateTime date) //tests if DateTime is past date
        {
            bool bln = false;

            if (date < DateTime.Now)
            {
                bln = true;
            }

            return bln;
        }

        public static bool IsPastOrPresDate(DateTime date) //tests if DateTime is past or present date
        {
            bool bln = false;

            if (date <= DateTime.Now)
            {
                bln = true;
            }

            return bln;
        }


        //tests if date is in the past
        public class MyDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                DateTime _dateJoin = Convert.ToDateTime(value);

                if (_dateJoin <= DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
        }


        public class StringOptionsValidate : Attribute, IModelValidator
        {
            public string[] Allowed { get; set; }
            public string ErrorMessage { get; set; }

            public IEnumerable<ModelValidationResult> Validate(ModelValidationContext context)
            {
                if (Allowed.Contains(context.Model as string))
                {
                    return Enumerable.Empty<ModelValidationResult>();
                }
                else
                {
                    return new List<ModelValidationResult> { new ModelValidationResult("", ErrorMessage) };
                }
            }
        }
    }
}
