using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationTracker
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<JobApplication> Applications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var projectRoot = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

            var dbPath = Path.Combine(projectRoot, "JobApplications.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
