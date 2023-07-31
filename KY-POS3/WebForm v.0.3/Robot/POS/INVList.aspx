﻿<%@ Page Title="Invoice" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="INVList.aspx.cs" Inherits="Robot.POS.INVList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddrole" runat="server" />



    <asp:HiddenField ID="HiddenField2" runat="server" />


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
        <div class="col-12">
            <div class="card">
                <div class="card-header">
                <div class="row">
                    <div class="col-md-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4 text-center">
                        <asp:Label runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="Black" ID="lblHeaderCaption"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <div class="btn-group" role="group" aria-label="Basic example">
                            <asp:LinkButton ID="btnNew" CssClass="btn btn btn-warning" runat="server" OnClick="btnNew_Click">   
                                 <i class="fa fa-plus"></i>&nbsp<span >NEW</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnExcel" CssClass="btn btn-secondary" runat="server"
                                OnClick="btnExcel_Click">
                                                <span  style="color:white">Excel</span> 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
                <div class="card-body pt-1 pb-1">
                    <div class="row " style="font-size: smaller">
                        <div class="col-md-11">
                            <asp:UpdatePanel ID="uptfilterby" runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span>กรองโดย</span>
                                                    <asp:DropDownList runat="server" ID="cbofilterby" CssClass="form-control form-control-sm" AutoPostBack="true" DropDownStyle="DropDownList" OnSelectedIndexChanged="cbofilterby_SelectedIndexChanged">
                                                        <asp:ListItem Text="Invoice Date" Value="invdate"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4" id="divdateFilter" runat="server">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <span>วันที่เริ่ม</span>
                                                    <dx:ASPxDateEdit ID="dtBegin"
                                                        CssClass="Sarabun"
                                                        DisplayFormatString="dd-MM-yyyy" 
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material"   Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                                <div class="col-md-6">
                                                    <span>ถึงวันที่</span>
                                                    <dx:ASPxDateEdit ID="dtEnd" 
                                                        CssClass="Sarabun"
                                                        DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy" runat="server"
                                                        Theme="Material" Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2" id="divsearch" runat="server">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span>Search </span>
                                                    <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="Material"></dx:ASPxTextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1 text-left pt-3">
                                            <dx:ASPxButton ID="btnSearch"  runat="server" Text="Load" BackColor="#009933" AutoPostBack="false" Theme="Material">
                                                <ClientSideEvents Click="SendCommentCallback" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div class="col-md-2" hidden="hidden">
                                            <br />
                                            <div class="btn-group" role="group" aria-label="Basic example">
                                                <asp:CheckBox ID="chkShowClose" Text=""  runat="server" AutoPostBack="true" OnCheckedChanged="chkShowClose_CheckedChanged" />Show completed                                                         
                                            </div>
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
        <div class="col-md-12">
            <div class="card">
         
                    <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                        runat="server"
                        OnCallback="CallbackPanel_Callback">
                        <SettingsLoadingPanel Enabled="false" />
                        <ClientSideEvents BeginCallback="OnBeginCallback" EndCallback="OnEndCallback" />
                        <PanelCollection>
                            <dx:PanelContent>
                                <%--<div  style="overflow-x: auto; width: 100%">--%>
                                <dx:ASPxGridView ID="grdDetail" runat="server"
                                    EnableTheming="True"
                                    Theme="SoftOrange"
                                    CssClass="Sarabun"
                                    AutoGenerateColumns="False"
                                    KeyFieldName="ID" 
                                    OnDataBinding="grdDetail_DataBinding"
                                    OnRowCommand="grdDetail_RowCommand"
                                    KeyboardSupport="True" Width="100%">
                                    <SettingsPager PageSize="40">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                    </SettingsPager>
                                    <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                    <SettingsPager PageSize="80">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                    </SettingsPager>
                                    <SettingsResizing ColumnResizeMode="Control" />
                                    <SettingsPager Mode="ShowPager" PageSize="80"></SettingsPager>

                                    <Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto"
                                        HorizontalScrollBarMode="Auto"
                                        VerticalScrollableHeight="400"
                                        VerticalScrollBarMode="Auto" />
                                    <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn Width="60">
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                    CommandName="Select" CommandArgument='<%# Eval("SOINVID") %>'>
                                                             <i class="fa fa-folder-open"></i> 
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                            <HeaderStyle ></HeaderStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Invoice No." FieldName="SOINVID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Ref To" FieldName="RefDocID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Company" FieldName="CompanyID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Document type" FieldName="DocTypeID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" Width="250">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Customer code" FieldName="CustomerID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Total amt." FieldName="NetTotalAmt">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Vat amt." FieldName="NetTotalVatAmt">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Total amt.(inc vat)" FieldName="NetTotalAmtIncVat">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="RC No." FieldName="RCID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="RC date" FieldName="RCDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>

                                        <dx:GridViewDataTextColumn Caption="Billing No." FieldName="BillingID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>                                                               

                                        <dx:GridViewDataTextColumn Caption="RC Last PayNo" FieldName="RCLastPayNo">
                                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                 
                                        <dx:GridViewDataTextColumn Caption="Pending amt" FieldName="INVPendingAmt">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Receipt amt" FieldName="RCAmt">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>                                   
                                        <dx:GridViewDataDateColumn Caption="Appointment date" FieldName="AppointmentDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>                                                                               

                                        <dx:GridViewDataTextColumn Caption="Term" FieldName="TermID">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                 
                                        <dx:GridViewDataDateColumn Caption="Payment due" FieldName="PayDueDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                            
                                        <dx:GridViewDataDateColumn Caption="Print Date" FieldName="PrintDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataCheckColumn Caption="Printed" FieldName="IsPrint" Visible="false">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" Font-Size="Small" />
                                            <CellStyle  Wrap="False" Font-Size="Smaller" />
                                        </dx:GridViewDataCheckColumn>
                                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Link Date" FieldName="LinkDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn Caption="Created by" FieldName="CreatedBy">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="Created date" FieldName="CreatedDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle  Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                    </Columns>
                                    <TotalSummary>
                                        <dx:ASPxSummaryItem FieldName="SOINVID" ShowInColumn="SOINVID" ShowInGroupFooterColumn="SOINVID" SummaryType="Count" DisplayFormat="{0:n0}" />                                                                                                                      
                                        <dx:ASPxSummaryItem FieldName="RCAmt" ShowInColumn="RCAmt" ShowInGroupFooterColumn="RCAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                        <dx:ASPxSummaryItem FieldName="INVPendingAmt" ShowInColumn="INVPendingAmt" ShowInGroupFooterColumn="INVPendingAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                        <dx:ASPxSummaryItem FieldName="NetTotalAmtIncVat" ShowInColumn="NetTotalAmtIncVat" ShowInGroupFooterColumn="NetTotalAmtIncVat" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                        <dx:ASPxSummaryItem FieldName="NetTotalVatAmt" ShowInColumn="NetTotalVatAmt" ShowInGroupFooterColumn="NetTotalVatAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                        <dx:ASPxSummaryItem FieldName="NetTotalAmt" ShowInColumn="NetTotalAmt" ShowInGroupFooterColumn="NetTotalAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                    </TotalSummary>
                                </dx:ASPxGridView>
                                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                                <%--  </div>--%>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxCallbackPanel>
          
            </div>
        </div>
    </div>
    <div class="row text-center">
        <div class="col-md-12">
            <span style="color: dimgray; font-size: 10px">O-INVOICE</span>
        </div>
    </div>
</asp:Content>