using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using DFProject.Managers;
using DFProject.Models;


namespace DFProject.Controllers
{
    public class HomeController : SiteBaseController
    {
        // GET: Home
        CategoryManager catMan = new CategoryManager(new DbInternProjectEntities());
        ContentManager conMan = new ContentManager(new DbInternProjectEntities());
        public PartialViewResult _Sidebar()
        {

            var langId = Thread.CurrentThread.CurrentCulture.Name == "tr-TR" ? 1 : 2;

            var categories = catMan.GetList(langId);
            return PartialView(categories);
        }
        
        public ActionResult Index()
        {
            var langId = Thread.CurrentThread.CurrentCulture.Name == "tr-TR" ? 1 : 2;
            var contents = conMan.getOrderedList(langId);
            return View(contents);
        }

        [Route("Search")]
        [HttpPost]
        public ActionResult Search(string search)
        {
            var langId = Thread.CurrentThread.CurrentCulture.Name == "tr-TR" ? 1 : 2;
            var searchResult = conMan.getSearchResult(search, langId);
            ViewBag.Search = search;
            return View(searchResult);
        }

        public ActionResult SelectLang(string lang = "1")
        {
            switch (lang)
            {
                case "1":
                    lang = "tr";
                    break;
                case "2":
                    lang = "en";
                    break;
            }
            setLanguage(lang);
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            return RedirectToAction("Index", controllerName);
        }
    }
}   