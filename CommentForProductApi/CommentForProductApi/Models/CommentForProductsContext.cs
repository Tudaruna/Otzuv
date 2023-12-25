using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CommentForProductApi.Models;

public partial class CommentForProductsContext : DbContext
{
    public CommentForProductsContext()
    {
    }

    public CommentForProductsContext(DbContextOptions<CommentForProductsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Type> Types { get; set; }

    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<RefreshToken> RefreshTokens {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Database=comment_for_products;Username=postgres;Password=2347");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comment_pkey");

            entity.ToTable("comment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Photo).HasColumnName("photo");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TextComment)
                .HasMaxLength(1000)
                .HasColumnName("text_comment");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("comment_id_product_fkey");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("comment_id_user_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.IdType).HasColumnName("id_type");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Photo).HasColumnName("photo");

            entity.HasOne(d => d.IdTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdType)
                .HasConstraintName("product_id_type_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Type>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("type_pkey");

            entity.ToTable("type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(50)
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("user_id_role_fkey");
            
         
        });
        
  
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
