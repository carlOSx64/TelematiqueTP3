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
        public DbSet<File> Files { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<ConnectedUser> ConnectedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.Username).IsRequired();
            builder.Entity<Group>().Property(g => g.Name).IsRequired();
            builder.Entity<File>().Property(f => f.Name).IsRequired();
            builder.Entity<ConnectedUser>().Property(c => c.UserId).IsRequired();

            // builder.Entity<File>()
            //     .HasKey(g => new {g.GroupId});
            builder.Entity<File>()
                .HasOne(g => g.Group);

            builder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });
            builder.Entity<UserGroup>().HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.UserId);
            builder.Entity<UserGroup>().HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);

            builder.Entity<Invitation>()
                .HasKey(i => new { i.UserId, i.GroupId });
            builder.Entity<Invitation>()
                .HasOne(i => i.User)
                .WithMany(u => u.Invitations)
                .HasForeignKey(i => i.UserId);
            builder.Entity<Invitation>()
                .HasOne(i => i.Group)
                .WithMany()
                .HasForeignKey(i => i.GroupId);
            builder.Entity<Invitation>()
                .HasOne(i => i.InvitedBy)
                .WithMany()
                .HasForeignKey(i => i.InvitedById);

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
