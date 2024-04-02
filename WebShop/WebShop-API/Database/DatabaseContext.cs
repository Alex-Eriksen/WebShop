namespace WebShop_API.Database
{
    /// <summary>
    /// Inheriting from DbContext
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext( DbContextOptions<DatabaseContext> options ) : base ( options ) { }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Manufacturer> Manufacturer { get; set; }

        /// <summary>
        /// Creating models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            // Create a model for Customer
            modelBuilder.Entity<Customer>( entity => {
                entity.HasOne( e => e.Account ).WithOne( e => e.Customer );
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
                entity.HasIndex( e => e.PhoneNumber ).IsUnique();
                entity.HasMany( e => e.Orders ).WithOne( e => e.Customer ).OnDelete( DeleteBehavior.Restrict );
            } );

            // Creating models for Account
            modelBuilder.Entity<Account>( entity => {
                entity.HasOne( e => e.Customer ).WithOne( e => e.Account );
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
                entity.Property( e => e.Role ).HasDefaultValue( "Customer" );
                entity.HasIndex( e => e.Username ).IsUnique();
                entity.HasIndex( e => e.Email ).IsUnique();
            } );

            // Giving some countries an id and a name
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    CountryID = 1,
                    CountryName = "Denmark"
                },
                new Country
                {
                    CountryID = 2,
                    CountryName = "Norway"
                },
                new Country
                {
                    CountryID = 3,
                    CountryName = "Sweden"
                },
                new Country
                {
                    CountryID = 4,
                    CountryName = "Poland"
                } );

            // Capturing the datetime when the entities was createt in the database. Sets Created_At default to getdate()
            modelBuilder.Entity<Payment>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<RefreshToken>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Order>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Discount>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Transaction>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Photo>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Product>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Category>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<ProductType>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );

            modelBuilder.Entity<Manufacturer>( entity => {
                entity.Property( e => e.Created_At ).HasDefaultValueSql( "getdate()" );
            } );
        }
    }
}
