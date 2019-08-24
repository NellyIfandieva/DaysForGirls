namespace DaysForGirls.Tests.Common
{
    using Data;
    using Microsoft.EntityFrameworkCore;
    using System;

    public static class DaysForGirlsDbContextInMemoryFactory
    {
        public static DaysForGirlsDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<DaysForGirlsDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            return new DaysForGirlsDbContext(options);
        }
    }
}
