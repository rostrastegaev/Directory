using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataService : DbContext, IDataService
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Image> Images { get; set; }

        public DataService(DbContextOptions<DataService> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AddConfiguration(new UserConfig());
            builder.AddConfiguration(new RecordConfig());
            builder.AddConfiguration(new ImageConfig());
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity, new() =>
            new Repository<T>(Set<T>());
    }
}
