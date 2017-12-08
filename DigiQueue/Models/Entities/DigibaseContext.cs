using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DigiQueue.Models.Entities
{
    public partial class DigibaseContext : DbContext
    {
        public virtual DbSet<Classroom> Classroom { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Problem> Problem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=tcp:digibaseserver.database.windows.net,1433;Initial Catalog=DigiBase;Persist Security Info=False;User ID=digiadmin;Password=digipass_2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom", "DigiSchema");

                entity.Property(e => e.AspUserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });

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
                    .HasConstraintName("FK__Message__Classro__72C60C4A");
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
                    .HasConstraintName("FK__Problem__Classro__6FE99F9F");
            });
        }
    }
}
