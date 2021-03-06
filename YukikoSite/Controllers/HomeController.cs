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

        [Route("sidingall")]
        public IActionResult SidingAll() => View("Siding/SidingAll");

        [Route("siding")]
        public async Task<IActionResult> Siding(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View("Siding/Siding", await PreparePagedDataAsync<SidingItem>(pageSize, page, "siding"));
        }

        [Route("sidingcomplect")]
        public async Task<IActionResult> SidingComplect(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View("Siding/SidingComplect", await PreparePagedDataAsync<SidingComplectItem>(pageSize, page, "sidingcomplect"));
        }

        [Route("fibrosall")]
        public IActionResult FibrosAll() => View("Fibros/FibrosAll");

        [Route("fibros14")]
        public async Task<IActionResult> Fibros14(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View("Fibros/Fibros14", await PreparePagedDataAsync<Fibro14Item>(pageSize, page, "fibros14"));
        }

        [Route("fibros16")]
        public async Task<IActionResult> Fibros16(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View("Fibros/Fibros16", await PreparePagedDataAsync<Fibro16Item>(pageSize, page, "fibros16"));
        }

        [Route("fibroscomplect")]
        public async Task<IActionResult> FibrosComplect(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View("Fibros/FibrosComplect", await PreparePagedDataAsync<FibroComplectItem>(pageSize, page, "fibroscomplect"));
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
            await PrepareViewBagData<NewsItem>(pageSize, page, "news");
            return View(await dbContext.NewsItems.OrderByDescending(n => n.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("newscontent")]
        public IActionResult NewsContent(int id) {
            NewsItem newsItem = dbContext.NewsItems.Include(n => n.NewsContentItems).FirstOrDefault(n => n.Id == id);
            if (newsItem == null)
                return RedirectToAction("news");

            return View(newsItem);
        }

        [Route("company")]
        public IActionResult Company() => View();

        [Route("aboutsiding")]
        public IActionResult AboutSiding() => View();

        [Route("map")]
        public IActionResult Map() => View();

        private async Task<List<T>> PreparePagedDataAsync<T>(int pageSize, int currentPage, string mapPath) where T : class {
            await PrepareViewBagData<T>(pageSize, currentPage, mapPath);
            return await dbContext.GetPagedDataAsync<T>(pageSize, currentPage);
        }

        private async Task PrepareViewBagData<T>(int pageSize, int currentPage, string mapPath) where T : class {
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;
            ViewBag.totalCount = await dbContext.GetCountAsync<T>();
            ViewBag.mapPath = mapPath;
        }

        private void ValidatePageId(ref int pageId) => pageId = pageId <= 0 ? 1 : pageId;
    }
}
