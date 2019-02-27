namespace CarDealer.Models.Domain
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CarContext : DbContext
    {
        public CarContext()
            : base("name=CarContext")
        {
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .Property(e => e.model)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.manufacturer)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Car>()
                .Property(e => e.country)
                .IsUnicode(false);

            modelBuilder.Entity<Car>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Car)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.firstName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.lastName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Customer1)
                .HasForeignKey(e => e.customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);
        }
    }
}
