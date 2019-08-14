using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class ProductTypeCreateInputModel
    {
        private const string RequiredFieldErrorMessage = "The field is required";

        [Required(ErrorMessage = RequiredFieldErrorMessage)]
        public string Name { get; set; }
    }
}
