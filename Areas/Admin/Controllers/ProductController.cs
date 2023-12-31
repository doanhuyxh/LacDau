﻿using Bogus;
using LacDau.Data;
using LacDau.Models;
using LacDau.Models.ProductVM;
using LacDau.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace LacDau.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        public ProductController(ApplicationDbContext context, ICommon common, IMemoryCache memoryCache, IConfiguration configuration)
        {
            _context = context;
            _memoryCache = memoryCache;
            _icommon = common;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetById(int id)
        {
            ProductVM vm = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);
            vm.TrademarkName = _context.Trademark.FirstOrDefault(i => i.Id == vm.TrademarkId)?.Name ?? "";
            vm.CategoryName = _context.Category.FirstOrDefault(i => i.Id == vm.CategoryId)?.Name ?? "";
            vm.ProductImg = await _context.ProductImg.Where(i => i.IsDelete == false && i.ProductId == id).ToListAsync();
            return PartialView("Detail", vm);
        }
        public async Task<IActionResult> GetData()
        {
            JsonResultVM json = new JsonResultVM();

            //List<Product> products = await _context.Product.Include(p => p.ProductImg).Where(p => p.IsDeleted == false && p.ProductImg.All(pi => pi.IsDelete == false)).ToListAsync();
            List<ProductVM> products = (from p in _context.Product
                                       where p.IsDeleted == false
                                       select new ProductVM
                                       {
                                           Id = p.Id,
                                           Name = p.Name,
                                           CategoryName = _context.Category.FirstOrDefault(i=>i.Id== p.CategoryId).Name ??"",
                                           TrademarkName = _context.Trademark.FirstOrDefault(i=>i.Id==p.TrademarkId).Name ??"",
                                           CreatedDate = p.CreatedDate,
                                           Description = p.Description,
                                           IsActive = p.IsActive,
                                           IsHome = p.IsHome,
                                           Price = p.Price,
                                           Slug = p.Slug,
                                           Video = p.Video,
                                           ProductImg = _context.ProductImg.Where(i=>i.IsDelete==false && i.ProductId == p.Id).ToList()
                                       }).ToList();
            json.Message = string.Empty;
            json.StatusCode = 200;
            json.Object = products;
            return Ok(json);

            //var rs = _memoryCache.Get(_configuration["MemoriesCache:Product"]);
            //if (rs != null)
            //{
            //    json.Message = string.Empty;
            //    json.StatusCode = 200;
            //    json.Object = (List<Product>)rs;
            //    return Ok(json);
            //}
            //else
            //{
            //    List<Product> products = await _context.Product.Include(p => p.ProductImg).Where(p => p.IsDeleted == false && p.ProductImg.All(pi => pi.IsDelete == false)).ToListAsync();
            //    json.Message = string.Empty;
            //    json.StatusCode = 200;
            //    json.Object = products;
            //    _memoryCache.Set(_configuration["MemoriesCache:Product"], products);
            //    return Ok(json);
            //}
        }
        public IActionResult AddData()
        {
            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();

            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");

            List<ItemDropDown> itemBrand = (from _l in _context.Trademark
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();
            ViewBag.Brand = new SelectList(itemBrand, "Id", "Name");
            ProductVM vm = new ProductVM();
            return PartialView("AddEditData", vm);
        }

        public async Task<IActionResult> EditData(int id)
        {
            List<ItemDropDown> itemCate = (from _l in _context.Category
                                           where _l.IsDeleted == false
                                           select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();

            ViewBag.Cate = new SelectList(itemCate, "Id", "Name");

            List<ItemDropDown> itemBrand = (from _l in _context.Trademark
                                            where _l.IsDeleted == false
                                            select new ItemDropDown { Id = _l.Id, Name = _l.Name }).ToList();

            ProductVM vm = new ProductVM();
            vm = await _context.Product.FirstOrDefaultAsync(x => x.Id == id);

            return PartialView("AddEditData", vm);
        }

        public async Task<IActionResult> SaveData(ProductVM vm)
        {
            JsonResultVM json = new JsonResultVM();
            try
            {
                Product product = new Product();
                _memoryCache.Remove(_configuration["MemoriesCache:Product"]);
                if (vm.VideoFile != null)
                {
                    vm.Video = await _icommon.UploadFileVideoProductAsync(vm.VideoFile);
                }

                if (vm.Id != 0)
                {
                    product = await _context.Product.FirstOrDefaultAsync(i => i.Id == vm.Id);
                    vm.CreatedDate = product.CreatedDate;
                    _context.Entry(product).CurrentValues.SetValues(vm);
                    _context.SaveChanges();
                    json.StatusCode = 200;
                    json.Message = "";
                    json.Object = product;
                    return Ok(json);
                }
                else
                {
                    product = vm;
                    product.CreatedDate = DateTime.Now;
                    product.IsDeleted = false;
                    _context.Add(product);
                    _context.SaveChanges();

                    json.StatusCode = 200;
                    json.Message = "";
                    json.Object = product;
                    return Ok(json);
                }



            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.StatusCode = 500;
                json.Object = ex;
                return Ok(json);
            }
        }
        public IActionResult AddImg(int product_id)
        {
            UploadFileImgProduct up = new UploadFileImgProduct();
            up.ProductId = product_id;
            return PartialView("Add_Img", up);
        }
        public async Task<IActionResult> SaveImg(UploadFileImgProduct vm)
        {
            string ImgPath = await _icommon.UploadFileImgProductAsync(vm.FileName);
            ProductImg img = new ProductImg();
            img.ImgPath = ImgPath;
            img.IsDelete = false;
            img.ProductId = vm.ProductId;
            _context.Add(img);
            _context.SaveChanges();

            return Ok(img);
        }
        public async Task<IActionResult> DeleteImgProduct(int id)
        {
            ProductImg img = await _context.ProductImg.FirstOrDefaultAsync(i => i.Id == id);
            img.IsDelete = true;
            _context.Update(img);
            _context.SaveChanges();
            _memoryCache.Remove(_configuration["MemoriesCache:Product"]);
            return Ok(img);
        }

        public async Task<IActionResult> DeleteData(int id)
        {
            Product pr = await _context.Product.FirstOrDefaultAsync(i => i.Id == id);
            pr.IsDeleted = true;
            _context.Update(pr);
            _context.SaveChanges();
            _memoryCache.Remove(_configuration["MemoriesCache:Product"]);
            return Ok();
        }

        public async Task<IActionResult> ChangeHome(int id)
        {
            Product pr = await _context.Product.FirstOrDefaultAsync(i => i.Id == id);
            pr.IsHome = !pr.IsHome;
            _context.Update(pr);
            _context.SaveChanges();
            _memoryCache.Remove(_configuration["MemoriesCache:Product"]);
            return Ok();
        }
        public async Task<IActionResult> ChangeActive(int id)
        {
            Product pr = await _context.Product.FirstOrDefaultAsync(i => i.Id == id);
            pr.IsActive = !pr.IsActive;
            _context.Update(pr);
            _context.SaveChanges();
            _memoryCache.Remove(_configuration["MemoriesCache:Product"]);
            return Ok();
        }

        public async Task<IActionResult> AddFakeProducts()
        {
            var cate = await _context.Category.Where(i => i.ParentId == 0 && i.IsDeleted == false).ToListAsync();

            foreach (var c in cate)
            {
                var fakeProductGenerator = new Faker<Product>()
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                        .RuleFor(p => p.Slug, (f, p) => _icommon.GenerateSlug(f.Commerce.ProductName()))
                        .RuleFor(p => p.TrademarkId, 1)
                        .RuleFor(p => p.CreatedDate, f=>DateTime.Now)
                        .RuleFor(p => p.Video, "/upload/productVideo/638372449822487778.mp4")
                        .RuleFor(p => p.IsActive, true)
                        .RuleFor(p => p.IsDeleted, false)
                        .RuleFor(p => p.IsHome, true)
                        .RuleFor(p => p.Price, f => f.Random.Decimal(10, 100).ToString())
                        .RuleFor(p => p.CategoryId, f => c.Id);

                List<Product> fakeProducts = fakeProductGenerator.Generate(10);

                _context.Product.AddRange(fakeProducts);
            }

            _context.SaveChanges();
            return Ok();
        }

        public async Task<IActionResult> AddFakeProductsImg()
        {
            var pr = _context.Product.Where(i=>i.IsDeleted == false).ToList();

            foreach (var item in pr)
            {
                var fackeImg = new Faker<ProductImg>()
                    .RuleFor(i => i.ProductId, f => item.Id)
                    .RuleFor(i => i.ImgPath, f => "https://lacdau.com/media/product/250-1675-z4258711824198_cbb201570adbb2f16190d016401f1162.jpg")
                    .RuleFor(i => i.IsDelete, false);

                List<ProductImg> list = fackeImg.Generate(5);
                _context.ProductImg.AddRange(fackeImg);
            }
            _context.SaveChanges();

            return Ok();
        }

        public IActionResult UpdateSlug()
        {
            var pr = _context.Product.ToList();

            foreach (var item in pr)
            {
                //item.Slug = _icommon.GenerateSlug(item.Name);
                item.IsHome = false;
                _context.Update(item);
            }

            _context.SaveChanges();


            return Ok();
        }
    }
}
