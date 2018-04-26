using System.Data.Entity;

namespace Benchmarks.DataStore
{
    public class ApplicationDbContext : DbContext
    {
        private const string DatabaseConnectionName = "EPiServerDB";

        public ApplicationDbContext() : base(DatabaseConnectionName)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<EntityPageViewsData> PageViewsData { get; set; }
    }
}