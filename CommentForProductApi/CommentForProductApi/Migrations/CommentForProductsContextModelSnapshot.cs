﻿// <auto-generated />
using System;
using CommentForProductApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommentForProductApi.Migrations
{
    [DbContext(typeof(CommentForProductsContext))]
    partial class CommentForProductsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CommentForProductApi.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("IdProduct")
                        .HasColumnType("integer")
                        .HasColumnName("id_product");

                    b.Property<int?>("IdUser")
                        .HasColumnType("integer")
                        .HasColumnName("id_user");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("bytea")
                        .HasColumnName("photo");

                    b.Property<int?>("Score")
                        .HasColumnType("integer")
                        .HasColumnName("score");

                    b.Property<string>("TextComment")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("text_comment");

                    b.HasKey("Id")
                        .HasName("comment_pkey");

                    b.HasIndex("IdProduct");

                    b.HasIndex("IdUser");

                    b.ToTable("comment", (string)null);
                });

            modelBuilder.Entity("CommentForProductApi.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<int?>("IdType")
                        .HasColumnType("integer")
                        .HasColumnName("id_type");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("name");

                    b.Property<byte[]>("Photo")
                        .HasColumnType("bytea")
                        .HasColumnName("photo");

                    b.HasKey("Id")
                        .HasName("product_pkey");

                    b.HasIndex("IdType");

                    b.ToTable("product", (string)null);
                });

            modelBuilder.Entity("CommentForProductApi.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("IdUser")
                        .HasColumnType("integer");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("CommentForProductApi.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("role_pkey");

                    b.ToTable("role", (string)null);
                });

            modelBuilder.Entity("CommentForProductApi.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("type_pkey");

                    b.ToTable("type", (string)null);
                });

            modelBuilder.Entity("CommentForProductApi.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("IdRefreshToken")
                        .HasColumnType("integer");

                    b.Property<int?>("IdRole")
                        .HasColumnType("integer")
                        .HasColumnName("id_role");

                    b.Property<int?>("IdUser")
                        .HasColumnType("integer");

                    b.Property<string>("Login")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("password");

                    b.Property<string>("Patronymic")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("patronymic");

                    b.Property<string>("Surname")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("surname");

                    b.HasKey("Id")
                        .HasName("user_pkey");

                    b.HasIndex("IdRole");

                    b.HasIndex("IdUser");

                    b.ToTable("user", (string)null);
                });

            modelBuilder.Entity("CommentForProductApi.Models.Comment", b =>
                {
                    b.HasOne("CommentForProductApi.Models.Product", "IdProductNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("IdProduct")
                        .HasConstraintName("comment_id_product_fkey");

                    b.HasOne("CommentForProductApi.Models.User", "IdUserNavigation")
                        .WithMany("Comments")
                        .HasForeignKey("IdUser")
                        .HasConstraintName("comment_id_user_fkey");

                    b.Navigation("IdProductNavigation");

                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("CommentForProductApi.Models.Product", b =>
                {
                    b.HasOne("CommentForProductApi.Models.Type", "IdTypeNavigation")
                        .WithMany("Products")
                        .HasForeignKey("IdType")
                        .HasConstraintName("product_id_type_fkey");

                    b.Navigation("IdTypeNavigation");
                });

            modelBuilder.Entity("CommentForProductApi.Models.User", b =>
                {
                    b.HasOne("CommentForProductApi.Models.Role", "IdRoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("IdRole")
                        .HasConstraintName("user_id_role_fkey");

                    b.HasOne("CommentForProductApi.Models.RefreshToken", "IdRefreshTokenNavigation")
                        .WithMany("IdUserNavigation")
                        .HasForeignKey("IdUser");

                    b.Navigation("IdRefreshTokenNavigation");

                    b.Navigation("IdRoleNavigation");
                });

            modelBuilder.Entity("CommentForProductApi.Models.Product", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("CommentForProductApi.Models.RefreshToken", b =>
                {
                    b.Navigation("IdUserNavigation");
                });

            modelBuilder.Entity("CommentForProductApi.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("CommentForProductApi.Models.Type", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("CommentForProductApi.Models.User", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}