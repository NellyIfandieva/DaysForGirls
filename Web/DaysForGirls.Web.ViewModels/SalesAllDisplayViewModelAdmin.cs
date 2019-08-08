using System;
using System.Collections.Generic;
using System.Text;

namespace DaysForGirls.Web.ViewModels
{
    public class SalesAllDisplayViewModelAdmin
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime EndsOn { get; set; }

        public string Picture { get; set; }

        public bool IsActive { get; set; }

        public List<ProductDisplayAllViewModel> Products { get; set; }
    }
}
