using Microsoft.EntityFrameworkCore;
using PhoneStore.Models;

namespace PhoneStore.Context;

public class MobileContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    public MobileContext(DbContextOptions<MobileContext> options) : base(options)
    {
    }
}