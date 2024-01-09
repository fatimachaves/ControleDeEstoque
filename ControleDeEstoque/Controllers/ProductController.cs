using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleDeEstoque.Models;
using ControleDeEstoque.MyConnections;
using OfficeOpenXml;
using Microsoft.AspNetCore.Http;

namespace ControleDeEstoque.Controllers
{
    public class ProductController : Controller
    {
        private readonly AplicationDBContext _context;

        public ProductController(AplicationDBContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Tipo,ValordeCompra,ValordeVenda,Codigodebarra,Fornecedor,Quantidade")] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = await _context.Products.FindAsync(id);
            if (productViewModel == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Tipo,ValordeCompra,ValordeVenda,Codigodebarra,Fornecedor,Quantidade")] ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductViewModelExists(productViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productViewModel = await _context.Products.FindAsync(id);
            if (productViewModel != null)
            {
                _context.Products.Remove(productViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductViewModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public IActionResult SearchByBarcode(string barcode)
        {
            return View(_context.Products.Where(x => x.Codigodebarra == barcode));
        }

        public IActionResult ExportToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var listProducts = _context.Products.ToList();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Lista de Produtos");
                worksheet.Cells[1, 1].Value = "Nome";
                worksheet.Cells[1, 2].Value = "Tipo";
                worksheet.Cells[1, 3].Value = "ValordeCompra";
                worksheet.Cells[1, 4].Value = "ValordeVenda";
                worksheet.Cells[1, 5].Value = "Codigodebarra";
                worksheet.Cells[1, 6].Value = "Fornecedor";
                worksheet.Cells[1, 7].Value = "Quantidade";

                for (int i = 0; i < listProducts.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = listProducts[i].Nome;
                    worksheet.Cells[i + 2, 2].Value = listProducts[i].Tipo;
                    worksheet.Cells[i + 2, 3].Value = listProducts[i].ValordeCompra;
                    worksheet.Cells[i + 2, 4].Value = listProducts[i].ValordeVenda;
                    worksheet.Cells[i + 2, 5].Value = listProducts[i].Codigodebarra;
                    worksheet.Cells[i + 2, 6].Value = listProducts[i].Fornecedor;
                    worksheet.Cells[i + 2, 7].Value = listProducts[i].Quantidade;
                }

                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);

                    string fileName = $"ListadeProdutos_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";

                    // Retorne o arquivo Excel como um FileResult
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        public IActionResult ImportProducts(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("arquivo", "Por favor, envie um arquivo.");
                return BadRequest(ModelState);
            }

            // Verifique se o arquivo é um arquivo Excel válido
            if (!file.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("arquivo", "Por favor, envie um arquivo Excel válido (.xlsx).");
                return BadRequest(ModelState);
            }
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    // Obtenha a planilha desejada (por nome ou índice)
                    var worksheet = package.Workbook.Worksheets[0]; 

                    // Ler dados da planilha
                    List<ProductViewModel> listProducts = new List<ProductViewModel>();
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        ProductViewModel productViewModel = new ProductViewModel();
                        productViewModel.Nome = worksheet.Cells[row, 1].Value?.ToString();
                        productViewModel.Tipo = worksheet.Cells[row, 2].Value?.ToString();
                        productViewModel.ValordeCompra = float.Parse(worksheet.Cells[row, 3].Value?.ToString());
                        productViewModel.ValordeVenda = float.Parse(worksheet.Cells[row, 4].Value?.ToString());
                        productViewModel.Codigodebarra = worksheet.Cells[row, 5].Value?.ToString();
                        productViewModel.Fornecedor = worksheet.Cells[row, 6].Value?.ToString();
                        productViewModel.Quantidade = int.Parse(worksheet.Cells[row, 7].Value?.ToString());
                        listProducts.Add(productViewModel);
                    }

                    InsertListProductInBase(listProducts);

                    return Ok("Dados importados com sucesso!");
                }
            }
        }

        private void InsertListProductInBase(List<ProductViewModel> listProducts)
        {
            foreach(var e in listProducts)
            {
                _context.Products.Add(e);
                _context.SaveChanges();
            }
        }
    }
}
