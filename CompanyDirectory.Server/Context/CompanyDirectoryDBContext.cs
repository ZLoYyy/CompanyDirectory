using CompanyDirectory.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompanyDirectory.Server.Context
{
    public class CompanyDirectoryDBContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Report> Reports { get; set; }
        public CompanyDirectoryDBContext(DbContextOptions<CompanyDirectoryDBContext> options) : base(options)
        {

        }
    }
}
