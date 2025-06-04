using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class DatabaseContext : DbContext
{
    public DbSet<AvailableProgram> AvailablePrograms { get; set; }
    public DbSet<ProgramModel> Programs { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
    public DbSet<WashingMachine>  WashingMachines { get; set; }
    
    
    
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }
}