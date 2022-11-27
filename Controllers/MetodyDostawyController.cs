﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Controllers
{
    [Authorize]
    public class MetodyDostawyController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IMetodyDostawy _metodyDostawy;

        public MetodyDostawyController(ApplicationDbContext context, IMetodyDostawy metodyDostawy)
        {
            _context = context;
            _metodyDostawy = metodyDostawy;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<MetodyDostawy> metodyDostawy =  _context.MetodyDostawy.ToList();
            return View(metodyDostawy);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MetodyDostawy metodaDostawy = new MetodyDostawy();
            return View(metodaDostawy);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MetodyDostawy metodaDostawy)
        {
            _context.MetodyDostawy.Add(metodaDostawy);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            MetodyDostawy metodaDostawy = _context.MetodyDostawy.Where(x => x.Id == id).FirstOrDefault();
            return View(metodaDostawy);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MetodyDostawy metodaDostawy)
        {
            await _metodyDostawy.Update(metodaDostawy.Id);
            return View(metodaDostawy);
        }

        public void Delete(int id)
        {
            _metodyDostawy.Delete(id);
        }

    }


}
