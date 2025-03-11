using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Tests.Common
{
    public class InMemoryDbContextFactory
    {
        public static PersonManagerContext Create()
        {
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            var context = new PersonManagerContext(options);

            context.Database.EnsureCreated();

            return context;
        }

        public static void Destroy(PersonManagerContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}