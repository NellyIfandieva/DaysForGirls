using DaysForGirls.Data;
using DaysForGirls.Data.Models;
using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public class QuantityService : IQuantityService
    {
        private readonly DaysForGirlsDbContext db;

        public QuantityService(DaysForGirlsDbContext db)
        {
            this.db = db;
        }

        public async Task<QuantityServiceModel> Create(QuantityServiceModel model)
        {
            Quantity quantity = new Quantity
            {
                AvailableItems = model.AvailableItems
            };

            this.db.Quantities.Add(quantity);
            await this.db.SaveChangesAsync();

            model.Id = quantity.Id;

            return model;
        }
    }
}
