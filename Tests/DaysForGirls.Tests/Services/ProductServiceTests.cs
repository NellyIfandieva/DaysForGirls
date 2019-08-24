namespace DaysForGirls.Tests.Services
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class ProductServiceTests
    {
        public void Test()
        {
            var options = new DbContextOptionsBuilder<DaysForGirlsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            using (var context = new DaysForGirlsDbContext(options))
            {

            }
        }
    }
}
