﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StealTheCatsApi;

#nullable disable

namespace StealTheCatsApi.Migrations
{
    [DbContext(typeof(CatsDbContext))]
    partial class CatsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StealTheCatsApi.Models.Cat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CatId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cats");
                });

            modelBuilder.Entity("StealTheCatsApi.Models.CatTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CatId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CatId");

                    b.HasIndex("TagId");

                    b.ToTable("CatTags");
                });

            modelBuilder.Entity("StealTheCatsApi.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("StealTheCatsApi.Models.CatTag", b =>
                {
                    b.HasOne("StealTheCatsApi.Models.Cat", "Cat")
                        .WithMany("CatTags")
                        .HasForeignKey("CatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StealTheCatsApi.Models.Tag", "Tag")
                        .WithMany("CatTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cat");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("StealTheCatsApi.Models.Cat", b =>
                {
                    b.Navigation("CatTags");
                });

            modelBuilder.Entity("StealTheCatsApi.Models.Tag", b =>
                {
                    b.Navigation("CatTags");
                });
#pragma warning restore 612, 618
        }
    }
}
