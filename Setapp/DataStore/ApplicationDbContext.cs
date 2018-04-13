using System.Data.Entity;

namespace Setapp.DataStore
{
    public class ApplicationDbContext : DbContext
    {
        private const string DatabaseConnectionName = "EPiServerDB";

        public ApplicationDbContext() : base(DatabaseConnectionName)
        {
        }

        public DbSet<EntityPageViewsData> PageViewsData { get; set; }
    }
}