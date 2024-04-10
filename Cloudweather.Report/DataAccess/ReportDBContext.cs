using System;
using Microsoft.EntityFrameworkCore;

namespace CloudWeather.Report.DataAccess
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(){}
        public ReportDbContext(DbContextOptions options) : base(options){}

        public DbSet<WeatherReport> WeatherReport{ get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            SnakeCaseIdentityTableNames(modelBuilder);
        }

        private static void SnakeCaseIdentityTableNames( ModelBuilder modelBuilder ){
            modelBuilder.Entity<WeatherReport>().ToTable("weather_report");
        }
    }
}