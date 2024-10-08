using DFProject.Managers;
using DFProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFProject.DefaultProcess
{
    public class DefaultProcess
    {
        public static CategoryManager catMan = new CategoryManager(new DbInternProjectEntities());

        public static List<Categories> getChildren(Categories cat)
        {
           return catMan.GetChildren(cat.Id);

        }
        public static string getAncestorPath(int parentId)
        {
           return catMan.ancestorPath(parentId);
        }

        

    }
}