using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModelBuilder.Models;
using System.Drawing;
using ModelBuilder.Tools;

namespace ModelBuilder.Web.Data
{
    public class UserProfileService : ICrud<UserProfile>
    {
        ModelBuilderDB db;
        public UserProfileService()
        {
            if (db == null) db = new ModelBuilderDB();
            //db.Database.EnsureCreated();
        }
        public bool DeleteData(object Id)
        {
            if (Id is long FID)
            {
                var data = from x in db.UserProfiles
                           where x.Id == FID
                           select x;
                foreach (var item in data)
                {
                    db.UserProfiles.Remove(item);
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public UserProfile GetItemByEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email)) return null;
            var selItem = db.UserProfiles.Where(x => x.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            return selItem;
        }
        public Roles GetUserRole(string Email)
        {
            var selItem = db.UserProfiles.Where(x => x.Username == Email).FirstOrDefault();
            return selItem.Role;
        }

        public UserProfile GetUserByEmail(string Email)
        {
            var selItem = db.UserProfiles.Where(x => x.Username == Email).FirstOrDefault();
            return selItem;
        }
        public UserProfile GetItemByPhone(string Phone)
        {
            var selItem = db.UserProfiles.Where(x => x.Phone.ToLower() == Phone.ToLower()).FirstOrDefault();
            return selItem;
        }
        public List<UserProfile> FindByKeyword(string Keyword)
        {
            var data = from x in db.UserProfiles
                       where x.Email.Contains(Keyword) || x.FullName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<UserProfile> GetAllData()
        {
            var data = from x in db.UserProfiles
                       select x;
            return data.ToList();
        }

        public UserProfile GetDataById(object Id)
        {
            if (Id is long FID)
            {
                var data = from x in db.UserProfiles
                           where x.Id == FID
                           select x;
                return data.FirstOrDefault();
            }
            return default;
        }

        public long GetLastId()
        {
            var lastId = db.UserProfiles.OrderByDescending(x => x.Id).FirstOrDefault();
            return lastId.Id + 1;
        }
        public bool IsUserExists(string Email)
        {
            if (string.IsNullOrEmpty(Email)) return true;
            //if (db.UserProfiles.Count() <= 0 ) return false;
            var exists = db.UserProfiles.Any(x => x.Username.ToLower() == Email.ToLower());
            return exists;
        }
        public bool InsertData(UserProfile data)
        {
            try
            {
                db.UserProfiles.Add(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

        }

        public bool UpdateData(UserProfile data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;


            }

        }
    }
}

