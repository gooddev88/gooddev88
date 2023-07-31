using Robot.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Robot
{
    public partial class Pur : System.Web.UI.MasterPage
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
                case "1000":
                    result = "Purchase";
                    break;
      case "1251":
                    result = "1251 License";
                    break;
                case "1103":
                    result = "1103 PO";
                    break;
                case "1201":
                    result = "1201 Good Receipt";
                    break;
                case "1211":
                    result = "1211 Packing tracking";
                    break;
                case "1111":
                    result = "1111 PO tracking";
                    break;
               
                default:
                    result = "xxxx";
                    break;
            }
            return result;



        }
    }
}