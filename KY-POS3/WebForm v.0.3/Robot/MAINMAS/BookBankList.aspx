﻿<%@ Page Title="BookBank Info" Language="C#" EnableEventValidation="false"  MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" CodeBehind="BookBankList.aspx.cs" Inherits="Robot.OMASTER.BookBankList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddrole" runat="server" />

    <asp:HiddenField ID="hddTopic" runat="server" />


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
    </script>

    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="LoadingPanel"
        Theme="Material"
        runat="server"
        Modal="true"
        HorizontalAlign="Center"
        VerticalAlign="Middle">
    </dx:ASPxLoadingPanel>

    <div class="row pb-1">
        <div class="col-md-12">

            <div class="card-header bg-success">
                <div class="row">
                    <div class="col-md-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:white"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span class="" style="font-size:medium;color:white"> กลับ</span>                                            
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4 text-center">
                        <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="White" ID="lblinfohead"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                    <div class="btn-group" role="group" aria-label="Basic example">
                            <asp:LinkButton ID="btnNew"  CssClass="btn btn-warning" runat="server"
                                OnClick="btnNew_Click">   
                                              <span   >***NEW***</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnExcel" CssClass="btn btn-dark" runat="server"
                                OnClick="btnExcel_Click">
                                                <span class="" >Excel</span> 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body pt-1 pb-1">
                            <div class="row ">
                                
                                <div class="col-md-4 text-right pl-0 pr-0"> 
                                        <asp:TextBox ID="txtSearch2" CssClass="form-control"
                                            Placeholder="ค้นหา" runat="server"></asp:TextBox> 
                                </div>
     <div class="col-1">
                       <dx:ASPxButton ID="btnSearch" Height="39" Width="100%" runat="server" Text="ค้นหา"
                                        AutoPostBack="false" Theme="Material">
                                        <ClientSideEvents Click="SendCommentCallback" />
                                    </dx:ASPxButton>
         </div>
                     <div class="col-3 pt-2  ">
                            <asp:CheckBox ID="chkShowClose" Text="แสดงที่ยกเลิก" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowClose_CheckedChanged" />
                        </div>
                                </div>
                  

                            
                            
                                    <div class="row">
                                        <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                                    </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="">
                    <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                        runat="server"
                        OnCallback="CallbackPanel_Callback">
                        <SettingsLoadingPanel Enabled="false" />
                        <ClientSideEvents BeginCallback="OnBeginCallback" EndCallback="OnEndCallback" />
                        <PanelCollection>
                            <dx:PanelContent>
                                <dx:ASPxGridView ID="grdDetail" runat="server"
                                    EnableTheming="True" 
                                    Theme="MaterialCompact"
                                    AutoGenerateColumns="False"
                                    KeyFieldName="BookID"
                                    CssClass="Sarabun"
                                    OnDataBinding="grdDetail_DataBinding"
                                    OnRowCommand="grdDetail_RowCommand"
                                    KeyboardSupport="True" Width="100%">
                                    <SettingsPager PageSize="80">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                    </SettingsPager>
                                    <SettingsPager Mode="ShowPager" PageSize="80"></SettingsPager>
                                    <SettingsResizing ColumnResizeMode="Control" />
                                    <Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto"
                                        HorizontalScrollBarMode="Auto"
                                        VerticalScrollableHeight="400"
                                        VerticalScrollBarMode="Auto" />
                                    <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn Width="60">
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                    CommandName="sel" CommandArgument='<%# Eval("BookID") %>'>
                                                             <i class="fa fa-folder-open"></i> 
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                            <HeaderStyle CssClass=""></HeaderStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="เลขบัญชี" FieldName="BookID" Width="200">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="รายละเอียดบัญชี" FieldName="BookDesc" Width="400">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ชื่อย่อธนาคาร" FieldName="BankCode" Width="120">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ชื่อธนาคาร" FieldName="BankName" Width="300">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="สาขา" FieldName="BranchName" Width="150">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ลำดับ" FieldName="Sort">
                                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                               <dx:GridViewDataCheckColumn FieldName="IsActive" Caption="ใช้งาน" Width="80">
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                        </dx:GridViewDataCheckColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>
    <div class="row text-center">
        <div class="col-md-12">
            <span style="color: dimgray; font-size: 10px">BookBank-Info</span>
        </div>
    </div>
</asp:Content>