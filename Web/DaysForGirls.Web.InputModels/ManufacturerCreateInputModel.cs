using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Web.InputModels
{
    public class ManufacturerCreateInputModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public IFormFile Logo { get; set; }
    }
}
