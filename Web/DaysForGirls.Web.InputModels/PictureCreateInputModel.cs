using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class PictureCreateInputModel
    {
        public IFormFile Picture { get; set; }
    }
}
