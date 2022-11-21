﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using NuGet.Protocol.Core.Types;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Linq;

namespace partner_aluro.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> CreateImageAddAsync(ImageModel imageModel)
        {
            string komunikat = "1";

            if (imageModel.ImageFile != null)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                //insert record

                _context.Add(imageModel);
                await _context.SaveChangesAsync();

                return wwwRootPath;
            }
            
            return komunikat;
        }
        public async Task<string> DeleteFrontImage(Product product) //Dodaj obrazek Front przy dodawaniu produktu
        {
            var imageModel = await _context.Images.FirstOrDefaultAsync(x=>x.ImageName == product.ImageUrl);






            if (imageModel != null && product.ImageUrl !=null)
            {
                //delete image from wwwroot/images
                var imagePath = Path.Combine(imageModel.path, imageModel.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //delete tge record
            }

            if (imageModel != null)
            {
                _context.Images.Attach(imageModel);
                _context.Images.Remove(imageModel);
            }

            await _context.SaveChangesAsync();

            return "";
        }
        public async Task<string> CreateImageAddAsync(Product product) //Dodaj obrazek Front przy dodawaniu produktu
        {
            string uniqueFileName = "";

            if (product.product_Image.ImageFile != null)
            {

                //Save image to wwwroot/image
                string webRootPath = _webHostEnvironment.WebRootPath;
                string path0 = "images\\produkty\\";
                var uploadsFolder = Path.Combine(webRootPath, "images\\produkty\\" + product.Symbol);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(product.product_Image.ImageFile.FileName);

                uniqueFileName = "Front_" + product.Symbol + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                string fileName = Path.GetFileNameWithoutExtension(product.product_Image.ImageFile.FileName);
                //string extension = Path.GetExtension(product.product_Image.ImageFile.FileName);
                //product.product_Image.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //string path = Path.Combine(wwwRootPath + "/Images/", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await product.product_Image.ImageFile.CopyToAsync(fileStream);
                }
                //insert record
                product.product_Image.path = path0 + product.Symbol;
                product.product_Image.kolejnosc = 0;
                product.product_Image.ProductId = product.ProductId;
                product.product_Image.Tytul = product.Name;
                product.product_Image.ProductImagesId = product.ProductId;
                product.product_Image.ImageName = uniqueFileName;


                _context.Add(product.product_Image);
                await _context.SaveChangesAsync();

                return uniqueFileName;
            }

            return uniqueFileName;
        }

        public async Task<List<ImageModel>> ListImageAsync()
        {

            return await _context.Images.ToListAsync();
        }

        public void Add(ImageModel imgModel)
        {

            _context.Add(imgModel);
            _context.SaveChanges();
        }

        public async Task Edit(int id, ImageModel imageModel)
        {
            if (id != imageModel.ImageId)
            {
                try
                {
                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
        }

    }
}
