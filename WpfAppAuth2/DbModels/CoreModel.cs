using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfAppAuth2.DbModels;

public partial class CoreModel : DbContext
{
    private static CoreModel instanse;
    public static CoreModel init()
    {
        if (instanse == null)
        {
            instanse = new CoreModel();
        }
        return instanse;
    }

    public CoreModel()
    {
    }

    public CoreModel(DbContextOptions<CoreModel> options)
        : base(options)
    {
    }

    public virtual DbSet<Authorization> Authorizations { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<DocumentByService> DocumentByServices { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductPhoto> ProductPhotos { get; set; }

    public virtual DbSet<ProductSale> ProductSales { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServicePhoto> ServicePhotos { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=cfif31.ru;port=3306;user id=ISPr23-35_RogozhnikovaYO;database=ISPr23-35_RogozhnikovaYO_wpf;password=ISPr23-35_RogozhnikovaYO;character set=utf8", ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Authorization>(entity =>
        {
            entity.HasKey(e => e.IdAuthorization).HasName("PRIMARY");

            entity.ToTable("Authorization");

            entity.Property(e => e.IdAuthorization).HasColumnName("idAuthorization");
            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(45);
            entity.Property(e => e.Role).HasMaxLength(45);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Client");

            entity.HasIndex(e => e.GenderCode, "FK_Client_Gender");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.GenderCode)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate).HasMaxLength(6);

            entity.HasOne(d => d.GenderCodeNavigation).WithMany(p => p.Clients)
                .HasForeignKey(d => d.GenderCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Client_Gender");

            entity.HasMany(d => d.Tags).WithMany(p => p.Clients)
                .UsingEntity<Dictionary<string, object>>(
                    "TagOfClient",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TagOfClient_Tag"),
                    l => l.HasOne<Client>().WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TagOfClient_Client"),
                    j =>
                    {
                        j.HasKey("ClientId", "TagId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("TagOfClient");
                        j.HasIndex(new[] { "TagId" }, "FK_TagOfClient_Tag");
                        j.IndexerProperty<int>("ClientId").HasColumnName("ClientID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ClientService");

            entity.HasIndex(e => e.ClientId, "FK_ClientService_Client");

            entity.HasIndex(e => e.ServiceId, "FK_ClientService_Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.StartTime).HasMaxLength(6);

            entity.HasOne(d => d.Client).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientService_Client");

            entity.HasOne(d => d.Service).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientService_Service");
        });

        modelBuilder.Entity<DocumentByService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("DocumentByService");

            entity.HasIndex(e => e.ClientServiceId, "FK_DocumentByService_ClientService");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientServiceId).HasColumnName("ClientServiceID");
            entity.Property(e => e.DocumentPath).HasMaxLength(1000);

            entity.HasOne(d => d.ClientService).WithMany(p => p.DocumentByServices)
                .HasForeignKey(d => d.ClientServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentByService_ClientService");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("Gender");

            entity.Property(e => e.Code)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(10);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Manufacturer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Product");

            entity.HasIndex(e => e.ManufacturerId, "FK_Product_Manufacturer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasPrecision(19, 4);
            entity.Property(e => e.MainImagePath).HasMaxLength(1000);
            entity.Property(e => e.ManufacturerId).HasColumnName("ManufacturerID");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("FK_Product_Manufacturer");

            entity.HasMany(d => d.AttachedProducts).WithMany(p => p.MainProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "AttachedProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("AttachedProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AttachedProduct_Product1"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("MainProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AttachedProduct_Product"),
                    j =>
                    {
                        j.HasKey("MainProductId", "AttachedProductId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("AttachedProduct");
                        j.HasIndex(new[] { "AttachedProductId" }, "FK_AttachedProduct_Product1");
                        j.IndexerProperty<int>("MainProductId").HasColumnName("MainProductID");
                        j.IndexerProperty<int>("AttachedProductId").HasColumnName("AttachedProductID");
                    });

            entity.HasMany(d => d.MainProducts).WithMany(p => p.AttachedProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "AttachedProduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("MainProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AttachedProduct_Product"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("AttachedProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AttachedProduct_Product1"),
                    j =>
                    {
                        j.HasKey("MainProductId", "AttachedProductId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("AttachedProduct");
                        j.HasIndex(new[] { "AttachedProductId" }, "FK_AttachedProduct_Product1");
                        j.IndexerProperty<int>("MainProductId").HasColumnName("MainProductID");
                        j.IndexerProperty<int>("AttachedProductId").HasColumnName("AttachedProductID");
                    });
        });

        modelBuilder.Entity<ProductPhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductPhoto");

            entity.HasIndex(e => e.ProductId, "FK_ProductPhoto_Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PhotoPath).HasMaxLength(1000);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPhotos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductPhoto_Product");
        });

        modelBuilder.Entity<ProductSale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ProductSale");

            entity.HasIndex(e => e.ClientServiceId, "FK_ProductSale_ClientService");

            entity.HasIndex(e => e.ProductId, "FK_ProductSale_Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClientServiceId).HasColumnName("ClientServiceID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SaleDate).HasMaxLength(6);

            entity.HasOne(d => d.ClientService).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.ClientServiceId)
                .HasConstraintName("FK_ProductSale_ClientService");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductSales)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductSale_Product");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cost).HasPrecision(19, 4);
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<ServicePhoto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ServicePhoto");

            entity.HasIndex(e => e.ServiceId, "FK_ServicePhoto_Service");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PhotoPath).HasMaxLength(1000);
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicePhotos)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicePhoto_Service");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Tag");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.Title).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
