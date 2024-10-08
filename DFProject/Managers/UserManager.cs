using DFProject.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace DFProject.Managers
{
    public class UserManager
    {
        DbInternProjectEntities db;

        public UserManager(DbInternProjectEntities db)
        {
            this.db = db;
        }

        public bool isUserExist(Users userLogin)
        {
            var getList = GetList();
            foreach (Users user in getList)
            {
                if(user.UserEmail == userLogin.UserEmail && user.Password == userLogin.Password)
                    return true;
            }
            return false;
        }
        public Users getByID(int id)
        {
            return db.Users.SingleOrDefault(x => x.Id == id);
        }

        public List<Users> GetList()
        {
            return db.Users.ToList();
        }

        public void UserAdd(Users user)
        {
            var addedContent = db.Entry(user);
            addedContent.State = EntityState.Added;
            db.SaveChanges();
        }

        public void UserRemove(Users user)
        {
            var addedContent = db.Entry(user);
            addedContent.State = EntityState.Added;
            db.SaveChanges();
        }

        public void UserUpdate(Users user)
        {
            var addedContent = db.Entry(user);
            addedContent.State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
