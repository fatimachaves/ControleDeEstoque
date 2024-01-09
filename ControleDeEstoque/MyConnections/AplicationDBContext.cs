using ControleDeEstoque.Models;
using ControleDeEstoque.MyConnections.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoque.MyConnections
{
    public class AplicationDBContext : DbContext
    {
        public AplicationDBContext() : base()
        {
        }
        public AplicationDBContext(DbContextOptions<AplicationDBContext> options) : base(options)
        {
        }
        public DbSet<ProductViewModel> Products {  get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=FC;Initial Catalog=ControleDeEstoque;Integrated Security=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.Entity<ProductViewModel>()
                .ToTable("Product")
                .HasKey(x => x.Id);

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.Nome)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.Tipo)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.ValordeCompra)
                .HasColumnType("Decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.ValordeVenda)
                .HasColumnType("Decimal(18,2)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.Codigodebarra)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.Fornecedor)
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            modelBuilder.Entity<ProductViewModel>()
                .Property(x => x.Quantidade)
                .HasColumnType("Int")
                .IsRequired();
        }

    }
}
