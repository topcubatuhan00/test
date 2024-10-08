using DFProject.Models;
using DFProject.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using System.Web.Security;

namespace DFProject.Controllers
{
    public class AdminController : Controller
    {
        // Entity Managers
        ContentManager conMan = new ContentManager(new DbInternProjectEntities());
        CategoryManager catMan = new CategoryManager(new DbInternProjectEntities());
        UserManager userMan = new UserManager(new DbInternProjectEntities());


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users user)
        {
            if (userMan.isUserExist(user))
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                return RedirectToAction("ShowContents");
            }

            else
            {
                ViewBag.isSuccessful = false;
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(Users user)
        {
            if (!ModelState.IsValid)
            {
                return View("AddAdmin");
            }
            userMan.UserAdd(user);
            ViewBag.isSuccessful = true;
            return View();
        }

        [Authorize]
        public ActionResult ShowContents(int page = 1, int langId = 1)
        {
            var contval = conMan.GetList(langId);
            var model = contval.ToPagedList(page, 7);
            ViewBag.contents = contval;
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddContent()
        {
            List<SelectListItem> leafCategories = (from x in catMan.GetLeaves()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.Id.ToString()

                                                   }).ToList();
            SelectListItem defaultCat = new SelectListItem();
            defaultCat.Text = "Bir Kategori Seçiniz";
            defaultCat.Value = "-1";
            defaultCat.Selected = true;
            leafCategories.Insert(0, defaultCat);
            ViewBag.cats = leafCategories;

            List<SelectListItem> langs = new List<SelectListItem>();
            SelectListItem tr = new SelectListItem();
            tr.Text = "Türkçe";
            tr.Value = "1";
            SelectListItem en = new SelectListItem();
            en.Text = "İngilizce";
            en.Value = "2";
            langs.Add(tr);
            langs.Add(en);
            ViewBag.langs = langs;
            return View();
        }

        [HttpPost]
        public ActionResult AddContent(Contents content)
        {
            if (content.CategoryId == -1)
            {
                TempData["AlertMessage"] = "Kategori Secilmedi!";
                return RedirectToAction("AddContent");
            }
            content.ContentViewCount = 0;
            conMan.ContentAdd(content);
            return RedirectToAction("ShowContents");

        }
        [Authorize]
        public ActionResult RemoveContent(int contId)
        {
            var deletedContent = conMan.getById(contId);
            conMan.ContentRemove(deletedContent);
            return RedirectToAction("ShowContents");
        }
        [Authorize]
        [HttpGet]
        public ActionResult UpdateContent(int contId)
        {
            var chosenContent = conMan.getById(contId);
            List<SelectListItem> leafCategories = (from x in catMan.GetLeaves()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.Id.ToString()

                                                   }).ToList();
        
            ViewBag.cats = leafCategories;

            List<SelectListItem> langs = new List<SelectListItem>();
            SelectListItem tr = new SelectListItem();
            tr.Text = "Türkçe";
            tr.Value = "1";
            SelectListItem en = new SelectListItem();
            en.Text = "İngilizce";
            en.Value = "2";
            langs.Add(tr);
            langs.Add(en);
            ViewBag.langs = langs;
            return View(chosenContent);
        }

        [HttpPost]
        public ActionResult UpdateContent(Contents content)
        {
         
            var updatedContent = conMan.getById(content.Id);
            updatedContent.ContentTitle = content.ContentTitle;
            updatedContent.ContentSummary = content.ContentSummary;
            updatedContent.ContentBody = content.ContentBody;
            updatedContent.CategoryId = content.CategoryId;
            updatedContent.ContentOrder = content.ContentOrder;
            updatedContent.LangId = content.LangId;
            conMan.ContentUpdate(updatedContent);
            return RedirectToAction("ShowContents");

        }

