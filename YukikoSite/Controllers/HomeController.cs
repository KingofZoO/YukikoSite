using Microsoft.AspNetCore.Mvc;
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

        [Route("map")]
        public IActionResult Map() => View();
    }
}
