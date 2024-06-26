using ExpenseSharingApp.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.Data
{
    public class ExpenseSharingContext : DbContext
    {
        public ExpenseSharingContext(DbContextOptions<ExpenseSharingContext> options) : base(options)
        {
        }
        public DbSet<UserEF> Users { get; set; }
        public DbSet<GroupEF> Groups { get; set; }
        public DbSet<ExpenseEF> Expenses { get; set; }
        public DbSet<ExpenseSplitEF> ExpenseSplits { get; set; }
        public DbSet<ExpenseSettlementEF> ExpenseSettlements { get; set; }
        public DbSet<UserGroupEF> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints here if needed
            modelBuilder.Entity<UserGroupEF>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            //modelBuilder.Entity<ExpenseSplitEF>()
            //    .HasKey(es => new { es.UserId, es.ExpenseId });
            modelBuilder.Entity<GroupEF>()
                .HasMany(g => g.Expenses)
                .WithOne(e => e.Group)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseEF>()
                .HasMany(e => e.SplitAmong)
                .WithOne(es => es.Expense)
                .OnDelete(DeleteBehavior.Cascade);



            base.OnModelCreating(modelBuilder);
        }
    }

}
