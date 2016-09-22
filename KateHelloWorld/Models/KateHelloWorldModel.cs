namespace KateHelloWorld.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class KateHelloWorldModel : DbContext
    {
        public KateHelloWorldModel()
            : base("name=KateHelloWorldModel")
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<RateCity> RateCities { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<HelloCity> HelloCities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .Property(e => e.CityName)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityStateOrProvince)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityCountry)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CityImgUri)
                .IsUnicode(false);

            //modelBuilder.Entity<City>()
            //    .HasMany(e => e.HelloCities)
            //    .WithRequired(e => e.City)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<City>()
                .HasMany(e => e.RateCities)
                .WithRequired(e => e.City)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserDisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RateCities)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>()
            //    .HasMany(e => e.HelloCities)
            //    .WithRequired(e => e.User)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<HelloCity>()
                .Property(e => e.Greeting)
                .IsUnicode(false);
        }
    }
}
