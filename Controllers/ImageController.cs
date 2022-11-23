using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public readonly IWebHostEnvironment _hostEnvironment;

        public readonly IImageService _imageService;

        public ImageController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IImageService imageServer)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _imageService = imageServer;
        }

        // GET: Image
        public async Task<IActionResult> Index()
        {
            //List<ImageModel> lista1 = await _context.Images.ToListAsync();

            //for (int i = 0; i < lista2.Count(); i++)
            //{

            //    var img = _context.Images.Where(x => x.ImageId == lista2[i].ImageId).FirstOrDefault();
            //    if (img != null)
            //    { 
            //    _context.Images.Remove(img);
            //    _context.SaveChanges();
            //    }
            //}
            //List<ImageModel> lista = await _context.Images.ToListAsync();

            //for (int i = 0; i < lista.Count; i++)
            //{

            //    string sz = lista[i].path;
            //    if (sz[sz.Length - 1] == '\\')
            //    {
            //    }
            //    else
            //    {
            //        //lista[i].path = lista[i].path + '\\';
            //        ImageModel image = await _context.Images.Where(x => x.ImageId == lista[i].ImageId).FirstOrDefaultAsync();
            //        image.path = image.path + '\\';
            //        _context.Update(image);
            //        _context.SaveChanges();
            //    }

            //}
            //return View(lista2);


            //await _context.Images.OrderByDescending(x => x.ImageId).Take(1000).ToListAsync();

            //List<ImageModel> lista = _context.Images.TakeLast(100).ToList();
            return View(await _context.Images.OrderByDescending(x => x.ImageId).Take(1000).ToListAsync());
        }
        public async Task<IActionResult> GetFileServer()
        {
            //String path = Server.MapPath("~/images/"); // get the server path images folder


            string wwwRootPath = _hostEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/images/");

            String[] imagesfiles = Directory.GetFiles(path); //get all file from path
            ViewBag.images = imagesfiles;

            return View();
        }

        // GET: Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageModel imageModel)
        {
            ModelState.Remove("Product");

            if (ModelState.IsValid)
            {
                //Sabe image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName =  fileName = fileName +"_"+ DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName); 
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                //insert record
                imageModel.path = "images\\";
                imageModel.fullPath = "images\\" + fileName;
                _context.Add(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // POST: Product/CreateImageProductFront
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> CreateImageProductFront(Product product)
        {
            ModelState.Remove("Product");

            if (ModelState.IsValid)
            {
                //Sabe image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.product_Image.ImageFile.FileName);
                string extension = Path.GetExtension(product.product_Image.ImageFile.FileName);
                product.product_Image.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/produkty", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.product_Image.ImageFile.CopyToAsync(fileStream);
                }
                //insert record

                //migajace
                //przenika



                _context.Add(product.product_Image);
                await _context.SaveChangesAsync();

                return RedirectToAction("Add", "Product", new { Product = product });
                //return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Add", "Product", new { Product = product });
            //return View(product.product_Image);
        }

        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        // GET: Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            return View(imageModel);
        }


        // POST: Image/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Edit(int id, ImageModel imageModel)
        {
            if (id != imageModel.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _imageService.Update(imageModel);

                    //await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.ImageId))
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
            return View(imageModel);
        }

        // GET: Image/Delete/5
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            _context.Images.Remove(imageModel);
            _context.SaveChanges();

            return View(imageModel);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageModel = await _context.Images.FindAsync(id);

            //delete image from wwwroot/images
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, imageModel.path, imageModel.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //delete tge record




            if (_context.Images == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Images'  is null.");
            }
            if (imageModel != null)
            {
                _context.Images.Remove(imageModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageModelExists(int id)
        {
          return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
