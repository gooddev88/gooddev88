using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess
{
    public static class PermissionService
    {
       
        
        public static List<vw_PermissionInMenu> ListAllMenuByLogin()
        {
            List<vw_PermissionInMenu> result = new List<vw_PermissionInMenu>();
            var username = LoginService.LoginInfo.CurrentUser;
            var menuList = LoginService.LoginInfo.UserInMenu.ToList();
            result = menuList.Where(o => o.Username == username).ToList();
            return result;
        }
        public static UserMenu GetMenuInfo(string menu)
        {
            UserMenu result = new UserMenu { MenuID = "", Name = "" };

            var menu_info = LoginService.LoginInfo.UserInMenu.Where(o => o.MenuID == menu).FirstOrDefault();

            if (menu_info == null)
            {
                return result;
            }
            result.MenuID = menu_info.MenuID;
            result.Name = menu_info.MenuName;
            result.MenuCode = menu_info.MenuCode;
            result.Desc1 = menu_info.MenuDesc1;
            result.Desc2 = menu_info.MenuDesc2;
            return result;
        }
        public static bool CanOpen(string menu)
        {
            bool result = false;
            var per = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (per != null)
            {
                
                result = Convert.ToBoolean(per.IsOpen);
            }
            return result;
        }
        public static bool CanCreate(string menu)
        {
            bool result = false;
            var per = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu).OrderByDescending(o => o.IsCreate).FirstOrDefault();
            if (per != null)
            {
                result = Convert.ToBoolean(per.IsCreate);
            }
            return result;
        }
        public static bool CanEdit(string menu)
        {
            bool result = false;
            var per = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu).OrderByDescending(o => o.IsEdit).FirstOrDefault();
            if (per != null)
            {
                result = Convert.ToBoolean(per.IsEdit);
            }
            return result;
        }
        public static bool CanDelete(string menu)
        {
            bool result = false;
            var per = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu).OrderByDescending(o => o.IsDelete).FirstOrDefault();
            if (per != null)
            {
                result = Convert.ToBoolean(per.IsDelete);
            }
            return result;
        }

        public static bool CanPrint(string menu)
        {
            bool result = false;
            var per = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu).OrderByDescending(o => o.IsPrint).FirstOrDefault();
            if (per != null)
            {
                result = Convert.ToBoolean(per.IsPrint);
            }
            return result;
        }



        public static UserMenu GetUserMenu(string menu)
        {
            UserMenu result = new UserMenu();
            using (GAEntities db = new GAEntities())
            {
                result = db.UserMenu.Where(o => (o.MenuID == menu)).FirstOrDefault();
            }

            return result;
        }


 
    }
}