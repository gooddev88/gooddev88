
namespace Robot.PrintServer.Rpt {
    partial class R411
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.logoimg = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblDocID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanyBranch = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltoday = new DevExpress.XtraReports.UI.XRLabel();
            this.lbluserlogin = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltable = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigaddr = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanybig = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblNetotalAfterRound = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRound = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmtIncVat = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLabel2,
            this.xrLabel14});
            this.Detail.Dpi = 254F;
            this.Detail.HeightF = 124.4599F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Qty]")});
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(57.19334F, 6.459553E-05F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(104.3532F, 76.19995F);
            this.xrLabel9.StylePriority.UseBorderDashStyle = false;
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "xrLabel6";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel9.TextFormatString = "{0:#,#}";
            // 
            // xrLabel2
            // 
            this.xrLabel2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalAmt]")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(492.9366F, 4.968887E-05F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(219.8899F, 76.19995F);
            this.xrLabel2.StylePriority.UseBorderDashStyle = false;
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel6";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel2.TextFormatString = "{0:n2}";
            // 
            // xrLabel14
            // 
            this.xrLabel14.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemName]")});
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(161.5466F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(331.3901F, 76.2F);
            this.xrLabel14.StylePriority.UseBorderDashStyle = false;
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel6";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(51, 51, 25, 0, 254F);
            this.TopMargin.StylePriority.UsePadding = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 46F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.HeightF = 0F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.logoimg,
            this.lblDocID,
            this.lblcompanyBranch,
            this.xrLine4,
            this.lbltoday,
            this.lbluserlogin,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLine2,
            this.lbltable,
            this.lblCombigaddr,
            this.lblcompanybig});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.ForeColor = System.Drawing.Color.Black;
            this.PageHeader.HeightF = 543.9099F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 25, 25, 254F);
            this.PageHeader.StylePriority.UseForeColor = false;
            // 
            // logoimg
            // 
            this.logoimg.Dpi = 254F;
            this.logoimg.LocationFloat = new DevExpress.Utils.PointFloat(271.3942F, 64.34663F);
            this.logoimg.Name = "logoimg";
            this.logoimg.SizeF = new System.Drawing.SizeF(219.2117F, 119.38F);
            this.logoimg.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblDocID
            // 
            this.lblDocID.Dpi = 254F;
            this.lblDocID.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblDocID.LocationFloat = new DevExpress.Utils.PointFloat(338.5517F, 467.9797F);
            this.lblDocID.Multiline = true;
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDocID.SizeF = new System.Drawing.SizeF(371.7346F, 60.11325F);
            this.lblDocID.StylePriority.UseFont = false;
            this.lblDocID.StylePriority.UsePadding = false;
            this.lblDocID.StylePriority.UseTextAlignment = false;
            this.lblDocID.Text = "lblDocID";
            this.lblDocID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblDocID.WordWrap = false;
            // 
            // lblcompanyBranch
            // 
            this.lblcompanyBranch.Dpi = 254F;
            this.lblcompanyBranch.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanyBranch.LocationFloat = new DevExpress.Utils.PointFloat(57.19345F, 184.3466F);
            this.lblcompanyBranch.Multiline = true;
            this.lblcompanyBranch.Name = "lblcompanyBranch";
            this.lblcompanyBranch.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblcompanyBranch.SizeF = new System.Drawing.SizeF(655.6329F, 64.34663F);
            this.lblcompanyBranch.StylePriority.UseFont = false;
            this.lblcompanyBranch.StylePriority.UsePadding = false;
            this.lblcompanyBranch.StylePriority.UseTextAlignment = false;
            this.lblcompanyBranch.Text = "lblcompanyBranch";
            this.lblcompanyBranch.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine4.Dpi = 254F;
            this.xrLine4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(52.11329F, 317.2731F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(655.6331F, 14.81668F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            // 
            // lbltoday
            // 
            this.lbltoday.Dpi = 254F;
            this.lbltoday.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lbltoday.LocationFloat = new DevExpress.Utils.PointFloat(159.0064F, 468.9799F);
            this.lbltoday.Multiline = true;
            this.lbltoday.Name = "lbltoday";
            this.lbltoday.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lbltoday.SizeF = new System.Drawing.SizeF(179.5452F, 60.11322F);
            this.lbltoday.StylePriority.UseFont = false;
            this.lbltoday.StylePriority.UsePadding = false;
            this.lbltoday.StylePriority.UseTextAlignment = false;
            this.lbltoday.Text = "lbltoday";
            this.lbltoday.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbltoday.WordWrap = false;
            // 
            // lbluserlogin
            // 
            this.lbluserlogin.Dpi = 254F;
            this.lbluserlogin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbluserlogin.LocationFloat = new DevExpress.Utils.PointFloat(226.6954F, 407.8665F);
            this.lbluserlogin.Multiline = true;
            this.lbluserlogin.Name = "lbluserlogin";
            this.lbluserlogin.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lbluserlogin.SizeF = new System.Drawing.SizeF(483.5912F, 60.11328F);
            this.lbluserlogin.StylePriority.UseFont = false;
            this.lbluserlogin.StylePriority.UsePadding = false;
            this.lbluserlogin.StylePriority.UseTextAlignment = false;
            this.lbluserlogin.Text = "lbluserlogin";
            this.lbluserlogin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbluserlogin.WordWrap = false;
            // 
            // xrLabel20
            // 
            this.xrLabel20.CanGrow = false;
            this.xrLabel20.Dpi = 254F;
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 7F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(52.11331F, 469.2266F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(106.8932F, 58.86652F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UsePadding = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "วันที่";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel20.WordWrap = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.CanGrow = false;
            this.xrLabel19.Dpi = 254F;
            this.xrLabel19.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(52.11329F, 407.8667F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(174.582F, 60.11319F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UsePadding = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "พนักงานขาย";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel19.WordWrap = false;
            // 
            // xrLine2
            // 
            this.xrLine2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(54.19354F, 529.0931F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(658.173F, 14.81671F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            // 
            // lbltable
            // 
            this.lbltable.Dpi = 254F;
            this.lbltable.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltable.LocationFloat = new DevExpress.Utils.PointFloat(52.11329F, 332.0898F);
            this.lbltable.Multiline = true;
            this.lbltable.Name = "lbltable";
            this.lbltable.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lbltable.SizeF = new System.Drawing.SizeF(655.6331F, 68.57996F);
            this.lbltable.StylePriority.UseFont = false;
            this.lbltable.StylePriority.UsePadding = false;
            this.lbltable.StylePriority.UseTextAlignment = false;
            this.lbltable.Text = "lbltable";
            this.lbltable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbltable.WordWrap = false;
            // 
            // lblCombigaddr
            // 
            this.lblCombigaddr.Dpi = 254F;
            this.lblCombigaddr.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigaddr.LocationFloat = new DevExpress.Utils.PointFloat(54.65331F, 250.3866F);
            this.lblCombigaddr.Name = "lblCombigaddr";
            this.lblCombigaddr.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 8, 0, 254F);
            this.lblCombigaddr.SizeF = new System.Drawing.SizeF(658.173F, 66.88654F);
            this.lblCombigaddr.StylePriority.UseFont = false;
            this.lblCombigaddr.StylePriority.UsePadding = false;
            this.lblCombigaddr.StylePriority.UseTextAlignment = false;
            this.lblCombigaddr.Text = "lblCombigaddr";
            this.lblCombigaddr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblcompanybig
            // 
            this.lblcompanybig.Dpi = 254F;
            this.lblcompanybig.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblcompanybig.LocationFloat = new DevExpress.Utils.PointFloat(57.19334F, 0F);
            this.lblcompanybig.Multiline = true;
            this.lblcompanybig.Name = "lblcompanybig";
            this.lblcompanybig.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblcompanybig.SizeF = new System.Drawing.SizeF(655.6331F, 64.34663F);
            this.lblcompanybig.StylePriority.UseFont = false;
            this.lblcompanybig.StylePriority.UsePadding = false;
            this.lblcompanybig.StylePriority.UseTextAlignment = false;
            this.lblcompanybig.Text = "lblcompanybig";
            this.lblcompanybig.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 64F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Silver;
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(57.19344F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(655.6331F, 60.11324F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "ออเดอร์ ไม่มี VAT";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.Visible = false;
            this.xrLabel3.WordWrap = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.lblNetotalAfterRound,
            this.xrLabel7,
            this.xrLabel1,
            this.lblRound,
            this.lblTotalAmtIncVat,
            this.xrLabel12,
            this.xrLine1,
            this.xrLabel5,
            this.lblTotalAmt});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 255.2699F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseTextAlignment = false;
            this.GroupFooter1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblNetotalAfterRound
            // 
            this.lblNetotalAfterRound.Dpi = 254F;
            this.lblNetotalAfterRound.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblNetotalAfterRound.LocationFloat = new DevExpress.Utils.PointFloat(492.9367F, 195.1565F);
            this.lblNetotalAfterRound.Multiline = true;
            this.lblNetotalAfterRound.Name = "lblNetotalAfterRound";
            this.lblNetotalAfterRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNetotalAfterRound.SizeF = new System.Drawing.SizeF(219.8897F, 60.11329F);
            this.lblNetotalAfterRound.StylePriority.UseFont = false;
            this.lblNetotalAfterRound.StylePriority.UsePadding = false;
            this.lblNetotalAfterRound.StylePriority.UseTextAlignment = false;
            this.lblNetotalAfterRound.Text = "lblNetotalAfterRound";
            this.lblNetotalAfterRound.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblNetotalAfterRound.TextFormatString = "{0:n2}";
            this.lblNetotalAfterRound.WordWrap = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.CanGrow = false;
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(141.9004F, 195.1566F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(351.0365F, 60.11327F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "สุทธิหลังปัดเศษ";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel7.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(57.19353F, 120.2265F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(435.7435F, 60.11329F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ปัดเศษ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel1.WordWrap = false;
            // 
            // lblRound
            // 
            this.lblRound.Dpi = 254F;
            this.lblRound.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRound.LocationFloat = new DevExpress.Utils.PointFloat(492.9369F, 120.2265F);
            this.lblRound.Multiline = true;
            this.lblRound.Name = "lblRound";
            this.lblRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRound.SizeF = new System.Drawing.SizeF(219.8897F, 60.11329F);
            this.lblRound.StylePriority.UseFont = false;
            this.lblRound.StylePriority.UsePadding = false;
            this.lblRound.StylePriority.UseTextAlignment = false;
            this.lblRound.Text = "lblRound";
            this.lblRound.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblRound.TextFormatString = "{0:n2}";
            this.lblRound.WordWrap = false;
            // 
            // lblTotalAmtIncVat
            // 
            this.lblTotalAmtIncVat.Dpi = 254F;
            this.lblTotalAmtIncVat.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmtIncVat.LocationFloat = new DevExpress.Utils.PointFloat(492.9369F, 60.11325F);
            this.lblTotalAmtIncVat.Multiline = true;
            this.lblTotalAmtIncVat.Name = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTotalAmtIncVat.SizeF = new System.Drawing.SizeF(219.8897F, 60.11329F);
            this.lblTotalAmtIncVat.StylePriority.UseFont = false;
            this.lblTotalAmtIncVat.StylePriority.UsePadding = false;
            this.lblTotalAmtIncVat.StylePriority.UseTextAlignment = false;
            this.lblTotalAmtIncVat.Text = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmtIncVat.TextFormatString = "{0:n2}";
            this.lblTotalAmtIncVat.WordWrap = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.CanGrow = false;
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(57.19353F, 60.11325F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(435.7435F, 60.11329F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UsePadding = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "รวมทั้งสิน";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel12.WordWrap = false;
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(54.65349F, 180.3398F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(658.1731F, 14.8167F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(333.1302F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(159.8064F, 60.11324F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "รวมเป็นเงิน";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel5.WordWrap = false;
            // 
            // lblTotalAmt
            // 
            this.lblTotalAmt.CanGrow = false;
            this.lblTotalAmt.Dpi = 254F;
            this.lblTotalAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalAmt.LocationFloat = new DevExpress.Utils.PointFloat(492.9366F, 0F);
            this.lblTotalAmt.Name = "lblTotalAmt";
            this.lblTotalAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTotalAmt.SizeF = new System.Drawing.SizeF(219.89F, 60.11324F);
            this.lblTotalAmt.StylePriority.UseFont = false;
            this.lblTotalAmt.StylePriority.UsePadding = false;
            this.lblTotalAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalAmt.Text = "lblTotalAmt";
            this.lblTotalAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmt.TextFormatString = "{0:n2}";
            this.lblTotalAmt.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(57.19354F, 195.1566F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 10, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(84.70685F, 60.11326F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "R411";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel4.WordWrap = false;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Robot.Data.POS_SaleLine);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // R411
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.PageFooter,
            this.GroupFooter1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 46);
            this.PageHeight = 2979;
            this.PageWidth = 762;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.RollPaper = true;
            this.SnapGridSize = 25F;
            this.Version = "20.1";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel lblcompanybig;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.XRLabel lblCombigaddr;
        private DevExpress.XtraReports.UI.XRLabel lbltable;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmt;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmtIncVat;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel lbltoday;
        private DevExpress.XtraReports.UI.XRLabel lbluserlogin;
        private DevExpress.XtraReports.UI.XRLabel lblcompanyBranch;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblDocID;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblRound;
        private DevExpress.XtraReports.UI.XRLabel lblNetotalAfterRound;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRPictureBox logoimg;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    }
}
