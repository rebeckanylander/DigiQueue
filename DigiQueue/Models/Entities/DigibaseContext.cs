using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DigiQueue.Models.Entities
{
    public partial class DigibaseContext : DbContext
    {
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Problem> Problem { get; set; }
        public virtual DbSet<UserExtension> UserExtension { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message", "DigiSchema");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Message__Classro__03F0984C");
            });

            modelBuilder.Entity<Problem>(entity =>
            {
                entity.ToTable("Problem", "DigiSchema");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.Problem)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Problem__Classro__04E4BC85");
            });

            modelBuilder.Entity<UserExtension>(entity =>
            {
                entity.ToTable("UserExtension", "DigiSchema");

                entity.Property(e => e.AspUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });
        }
    }
}
