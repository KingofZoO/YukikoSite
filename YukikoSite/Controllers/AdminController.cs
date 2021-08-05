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
            if (page <= 0)
                page = 1;

            int pageSize = 40;
            await SetPaginator(pageSize, page, "glovestochange");
            return View(await dbContext.Gloves.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("sidingtochange")]
        public async Task<IActionResult> SidingToChange(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 40;
            await SetPaginator(pageSize, page, "sidingtochange");
            return View(await dbContext.Siding.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("ventilationtochange")]
        public async Task<IActionResult> VentilationToChange(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 40;
            await SetPaginator(pageSize, page, "ventilationtochange");
            return View(await dbContext.Ventilation.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("otherstochange")]
        public async Task<IActionResult> OthersToChange(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 40;
            await SetPaginator(pageSize, page, "otherstochange");
            return View(await dbContext.Others.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("gallerytochange")]
        public async Task<IActionResult> GalleryToChange(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 40;
            await SetPaginator(pageSize, page, "gallerytochange");
            return View(await dbContext.GalleryItems.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        [Route("newstochange")]
        public async Task<IActionResult> NewsToChange(int page = 1) {
            if (page <= 0)
                page = 1;

            int pageSize = 10;
            await SetPaginator(pageSize, page, "newstochange");
            return View(await dbContext.NewsItems.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync());
        }

        private async Task SetPaginator(int pageSize, int currentPage, string mapPath) {
            int totalCount = 0;
            switch (mapPath) {
                case "glovestochange":
                    totalCount = await dbContext.Gloves.CountAsync();
                    break;
                case "sidingtochange":
                    totalCount = await dbContext.Siding.CountAsync();
                    break;
                case "ventilationtochange":
                    totalCount = await dbContext.Ventilation.CountAsync();
                    break;
                case "otherstochange":
                    totalCount = await dbContext.Others.CountAsync();
                    break;
                case "gallerytochange":
                    totalCount = await dbContext.GalleryItems.CountAsync();
                    break;
                case "newstochange":
                    totalCount = await dbContext.NewsItems.CountAsync();
                    break;
            }

            ViewBag.pageSize = pageSize;
            ViewBag.currentPage = currentPage;
            ViewBag.totalCount = totalCount;
            ViewBag.mapPath = mapPath;
        }

        #endregion

        #region HttpGet ChangeMethods
        [HttpGet]
        [Route("changegloves")]
        public IActionResult ChangeGloves(int id = 0) {
            ViewBag.GlovesId = id;
            if (id == 0)
                return View();
            else {
                GlovesItem gloves = dbContext.Gloves.First(g => g.Id == id);
                return View(gloves);
            }
        }

        [HttpGet]
        [Route("changesiding")]
        public IActionResult ChangeSiding(int id = 0) {
            ViewBag.SidingId = id;
            if (id == 0)
                return View();
            else {
                SidingItem siding = dbContext.Siding.First(g => g.Id == id);
                return View(siding);
            }
        }

        [HttpGet]
        [Route("changeventilation")]
        public IActionResult ChangeVentilation(int id = 0) {
            ViewBag.VentilationId = id;
            if (id == 0)
                return View();
            else {
                VentilationItem ventilation = dbContext.Ventilation.First(g => g.Id == id);
                return View(ventilation);
            }
        }

        [HttpGet]
        [Route("changeothers")]
        public IActionResult ChangeOthers(int id = 0) {
            ViewBag.OthersId = id;
            if (id == 0)
                return View();
            else {
                OthersItem others = dbContext.Others.First(g => g.Id == id);
                return View(others);
            }
        }

        [HttpGet]
        [Route("changegallery")]
        public IActionResult ChangeGallery(int id = 0) {
            ViewBag.GalleryId = id;
            if (id == 0)
                return View();
            else {
                GalleryItem galleryItem = dbContext.GalleryItems.First(g => g.Id == id);
                return View(galleryItem);
            }
        }

        [HttpGet]
        [Route("changenews")]
        public IActionResult ChangeNews(int id = 0) {
            ViewBag.NewsId = id;
            if (id == 0)
                return View();
            else {
                NewsItem newsItem = dbContext.NewsItems.Include(n => n.NewsContentItems).First(g => g.Id == id);
                return View(newsItem);
            }
        }

        #endregion

        #region HttpPost ChangeMethods
        [HttpPost]
        [Route("changegloves")]
        public async Task<IActionResult> ChangeGloves(GlovesItem gloves, IFormFile imageFile) {
            if (imageFile != null) {
                if(gloves.ImagePath != null && gloves.ImagePath != imageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "gloves", gloves.ImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }

                gloves.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "gloves", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (gloves.Id == 0)
                dbContext.Gloves.Add(gloves);
            else
                dbContext.Gloves.Update(gloves);

            dbContext.SaveChanges();
            return RedirectToAction("glovestochange");
        }

        [HttpPost]
        [Route("changesiding")]
        public async Task<IActionResult> ChangeSiding(SidingItem siding, IFormFile imageFile) {
            if (imageFile != null) {
                if (siding.ImagePath != null && siding.ImagePath != imageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "siding", siding.ImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }

                siding.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "siding", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (siding.Id == 0)
                dbContext.Siding.Add(siding);
            else
                dbContext.Siding.Update(siding);

            dbContext.SaveChanges();
            return RedirectToAction("sidingtochange");
        }

        [HttpPost]
        [Route("changeventilation")]
        public async Task<IActionResult> ChangeVentilation(VentilationItem ventilation, IFormFile imageFile) {
            if (imageFile != null) {
                if (ventilation.ImagePath != null && ventilation.ImagePath != imageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "ventilation", ventilation.ImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }

                ventilation.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "ventilation", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ventilation.Id == 0)
                dbContext.Ventilation.Add(ventilation);
            else
                dbContext.Ventilation.Update(ventilation);

            dbContext.SaveChanges();
            return RedirectToAction("ventilationtochange");
        }

        [HttpPost]
        [Route("changeothers")]
        public async Task<IActionResult> ChangeOthers(OthersItem others, IFormFile imageFile) {
            if (imageFile != null) {
                if (others.ImagePath != null && others.ImagePath != imageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "others", others.ImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }

                others.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "others", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (others.Id == 0)
                dbContext.Others.Add(others);
            else
                dbContext.Others.Update(others);

            dbContext.SaveChanges();
            return RedirectToAction("otherstochange");
        }

        [HttpPost]
        [Route("changegallery")]
        public async Task<IActionResult> ChangeGallery(GalleryItem galleryItem, IFormFile imageFile) {
            if (imageFile != null) {
                if (galleryItem.ImagePath != null && galleryItem.ImagePath != imageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", galleryItem.ImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }

                galleryItem.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (galleryItem.Id == 0)
                dbContext.GalleryItems.Add(galleryItem);
            else
                dbContext.GalleryItems.Update(galleryItem);

            dbContext.SaveChanges();
            return RedirectToAction("gallerytochange");
        }

        [HttpPost]
        [Route("changenews")]
        public async Task<IActionResult> ChangeNews(NewsItem newsItem, IFormFile titleImageFile, IFormFile[] contentFiles) {
            bool isTitleDefined = titleImageFile != null;
            bool isContentDefined = contentFiles != null && contentFiles.Length != 0;

            if (isTitleDefined) {
                if (newsItem.TitleImagePath != null && newsItem.TitleImagePath != titleImageFile.FileName) {
                    FileInfo oldImage = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{newsItem.Id}", newsItem.TitleImagePath));
                    if (oldImage.Exists)
                        oldImage.Delete();
                }
                newsItem.TitleImagePath = titleImageFile.FileName;
            }
            if(isContentDefined) {
                foreach (var el in contentFiles)
                    newsItem.NewsContentItems.Add(new NewsContentItem {
                        ItemPath = el.FileName
                    });
            }

            if (newsItem.Id == 0)
                dbContext.NewsItems.Add(newsItem);
            else
                dbContext.NewsItems.Update(newsItem);

            dbContext.SaveChanges(); // New entity id is set now and we can use it for organize file storage

            DirectoryInfo newsFolder = new DirectoryInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{newsItem.Id}"));
            if (!newsFolder.Exists)
                newsFolder.Create();

            if (isTitleDefined) {
                using (FileStream stream = new FileStream(Path.Combine(newsFolder.FullName, titleImageFile.FileName), FileMode.Create)) {
                    await titleImageFile.CopyToAsync(stream);
                }
            }
            if (isContentDefined) {
                List<Task> tasks = new List<Task>();
                foreach (var el in contentFiles) {
                    tasks.Add(Task.Run(async () => {
                        using (FileStream stream = new FileStream(Path.Combine(newsFolder.FullName, el.FileName), FileMode.Create)) {
                            await el.CopyToAsync(stream);
                        }
                    }));
                }
                await Task.WhenAll(tasks);
            }

            return RedirectToAction("newstochange");
        }

        #endregion

        #region DeleteMethods
        [Route("deletegloves")]
        public IActionResult DeleteGloves(int id) {
            GlovesItem gloves = dbContext.Gloves.First(g => g.Id == id);

            if (gloves.ImagePath != null) {
                FileInfo imageFile = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "gloves", gloves.ImagePath));
                if (imageFile.Exists)
                    imageFile.Delete();
            }

            dbContext.Gloves.Remove(gloves);
            dbContext.SaveChanges();
            return RedirectToAction("glovestochange");
        }

        [Route("deletesiding")]
        public IActionResult DeleteSiding(int id) {
            SidingItem siding = dbContext.Siding.First(g => g.Id == id);

            if (siding.ImagePath != null) {
                FileInfo imageFile = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "siding", siding.ImagePath));
                if (imageFile.Exists)
                    imageFile.Delete();
            }

            dbContext.Siding.Remove(siding);
            dbContext.SaveChanges();
            return RedirectToAction("sidingtochange");
        }

        [Route("deleteventilation")]
        public IActionResult DeleteVentilation(int id) {
            VentilationItem ventilation = dbContext.Ventilation.First(g => g.Id == id);

            if (ventilation.ImagePath != null) {
                FileInfo imageFile = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "ventilation", ventilation.ImagePath));
                if (imageFile.Exists)
                    imageFile.Delete();
            }

            dbContext.Ventilation.Remove(ventilation);
            dbContext.SaveChanges();
            return RedirectToAction("ventilationtochange");
        }

        [Route("deleteothers")]
        public IActionResult DeleteOthers(int id) {
            OthersItem others = dbContext.Others.First(g => g.Id == id);

            if (others.ImagePath != null) {
                FileInfo imageFile = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "others", others.ImagePath));
                if (imageFile.Exists)
                    imageFile.Delete();
            }

            dbContext.Others.Remove(others);
            dbContext.SaveChanges();
            return RedirectToAction("otherstochange");
        }

        [Route("deletegallery")]
        public IActionResult DeleteGallery(int id) {
            GalleryItem galleryItem = dbContext.GalleryItems.First(g => g.Id == id);

            if (galleryItem.ImagePath != null) {
                FileInfo imageFile = new FileInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", galleryItem.ImagePath));
                if (imageFile.Exists)
                    imageFile.Delete();
            }

            dbContext.GalleryItems.Remove(galleryItem);
            dbContext.SaveChanges();
            return RedirectToAction("gallerytochange");
        }

        [Route("deletenews")]
        public IActionResult DeleteNews(int id) {
            NewsItem newsItem = dbContext.NewsItems.First(g => g.Id == id);

            DirectoryInfo newsFolder = new DirectoryInfo(Path.Combine(hostingEnvironment.WebRootPath, "images", "news", $"{id}"));
            if (newsFolder.Exists)
                newsFolder.Delete(true);

            dbContext.NewsItems.Remove(newsItem);
            dbContext.SaveChanges();
            return RedirectToAction("newstochange");
        }

        #endregion
    }
}
