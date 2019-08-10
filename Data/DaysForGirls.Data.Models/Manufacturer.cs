﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DaysForGirls.Data.Models
{
    public class Manufacturer : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Logo Logo { get; set; }

        public bool IsDeleted { get; set; }
    }
}
