<%@ Page Title="รายงานยอดขาย" Language="C#" EnableEventValidation="false" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSReports.aspx.cs" Inherits="Robot.POSC.POSReports" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />

    <asp:HiddenField ID="hddPreviouspage" runat="server" />

    <asp:HiddenField ID="hddParentPage" runat="server" />

    <%--begin of Loading callback script--%>
    <script>
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };

        function OnBeginCallback(s, e) {
            LoadingPanel.Show();
        };

        function OnEndCallback(s, e) {
            LoadingPanel.Hide();
        };
        function OnClosePopupAlert() {
            popAlert.Hide();
        }

    </script>

    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="LoadingPanel"
        Theme="Material"
        runat="server"
        Modal="true"
        HorizontalAlign="Center"
        VerticalAlign="Middle">
    </dx:ASPxLoadingPanel>

    <asp:UpdatePanel ID="udpAlert" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="600" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
                HeaderText="Infomation" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsg" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="OK" OnClientClick="return OnClosePopupAlert();" />


                                            </div>

                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>

            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="row pt-2">
        <div class="col-12">

            <div class="card">
                <div class="card-header pt-1 pb-1">
                    <div class="row">
                        <div class="col-md-8">

                            <span>
                                <asp:Label ID="lblTopic" runat="server"></asp:Label>

                            </span>
                        </div>
                        <div class="col-md-4 text-right pb-3">
                            <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnExcel" CssClass="btn btn btn-secondary btn-sm " runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>
                                <asp:LinkButton ID="btnRefreshDataDiff" CssClass="btn btn btn-danger btn-sm " runat="server" Text="Recal Bill" OnClick="btnRefreshDataDiff_Click"></asp:LinkButton>
                                <asp:LinkButton ID="btnClear" CssClass="btn btn btn-danger btn-sm " runat="server" Text="เคลียไฟล์ขยะ" OnClick="btnClear_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row ">
                        <div class="col-md-11">
                            <asp:UpdatePanel ID="uptfilterby" runat="server">
                                <ContentTemplate>
                                    <div class="row">

                                        <div class="col-md-4" id="divdateFilter" runat="server">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <span>วันที่ขาย (ตั้งแต่)</span>
                                                    <dx:ASPxDateEdit ID="dtBegin" DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material" Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-6">
                                                    <span>ถึงวันที่</span>
                                                    <dx:ASPxDateEdit ID="dtEnd" DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material" Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span>สาขา </span>
                                                    <dx:ASPxComboBox ID="cboCompany"
                                                        runat="server" DropDownStyle="DropDown"
                                                        Theme="Material"
                                                        CssClass="Sarabun"
                                                        ValueField="CompanyID" ValueType="System.String"
                                                        ViewStateMode="Enabled" TextFormatString="{0} {1}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="CompanyID" Caption="รหัส" />
                                                            <dx:ListBoxColumn FieldName="Name" Width="300px" Caption="ชื่อสาขา" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="divsearch" runat="server" style="display: none;">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span>Search &nbsp</span>
                                                    <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="Material"></dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <br />
                                            <dx:ASPxButton ID="btnSearch" runat="server" Text="Load" BackColor="#009933" AutoPostBack="false" Theme="Material">
                                                <ClientSideEvents Click="SendCommentCallback" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </ContentTemplate>

                            </asp:UpdatePanel>


                        </div>

                        <div class="row">
                            <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </div>

    <div class="row  pt-2 text-center">
        <div class="col-12">
            <div class="input-group">
                <asp:LinkButton ID="RptPOS140" CssClass="btn btn-secondary " runat="server" Text="รายงานภาษีขาย" OnClick="RptPOS140_Click"></asp:LinkButton>&nbsp
                                <asp:LinkButton ID="RptPOS138" CssClass="btn btn-secondary " runat="server" Text="รายงานยกเลิกบิล" OnClick="RptPOS138_Click"></asp:LinkButton>&nbsp
                                <asp:LinkButton ID="RptPOS134" CssClass="btn btn-secondary " runat="server" Text="รายงานสินค้ารายชิ้น" OnClick="RptPOS134_Click"></asp:LinkButton>&nbsp
                                <asp:LinkButton ID="btnRptPOS133" CssClass="btn btn-secondary " runat="server" Text="สรุปยอดขาย" OnClick="RptPOS133_Click"></asp:LinkButton>

            </div>
        </div>
    </div>

    <div class="row pt-2">
        <div class="col-12 ">
            <div class="card">
                <div class="card-body">

                    <dx:ASPxCallbackPanel ID="CallbackPanel"
                        ClientInstanceName="CallbackPanel"
                        runat="server"
                        OnCallback="CallbackPanel_Callback">
                        <SettingsLoadingPanel Enabled="false" />
                        <ClientSideEvents BeginCallback="OnBeginCallback"
                            EndCallback="OnEndCallback" />
                        <PanelCollection>
                            <dx:PanelContent>
                                <div style="overflow-x: auto; width: 100%">
                                    <dx:ASPxGridView ID="grdDetail"
                                        runat="server"
                                        EnableTheming="True"
                                        CssClass="Sarabun"
                                        Theme="Material"
                                        OnDataBinding="grdDetail_DataBinding"
                                        AutoGenerateColumns="False"
                                        KeyboardSupport="true">

                                        <Settings ShowFilterRow="True" ShowFooter="True" GroupFormat="{1} {2}" ShowGroupedColumns="True" ShowGroupFooter="VisibleAlways" ShowGroupPanel="True" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                        <SettingsBehavior AutoExpandAllGroups="True" />
                                        <Columns>

                                            <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID" GroupIndex="0">
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="บิล" FieldName="INVID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>

                                            <dx:GridViewDataDateColumn Caption="วันที่บิล" FieldName="BillDate">
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                            </dx:GridViewDataDateColumn>

                                            <dx:GridViewDataTextColumn Caption="จำนวนรายการ" FieldName="Qty">
                                                <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวนเงิน" FieldName="NetTotalAmt">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ภาษี" FieldName="NetTotalVatAmt">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวนเงิน(Inc.VAT)" FieldName="NetTotalAmtIncVat">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ส่วนลด(Exc.VAT)" FieldName="LineDisc" Width="180px">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวนเงินสด" FieldName="PayByCash">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวนเงินโอน" FieldName="PayByOther">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวนเครดิต" FieldName="PayByCredit">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="จำนวน Voucher" FieldName="PayByVoucher">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ใบกำกับภาษี" FieldName="FINVID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                             <dx:GridViewDataTextColumn Caption="Member Code" FieldName="CustomerID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ชื่อผู้ซื้อสินค้า" FieldName="CustomerName">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="เลขประจำตัวผู้เสียภาษี" FieldName="CustTaxID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="สำนักงาน/สาขา" FieldName="CustBranchName">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle Wrap="False" />
                                                <CellStyle Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                        </Columns>

                                        <TotalSummary>
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmt" ShowInColumn="NetTotalAmt" ShowInGroupFooterColumn="NetTotalAmt" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalVatAmt" ShowInColumn="NetTotalVatAmt" ShowInGroupFooterColumn="NetTotalVatAmt" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmtIncVat" ShowInColumn="NetTotalAmtIncVat" ShowInGroupFooterColumn="NetTotalAmtIncVat" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmtIncVat" ShowInColumn="LineDisc" ShowInGroupFooterColumn="LineDisc" SummaryType="Sum" />
          <dx:ASPxSummaryItem FieldName="PayByCash" ShowInColumn="PayByCash" ShowInGroupFooterColumn="PayByCash" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="PayByOther" ShowInColumn="PayByOther" ShowInGroupFooterColumn="PayByOther" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="PayByCredit" ShowInColumn="PayByCredit" ShowInGroupFooterColumn="PayByCredit" SummaryType="Sum" />
                                        </TotalSummary>
                                        <GroupSummary>
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmt" ShowInColumn="Name1" ShowInGroupFooterColumn="NetTotalAmt" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalVatAmt" ShowInColumn="Name1" ShowInGroupFooterColumn="NetTotalVatAmt" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmtIncVat" ShowInColumn="Name1" ShowInGroupFooterColumn="NetTotalAmtIncVat" SummaryType="Sum" />
                                            <dx:ASPxSummaryItem FieldName="NetTotalAmtIncVat" ShowInColumn="LineDisc" ShowInGroupFooterColumn="LineDisc" SummaryType="Sum" />
                                        </GroupSummary>
                                    </dx:ASPxGridView>
                                    <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                                </div>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
