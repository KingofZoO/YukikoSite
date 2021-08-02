using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
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
        public IActionResult GlovesToChange() => View(dbContext.Gloves);

        [Route("sidingtochange")]
        public IActionResult SidingToChange() => View(dbContext.Siding);

        [Route("ventilationtochange")]
        public IActionResult VentilationToChange() => View(dbContext.Ventilation);

        [Route("otherstochange")]
        public IActionResult OthersToChange() => View(dbContext.Others);

        [Route("gallerytochange")]
        public IActionResult GalleryToChange() => View(dbContext.GalleryItems);
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
        #endregion

        #region HttpPost ChangeMethods
        [HttpPost]
        [Route("changegloves")]
        public async Task<IActionResult> ChangeGloves(GlovesItem gloves, IFormFile imageFile) {
            if (imageFile != null) {
                gloves.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "gloves", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ViewBag.GlovesId == 0)
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
                siding.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "siding", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ViewBag.SidingId == 0)
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
                ventilation.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "ventilation", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ViewBag.VentilationId == 0)
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
                others.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "others", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ViewBag.OthersId == 0)
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
                galleryItem.ImagePath = imageFile.FileName;
                using (FileStream stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images", "gallery", imageFile.FileName), FileMode.Create)) {
                    await imageFile.CopyToAsync(stream);
                }
            }

            if (ViewBag.GalleryId == 0)
                dbContext.GalleryItems.Add(galleryItem);
            else
                dbContext.GalleryItems.Update(galleryItem);

            dbContext.SaveChanges();
            return RedirectToAction("gallerytochange");
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
        #endregion
    }
}
