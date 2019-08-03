using System.ComponentModel.DataAnnotations;

namespace DaysForGirls.Data.Models
{
    public class BaseModel<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}