using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot
{
    public partial class Fin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public string SetMenu(object menu_id)
        {
            string result = "0";
            var data = PermissionService.ListAllMenuByLogin().Where(o => o.MenuID == menu_id.ToString()).OrderByDescending(o => o.IsOpen).FirstOrDefault();
            if (data != null)
            {
                result = data.IsOpen.ToString();
            }
            return result;
        }

        public string SetCaption(object menu_id)
        {

            //string result       = LanguageService.GetValue(menu_id.ToString());
            //return result;
            string result = "";
            string menuid = menu_id.ToString();
            switch (menuid)
            {
                case "3000":
                    result = "Finance";
                    break;
                case "3301":
                    result = "3301 Import STM";
                    break;
                case "3302":
                    result = "3302 STM Fin. Memo";
                    break;
                case "3303":
                    result = "3303 STM Fin sup.Memo";
                    break;
                case "3304":
                    result = "3304 STM Acc. Memo";
                    break;
                case "3305":
                    result = "3305 STM Acc sup.Memo";
                    break;
                case "3310":
                    result = "3310 STM History";
                    break;
                default:
                    result = "xxxx";
                    break;
            }
            return result;



        }
    }
}