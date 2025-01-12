﻿// <auto-generated />
using System;
using GostStorage.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GostStorage.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250104081238_RenameDbSets")]
    partial class RenameDbSets
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GostStorage.Entities.Document", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ActualFieldId")
                        .HasColumnType("bigint");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("PrimaryFieldId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("GostStorage.Entities.DocumentReference", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("ChildDocId")
                        .HasColumnType("bigint");

                    b.Property<long>("ParentalDocId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("References");
                });

            modelBuilder.Entity("GostStorage.Entities.Field", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int?>("AcceptanceYear")
                        .HasColumnType("integer");

                    b.Property<string>("AcceptedFirstTimeOrReplaced")
                        .HasColumnType("text");

                    b.Property<string>("ActivityField")
                        .HasColumnType("text");

                    b.Property<int?>("AdoptionLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Amendments")
                        .HasColumnType("text");

                    b.Property<string>("ApplicationArea")
                        .HasColumnType("text");

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<string>("Changes")
                        .HasColumnType("text");

                    b.Property<string>("CodeOks")
                        .HasColumnType("text");

                    b.Property<int?>("CommissionYear")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("Designation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("DocId")
                        .HasColumnType("bigint");

                    b.Property<string>("DocumentText")
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<int?>("Harmonization")
                        .HasColumnType("integer");

                    b.Property<bool>("IsPrimary")
                        .HasColumnType("boolean");

                    b.Property<string>("KeyWords")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastEditTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("GostStorage.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OrgActivity")
                        .HasColumnType("text");

                    b.Property<string>("OrgBranch")
                        .HasColumnType("text");

                    b.Property<string>("OrgName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GostStorage.Entities.UserAction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("Action")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("DocId")
                        .HasColumnType("bigint");

                    b.Property<string>("OrgBranch")
                        .HasColumnType("text");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("UserActions");
                });
#pragma warning restore 612, 618
        }
    }
}
