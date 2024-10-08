using DFProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DFProject.Managers
{
    public class ContentManager
    {
        DbInternProjectEntities db;

        public ContentManager(DbInternProjectEntities db)
        {
            this.db = db;
        }
        public void ContentAdd(Contents content)
        {
            var addedContent = db.Entry(content);
            addedContent.State = EntityState.Added;
            db.SaveChanges();
        }

        public void ContentRemove(Contents content)
        {
            var deletedContent = db.Entry(content);
            deletedContent.State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void ContentUpdate(Contents content)
        {
            var updatedContent = db.Entry(content);
            updatedContent.State = EntityState.Modified;
            db.SaveChanges();
        }

        public Contents getById(int id)
        {
            return db.Contents.SingleOrDefault(c => c.Id == id);
        }

        public Contents getCatId(int id)
        {
            return db.Contents.SingleOrDefault(x => x.CategoryId == id);
        }

        public Contents getCatId(int id, int langId)
        {
            return db.Contents.SingleOrDefault(x => x.CategoryId == id && x.LangId == langId);
        }

        public List<Contents> GetList()
        {
            return db.Contents.Where(x => x.Categories.CategoryStatus == true).OrderBy(c => c.ContentOrder).ToList();
        }

        public List<Contents> GetList(int langId)
        {
            return db.Contents.Where(x => x.LangId == langId && x.Categories.CategoryStatus == true).OrderBy(x => x.ContentOrder).ToList();
        }

        public List<Contents> getListByCatId(int catId, int langId)
        {
            return db.Contents.Where(x => x.CategoryId == catId && x.LangId == langId).ToList();
        }

        public List<Contents> getOrderedList(int langId)
        {
            return db.Contents.Where(x => x.LangId == langId && x.Categories.CategoryStatus == true).OrderByDescending(x => x.ContentViewCount).ToList();
        }

        public List<Contents> getOrderedListbyCatId(int catId, int langId)
        {
            return db.Contents.Where(x => x.CategoryId == catId && x.LangId == langId).OrderBy(x => x.ContentOrder).ToList();
        }

        public List<Contents> getSearchResult(string search, int langId)
        {
            try
            {
                return db.Contents
                 .Where(c => (c.ContentTitle.Contains(search) || c.ContentSummary.Contains(search) || c.ContentBody.Contains(search)) && (c.LangId == langId) && (c.Categories.CategoryStatus == true))
                 .ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}