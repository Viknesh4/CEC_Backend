using Microsoft.EntityFrameworkCore;
using CEC_CRM.models;

namespace CEC_CRM.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Users> Users {  get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Ticket> ticket { get; set; }
        public DbSet<Suggestion> suggestions { get; set; }
        public DbSet<Images> Images { get; set; }

    }


}
