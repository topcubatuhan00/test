using DFProject.Managers;
using DFProject.Models;
using DFProject.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace DFProject.Controllers
{
    public class ContentController : SiteBaseController
    {
        // GET: Content
        ContentManager conMan = new ContentManager(new DbInternProjectEntities());
        CategoryManager catMan = new CategoryManager(new DbInternProjectEntities());

        [Route("{lang}/cat/{category}-{catId:int}")]
        public ActionResult ContentsByCatId(int catId = 1)
        {
            var langId = Thread.CurrentThread.CurrentCulture.Name == "tr-TR" ? 1 : 2;
            var conVal = conMan.getOrderedListbyCatId(catId, langId);
            var chosenCatVal = catMan.getById(catId);
            ViewBag.chosenCat = chosenCatVal;
            return View(conVal);

        }

        [Route("{lang}/cont/{content}-{contId:int}")]
        public ActionResult OpenContentCat(int contId)
        {
            var chosenContentVal = conMan.getById(contId);
            ViewBag.chosenContent = chosenContentVal;
            return View();
        }

        public ActionResult IncrementViewCount(int contId)
        {
            var chosenContentVal = conMan.getById(contId);
            chosenContentVal.ContentViewCount++;
            conMan.ContentUpdate(chosenContentVal);
            return RedirectToAction("OpenContentCat", "Content", new { lang = Thread.CurrentThread.CurrentCulture.Name, content = Helper.FriendlyURLTitle(chosenContentVal.ContentTitle), contId });
           
        }

        public ActionResult SelectLang(string lang)
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
            return RedirectToAction("ContentsByCatID", controllerName);
        }

    }
}