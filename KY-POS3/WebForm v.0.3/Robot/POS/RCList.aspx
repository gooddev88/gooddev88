<%@ Page Title="Receipt" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="RCList.aspx.cs" Inherits="Robot.POS.RCList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
   

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
        <div class="col-md-12">
            <div class="row grid-margin">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header pt-1 pb-1">
                            <div class="row ">
                                <div class="col-md-8">
                                    <ul class="nav nav-pills card-header-pills" role="tablist">
                                        <li class="nav-item">
                                           <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black">  <%=hddTopic.Value %></span>                                            
                        </asp:LinkButton>
                                        </li>
                                   
                                    </ul>
                                </div>
                                <div class="col-md-4 text-right">
                                    <div class="btn-group" role="group" aria-label="Basic example">
                             <asp:LinkButton ID="btnNew"   class="btn btn-success" runat="server" OnClick="btnNew_Click">   
                                              <span >***NEW***</span> 
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnExcel" class="btn btn-dark" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>
                                </div>
                                    </div>

                            </div>
                        </div>
                        <div class="card-body pt-1 pb-1">
                            <div class="row ">
                                <div class="col-md-11">
                                    <asp:UpdatePanel ID="uptfilterby" runat="server">
                                        <ContentTemplate>
                                            <div class="row" style="font-size: smaller">

                                                <div class="col-md-2">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <span>Filter by</span>
                                                            <asp:DropDownList runat="server" ID="cbofilterby"
                                                                CssClass="form-control form-control-sm" AutoPostBack="true"
                                                                DropDownStyle="DropDownList" OnSelectedIndexChanged="cbofilterby_SelectedIndexChanged">
                                                                <asp:ListItem Text="RC Date" Value="DOCDATE"></asp:ListItem>
                                                              
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>


                                                <div class="col-md-4" id="divdateFilter" runat="server">
                                                    <div class="row">
                                                        <div class="col-md-6">
                                                            <span>Begin</span>
                                                            <dx:ASPxDateEdit ID="dtBegin" DisplayFormatString="dd-MM-yyyy"
                                                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" CssClass="Sarabun" Width="100%">
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>End</span>
                                                            <dx:ASPxDateEdit ID="dtEnd" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                                                runat="server" Theme="Material" CssClass="Sarabun" Width="100%">
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" id="divsearch" runat="server">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <span>Search &nbsp</span>
                                                            <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="Material"></dx:ASPxTextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2" hidden="hidden">

                                                    <br />
                                                    <div class="btn-group" role="group" aria-label="Basic example">
                                                        <asp:CheckBox ID="chkShowClose" Text=""  runat="server"
                                                            AutoPostBack="true" 
                                                            OnCheckedChanged="chkShowClose_CheckedChanged" />Show Confirm
                                                       
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <br />
                                                    <dx:ASPxButton ID="btnSearch"  runat="server" Text="Load" BackColor="#009933" AutoPostBack="false" Theme="Material">
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
        </div>
    </div>


    <div class="row">
        <div class="col-lg-12 grid-margin stretch-card">
            <div class="card">
                <div class="">
                    <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                        runat="server"
                        OnCallback="CallbackPanel_Callback">
                        <SettingsLoadingPanel Enabled="false" />
                        <ClientSideEvents BeginCallback="OnBeginCallback" EndCallback="OnEndCallback" />
                        <PanelCollection>
                            <dx:PanelContent>
                                <div  style="overflow-x: auto; width: 100%">
                                    <dx:ASPxGridView ID="grdDetail" runat="server"
                                        EnableTheming="True"
                                            Theme="Material"
                                        AutoGenerateColumns="False"
                                        KeyFieldName="ID"
                                        CssClass="Sarabun"
                                        OnDataBinding="grdDetail_DataBinding"
                                        OnRowCommand="grdDetail_RowCommand" KeyboardSupport="True" Width="100%">
                                        <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="">
                                                <DataItemTemplate>
                                                    <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                        CommandName="sel_row" CommandArgument='<%# Eval("ID") %>'>
                                                             <i class="fa fa-folder-open"></i> 
                                                    </asp:LinkButton>
                                                </DataItemTemplate>
                                                <HeaderStyle ></HeaderStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="RC No." FieldName="RCID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataDateColumn Caption="วันที่เอกสาร" FieldName="RCDate">
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                            </dx:GridViewDataDateColumn>
                                              <dx:GridViewDataTextColumn Caption="Status" FieldName="RCStatus">
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Company" FieldName="CompanyID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="รหัสลูกค้า" FieldName="CustomerID">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ลูกค้า" FieldName="CustomerName" Width="100%">
                                                <Settings AutoFilterCondition="Contains" />
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
 
                                        
                                              <dx:GridViewDataTextColumn Caption="ชำระ" FieldName="PayTotalAmt">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                                  <dx:GridViewDataTextColumn Caption="ตัดยอดอินวอยซ์" FieldName="PayINVTotalAmt">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="ส่วนต่าง" FieldName="PayTotalDiffINVAmt">
                                                <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                <CellStyle  Wrap="False" />
                                            </dx:GridViewDataTextColumn>
                                          
                                                     <dx:GridViewDataDateColumn Caption="Completed Date" FieldName="CompletedDate">
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
