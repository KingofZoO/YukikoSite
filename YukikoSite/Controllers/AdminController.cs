using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using YukikoSite.Models;
using YukikoSite.Models.Account;

namespace YukikoSite.Controllers {
    [Authorize(Roles = RoleTypes.AdminRole)]
    public class AdminController : Controller {
        private AppDbContext dbContext { get; set; }
        private readonly IWebHostEnvironment hostingEnvironment;

        public AdminController(AppDbContext dbContext, IWebHostEnvironment hostingEnvironment) {
            this.dbContext = dbContext;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Route("choosecontent")]
        public IActionResult ChooseContent() => View();

        #region ToChangeMethods
        [Route("glovestochange")]
        public async Task<IActionResult> GlovesToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<GlovesItem>(pageSize, page, "glovestochange"));
        }

        [Route("sidingtochange")]
        public async Task<IActionResult> SidingToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<SidingItem>(pageSize, page, "sidingtochange"));
        }

        [Route("ventilationtochange")]
        public async Task<IActionResult> VentilationToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<VentilationItem>(pageSize, page, "ventilationtochange"));
        }

        [Route("otherstochange")]
        public async Task<IActionResult> OthersToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<OthersItem>(pageSize, page, "otherstochange"));
        }

        [Route("gallerytochange")]
        public async Task<IActionResult> GalleryToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 20;
            return View(await PreparePagedDataAsync<GalleryItem>(pageSize, page, "gallerytochange"));
        }

        [Route("newstochange")]
        public async Task<IActionResult> NewsToChange(int page = 1) {
            ValidatePageId(ref page);
            int pageSize = 5;
            return View(await PreparePagedDataAsync<NewsItem>(pageSize, page, "newstochange"));
        }

        private async Task<List<T>> PreparePagedDataAsync<T>(int pageSize, int currentPage, string mapPath) where T : class {
            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;
            ViewBag.totalCount = await dbContext.GetCountAsync<T>();
            ViewBag.mapPath = mapPath;

            return await dbContext.GetPagedDataAsync<T>(pageSize, currentPage);
        }

        private void ValidatePageId(ref int pageId) => pageId = pageId <= 0 ? 1 : pageId;

        #endregion

        #region HttpGet ChangeMethods
        [HttpGet]
        [Route("changegloves")]
        public async Task<IActionResult> ChangeGloves(int id = 0) => View(await PrepareChangeGetViewAsync<GlovesItem>(id));

        [HttpGet]
        [Route("changesiding")]
        public async Task<IActionResult> ChangeSiding(int id = 0) => View(await PrepareChangeGetViewAsync<SidingItem>(id));

        [HttpGet]
        [Route("changeventilation")]
        public async Task<IActionResult> ChangeVentilation(int id = 0) => View(await PrepareChangeGetViewAsync<VentilationItem>(id));

        [HttpGet]
        [Route("changeothers")]
        public async Task<IActionResult> ChangeOthers(int id = 0) => View(await PrepareChangeGetViewAsync<OthersItem>(id));

        [HttpGet]
        [Route("changegallery")]
        public async Task<IActionResult> ChangeGallery(int id = 0) {
            ViewBag.Id = id;
            if (id == 0)
                return View();
            else
                return View(await dbContext.GalleryItems.FirstAsync(g => g.Id == id));
        }

        [HttpGet]
        [Route("changenews")]
        public async Task<IActionResult> ChangeNews(int id = 0) {
            ViewBag.Id = id;
            if (id == 0)
                return View();
            else
                return View(await dbContext.NewsItems.Include(n => n.NewsContentItems).FirstAsync(n => n.Id == id));
        }

        private async Task<T> PrepareChangeGetViewAsync<T>(int id) where T : class, IModelItem {
            ViewBag.Id = id;
            return await dbContext.GetFirstOrDefaultAsync<T>(id);
        }

        #endregion

        #region HttpPost ChangeMethods
        [HttpPost]
        [Route("changegloves")]
        public async Task<IActionResult> ChangeGloves(GlovesItem gloves, IFormFile imageFile) => await PrepareChangePostViewAsync(gloves, imageFile, "gloves");

        [HttpPost]
        [Route("changesiding")]
        public async Task<IActionResult> ChangeSiding(SidingItem siding, IFormFile imageFile) => await PrepareChangePostViewAsync(siding, imageFile, "siding");

        [HttpPost]
        [Route("changeventilation")]
        public async Task<IActionResult> ChangeVentilation(VentilationItem ventilation, IFormFile imageFile) => await PrepareChangePostViewAsync(ventilation, imageFile, "ventilation");

        [HttpPost]
        [Route("changeothers")]
        public async Task<IActionResult> ChangeOthers(OthersItem others, IFormFile imageFile) => await PrepareChangePostViewAsync(others, imageFile, "others");

        [HttpPost]
        [Route("changegallery")]
        public async Task<IActionResult> ChangeGallery(GalleryItem galleryItem, IFormFile imageFile) {
            if (imageFile != null) {
                if (galleryItem.ImagePath != null && galleryItem.ImagePath != imageFile.FileName)
                    DeleteOldFile(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", galleryItem.ImagePath));

                galleryItem.ImagePath = imageFile.FileName;
                await SaveNewFileAsync(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", imageFile.FileName), imageFile);
            }

            if (galleryItem.Id == 0)
                dbContext.GalleryItems.Add(galleryItem);
            else
                dbContext.GalleryItems.Update(galleryItem);

            await dbContext.SaveChangesAsync();
            return RedirectToAction("gallerytochange");
        }

        [HttpPost]
        [Route("changenews")]
        public async Task<IActionResult> ChangeNews(NewsItem newsItem, IFormFile titleImageFile, IFormFile[] contentFiles) {
            bool isTitleDefined = titleImageFile != null;
            bool isContentDefined = contentFiles != null && contentFiles.Length != 0;

            if (isTitleDefined) {
                if (newsItem.TitleImagePath != null && newsItem.TitleImagePath != titleImageFile.FileName)
                    DeleteOldFile(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{newsItem.Id}", newsItem.TitleImagePath));

                newsItem.TitleImagePath = titleImageFile.FileName;
            }
            if(isContentDefined)
                foreach (var el in contentFiles)
                    newsItem.NewsContentItems.Add(new NewsContentItem { ItemPath = el.FileName });

            if (newsItem.Id == 0)
                dbContext.NewsItems.Add(newsItem);
            else
                dbContext.NewsItems.Update(newsItem);

            await dbContext.SaveChangesAsync(); // New entity id is set now and we can use it for organize file storage

            DirectoryInfo newsFolder = new DirectoryInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{newsItem.Id}"));
            if (!newsFolder.Exists)
                newsFolder.Create();

            if (isTitleDefined)
                await SaveNewFileAsync(Path.Combine(newsFolder.FullName, titleImageFile.FileName), titleImageFile);

            if (isContentDefined) {
                List<Task> tasks = new List<Task>();
                foreach (var el in contentFiles)
                    tasks.Add(Task.Run(async () => await SaveNewFileAsync(Path.Combine(newsFolder.FullName, el.FileName), el)));

                await Task.WhenAll(tasks);
            }

            return RedirectToAction("newstochange");
        }

        private async Task<IActionResult> PrepareChangePostViewAsync<T>(T entity, IFormFile imageFile, string mapPath) where T : class, IModelItem {
            if (imageFile != null) {
                if (entity.ImagePath != null && entity.ImagePath != imageFile.FileName)
                    DeleteOldFile(Path.Combine(hostingEnvironment.WebRootPath, "images", mapPath, entity.ImagePath));

                entity.ImagePath = imageFile.FileName;
                await SaveNewFileAsync(Path.Combine(hostingEnvironment.WebRootPath, "images", mapPath, imageFile.FileName), imageFile);
            }

            if (entity.Id == 0)
                dbContext.Add(entity);
            else
                dbContext.Update(entity);

            await dbContext.SaveChangesAsync();
            return RedirectToAction(mapPath + "tochange");
        }

        private async Task SaveNewFileAsync(string path, IFormFile file) {
            using (FileStream stream = new FileStream(path, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }
        }

        #endregion

        #region DeleteMethods
        [Route("deletegloves")]
        public async Task<IActionResult> DeleteGloves(int id) => await DeleteEntityAsync<GlovesItem>(id, "gloves");

        [Route("deletesiding")]
        public async Task<IActionResult> DeleteSiding(int id) => await DeleteEntityAsync<SidingItem>(id, "siding");

        [Route("deleteventilation")]
        public async Task<IActionResult> DeleteVentilation(int id) => await DeleteEntityAsync<VentilationItem>(id, "ventilation");

        [Route("deleteothers")]
        public async Task<IActionResult> DeleteOthers(int id) => await DeleteEntityAsync<OthersItem>(id, "others");

        [Route("deletegallery")]
        public async Task<IActionResult> DeleteGallery(int id) {
            GalleryItem galleryItem = dbContext.GalleryItems.First(g => g.Id == id);

            if (galleryItem.ImagePath != null)
                DeleteOldFile(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", galleryItem.ImagePath));

            dbContext.GalleryItems.Remove(galleryItem);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("gallerytochange");
        }

        [Route("deletenews")]
        public async Task<IActionResult> DeleteNews(int id) {
            NewsItem newsItem = dbContext.NewsItems.First(n => n.Id == id);

            DirectoryInfo newsFolder = new DirectoryInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{id}"));
            if (newsFolder.Exists)
                newsFolder.Delete(true);

            dbContext.NewsItems.Remove(newsItem);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("newstochange");
        }

        private async Task<IActionResult> DeleteEntityAsync<T>(int id, string mapPath) where T : class, IModelItem {
            T entity = await dbContext.GetFirstOrDefaultAsync<T>(id);
            if (entity.ImagePath != null)
                DeleteOldFile(Path.Combine(hostingEnvironment.WebRootPath, "images", mapPath, entity.ImagePath));

            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(mapPath + "tochange");
        }

        private void DeleteOldFile(string path) {
            FileInfo file = new FileInfo(path);
            if (file.Exists)
                file.Delete();
        }

        #endregion
    }
}
