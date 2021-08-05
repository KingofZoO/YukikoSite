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
        public async Task<IActionResult> Gloves(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 20;
            await SetPaginator(pageSize, page, "gloves");
            return View(await dbContext.Gloves.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("siding")]
        public async Task<IActionResult> Siding(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 20;
            await SetPaginator(pageSize, page, "siding");
            return View(await dbContext.Siding.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("ventilation")]
        public async Task<IActionResult> Ventilation(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 20;
            await SetPaginator(pageSize, page, "ventilation");
            return View(await dbContext.Ventilation.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("others")]
        public async Task<IActionResult> Others(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 20;
            await SetPaginator(pageSize, page, "others");
            return View(await dbContext.Others.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("gallery")]
        public async Task<IActionResult> Gallery(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 20;
            await SetPaginator(pageSize, page, "gallery");
            return View(await dbContext.GalleryItems.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("news")]
        public async Task<IActionResult> News(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 5;
            await SetPaginator(pageSize, page, "news");
            return View(await dbContext.NewsItems.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("newscontent")]
        public IActionResult NewsContent(int id) {
            NewsItem newsItem = dbContext.NewsItems.Include(n => n.NewsContentItems).FirstOrDefault(n => n.Id == id);
            if (newsItem == null)
                return RedirectToAction("news");

            return View(newsItem);
        }

        [Route("map")]
        public IActionResult Map() => View();

        private async Task SetPaginator(int pageSize, int currentPage, string mapPath) {
            int totalCount = 0;
            switch (mapPath) {
                case "gloves":
                    totalCount = await dbContext.Gloves.CountAsync();
                    break;
                case "siding":
                    totalCount = await dbContext.Siding.CountAsync();
                    break;
                case "ventilation":
                    totalCount = await dbContext.Ventilation.CountAsync();
                    break;
                case "others":
                    totalCount = await dbContext.Others.CountAsync();
                    break;
                case "gallery":
                    totalCount = await dbContext.GalleryItems.CountAsync();
                    break;
                case "news":
                    totalCount = await dbContext.NewsItems.CountAsync();
                    break;
            }

            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;
            ViewBag.totalCount = totalCount;
            ViewBag.mapPath = mapPath;
        }
    }
}
