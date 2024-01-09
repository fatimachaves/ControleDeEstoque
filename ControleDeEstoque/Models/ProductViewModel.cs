using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ControleDeEstoque.Models
{
    public class ProductViewModel
    {
            [Key]
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Tipo { get; set; }
            public float ValordeCompra { get; set; }
            public float ValordeVenda { get; set; }
            public string Codigodebarra { get; set; }
            public string Fornecedor { get; set; }
            public int Quantidade { get; set; }
    }
}
