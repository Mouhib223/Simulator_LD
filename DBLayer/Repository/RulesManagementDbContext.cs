using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SimulatorLD.DBLayer.DAOs;

namespace SimulatorLD.DBLayer.Repository
{
    public partial class RulesManagementDbContext : DbContext
    {
        public RulesManagementDbContext()
        {
        }

        public RulesManagementDbContext(DbContextOptions<RulesManagementDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fixmessage> Fixmessages { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<Rule> Rules { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=RulesDB;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fixmessage>(entity =>
            {
                entity.HasKey(e => e.MsgId)
                    .HasName("PK__FIXMessa__662358727E7A3975");

                entity.Property(e => e.MsgId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.Property(e => e.RuleId).ValueGeneratedOnAdd();
                entity.Property(r => r.RuleType)
                   .HasConversion(
                       v => v.ToString(),
                       v => (RuleTypesEnum)Enum.Parse(typeof(RuleTypesEnum), v));

            });

           
         

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
