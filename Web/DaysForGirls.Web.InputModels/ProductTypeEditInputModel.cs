using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.InputModels
{
    public class ProductTypeEditInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
