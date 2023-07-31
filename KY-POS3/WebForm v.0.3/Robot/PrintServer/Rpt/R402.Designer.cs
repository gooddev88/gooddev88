namespace Robot.PrintServer.Rpt {
    partial class R402 {
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
            this.lbltable = new DevExpress.XtraReports.UI.XRLabel();
            this.logoimg = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblcompanyBranch = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblinvid = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltoday = new DevExpress.XtraReports.UI.XRLabel();
            this.lbluserlogin = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigtax = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigaddr = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanybig = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAfterRound = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRound = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbltransfer = new DevExpress.XtraReports.UI.XRLabel();
            this.lblChange = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcash = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmtIncVat = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVatInfoCaption = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblTotalVatAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVatCaption = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
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
            this.Detail.HeightF = 30F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Qty]")});
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(31.84032F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(41.08394F, 30F);
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
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalAmt]")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(266.0885F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(60.36829F, 30F);
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
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemName]")});
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(72.92426F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(193.1642F, 30F);
            this.xrLabel14.StylePriority.UseBorderDashStyle = false;
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel6";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(20, 20, 10, 0, 100F);
            this.TopMargin.StylePriority.UsePadding = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 18F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.HeightF = 0F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbltable,
            this.logoimg,
            this.lblcompanyBranch,
            this.xrLabel6,
            this.lblinvid,
            this.xrLine4,
            this.lbltoday,
            this.lbluserlogin,
            this.xrLabel20,
            this.xrLabel19,
            this.xrLine2,
            this.xrLabel1,
            this.lblCombigtax,
            this.lblCombigaddr,
            this.lblcompanybig});
            this.PageHeader.ForeColor = System.Drawing.Color.Black;
            this.PageHeader.HeightF = 290.1574F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 10, 100F);
            this.PageHeader.StylePriority.UseForeColor = false;
            // 
            // lbltable
            // 
            this.lbltable.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltable.LocationFloat = new DevExpress.Utils.PointFloat(30.84025F, 209.4999F);
            this.lbltable.Multiline = true;
            this.lbltable.Name = "lbltable";
            this.lbltable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltable.SizeF = new System.Drawing.SizeF(295.6164F, 26.99997F);
            this.lbltable.StylePriority.UseFont = false;
            this.lbltable.StylePriority.UsePadding = false;
            this.lbltable.StylePriority.UseTextAlignment = false;
            this.lbltable.Text = "lbltable";
            this.lbltable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbltable.WordWrap = false;
            // 
            // logoimg
            // 
            this.logoimg.LocationFloat = new DevExpress.Utils.PointFloat(136.8481F, 25.33332F);
            this.logoimg.Name = "logoimg";
            this.logoimg.SizeF = new System.Drawing.SizeF(86.3038F, 47F);
            this.logoimg.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblcompanyBranch
            // 
            this.lblcompanyBranch.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanyBranch.LocationFloat = new DevExpress.Utils.PointFloat(30.84031F, 72.3333F);
            this.lblcompanyBranch.Multiline = true;
            this.lblcompanyBranch.Name = "lblcompanyBranch";
            this.lblcompanyBranch.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanyBranch.SizeF = new System.Drawing.SizeF(295.6165F, 25.33332F);
            this.lblcompanyBranch.StylePriority.UseFont = false;
            this.lblcompanyBranch.StylePriority.UsePadding = false;
            this.lblcompanyBranch.StylePriority.UseTextAlignment = false;
            this.lblcompanyBranch.Text = "lblcompanyBranch";
            this.lblcompanyBranch.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(30.84026F, 177.3332F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(87.0896F, 23.66666F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "เลขที่";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel6.WordWrap = false;
            // 
            // lblinvid
            // 
            this.lblinvid.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblinvid.LocationFloat = new DevExpress.Utils.PointFloat(117.9299F, 177.3332F);
            this.lblinvid.Multiline = true;
            this.lblinvid.Name = "lblinvid";
            this.lblinvid.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblinvid.SizeF = new System.Drawing.SizeF(208.5269F, 23.66663F);
            this.lblinvid.StylePriority.UseFont = false;
            this.lblinvid.StylePriority.UsePadding = false;
            this.lblinvid.StylePriority.UseTextAlignment = false;
            this.lblinvid.Text = "lblinvid";
            this.lblinvid.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblinvid.WordWrap = false;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(30.84031F, 203.1665F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(295.6164F, 5.833328F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            // 
            // lbltoday
            // 
            this.lbltoday.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbltoday.LocationFloat = new DevExpress.Utils.PointFloat(118.9299F, 260.1667F);
            this.lbltoday.Multiline = true;
            this.lbltoday.Name = "lbltoday";
            this.lbltoday.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltoday.SizeF = new System.Drawing.SizeF(207.5266F, 23.66663F);
            this.lbltoday.StylePriority.UseFont = false;
            this.lbltoday.StylePriority.UsePadding = false;
            this.lbltoday.StylePriority.UseTextAlignment = false;
            this.lbltoday.Text = "lbltoday";
            this.lbltoday.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbltoday.WordWrap = false;
            // 
            // lbluserlogin
            // 
            this.lbluserlogin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbluserlogin.LocationFloat = new DevExpress.Utils.PointFloat(118.9299F, 236.4998F);
            this.lbluserlogin.Multiline = true;
            this.lbluserlogin.Name = "lbluserlogin";
            this.lbluserlogin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbluserlogin.SizeF = new System.Drawing.SizeF(207.5267F, 23.66664F);
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
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(29.83327F, 260.6574F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(89.09659F, 23.66669F);
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
            this.xrLabel19.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(29.83327F, 236.4999F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(89.09659F, 23.66663F);
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
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(29.83327F, 284.3241F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(297.6234F, 5.833313F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(30.84031F, 150.3333F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(295.6164F, 26.99998F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ใบเสร็จรับเงิน/ใบกํากับภาษีอยางย่อ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel1.WordWrap = false;
            // 
            // lblCombigtax
            // 
            this.lblCombigtax.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigtax.LocationFloat = new DevExpress.Utils.PointFloat(30.84026F, 123.9999F);
            this.lblCombigtax.Name = "lblCombigtax";
            this.lblCombigtax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigtax.SizeF = new System.Drawing.SizeF(295.6164F, 26.33331F);
            this.lblCombigtax.StylePriority.UseFont = false;
            this.lblCombigtax.StylePriority.UsePadding = false;
            this.lblCombigtax.StylePriority.UseTextAlignment = false;
            this.lblCombigtax.Text = "lblCombigtax";
            this.lblCombigtax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblCombigaddr
            // 
            this.lblCombigaddr.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigaddr.LocationFloat = new DevExpress.Utils.PointFloat(30.84031F, 97.66663F);
            this.lblCombigaddr.Name = "lblCombigaddr";
            this.lblCombigaddr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigaddr.SizeF = new System.Drawing.SizeF(295.6164F, 26.33329F);
            this.lblCombigaddr.StylePriority.UseFont = false;
            this.lblCombigaddr.StylePriority.UsePadding = false;
            this.lblCombigaddr.StylePriority.UseTextAlignment = false;
            this.lblCombigaddr.Text = "lblCombigaddr";
            this.lblCombigaddr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblcompanybig
            // 
            this.lblcompanybig.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanybig.LocationFloat = new DevExpress.Utils.PointFloat(30.84025F, 0F);
            this.lblcompanybig.Multiline = true;
            this.lblcompanybig.Name = "lblcompanybig";
            this.lblcompanybig.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanybig.SizeF = new System.Drawing.SizeF(295.6165F, 25.33332F);
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
            this.PageFooter.HeightF = 25F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Silver;
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(22.26819F, 0.6666851F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(315.4636F, 23.66663F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "ใบเสร็จรับเงินแบบ มี VAT";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.Visible = false;
            this.xrLabel3.WordWrap = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel7,
            this.lblAfterRound,
            this.xrLabel4,
            this.lblRound,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel21,
            this.lbltransfer,
            this.lblChange,
            this.lblcash,
            this.xrLabel16,
            this.xrLabel15,
            this.lblTotalAmtIncVat,
            this.lblVatInfoCaption,
            this.xrLine1,
            this.lblTotalVatAmt,
            this.lblVatCaption,
            this.xrLabel5,
            this.lblTotalAmt});
            this.GroupFooter1.HeightF = 236.1665F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseTextAlignment = false;
            this.GroupFooter1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.CanGrow = false;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(29.83344F, 114.5F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(154.2493F, 23.66663F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "สุทธิหลังปัดเศษ";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel7.WordWrap = false;
            // 
            // lblAfterRound
            // 
            this.lblAfterRound.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblAfterRound.LocationFloat = new DevExpress.Utils.PointFloat(185.0757F, 114.4999F);
            this.lblAfterRound.Multiline = true;
            this.lblAfterRound.Name = "lblAfterRound";
            this.lblAfterRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAfterRound.SizeF = new System.Drawing.SizeF(142.374F, 23.66662F);
            this.lblAfterRound.StylePriority.UseFont = false;
            this.lblAfterRound.StylePriority.UsePadding = false;
            this.lblAfterRound.StylePriority.UseTextAlignment = false;
            this.lblAfterRound.Text = "lblAfterRound";
            this.lblAfterRound.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblAfterRound.TextFormatString = "{0:n2}";
            this.lblAfterRound.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(29.83343F, 90.83332F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(154.2493F, 23.66663F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "ปัดเศษ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel4.WordWrap = false;
            // 
            // lblRound
            // 
            this.lblRound.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRound.LocationFloat = new DevExpress.Utils.PointFloat(185.0757F, 90.83324F);
            this.lblRound.Multiline = true;
            this.lblRound.Name = "lblRound";
            this.lblRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRound.SizeF = new System.Drawing.SizeF(142.374F, 23.66662F);
            this.lblRound.StylePriority.UseFont = false;
            this.lblRound.StylePriority.UsePadding = false;
            this.lblRound.StylePriority.UseTextAlignment = false;
            this.lblRound.Text = "lblRound";
            this.lblRound.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblRound.TextFormatString = "{0:n2}";
            this.lblRound.WordWrap = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(29.83327F, 67.16663F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(154.2492F, 23.66665F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "รวมทั้งสิ้น";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(30.82621F, 209.1665F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(263.2813F, 26.99998F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UsePadding = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "ขอบคุณที่ใช้บริการ";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel10.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.CanGrow = false;
            this.xrLabel21.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(30.82629F, 161.8331F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(154.2493F, 23.66662F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UsePadding = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "เงินโอน";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel21.WordWrap = false;
            // 
            // lbltransfer
            // 
            this.lbltransfer.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbltransfer.LocationFloat = new DevExpress.Utils.PointFloat(185.0757F, 161.8331F);
            this.lbltransfer.Multiline = true;
            this.lbltransfer.Name = "lbltransfer";
            this.lbltransfer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltransfer.SizeF = new System.Drawing.SizeF(142.374F, 23.66661F);
            this.lbltransfer.StylePriority.UseFont = false;
            this.lbltransfer.StylePriority.UsePadding = false;
            this.lbltransfer.StylePriority.UseTextAlignment = false;
            this.lbltransfer.Text = "lbltransfer";
            this.lbltransfer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lbltransfer.TextFormatString = "{0:n2}";
            this.lbltransfer.WordWrap = false;
            // 
            // lblChange
            // 
            this.lblChange.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblChange.LocationFloat = new DevExpress.Utils.PointFloat(185.0757F, 185.4999F);
            this.lblChange.Multiline = true;
            this.lblChange.Name = "lblChange";
            this.lblChange.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblChange.SizeF = new System.Drawing.SizeF(142.374F, 23.66661F);
            this.lblChange.StylePriority.UseFont = false;
            this.lblChange.StylePriority.UsePadding = false;
            this.lblChange.StylePriority.UseTextAlignment = false;
            this.lblChange.Text = "lblChange";
            this.lblChange.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblChange.TextFormatString = "{0:n2}";
            this.lblChange.WordWrap = false;
            // 
            // lblcash
            // 
            this.lblcash.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblcash.LocationFloat = new DevExpress.Utils.PointFloat(185.0757F, 138.1665F);
            this.lblcash.Multiline = true;
            this.lblcash.Name = "lblcash";
            this.lblcash.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcash.SizeF = new System.Drawing.SizeF(142.374F, 23.66662F);
            this.lblcash.StylePriority.UseFont = false;
            this.lblcash.StylePriority.UsePadding = false;
            this.lblcash.StylePriority.UseTextAlignment = false;
            this.lblcash.Text = "lblcash";
            this.lblcash.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblcash.TextFormatString = "{0:n2}";
            this.lblcash.WordWrap = false;
            // 
            // xrLabel16
            // 
            this.xrLabel16.CanGrow = false;
            this.xrLabel16.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(30.82629F, 185.4999F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(154.2491F, 23.66661F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UsePadding = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "เงินทอน";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel16.WordWrap = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.CanGrow = false;
            this.xrLabel15.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(30.82629F, 138.1666F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(154.2493F, 23.66663F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "เงินสด";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel15.WordWrap = false;
            // 
            // lblTotalAmtIncVat
            // 
            this.lblTotalAmtIncVat.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmtIncVat.LocationFloat = new DevExpress.Utils.PointFloat(184.0827F, 67.1666F);
            this.lblTotalAmtIncVat.Multiline = true;
            this.lblTotalAmtIncVat.Name = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalAmtIncVat.SizeF = new System.Drawing.SizeF(142.3741F, 23.66665F);
            this.lblTotalAmtIncVat.StylePriority.UseFont = false;
            this.lblTotalAmtIncVat.StylePriority.UsePadding = false;
            this.lblTotalAmtIncVat.StylePriority.UseTextAlignment = false;
            this.lblTotalAmtIncVat.Text = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmtIncVat.TextFormatString = "{0:n2}";
            this.lblTotalAmtIncVat.WordWrap = false;
            // 
            // lblVatInfoCaption
            // 
            this.lblVatInfoCaption.CanGrow = false;
            this.lblVatInfoCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblVatInfoCaption.LocationFloat = new DevExpress.Utils.PointFloat(29.83334F, 53.16661F);
            this.lblVatInfoCaption.Name = "lblVatInfoCaption";
            this.lblVatInfoCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblVatInfoCaption.SizeF = new System.Drawing.SizeF(154.2493F, 23.66665F);
            this.lblVatInfoCaption.StylePriority.UseFont = false;
            this.lblVatInfoCaption.StylePriority.UsePadding = false;
            this.lblVatInfoCaption.StylePriority.UseTextAlignment = false;
            this.lblVatInfoCaption.Text = "(ราคารวมภาษีมูลค่าเพิ่ม)";
            this.lblVatInfoCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblVatInfoCaption.WordWrap = false;
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(30.84025F, 47.33327F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(295.6165F, 5.83334F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // lblTotalVatAmt
            // 
            this.lblTotalVatAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalVatAmt.LocationFloat = new DevExpress.Utils.PointFloat(245.4149F, 23.66664F);
            this.lblTotalVatAmt.Multiline = true;
            this.lblTotalVatAmt.Name = "lblTotalVatAmt";
            this.lblTotalVatAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalVatAmt.SizeF = new System.Drawing.SizeF(81.04175F, 23.66663F);
            this.lblTotalVatAmt.StylePriority.UseFont = false;
            this.lblTotalVatAmt.StylePriority.UsePadding = false;
            this.lblTotalVatAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalVatAmt.Text = "lblTotalVatAmt";
            this.lblTotalVatAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalVatAmt.TextFormatString = "{0:n2}";
            this.lblTotalVatAmt.WordWrap = false;
            // 
            // lblVatCaption
            // 
            this.lblVatCaption.CanGrow = false;
            this.lblVatCaption.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblVatCaption.LocationFloat = new DevExpress.Utils.PointFloat(182.499F, 23.66664F);
            this.lblVatCaption.Name = "lblVatCaption";
            this.lblVatCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblVatCaption.SizeF = new System.Drawing.SizeF(62.91589F, 23.66663F);
            this.lblVatCaption.StylePriority.UseFont = false;
            this.lblVatCaption.StylePriority.UsePadding = false;
            this.lblVatCaption.StylePriority.UseTextAlignment = false;
            this.lblVatCaption.Text = "Vat7%";
            this.lblVatCaption.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblVatCaption.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(182.499F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(62.91591F, 23.66663F);
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
            this.lblTotalAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalAmt.LocationFloat = new DevExpress.Utils.PointFloat(245.4149F, 0F);
            this.lblTotalAmt.Name = "lblTotalAmt";
            this.lblTotalAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalAmt.SizeF = new System.Drawing.SizeF(81.04175F, 23.66663F);
            this.lblTotalAmt.StylePriority.UseFont = false;
            this.lblTotalAmt.StylePriority.UsePadding = false;
            this.lblTotalAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalAmt.Text = "lblTotalAmt";
            this.lblTotalAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmt.TextFormatString = "{0:n2}";
            this.lblTotalAmt.WordWrap = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.CanGrow = false;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(294.1075F, 209.1665F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(33.34915F, 26.99998F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "R402";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel8.WordWrap = false;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Robot.Data.POS_SaleLine);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // R402
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
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 18);
            this.PageHeight = 1173;
            this.PageWidth = 360;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.RollPaper = true;
            this.SnapGridSize = 9.84252F;
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
        private DevExpress.XtraReports.UI.XRLabel lblCombigtax;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmt;
        private DevExpress.XtraReports.UI.XRLabel lblTotalVatAmt;
        private DevExpress.XtraReports.UI.XRLabel lblVatCaption;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmtIncVat;
        private DevExpress.XtraReports.UI.XRLabel lblVatInfoCaption;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel lblChange;
        private DevExpress.XtraReports.UI.XRLabel lblcash;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel lbltoday;
        private DevExpress.XtraReports.UI.XRLabel lbluserlogin;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel lbltransfer;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel lblinvid;
        private DevExpress.XtraReports.UI.XRLabel lblcompanyBranch;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel lblAfterRound;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel lblRound;
        private DevExpress.XtraReports.UI.XRPictureBox logoimg;
        private DevExpress.XtraReports.UI.XRLabel lbltable;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
    }
}
