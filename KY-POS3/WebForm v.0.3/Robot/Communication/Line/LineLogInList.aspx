﻿<%@ Page Title="Line Login Request" Language="C#" EnableEventValidation="false"  MasterPageFile="~/Communication/SiteA.Master"  AutoEventWireup="true" CodeBehind="LineLogInList.aspx.cs" Inherits="Robot.Communication.Line.LineLogInList" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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

          function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }

     
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
            <div class="card-header">
                <div class="row">
                    <div class="col-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                        </asp:LinkButton>
                    </div>
                    <div class="col-4  text-center">
                        <asp:Literal ID="lblTopic" runat="server"></asp:Literal>
                    </div>
                    <div class="col-4 text-right">
                        <div class="btn-group" role="group" aria-label="Basic example">
                           
                            <asp:LinkButton ID="btnExcel" ForeColor="#6699ff" CssClass="btn btn-default" runat="server"
                                OnClick="btnExcel_Click">
                                                <span  >Excel</span> 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body pt-2 pb-2">
                            <div class="row ">
                          
                                <div class="col-md-2" hidden="hidden">
                                    <span>สถานะ</span>
                                    <asp:DropDownList runat="server" ID="cboStatus" CssClass="form-control " DropDownStyle="DropDownList">
                                        <asp:ListItem Text="PENDING" Value="PENDING"></asp:ListItem>
                                            <asp:ListItem Text="ACCEPTED" Value="ACCEPTED"></asp:ListItem>
                                        <asp:ListItem Text="REJECTED" Value="REJECTED"></asp:ListItem> 
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-5 text-right pt-4">
                                    <div class="input-group mb-12">
                                        <asp:TextBox ID="txtSearch2" CssClass="form-control"
                                            ForeColor="Black"
                                            Placeholder="ค้นหา" runat="server"></asp:TextBox>
                                    </div>
                                  
                                </div>
                                <div class="col-sm-12 col-md-1 pt-4">
                                    <dx:ASPxButton ID="btnSearch" Height="39" Width="100%" runat="server" Text="Load"
                                        AutoPostBack="false" Theme="Material">
                                        <ClientSideEvents Click="SendCommentCallback" />
                                    </dx:ASPxButton>
                                </div>
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
                                KeyFieldName="ID"
                                OnDataBinding="grdDetail_DataBinding"
                                OnRowCommand="grdDetail_RowCommand"
                                KeyboardSupport="True"
                                Width="100%">
                                <SettingsPager PageSize="80">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                </SettingsPager>
                                <SettingsPager Mode="ShowPager" PageSize="80"></SettingsPager>
                                <SettingsResizing ColumnResizeMode="Control" />
                                <Settings ShowTitlePanel="true" ShowFilterRow="true" 
                                    ShowFilterBar="Auto"
                                    HorizontalScrollBarMode="Auto"
                                    VerticalScrollableHeight="400"
                                    VerticalScrollBarMode="Auto" />
                                <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" 
                                    ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                <Columns>
                                    <dx:GridViewDataTextColumn Width="60">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                CommandName="sel" CommandArgument='<%# Eval("ID") %>'>
                                                            <i class="fa fa-folder-open"></i> 
                                            </asp:LinkButton>
                                        </DataItemTemplate>
                                        <HeaderStyle ></HeaderStyle>
                                    </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Apps" FieldName="AppID" Width="120">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                               <dx:GridViewDataTextColumn Caption="Emp code" FieldName="UserID" Width="100">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ชื่อ-นามสกุล" FieldName="UserFullName" Width="180">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                             <%--<dx:GridViewDataTextColumn Caption="ประเภท" FieldName="EmpType" Width="120">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>--%>
                                          <%--<dx:GridViewDataTextColumn Caption="Cost No." FieldName="UserCompanyID" Width="150">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>--%>
<%--      <dx:GridViewDataTextColumn Caption="Cost Name" FieldName="UserCompanyName" Width="200">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>--%>
                                         <dx:GridViewDataTextColumn Caption="บันทึกคำขอ" FieldName="ReqMemo" Width="200">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                  <dx:GridViewDataTextColumn Caption="บันทึกการอนุมัติ" FieldName="ApprovedMemo" Width="200">
                                        <Settings AutoFilterCondition="Contains" />
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                               
 
                                    <dx:GridViewDataDateColumn Caption="วันที่ขอ" FieldName="RequestDate" Width="120">
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                    </dx:GridViewDataDateColumn>
                                    <dx:GridViewDataDateColumn Caption="วันที่อนุมัติ" FieldName="ApprovedDate" Width="120">
                                        <HeaderStyle  Wrap="False" />
                                        <CellStyle  Wrap="False" />
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
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