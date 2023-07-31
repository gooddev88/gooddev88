namespace Robot.POSC.POSPrint
{
    partial class R421
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
            this.lblcompanyBranch = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigtax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblinvid = new DevExpress.XtraReports.UI.XRLabel();
            this.lblisfull = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustTaxID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustBranchName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustomerName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltoday = new DevExpress.XtraReports.UI.XRLabel();
            this.lbluserlogin = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblline2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigaddr = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanybig = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblChange = new DevExpress.XtraReports.UI.XRLabel();
            this.lbltransfer = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcash = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmtIncVat = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblTotalVatAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustAddr = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(23.56674F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(38.40762F, 30F);
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
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(249.5502F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(83.28342F, 30F);
            this.xrLabel2.StylePriority.UseBorderDashStyle = false;
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UsePadding = false;
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
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(61.97436F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(187.5759F, 30F);
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
            this.BottomMargin.HeightF = 56F;
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
            this.lblCustAddr,
            this.logoimg,
            this.lblcompanyBranch,
            this.lblCombigtax,
            this.xrLabel26,
            this.lblinvid,
            this.lblisfull,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.lblCustTaxID,
            this.lblCustBranchName,
            this.lblCustomerName,
            this.xrLine4,
            this.lbltoday,
            this.lbluserlogin,
            this.xrLabel20,
            this.xrLabel19,
            this.lblline2,
            this.xrLine2,
            this.xrLabel1,
            this.lblCombigaddr,
            this.lblcompanybig});
            this.PageHeader.ForeColor = System.Drawing.Color.Black;
            this.PageHeader.HeightF = 417.8779F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 10, 100F);
            this.PageHeader.StylePriority.UseForeColor = false;
            // 
            // logoimg
            // 
            this.logoimg.LocationFloat = new DevExpress.Utils.PointFloat(133.8128F, 59.16333F);
            this.logoimg.Name = "logoimg";
            this.logoimg.SizeF = new System.Drawing.SizeF(86.3038F, 47F);
            this.logoimg.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblcompanyBranch
            // 
            this.lblcompanyBranch.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanyBranch.LocationFloat = new DevExpress.Utils.PointFloat(35.56668F, 106.1633F);
            this.lblcompanyBranch.Multiline = true;
            this.lblcompanyBranch.Name = "lblcompanyBranch";
            this.lblcompanyBranch.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanyBranch.SizeF = new System.Drawing.SizeF(289.1662F, 25.33332F);
            this.lblcompanyBranch.StylePriority.UseFont = false;
            this.lblcompanyBranch.StylePriority.UsePadding = false;
            this.lblcompanyBranch.StylePriority.UseTextAlignment = false;
            this.lblcompanyBranch.Text = "lblcompanyBranch";
            this.lblcompanyBranch.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblCombigtax
            // 
            this.lblCombigtax.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigtax.LocationFloat = new DevExpress.Utils.PointFloat(34.56668F, 159.3367F);
            this.lblCombigtax.Multiline = true;
            this.lblCombigtax.Name = "lblCombigtax";
            this.lblCombigtax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigtax.SizeF = new System.Drawing.SizeF(290.1664F, 26.32999F);
            this.lblCombigtax.StylePriority.UseFont = false;
            this.lblCombigtax.StylePriority.UsePadding = false;
            this.lblCombigtax.StylePriority.UseTextAlignment = false;
            this.lblCombigtax.Text = "lblCombigtax";
            this.lblCombigtax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel26
            // 
            this.xrLabel26.CanGrow = false;
            this.xrLabel26.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(177.5976F, 0.8333333F);
            this.xrLabel26.Multiline = true;
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(106.1895F, 33F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UsePadding = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "ออกแทนใบกำกับภาษีแบบย่อ";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel26.Visible = false;
            // 
            // lblinvid
            // 
            this.lblinvid.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblinvid.LocationFloat = new DevExpress.Utils.PointFloat(178.6046F, 10.16673F);
            this.lblinvid.Multiline = true;
            this.lblinvid.Name = "lblinvid";
            this.lblinvid.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblinvid.SizeF = new System.Drawing.SizeF(159.1504F, 23.66661F);
            this.lblinvid.StylePriority.UseFont = false;
            this.lblinvid.StylePriority.UsePadding = false;
            this.lblinvid.StylePriority.UseTextAlignment = false;
            this.lblinvid.Text = "lblinvid";
            this.lblinvid.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblinvid.Visible = false;
            this.lblinvid.WordWrap = false;
            // 
            // lblisfull
            // 
            this.lblisfull.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblisfull.LocationFloat = new DevExpress.Utils.PointFloat(155.5825F, 212.6665F);
            this.lblisfull.Multiline = true;
            this.lblisfull.Name = "lblisfull";
            this.lblisfull.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblisfull.SizeF = new System.Drawing.SizeF(169.1504F, 23.66663F);
            this.lblisfull.StylePriority.UseFont = false;
            this.lblisfull.StylePriority.UsePadding = false;
            this.lblisfull.StylePriority.UseTextAlignment = false;
            this.lblisfull.Text = "lblisfull";
            this.lblisfull.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblisfull.WordWrap = false;
            // 
            // xrLabel25
            // 
            this.xrLabel25.CanGrow = false;
            this.xrLabel25.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(35.56668F, 212.6667F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(120.0158F, 23.66664F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UsePadding = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "เลขที่";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel25.WordWrap = false;
            // 
            // xrLabel24
            // 
            this.xrLabel24.CanGrow = false;
            this.xrLabel24.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(35.56668F, 335.2113F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 2, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(73.41587F, 23.66664F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UsePadding = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "เลขผู้เสียภาษี";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel24.WordWrap = false;
            // 
            // xrLabel23
            // 
            this.xrLabel23.CanGrow = false;
            this.xrLabel23.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(36.33343F, 266.0447F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(73.41589F, 69.1666F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UsePadding = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "ที่อยู่";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel23.WordWrap = false;
            // 
            // xrLabel22
            // 
            this.xrLabel22.CanGrow = false;
            this.xrLabel22.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(215.1165F, 335.2113F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 2, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(29.88329F, 23.66663F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UsePadding = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "สาขา";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel22.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.CanGrow = false;
            this.xrLabel21.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(35.56668F, 242.3779F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(73.41589F, 23.66663F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UsePadding = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "ลูกค้า";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrLabel21.WordWrap = false;
            // 
            // lblCustTaxID
            // 
            this.lblCustTaxID.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCustTaxID.LocationFloat = new DevExpress.Utils.PointFloat(108.9826F, 335.2113F);
            this.lblCustTaxID.Multiline = true;
            this.lblCustTaxID.Name = "lblCustTaxID";
            this.lblCustTaxID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 0, 100F);
            this.lblCustTaxID.SizeF = new System.Drawing.SizeF(106.134F, 23.66663F);
            this.lblCustTaxID.StylePriority.UseFont = false;
            this.lblCustTaxID.StylePriority.UsePadding = false;
            this.lblCustTaxID.StylePriority.UseTextAlignment = false;
            this.lblCustTaxID.Text = "lblCustTaxID";
            this.lblCustTaxID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblCustBranchName
            // 
            this.lblCustBranchName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCustBranchName.LocationFloat = new DevExpress.Utils.PointFloat(244.9998F, 335.2113F);
            this.lblCustBranchName.Multiline = true;
            this.lblCustBranchName.Name = "lblCustBranchName";
            this.lblCustBranchName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 0, 100F);
            this.lblCustBranchName.SizeF = new System.Drawing.SizeF(84.28349F, 23.66664F);
            this.lblCustBranchName.StylePriority.UseFont = false;
            this.lblCustBranchName.StylePriority.UsePadding = false;
            this.lblCustBranchName.StylePriority.UseTextAlignment = false;
            this.lblCustBranchName.Text = "lblCustBranchName";
            this.lblCustBranchName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCustomerName.LocationFloat = new DevExpress.Utils.PointFloat(108.9826F, 242.378F);
            this.lblCustomerName.Multiline = true;
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCustomerName.SizeF = new System.Drawing.SizeF(220.3008F, 23.66664F);
            this.lblCustomerName.StylePriority.UseFont = false;
            this.lblCustomerName.StylePriority.UsePadding = false;
            this.lblCustomerName.StylePriority.UseTextAlignment = false;
            this.lblCustomerName.Text = "lblCustomerName";
            this.lblCustomerName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(34.56667F, 236.5447F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(294.7166F, 5.833328F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            // 
            // lbltoday
            // 
            this.lbltoday.CanGrow = false;
            this.lbltoday.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbltoday.LocationFloat = new DevExpress.Utils.PointFloat(109.7493F, 388.3779F);
            this.lbltoday.Name = "lbltoday";
            this.lbltoday.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltoday.SizeF = new System.Drawing.SizeF(219.5339F, 23.66663F);
            this.lbltoday.StylePriority.UseFont = false;
            this.lbltoday.StylePriority.UsePadding = false;
            this.lbltoday.StylePriority.UseTextAlignment = false;
            this.lbltoday.Text = "lbltoday";
            this.lbltoday.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbltoday.WordWrap = false;
            // 
            // lbluserlogin
            // 
            this.lbluserlogin.CanGrow = false;
            this.lbluserlogin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbluserlogin.LocationFloat = new DevExpress.Utils.PointFloat(108.9826F, 364.7112F);
            this.lbluserlogin.Name = "lbluserlogin";
            this.lbluserlogin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbluserlogin.SizeF = new System.Drawing.SizeF(220.3006F, 23.66663F);
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
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(34.56668F, 388.3779F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(74.41591F, 23.66663F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UsePadding = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "วันที่ :";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel20.WordWrap = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.CanGrow = false;
            this.xrLabel19.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(34.56668F, 364.7112F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(74.41589F, 23.66663F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UsePadding = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "พนักงานขาย :";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel19.WordWrap = false;
            // 
            // lblline2
            // 
            this.lblline2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.lblline2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblline2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.lblline2.LocationFloat = new DevExpress.Utils.PointFloat(23.17367F, 358.8779F);
            this.lblline2.Name = "lblline2";
            this.lblline2.SizeF = new System.Drawing.SizeF(306.1096F, 5.833374F);
            this.lblline2.StylePriority.UseBorderDashStyle = false;
            this.lblline2.StylePriority.UseBorders = false;
            // 
            // xrLine2
            // 
            this.xrLine2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(23.17367F, 412.0446F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(306.1095F, 5.833344F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(35.56668F, 185.6667F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(289.1662F, 26.99998F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ใบเสร็จรับเงิน/ใบกํากับภาษี";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel1.WordWrap = false;
            // 
            // lblCombigaddr
            // 
            this.lblCombigaddr.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigaddr.LocationFloat = new DevExpress.Utils.PointFloat(34.56668F, 132.1634F);
            this.lblCombigaddr.Multiline = true;
            this.lblCombigaddr.Name = "lblCombigaddr";
            this.lblCombigaddr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigaddr.SizeF = new System.Drawing.SizeF(290.1664F, 26.32999F);
            this.lblCombigaddr.StylePriority.UseFont = false;
            this.lblCombigaddr.StylePriority.UsePadding = false;
            this.lblCombigaddr.StylePriority.UseTextAlignment = false;
            this.lblCombigaddr.Text = "lblCombigaddr";
            this.lblCombigaddr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblcompanybig
            // 
            this.lblcompanybig.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanybig.LocationFloat = new DevExpress.Utils.PointFloat(34.56674F, 33.83334F);
            this.lblcompanybig.Multiline = true;
            this.lblcompanybig.Name = "lblcompanybig";
            this.lblcompanybig.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanybig.SizeF = new System.Drawing.SizeF(290.1664F, 25.32999F);
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
            this.PageFooter.HeightF = 26.33331F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Silver;
            this.xrLabel3.CanGrow = false;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(22.2682F, 1.33334F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(315.4636F, 23.66663F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "ใบกำกับแบบเต็ม มี VAT";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.Visible = false;
            this.xrLabel3.WordWrap = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel11,
            this.xrLine3,
            this.xrLabel7,
            this.xrLabel6,
            this.lblChange,
            this.lbltransfer,
            this.lblcash,
            this.xrLabel16,
            this.xrLabel15,
            this.lblTotalAmtIncVat,
            this.xrLine1,
            this.lblTotalVatAmt,
            this.xrLabel8,
            this.xrLabel5,
            this.lblTotalAmt});
            this.GroupFooter1.HeightF = 227.4445F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanGrow = false;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(21.55977F, 53.16661F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(160.9159F, 23.66665F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UsePadding = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "(ราคารวมภาษีมูลค่าเพิ่ม)";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel10.WordWrap = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanGrow = false;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(21.55977F, 67.1666F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(98.75618F, 23.66665F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "รวมทั้งสิ้น";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLine3
            // 
            this.xrLine3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine3.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(47.39309F, 194.6111F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(256.3399F, 5.833344F);
            this.xrLine3.StylePriority.UseBorderDashStyle = false;
            // 
            // xrLabel7
            // 
            this.xrLabel7.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(47.3931F, 200.4445F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(256.34F, 27.00002F);
            this.xrLabel7.StylePriority.UseBorderDashStyle = false;
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "ผู้รับเงิน";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel7.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(21.55983F, 139.1665F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(98.75618F, 23.66663F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "เงินทอน";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel6.WordWrap = false;
            // 
            // lblChange
            // 
            this.lblChange.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblChange.LocationFloat = new DevExpress.Utils.PointFloat(234.9999F, 139.1664F);
            this.lblChange.Multiline = true;
            this.lblChange.Name = "lblChange";
            this.lblChange.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lblChange.SizeF = new System.Drawing.SizeF(83.2834F, 23.66663F);
            this.lblChange.StylePriority.UseFont = false;
            this.lblChange.StylePriority.UsePadding = false;
            this.lblChange.StylePriority.UseTextAlignment = false;
            this.lblChange.Text = "lblChange";
            this.lblChange.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblChange.TextFormatString = "{0:n2}";
            this.lblChange.WordWrap = false;
            // 
            // lbltransfer
            // 
            this.lbltransfer.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbltransfer.LocationFloat = new DevExpress.Utils.PointFloat(234.9999F, 115.4998F);
            this.lbltransfer.Multiline = true;
            this.lbltransfer.Name = "lbltransfer";
            this.lbltransfer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lbltransfer.SizeF = new System.Drawing.SizeF(83.2834F, 23.66662F);
            this.lbltransfer.StylePriority.UseFont = false;
            this.lbltransfer.StylePriority.UsePadding = false;
            this.lbltransfer.StylePriority.UseTextAlignment = false;
            this.lbltransfer.Text = "lbltransfer";
            this.lbltransfer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lbltransfer.TextFormatString = "{0:n2}";
            this.lbltransfer.WordWrap = false;
            // 
            // lblcash
            // 
            this.lblcash.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblcash.LocationFloat = new DevExpress.Utils.PointFloat(234.9999F, 91.83319F);
            this.lblcash.Multiline = true;
            this.lblcash.Name = "lblcash";
            this.lblcash.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lblcash.SizeF = new System.Drawing.SizeF(83.2834F, 23.66663F);
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
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(21.55977F, 115.4999F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(98.75622F, 23.66663F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UsePadding = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "เงินโอน";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel16.WordWrap = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.CanGrow = false;
            this.xrLabel15.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(21.55983F, 91.8333F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(98.75612F, 23.66664F);
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
            this.lblTotalAmtIncVat.LocationFloat = new DevExpress.Utils.PointFloat(235F, 67.1666F);
            this.lblTotalAmtIncVat.Multiline = true;
            this.lblTotalAmtIncVat.Name = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lblTotalAmtIncVat.SizeF = new System.Drawing.SizeF(83.28336F, 23.66665F);
            this.lblTotalAmtIncVat.StylePriority.UseFont = false;
            this.lblTotalAmtIncVat.StylePriority.UsePadding = false;
            this.lblTotalAmtIncVat.StylePriority.UseTextAlignment = false;
            this.lblTotalAmtIncVat.Text = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmtIncVat.TextFormatString = "{0:n2}";
            this.lblTotalAmtIncVat.WordWrap = false;
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(36.83351F, 47.33327F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(296.0001F, 5.833344F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // lblTotalVatAmt
            // 
            this.lblTotalVatAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalVatAmt.LocationFloat = new DevExpress.Utils.PointFloat(234.9999F, 23.66666F);
            this.lblTotalVatAmt.Multiline = true;
            this.lblTotalVatAmt.Name = "lblTotalVatAmt";
            this.lblTotalVatAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lblTotalVatAmt.SizeF = new System.Drawing.SizeF(97.83366F, 23.66663F);
            this.lblTotalVatAmt.StylePriority.UseFont = false;
            this.lblTotalVatAmt.StylePriority.UsePadding = false;
            this.lblTotalVatAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalVatAmt.Text = "lblTotalVatAmt";
            this.lblTotalVatAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalVatAmt.TextFormatString = "{0:n2}";
            this.lblTotalVatAmt.WordWrap = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.CanGrow = false;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(23.56667F, 23.66666F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(211.4332F, 23.66663F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Vat7%";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel8.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanGrow = false;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(23.56667F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(211.4332F, 23.66663F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "รวมเป็นเงิน";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel5.WordWrap = false;
            // 
            // lblTotalAmt
            // 
            this.lblTotalAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalAmt.LocationFloat = new DevExpress.Utils.PointFloat(234.9999F, 0F);
            this.lblTotalAmt.Multiline = true;
            this.lblTotalAmt.Name = "lblTotalAmt";
            this.lblTotalAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 4, 0, 0, 100F);
            this.lblTotalAmt.SizeF = new System.Drawing.SizeF(97.83366F, 23.66663F);
            this.lblTotalAmt.StylePriority.UseFont = false;
            this.lblTotalAmt.StylePriority.UsePadding = false;
            this.lblTotalAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalAmt.Text = "lblTotalAmt";
            this.lblTotalAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmt.TextFormatString = "{0:n2}";
            this.lblTotalAmt.WordWrap = false;
            // 
            // lblCustAddr
            // 
            this.lblCustAddr.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCustAddr.LocationFloat = new DevExpress.Utils.PointFloat(109.7493F, 266.0447F);
            this.lblCustAddr.Multiline = true;
            this.lblCustAddr.Name = "lblCustAddr";
            this.lblCustAddr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCustAddr.SizeF = new System.Drawing.SizeF(220.3008F, 69.1666F);
            this.lblCustAddr.StylePriority.UseFont = false;
            this.lblCustAddr.Text = "lblCustAddr";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Robot.Data.POS_SaleLine);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // R421
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
            this.Margins = new System.Drawing.Printing.Margins(0, 1, 0, 56);
            this.PageHeight = 1169;
            this.PageWidth = 361;
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
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmtIncVat;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel lbltransfer;
        private DevExpress.XtraReports.UI.XRLabel lblcash;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLine lblline2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel lbltoday;
        private DevExpress.XtraReports.UI.XRLabel lbluserlogin;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel lblCustomerName;
        private DevExpress.XtraReports.UI.XRLabel lblCustTaxID;
        private DevExpress.XtraReports.UI.XRLabel lblCustBranchName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLabel lblinvid;
        private DevExpress.XtraReports.UI.XRLabel lblisfull;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel lblChange;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel lblcompanyBranch;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRPictureBox logoimg;
        private DevExpress.XtraReports.UI.XRLabel lblCustAddr;
    }
}
