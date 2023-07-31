namespace Robot.POSC.POSPrint
{
    partial class R412 {
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
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.logoimg = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbltoday = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDocID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanyBranch = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.lbluserlogin = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lbltable = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigtax = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCombigaddr = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcompanybig = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblNetotalAfterRound = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRound = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmtIncVat = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblTotalVatAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVatCaption = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmt = new DevExpress.XtraReports.UI.XRLabel();
            this.lblBarCode = new DevExpress.XtraReports.UI.XRBarCode();
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
            this.Detail.HeightF = 30.00001F;
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
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(25.66667F, 2.543131E-05F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(41.08394F, 29.99998F);
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
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(245.4149F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(84.20844F, 29.99998F);
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
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(66.75062F, 0F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(178.6643F, 30F);
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
            this.logoimg,
            this.xrLabel20,
            this.lbltoday,
            this.lblDocID,
            this.lblcompanyBranch,
            this.xrLine4,
            this.lbluserlogin,
            this.xrLabel19,
            this.xrLine2,
            this.lbltable,
            this.lblCombigtax,
            this.lblCombigaddr,
            this.lblcompanybig});
            this.PageHeader.ForeColor = System.Drawing.Color.Black;
            this.PageHeader.HeightF = 240.8242F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.SnapLinePadding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 10, 10, 100F);
            this.PageHeader.StylePriority.UseForeColor = false;
            // 
            // logoimg
            // 
            this.logoimg.LocationFloat = new DevExpress.Utils.PointFloat(136.8481F, 25.33332F);
            this.logoimg.Name = "logoimg";
            this.logoimg.SizeF = new System.Drawing.SizeF(86.3038F, 47F);
            this.logoimg.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel20
            // 
            this.xrLabel20.CanGrow = false;
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(24.6666F, 210.8241F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(42.08402F, 23.66667F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UsePadding = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "วันที่";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel20.WordWrap = false;
            // 
            // lbltoday
            // 
            this.lbltoday.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbltoday.LocationFloat = new DevExpress.Utils.PointFloat(66.75062F, 210.8242F);
            this.lbltoday.Multiline = true;
            this.lbltoday.Name = "lbltoday";
            this.lbltoday.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltoday.SizeF = new System.Drawing.SizeF(81.43272F, 23.66663F);
            this.lbltoday.StylePriority.UseFont = false;
            this.lbltoday.StylePriority.UsePadding = false;
            this.lbltoday.StylePriority.UseTextAlignment = false;
            this.lbltoday.Text = "lbltoday";
            this.lbltoday.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbltoday.WordWrap = false;
            // 
            // lblDocID
            // 
            this.lblDocID.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblDocID.LocationFloat = new DevExpress.Utils.PointFloat(148.1833F, 210.8242F);
            this.lblDocID.Multiline = true;
            this.lblDocID.Name = "lblDocID";
            this.lblDocID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDocID.SizeF = new System.Drawing.SizeF(181.4398F, 23.66663F);
            this.lblDocID.StylePriority.UseFont = false;
            this.lblDocID.StylePriority.UsePadding = false;
            this.lblDocID.StylePriority.UseTextAlignment = false;
            this.lblDocID.Text = "lblDocID";
            this.lblDocID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblDocID.WordWrap = false;
            // 
            // lblcompanyBranch
            // 
            this.lblcompanyBranch.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanyBranch.LocationFloat = new DevExpress.Utils.PointFloat(25.66671F, 72.3333F);
            this.lblcompanyBranch.Multiline = true;
            this.lblcompanyBranch.Name = "lblcompanyBranch";
            this.lblcompanyBranch.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanyBranch.SizeF = new System.Drawing.SizeF(303.9565F, 25.33332F);
            this.lblcompanyBranch.StylePriority.UseFont = false;
            this.lblcompanyBranch.StylePriority.UsePadding = false;
            this.lblcompanyBranch.StylePriority.UseTextAlignment = false;
            this.lblcompanyBranch.Text = "lblcompanyBranch";
            this.lblcompanyBranch.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine4
            // 
            this.xrLine4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrLine4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(25.6669F, 150.9999F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(304.9566F, 5.833328F);
            this.xrLine4.StylePriority.UseBorderDashStyle = false;
            // 
            // lbluserlogin
            // 
            this.lbluserlogin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lbluserlogin.LocationFloat = new DevExpress.Utils.PointFloat(92.58957F, 186.6666F);
            this.lbluserlogin.Multiline = true;
            this.lbluserlogin.Name = "lbluserlogin";
            this.lbluserlogin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbluserlogin.SizeF = new System.Drawing.SizeF(237.0338F, 23.66664F);
            this.lbluserlogin.StylePriority.UseFont = false;
            this.lbluserlogin.StylePriority.UsePadding = false;
            this.lbluserlogin.StylePriority.UseTextAlignment = false;
            this.lbluserlogin.Text = "lbluserlogin";
            this.lbluserlogin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lbluserlogin.WordWrap = false;
            // 
            // xrLabel19
            // 
            this.xrLabel19.CanGrow = false;
            this.xrLabel19.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(24.66666F, 186.6667F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(67.92291F, 23.66661F);
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
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(24.66666F, 234.4908F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(304.9565F, 5.833344F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            // 
            // lbltable
            // 
            this.lbltable.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltable.LocationFloat = new DevExpress.Utils.PointFloat(25.66667F, 159.6667F);
            this.lbltable.Multiline = true;
            this.lbltable.Name = "lbltable";
            this.lbltable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltable.SizeF = new System.Drawing.SizeF(303.9566F, 26.99999F);
            this.lbltable.StylePriority.UseFont = false;
            this.lbltable.StylePriority.UsePadding = false;
            this.lbltable.StylePriority.UseTextAlignment = false;
            this.lbltable.Text = "lbltable";
            this.lbltable.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lbltable.WordWrap = false;
            // 
            // lblCombigtax
            // 
            this.lblCombigtax.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigtax.LocationFloat = new DevExpress.Utils.PointFloat(24.6666F, 124.6666F);
            this.lblCombigtax.Name = "lblCombigtax";
            this.lblCombigtax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigtax.SizeF = new System.Drawing.SizeF(304.9566F, 26.33331F);
            this.lblCombigtax.StylePriority.UseFont = false;
            this.lblCombigtax.StylePriority.UsePadding = false;
            this.lblCombigtax.StylePriority.UseTextAlignment = false;
            this.lblCombigtax.Text = "lblCombigtax";
            this.lblCombigtax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblCombigaddr
            // 
            this.lblCombigaddr.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblCombigaddr.LocationFloat = new DevExpress.Utils.PointFloat(24.66666F, 98.33331F);
            this.lblCombigaddr.Name = "lblCombigaddr";
            this.lblCombigaddr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 3, 0, 100F);
            this.lblCombigaddr.SizeF = new System.Drawing.SizeF(304.9566F, 26.33329F);
            this.lblCombigaddr.StylePriority.UseFont = false;
            this.lblCombigaddr.StylePriority.UsePadding = false;
            this.lblCombigaddr.StylePriority.UseTextAlignment = false;
            this.lblCombigaddr.Text = "lblCombigaddr";
            this.lblCombigaddr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblcompanybig
            // 
            this.lblcompanybig.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblcompanybig.LocationFloat = new DevExpress.Utils.PointFloat(25.66667F, 0F);
            this.lblcompanybig.Multiline = true;
            this.lblcompanybig.Name = "lblcompanybig";
            this.lblcompanybig.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcompanybig.SizeF = new System.Drawing.SizeF(303.9566F, 25.33332F);
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
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(22.2682F, 0.6666851F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(315.4636F, 23.66663F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "ออเดอร์ มี VAT";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel3.Visible = false;
            this.xrLabel3.WordWrap = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblBarCode,
            this.xrLabel4,
            this.lblNetotalAfterRound,
            this.xrLabel6,
            this.xrLabel1,
            this.lblRound,
            this.lblTotalAmtIncVat,
            this.xrLabel12,
            this.xrLine1,
            this.lblTotalVatAmt,
            this.lblVatCaption,
            this.xrLabel5,
            this.lblTotalAmt});
            this.GroupFooter1.HeightF = 158.5894F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseTextAlignment = false;
            this.GroupFooter1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblNetotalAfterRound
            // 
            this.lblNetotalAfterRound.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblNetotalAfterRound.LocationFloat = new DevExpress.Utils.PointFloat(238.1319F, 100.4999F);
            this.lblNetotalAfterRound.Multiline = true;
            this.lblNetotalAfterRound.Name = "lblNetotalAfterRound";
            this.lblNetotalAfterRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNetotalAfterRound.SizeF = new System.Drawing.SizeF(91.49158F, 23.66665F);
            this.lblNetotalAfterRound.StylePriority.UseFont = false;
            this.lblNetotalAfterRound.StylePriority.UsePadding = false;
            this.lblNetotalAfterRound.StylePriority.UseTextAlignment = false;
            this.lblNetotalAfterRound.Text = "lblNetotalAfterRound";
            this.lblNetotalAfterRound.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblNetotalAfterRound.TextFormatString = "{0:n2}";
            this.lblNetotalAfterRound.WordWrap = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.CanGrow = false;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(25.6669F, 100.4998F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(212.465F, 23.66665F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "สุทธิหลังปัดเศษ";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.CanGrow = false;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(25.66677F, 70.9999F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(212.465F, 23.66665F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ปัดเศษ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel1.WordWrap = false;
            // 
            // lblRound
            // 
            this.lblRound.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblRound.LocationFloat = new DevExpress.Utils.PointFloat(238.1318F, 70.99993F);
            this.lblRound.Multiline = true;
            this.lblRound.Name = "lblRound";
            this.lblRound.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRound.SizeF = new System.Drawing.SizeF(91.49158F, 23.66665F);
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
            this.lblTotalAmtIncVat.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmtIncVat.LocationFloat = new DevExpress.Utils.PointFloat(238.1318F, 47.33327F);
            this.lblTotalAmtIncVat.Multiline = true;
            this.lblTotalAmtIncVat.Name = "lblTotalAmtIncVat";
            this.lblTotalAmtIncVat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalAmtIncVat.SizeF = new System.Drawing.SizeF(91.49158F, 23.66665F);
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
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(25.66674F, 47.33327F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(212.465F, 23.66665F);
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
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(25.66677F, 94.6666F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(303.9567F, 5.833344F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // lblTotalVatAmt
            // 
            this.lblTotalVatAmt.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblTotalVatAmt.LocationFloat = new DevExpress.Utils.PointFloat(245.4149F, 23.66664F);
            this.lblTotalVatAmt.Multiline = true;
            this.lblTotalVatAmt.Name = "lblTotalVatAmt";
            this.lblTotalVatAmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalVatAmt.SizeF = new System.Drawing.SizeF(84.20837F, 23.66663F);
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
            this.lblVatCaption.LocationFloat = new DevExpress.Utils.PointFloat(179.499F, 23.66664F);
            this.lblVatCaption.Name = "lblVatCaption";
            this.lblVatCaption.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblVatCaption.SizeF = new System.Drawing.SizeF(65.91588F, 23.66663F);
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
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(179.499F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(65.91591F, 23.66663F);
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
            this.lblTotalAmt.SizeF = new System.Drawing.SizeF(84.20837F, 23.66663F);
            this.lblTotalAmt.StylePriority.UseFont = false;
            this.lblTotalAmt.StylePriority.UsePadding = false;
            this.lblTotalAmt.StylePriority.UseTextAlignment = false;
            this.lblTotalAmt.Text = "lblTotalAmt";
            this.lblTotalAmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lblTotalAmt.TextFormatString = "{0:n2}";
            this.lblTotalAmt.WordWrap = false;
            // 
            // lblBarCode
            // 
            this.lblBarCode.AutoModule = true;
            this.lblBarCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblBarCode.LocationFloat = new DevExpress.Utils.PointFloat(1.815294F, 23.66664F);
            this.lblBarCode.Module = 5F;
            this.lblBarCode.Name = "lblBarCode";
            this.lblBarCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.lblBarCode.ShowText = false;
            this.lblBarCode.SizeF = new System.Drawing.SizeF(146.368F, 130.5544F);
            this.lblBarCode.StylePriority.UseBorders = false;
            this.lblBarCode.StylePriority.UseTextAlignment = false;
            this.lblBarCode.Symbology = qrCodeGenerator1;
            this.lblBarCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblBarCode.Visible = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1.815294F, 0F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(146.368F, 26.99997F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "สแกนจ่าย QR Code";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel4.Visible = false;
            this.xrLabel4.WordWrap = false;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(Robot.Data.POS_SaleLine);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // R412
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
        private DevExpress.XtraReports.UI.XRLabel lbltable;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmt;
        private DevExpress.XtraReports.UI.XRLabel lblTotalVatAmt;
        private DevExpress.XtraReports.UI.XRLabel lblVatCaption;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmtIncVat;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel lbluserlogin;
        private DevExpress.XtraReports.UI.XRLabel lblcompanyBranch;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel lbltoday;
        private DevExpress.XtraReports.UI.XRLabel lblDocID;
        private DevExpress.XtraReports.UI.XRLabel lblNetotalAfterRound;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblRound;
        private DevExpress.XtraReports.UI.XRPictureBox logoimg;
        private DevExpress.XtraReports.UI.XRBarCode lblBarCode;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    }
}
