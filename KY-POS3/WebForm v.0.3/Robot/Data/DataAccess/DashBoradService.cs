using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Robot.Data.DataAccess {
    public static class DashBoradService {

        public static DashBoard GetBoardByID(string boardID) {
            DashBoard result = new DashBoard();
            using (GAEntities db = new GAEntities())
            {
                result = db.DashBoard.Where(o => o.DashBoardID == boardID).FirstOrDefault();
            }
            return result;
        }

        public static void GetNextBoard() {

            var cb = LoginService.LoginInfo.CurrentBoard;

            LoginService.LoginInfo.CurrentBoard = LoginService.LoginInfo.UserInBoard.Where(o => o.BoardSort > cb.BoardSort && o.DashBoardID != cb.DashBoardID).FirstOrDefault();
            if (LoginService.LoginInfo.CurrentBoard == null)
            {//current board เป็น board สุดท้าย
                LoginService.LoginInfo.CurrentBoard = LoginService.LoginInfo.UserInBoard.OrderBy(o => o.BoardSort).FirstOrDefault();
            }


        }
        //public static DashBoard GetBoardByID(string boardID) {
        //    DashBoard result = new DashBoard();
        //    using (GAEntities db = new GAEntities()) {
        //        result = db.DashBoard.Where(o => o.DashBoardID == boardID).FirstOrDefault();
        //    }
        //    return result;
        //}

        //public static DashBoard GetNextBoard() {
        //    if (HttpContext.Current.Session["cur_b_index"] == null) {
        //        HttpContext.Current.Session["cur_b_index"] = 0;
        //    }
        //    int cur_index = (int)HttpContext.Current.Session["cur_b_index"];

        //    DashBoard result = new DashBoard();
        //    // var userInBoard = (List<vw_PermissionInBoard>)HttpContext.Current.Session["userinboard"];
        //    var userInBoard = LoginService.GetLoginInfo().UserInBoard;
        //    if (userInBoard == null) {
        //        return null;
        //    }
        //    if (userInBoard.Count() == 0) {
        //        return null;
        //    }
        //    if (userInBoard.Count() == 1) {
        //        cur_index = 1;
        //        var uib = userInBoard.FirstOrDefault();
        //        return GetBoardByID(uib.DashBoardID);
        //    }
        //    if (userInBoard.Count() > 1) {
        //        var next_b = userInBoard.Where(o => o.RN > cur_index).FirstOrDefault();
        //        if (next_b == null) {
        //            HttpContext.Current.Session["cur_b_index"] = 1;
        //            var first_board = userInBoard.OrderBy(o => o.RN).FirstOrDefault();
        //            return GetBoardByID(first_board.DashBoardID);
        //        } else {
        //            cur_index++;
        //            HttpContext.Current.Session["cur_b_index"] = cur_index;
        //            return GetBoardByID(next_b.DashBoardID);
        //        }
        //    }
        //    return result;
        //} 
    }
}