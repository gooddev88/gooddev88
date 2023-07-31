﻿<%@ Page Title="ประวัติการยกเลิกออเดอร์" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSCancelBillList.aspx.cs" Inherits="Robot.POSC.POSCancelBillList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddcompany" runat="server" />


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
    </script>

    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="LoadingPanel"
        Theme="Material"
        runat="server"
        Modal="true"
        HorizontalAlign="Center"
        VerticalAlign="Middle">
    </dx:ASPxLoadingPanel>



    <div class="row pt-2">

        <div class="col-12">
            <div class="card">
                <div class="card-header pt-1 pb-1">
                    <div class="row ">
                          <div class="col-md-4">
                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4"> <span>ประวัติการยกเลิกออเดอร์</span></div>
                        <div class="col-md-4 text-right pb-3">
                            <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnExcel" CssClass="btn btn btn-secondary" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body pt-1 pb-1">
                    <div class="row ">
                        <div class="col-md-11">
                            <asp:UpdatePanel ID="uptfilterby" runat="server">
                                <ContentTemplate>
                                    <div class="row">

                                        <div class="col-md-4" id="divdateFilter" runat="server">
                                            <div class="row">

                                                <div class="col-md-6">
                                                    <span>วันที่(จาก)</span>
                                                    <dx:ASPxDateEdit ID="dtBegin"
                                                        DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material" Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-6">
                                                    <span>วันที่(ถึง)</span>
                                                    <dx:ASPxDateEdit ID="dtEnd" DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material" Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="row">
                                                <div class="col-12">
                                                    <span>สาขา </span>
                                                    <dx:ASPxComboBox ID="cboCompany" runat="server"
                                                        DropDownStyle="DropDown" Theme="Material"
                                                        ValueField="CompanyID" ValueType="System.String" ViewStateMode="Enabled" TextFormatString="{0} {1}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="CompanyID" Caption="รหัส" />
                                                            <dx:ListBoxColumn FieldName="Name" Width="300px" Caption="ชื่อสาขา" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="divsearch" runat="server">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span>ค้นหา </span>
                                                    <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="Material"></dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2 pt-4">
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



    <div class="row">
        <div class="col-12">
            <div style="overflow-x: auto; width: 100%">
                <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                    runat="server"
                    OnCallback="CallbackPanel_Callback">
                    <SettingsLoadingPanel Enabled="false" />
                    <ClientSideEvents
                        BeginCallback="OnBeginCallback"
                        EndCallback="OnEndCallback" />
                    <PanelCollection>
                        <dx:PanelContent>
                            <dx:ASPxGridView ID="grdDetail" runat="server"
                                EnableTheming="True"
                                Theme="MaterialCompact"
                                AutoGenerateColumns="False"
                                KeyFieldName="BillID"
                                OnDataBinding="grdDetail_DataBinding"
                                OnRowCommand="grdDetail_RowCommand" KeyboardSupport="True">

                                <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />

                                <Columns>
                                    <dx:GridViewDataTextColumn FieldName="" Caption="เลือก" Width="80px">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                CommandName="sel" CommandArgument='<%# Eval("BillID") %>'>
                                                             <i class="fa fa-folder-open"></i> 
                                            </asp:LinkButton>
                                        </DataItemTemplate>
                                        <HeaderStyle></HeaderStyle>
                                    </dx:GridViewDataTextColumn>


                                    <dx:GridViewDataTextColumn Caption="เลขออเดอร์" FieldName="BillID" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="เลขบิล" FieldName="INVID" Width="160px">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="เครื่อง" FieldName="MacNo">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ขายให้" FieldName="ShipToLocID">
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
                                    <dx:GridViewDataTextColumn Caption="เบอร์โต๊ะ" FieldName="TableName">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Member Code" FieldName="CustomerID">
                                        <Settings AutoFilterCondition="Contains" />
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
                                    <dx:GridViewDataTextColumn Caption="Disc Amt" FieldName="ItemDiscAmt">
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
                                    
                                    <dx:GridViewDataTextColumn Caption="Created by" FieldName="CreatedBy">
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn Caption="Created date" FieldName="CreatedDate">
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataTextColumn Caption="Modified by" FieldName="ModifiedBy">
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataDateColumn Caption="ModifiedBy date" FieldName="ModifiedDate">
                                        <HeaderStyle Wrap="False" />
                                        <CellStyle Wrap="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                </Columns>
                            </dx:ASPxGridView>
                            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </div>





        </div>
    </div>


</asp:Content>