        [HttpPost]
        public ActionResult RefreshContents(List<Contents> contents)
        {
            if (contents != null)
            {
                foreach (var cont in contents)
                {
                    var updatedCont = conMan.getById(cont.Id);
                    if (updatedCont != null)
                    {
                        updatedCont.ContentOrder = cont.ContentOrder;
                        conMan.ContentUpdate(updatedCont);
                    }
                }
            }
            return RedirectToAction("ShowContents");
        }

        [Authorize]
        public ActionResult ShowCategories(int page = 1, int langId = 1)
        {
            var catval = catMan.GetList(langId);
            var model = catval.ToPagedList(page, 7);
            ViewBag.categories = catval;
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddCategory()
        {
            List<SelectListItem> categories = (from x in catMan.GetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.Id.ToString()
                                               }).ToList();

            SelectListItem mainCat = new SelectListItem();
            mainCat.Text = "YOK";
            mainCat.Value = "0";
            categories.Add(mainCat);
            SelectListItem defaultCat = new SelectListItem();
            defaultCat.Text = "Bir Kategori Seçiniz";
            defaultCat.Value = "-1";
            defaultCat.Selected = true;
            categories.Insert(0, defaultCat);
            ViewBag.cats = categories;

            List<SelectListItem> langs = new List<SelectListItem>();
            SelectListItem tr = new SelectListItem();
            tr.Text = "Türkçe";
            tr.Value = "1";
            SelectListItem en = new SelectListItem();
            en.Text = "İngilizce";
            en.Value = "2";
            langs.Add(tr);
            langs.Add(en);
            ViewBag.langs = langs;
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Categories cat)
        {
            if (cat.ParentId == -1)
            {
                TempData["AlertMessage"] = "Ust Kategori Secilmedi!";
                return RedirectToAction("AddCategory");
            }
            cat.CategoryStatus = true;
            catMan.CategoryAdd(cat);
            return RedirectToAction("ShowCategories");

        }

        public ActionResult RemoveCategory(int catId)
        {
            var deletedCategory = catMan.getById(catId);
            catMan.CategoryRemove(deletedCategory);
            return RedirectToAction("ShowCategories");
        }
        [Authorize]
        [HttpGet]
        public ActionResult UpdateCategory(int catId)
        {
            var chosenCategory = catMan.getById(catId);
            List<SelectListItem> categories = (from x in catMan.GetList()
                                               where x.Id != catId && !catMan.childCheck(x.Id, catId)
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.Id.ToString()
                                               }).ToList();

            SelectListItem mainCat = new SelectListItem();
            mainCat.Text = "YOK";
            mainCat.Value = "0";
            categories.Add(mainCat);
       
            ViewBag.cats = categories;

            List<SelectListItem> langs = new List<SelectListItem>();
            SelectListItem tr = new SelectListItem();
            tr.Text = "Türkçe";
            tr.Value = "1";
            SelectListItem en = new SelectListItem();
            en.Text = "İngilizce";
            en.Value = "2";
            langs.Add(tr);
            langs.Add(en);
            ViewBag.langs = langs;
            return View(chosenCategory);
        }

        [HttpPost]
        public ActionResult UpdateCategory(Categories cat)
        {
        
            var updatedCat = catMan.getById(cat.Id);
            updatedCat.CategoryName = cat.CategoryName;
            updatedCat.Contents = cat.Contents;
            updatedCat.ParentId = cat.ParentId;
            updatedCat.CategoryOrder = cat.CategoryOrder;
            updatedCat.LangId = cat.LangId;
            catMan.CategoryUpdate(updatedCat);
            return RedirectToAction("ShowCategories");

        }
        [HttpPost]
        public ActionResult RefreshCategories(List<Categories> categories)
        {
            if (categories != null)
            {
                foreach (var cat in categories)
                {
                    var updatedCat = catMan.getById(cat.Id);
                    if (updatedCat != null)
                    {
                        updatedCat.CategoryOrder = cat.CategoryOrder;
                        catMan.CategoryUpdate(updatedCat);
                    }
                }
            }
            return RedirectToAction("ShowCategories");
        }
    }
}