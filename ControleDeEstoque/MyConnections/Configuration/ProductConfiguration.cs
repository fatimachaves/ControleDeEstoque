using ControleDeEstoque.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeEstoque.MyConnections.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductViewModel>
    {
        public void Configure(EntityTypeBuilder<ProductViewModel> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(x => x.Tipo).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(x => x.ValordeCompra).HasColumnType("Decimal(18,2)").IsRequired();
            builder.Property(x => x.ValordeVenda).HasColumnType("Decimal(18,2)").IsRequired();
            builder.Property(x => x.Codigodebarra).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(x => x.Fornecedor).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(x => x.Quantidade).HasColumnType("Int").IsRequired();
        }
    }
}
