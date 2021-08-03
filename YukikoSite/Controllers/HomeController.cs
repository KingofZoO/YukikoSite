using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YukikoSite.Models;

namespace YukikoSite.Controllers {
    public class HomeController : Controller {
        private AppDbContext dbContext { get; set; }

        public HomeController(AppDbContext dbContext) {
            this.dbContext = dbContext;
        }

        [Route("~/")]
        public IActionResult Index() => View();

        [Route("gloves")]
        public IActionResult Gloves() => View(dbContext.Gloves);

        [Route("siding")]
        public IActionResult Siding() => View(dbContext.Siding);

        [Route("ventilation")]
        public IActionResult Ventilation() => View(dbContext.Ventilation);

        [Route("others")]
        public IActionResult Others() => View(dbContext.Others);

        [Route("gallery")]
        public IActionResult Gallery() => View(dbContext.GalleryItems);

        [Route("news")]
        public IActionResult News() => View(dbContext.NewsItems);

        [Route("newscontent")]
        public IActionResult NewsContent(int id) {
            NewsItem newsItem = dbContext.NewsItems.Include(n => n.NewsContentItems).FirstOrDefault(n => n.Id == id);
            if (newsItem == null)
                return RedirectToAction("news");

            return View(newsItem);
        }

        [Route("map")]
        public IActionResult Map() => View();
    }
}
