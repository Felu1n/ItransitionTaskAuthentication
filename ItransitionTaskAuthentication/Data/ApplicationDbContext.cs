using ItransitionTaskAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ItransitionTaskAuthentication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
