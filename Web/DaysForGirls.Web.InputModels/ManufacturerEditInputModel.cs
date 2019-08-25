using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class ManufacturerEditInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Logo { get; set; }
    }
}
