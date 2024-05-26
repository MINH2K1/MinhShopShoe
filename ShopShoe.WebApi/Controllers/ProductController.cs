﻿using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using ShopShoe.Application.Implement;
using ShopShoe.Application.Interface;
using ShopShoe.Application.ViewModel.Product;
using ShopShoe.Utilities.Helper;
using Microsoft.Net.Http.Headers;

namespace ShopShoe.WebApi.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductController : Controller
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IProductService productService,
            IProductCategoryService productCategoryService,
            IWebHostEnvironment hostingEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet("get-all-categories")]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet("getallpaging-{categoryId}-{keyword}-{page}-{pageSize}")]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet("getbyid-{id}")]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost("SaveEntity")]
        public IActionResult SaveEntity(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.Id == 0)
                {
                    _productService.Add(productVm);
                }
                else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
                return new OkObjectResult(productVm);
            }
        }

        [HttpDelete("id")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _productService.Delete(id);
                _productService.Save();

                return new OkObjectResult(id);
            }
        }
        [HttpPost("SaveQuantities")]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantities)
        {
            _productService.AddQuantity(productId, quantities);
            _productService.Save();
            return new OkObjectResult(quantities);
        }

        [HttpGet("GetQuantities")]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }
        [HttpPost("SaveImages")]
        public IActionResult SaveImages(int productId, string[] images)
        {
            _productService.AddImages(productId, images);
            _productService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet("GetImages/{productId}")]
        public IActionResult GetImages(int productId)
        {
            var images = _productService.GetImages(productId);
            return new OkObjectResult(images);
        }

        [HttpPost("SaveWholePrice")]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }

        [HttpGet("GetWholePrices")]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrices = _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }
        //[HttpPost]
        //public IActionResult ImportExcel(IList<IFormFile> files, int categoryId)
        //{
        //    if (files != null && files.Count > 0)
        //    {
        //        var file = files[0];
        //        var filename = ContentDispositionHeaderValue
        //                           .Parse(file.ContentDisposition)
        //                           .FileName
        //                           .Trim('"');

        //        string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        string filePath = Path.Combine(folder, filename);

        //        using (FileStream fs = System.IO.File.Create(filePath))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }
        //        _productService.ImportExcel(filePath, categoryId);
        //        _productService.Save();
        //        return new OkObjectResult(filePath);
        //    }
        //    return new NoContentResult();
        //}
        //[HttpPost]
        //public IActionResult ExportExcel()
        //{
        //    string sWebRootFolder = _hostingEnvironment.WebRootPath;
        //    string directory = Path.Combine(sWebRootFolder, "export-files");
        //    if (!Directory.Exists(directory))
        //    {
        //        Directory.CreateDirectory(directory);
        //    }
        //    string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
        //    string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
        //    FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
        //    if (file.Exists)
        //    {
        //        file.Delete();
        //        file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
        //    }
        //    var products = _productService.GetAll();
        //    using (ExcelPackage package = new ExcelPackage(file))
        //    {
        //        // add a new worksheet to the empty workbook
        //        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
        //        worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
        //        worksheet.Cells.AutoFitColumns();
        //        package.Save(); //Save the workbook.
        //    }
        //    return new OkObjectResult(fileUrl);
        //}
        
    }
}
