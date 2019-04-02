using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Data
{

    /// <summary>
    /// Entity framework context
    /// </summary>
    public class SqliteContext : DbContext
    {
        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.Username).IsRequired();
        }
    }

    public static class SqliteFactory
    {
        public static SqliteContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteContext>();
            optionsBuilder.UseSqlite(connectionString);

            var context = new SqliteContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}
