using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1
{
    public partial class CarInspectionContext : DbContext
    {
        public CarInspectionContext()
        {
        }

        public CarInspectionContext(DbContextOptions<CarInspectionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<CarColor> CarColor { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<Drivers> Drivers { get; set; }
        public virtual DbSet<Fines> Fines { get; set; }
        public virtual DbSet<FinesList> FinesList { get; set; }
        public virtual DbSet<Licence> Licences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CarInspection;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<CarColor>(entity =>
            {
                entity.HasKey(e => e.ColorNum);

                entity.ToTable("car_color");

                entity.Property(e => e.ColorNum)
                    .HasColumnName("Color num")
                    .ValueGeneratedNever();

                entity.Property(e => e.ColorCode)
                    .HasColumnName("Color code")
                    .HasMaxLength(255);

                entity.Property(e => e.ColorName)
                    .HasColumnName("Color name")
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Cars>(entity =>
            {
                entity.HasKey(e => e.CarId);

                entity.ToTable("cars");

                entity.Property(e => e.CarId).HasColumnName("Car_id");

                entity.Property(e => e.DateDtp)
                    .HasColumnName("Date_DTP")
                    .HasColumnType("date");

                entity.Property(e => e.Dtp).HasColumnName("DTP");

                entity.Property(e => e.EngineType)
                    .HasColumnName("Engine Type")
                    .HasMaxLength(255);

                entity.Property(e => e.IdDriver).HasColumnName("id-driver");

                entity.Property(e => e.Manufacturer).HasMaxLength(255);

                entity.Property(e => e.Model).HasMaxLength(255);

                entity.Property(e => e.PhotoDtp)
                    .HasColumnName("Photo_DTP")
                    .HasMaxLength(50);

                entity.Property(e => e.StateNumber)
                    .HasColumnName("State number")
                    .HasMaxLength(9)
                    .IsFixedLength();

                entity.Property(e => e.TypeOfDrive)
                    .HasColumnName("type of drive")
                    .HasMaxLength(255);

                entity.Property(e => e.Vin)
                    .HasColumnName("VIN")
                    .HasMaxLength(255);

                entity.Property(e => e.Year).HasMaxLength(255);

                entity.HasOne(d => d.ColorNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.Color)
                    .HasConstraintName("FK_cars_car_color");

                entity.HasOne(d => d.IdDriverNavigation)
                    .WithMany(p => p.Cars)
                    .HasForeignKey(d => d.IdDriver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_cars_drivers1");
            });

            modelBuilder.Entity<Drivers>(entity =>
            {
                entity.ToTable("drivers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255);

                entity.Property(e => e.Company)
                    .HasColumnName("company")
                    .HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255);

                entity.Property(e => e.Jobname)
                    .HasColumnName("jobname")
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.PassportNumber).HasColumnName("passport number");

                entity.Property(e => e.PassportSerial).HasColumnName("passport serial");

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(255);

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Fines>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateFine)
                    .HasColumnName("date_fine")
                    .HasColumnType("date");

                entity.Property(e => e.IdDriver).HasColumnName("id_driver");

                entity.Property(e => e.IdFine).HasColumnName("id_fine");

                entity.Property(e => e.PhotoFine)
                    .HasColumnName("photo_fine")
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdDriverNavigation)
                    .WithMany(p => p.Fines)
                    .HasForeignKey(d => d.IdDriver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fines_drivers");

                entity.HasOne(d => d.IdFineNavigation)
                    .WithMany(p => p.Fines)
                    .HasForeignKey(d => d.IdFine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Fines_Fines_list");
            });

            modelBuilder.Entity<FinesList>(entity =>
            {
                entity.ToTable("Fines_list");
            });

            modelBuilder.Entity<Licence>(entity =>
            {
                entity.HasKey(e => e.LicenceNumber);

                entity.ToTable("licences");

                entity.Property(e => e.LicenceNumber).HasColumnName("licence number");

                entity.Property(e => e.Categories)
                    .HasColumnName("categories")
                    .HasMaxLength(255);

                entity.Property(e => e.ExpireDate)
                    .HasColumnName("expire date")
                    .HasColumnType("date");

                entity.Property(e => e.IdDriver).HasColumnName("id-driver");

                entity.Property(e => e.LicenceDate)
                    .HasColumnName("licence date")
                    .HasColumnType("date");

                entity.Property(e => e.LicenceSeries)
                    .HasColumnName("licence series")
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdDriverNavigation)
                    .WithMany(p => p.Licence)
                    .HasForeignKey(d => d.IdDriver)
                    .HasConstraintName("FK_licences_drivers1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
