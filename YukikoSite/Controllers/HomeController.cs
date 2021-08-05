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
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<GlovesItem>(pageSize, page, "gloves"));
        }

        [Route("siding")]
        public async Task<IActionResult> Siding(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<SidingItem>(pageSize, page, "siding"));
        }

        [Route("ventilation")]
        public async Task<IActionResult> Ventilation(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<VentilationItem>(pageSize, page, "ventilation"));
        }

        [Route("others")]
        public async Task<IActionResult> Others(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<OthersItem>(pageSize, page, "others"));
        }

        [Route("gallery")]
        public async Task<IActionResult> Gallery(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<GalleryItem>(pageSize, page, "gallery"));
        }

        [Route("news")]
        public async Task<IActionResult> News(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 5;
            return View(await PreparePagedDataAsync<NewsItem>(pageSize, page, "news"));
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

        private async Task<List<T>> PreparePagedDataAsync<T>(int pageSize, int currentPage, string mapPath) where T : class {
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;
            ViewBag.totalCount = await dbContext.GetCountAsync<T>();
            ViewBag.mapPath = mapPath;

            return await dbContext.GetPagedDataAsync<T>(pageSize, currentPage);
        }

        private void ValidatePageId(ref int pageId) => pageId = pageId <= 0 ? 1 : pageId;
    }
}
