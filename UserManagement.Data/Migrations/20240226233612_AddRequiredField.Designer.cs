﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserManagement.Data;

#nullable disable

namespace UserManagement.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240226233612_AddRequiredField")]
    partial class AddRequiredField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserManagement.Models.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ActionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogEvent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Msg")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Severity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpanId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TraceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogEntries", (string)null);
                });

            modelBuilder.Entity("UserManagement.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("DateOfBirth")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            DateOfBirth = new DateTime(1991, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ploew@example.com",
                            Forename = "Peter",
                            IsActive = true,
                            Surname = "Loew"
                        },
                        new
                        {
                            Id = 2L,
                            DateOfBirth = new DateTime(1972, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "bfgates@example.com",
                            Forename = "Benjamin Franklin",
                            IsActive = true,
                            Surname = "Gates"
                        },
                        new
                        {
                            Id = 3L,
                            DateOfBirth = new DateTime(1989, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "ctroy@example.com",
                            Forename = "Castor",
                            IsActive = false,
                            Surname = "Troy"
                        },
                        new
                        {
                            Id = 4L,
                            DateOfBirth = new DateTime(1985, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "mraines@example.com",
                            Forename = "Memphis",
                            IsActive = true,
                            Surname = "Raines"
                        },
                        new
                        {
                            Id = 5L,
                            DateOfBirth = new DateTime(1970, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "sgodspeed@example.com",
                            Forename = "Stanley",
                            IsActive = true,
                            Surname = "Goodspeed"
                        },
                        new
                        {
                            Id = 6L,
                            DateOfBirth = new DateTime(1982, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "himcdunnough@example.com",
                            Forename = "H.I.",
                            IsActive = true,
                            Surname = "McDunnough"
                        },
                        new
                        {
                            Id = 7L,
                            DateOfBirth = new DateTime(1986, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "cpoe@example.com",
                            Forename = "Cameron",
                            IsActive = false,
                            Surname = "Poe"
                        },
                        new
                        {
                            Id = 8L,
                            DateOfBirth = new DateTime(1997, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "emalus@example.com",
                            Forename = "Edward",
                            IsActive = false,
                            Surname = "Malus"
                        },
                        new
                        {
                            Id = 9L,
                            DateOfBirth = new DateTime(1997, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "dmacready@example.com",
                            Forename = "Damon",
                            IsActive = false,
                            Surname = "Macready"
                        },
                        new
                        {
                            Id = 10L,
                            DateOfBirth = new DateTime(1980, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jblaze@example.com",
                            Forename = "Johnny",
                            IsActive = true,
                            Surname = "Blaze"
                        },
                        new
                        {
                            Id = 11L,
                            DateOfBirth = new DateTime(1981, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "rfeld@example.com",
                            Forename = "Robin",
                            IsActive = true,
                            Surname = "Feld"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
