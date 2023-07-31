﻿using RobotWasm.Shared.Data.DimsDB;
using static RobotWasm.Shared.Data.ML.Shared.I_Result;

namespace RobotWasm.Shared.Data.ML.Master.UserGroup {

    public class xusergroup_in_menu : usergroup_in_menu {
        public bool IsSelect { get; set; }

        public string MenuCode { get; set; }
        public string MenuName { get; set; }

        public string MenuDesc1 { get; set; }
        public string MenuDesc2 { get; set; }
        public string MenuTypeID { get; set; }
        public string MenuGroupID { get; set; }
        public int MenuGroupSort { get; set; }
        public int MenuSort { get; set; }

        public int NeedOpenPermission { get; set; }
        public int NeedCreatePermission { get; set; }
        public int NeedEditPermission { get; set; }
        public int NeedDeletePermission { get; set; }
        public int NeedPrintPermission { get; set; }

        public string CaptionOpenPermission { get; set; }
        public string CaptionCreatePermission { get; set; }
        public string CaptionEditPermission { get; set; }
        public string CaptionDeletePermission { get; set; }
        public string CaptionPrintPermission { get; set; }

        public bool isOpenBind { get; set; }
        public bool isCreateBind { get; set; }
        public bool isEditBind { get; set; }
        public bool isDeleteBind { get; set; }
        public bool isPrintBind { get; set; }

        public int Sort { get; set; }
    }

}