﻿// <auto-generated />
using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EmployeeManagement.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20240331081719_TwoMoreEmployeesAdded")]
    partial class TwoMoreEmployeesAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.28")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("EmployeeManagement.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Department")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Department = 2,
                            Email = "fellyka@sollers.co.za",
                            Name = "Felly KANYIKI"
                        },
                        new
                        {
                            Id = 2,
                            Department = 1,
                            Email = "davidku@sollers.co.za",
                            Name = "David KUELA"
                        },
                        new
                        {
                            Id = 3,
                            Department = 3,
                            Email = "maryma@sollers.co.za",
                            Name = "Mary MAEL"
                        },
                        new
                        {
                            Id = 4,
                            Department = 2,
                            Email = "gaelmb@sollers.co.za",
                            Name = "Gael MBULO"
                        },
                        new
                        {
                            Id = 5,
                            Department = 1,
                            Email = "johnri@sollers.co.za",
                            Name = "John RIKOLO"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}