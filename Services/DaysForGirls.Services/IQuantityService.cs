using DaysForGirls.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaysForGirls.Services
{
    public interface IQuantityService
    {
        Task<QuantityServiceModel> Create(QuantityServiceModel model);
    }
}
