﻿// <auto-generated />
using Backend.Modules.Products.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace orders_backend.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    partial class ProductsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("products")
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.AnimalCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AnimalCategories", "products");
                });

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Stock")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Products", "products");
                });

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.ProductAnimalCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalCategoryId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnimalCategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAnimalCategories", "products");
                });

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.ProductAnimalCategory", b =>
                {
                    b.HasOne("Backend.Modules.Products.Domain.Entities.AnimalCategory", "AnimalCategory")
                        .WithMany("ProductAnimalCategories")
                        .HasForeignKey("AnimalCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Modules.Products.Domain.Entities.Product", "Product")
                        .WithMany("ProductAnimalCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimalCategory");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.AnimalCategory", b =>
                {
                    b.Navigation("ProductAnimalCategories");
                });

            modelBuilder.Entity("Backend.Modules.Products.Domain.Entities.Product", b =>
                {
                    b.Navigation("ProductAnimalCategories");
                });
#pragma warning restore 612, 618
        }
    }
}
