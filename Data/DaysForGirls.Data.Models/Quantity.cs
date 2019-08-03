namespace DaysForGirls.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Quantity : BaseModel<int>
    {

        private const int MinQuantity = 0;
        private const int MaxQuantity = 10;

        [Range(MinQuantity, MaxQuantity)]
        public int AvailableItems { get; set; }
    }
}
