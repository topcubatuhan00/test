using DFProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DFProject.Managers
{
    public class CategoryManager
    {
        DbInternProjectEntities db;

        public CategoryManager(DbInternProjectEntities db)
        {
            this.db = db;
        }
        public void CategoryAdd(Categories category)
        {
            var addedCategory = db.Entry(category);
            addedCategory.State = EntityState.Added;
            db.SaveChanges();
        }

        public void CategoryRemove(Categories category)
        {
            category.CategoryStatus = false;
            CategoryUpdate(category);
            db.SaveChanges();
        }

        public void CategoryUpdate(Categories category)
        {
            var addedCategory = db.Entry(category);
            addedCategory.State = EntityState.Modified;
            db.SaveChanges();
        }

        public void refresh()
        {
            var categories = GetList();
            foreach(var cat in categories)
            {
                CategoryUpdate(cat);
            }
        }

        public Categories getById(int id)
        {
            return db.Categories.SingleOrDefault(x => x.Id == id);
        }
       
        public List<Categories> GetList()
        {
            return db.Categories.Where(x => x.CategoryStatus == true).OrderBy(x => x.CategoryOrder).ToList();
        }

        public List<Categories> GetList(int langId)
        {
            return db.Categories.Where(x => x.CategoryStatus == true && x.LangId == langId).OrderBy(x => x.CategoryOrder).ToList();
        }

        public List<Categories> GetChildren(int parentId)
        {
            try
            {
                return db.Categories.Where(x => x.ParentId == parentId && x.CategoryStatus == true).OrderBy(x => x.CategoryOrder).ToList();
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool childCheck(int parentId, int childId)
        {
            var children = GetChildren(parentId);
            foreach (var child in children)
            {
                if (child.Id == childId)
                    return true;
            }
            return false;
        }

        public List<Categories> GetLeaves()
        {
            var categories = GetList();
            var leaves = new List<Categories>();
            var children = new List<Categories>();
            foreach (var category in categories)
            {
                children = GetChildren(category.Id);
                if (!children.Any())
                {
                    leaves.Add(category);
                }
            }

            return leaves;
        }

        public int? getParentCategoryOrder(int parentId)
        {
            var parent = getById(parentId);
            return parent.CategoryOrder;
        }

        public List<int?> getAncestorCategoryOrderList(int parentId)
        {
            var ancestorList = new List<int?>();
            var parent = getById(parentId);
            while (parent != null)
            {
                ancestorList.Add(parent.CategoryOrder);
                parent = getById((int)parent.ParentId);
            }
            return ancestorList;
        }
        public string ancestorPath(int parentId)
        {
            var ancestorList = getAncestorCategoryOrderList(parentId);
            string path = "";
            for (int i = ancestorList.Count - 1; i >= 0; i--)
            {
                path = path + ancestorList[i] + ".";
            }
            return path;
        }
    }
}
