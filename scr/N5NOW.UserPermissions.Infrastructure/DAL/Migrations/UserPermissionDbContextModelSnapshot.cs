﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using N5NOW.UserPermissions.Infrastructure.DAL.Configurations;

#nullable disable

namespace N5NOW.UserPermissions.Infrastructure.DAL.Migrations
{
    [DbContext(typeof(UserPermissionDbContext))]
    partial class UserPermissionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Users")
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("N5NOW.UserPermissions.Domain.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmployeeForename")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("employeeforename");

                    b.Property<string>("EmployeeSurname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("employeesurname");

                    b.Property<DateTime>("PermissionDate")
                        .HasColumnType("datetime")
                        .HasColumnName("permissiondate");

                    b.Property<int>("PermissionTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("permissiontype");

                    b.HasKey("Id");

                    b.HasIndex("PermissionTypeId")
                        .IsUnique();

                    b.ToTable("permissions", "Users");
                });

            modelBuilder.Entity("N5NOW.UserPermissions.Domain.Entities.PermissionType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.HasKey("Id");

                    b.ToTable("permissiontypes", "Users");
                });

            modelBuilder.Entity("N5NOW.UserPermissions.Domain.Entities.Permission", b =>
                {
                    b.HasOne("N5NOW.UserPermissions.Domain.Entities.PermissionType", "PermissionType")
                        .WithOne()
                        .HasForeignKey("N5NOW.UserPermissions.Domain.Entities.Permission", "PermissionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PermissionType");
                });
#pragma warning restore 612, 618
        }
    }
}