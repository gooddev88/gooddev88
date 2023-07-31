<%@ Page Title="CustomerList" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POSA1.Master" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="Robot.CRM.CustomerList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddcompany" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
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

    <asp:UpdatePanel ID="udpPost" runat="server">
        <ContentTemplate>
            <div class="row pb-1">
                <div class="col-md-12">
                    <div class="row grid-margin">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-header pt-1 pb-1">
                                    <div class="row kanit">
                                        <div class="col-md-8">
                                            <ul class="nav nav-pills card-header-pills" role="tablist">
                                                <li class="nav-item">
                                                     <asp:LinkButton ID="btnBackList" Font-Size="Small" CssClass="nav-link active" runat="server" OnClick="btnBackList_Click" meta:resourcekey="btnBackListResource1"> 
                                                        <i class="fa fa-chevron-circle-left"></i>&nbsp<span class="kanit">Back</span> 
                                                    </asp:LinkButton>
                                                </li>
                                                <li class="nav-item" runat="server">
                                                    <a class="nav-link " id="a_tab_home"><span class="kanit"><i class="fa fa-filter"></i>&nbsp<%=hddTopic.Value %></span></a>
                                                </li>                             
                                            </ul>
                                        </div>
                                        <div class="col-md-4 text-right pb-3">
                                            <div class="btn-group" role="group" aria-label="Basic example">
                                                <asp:LinkButton ID="btnExcel" CssClass="btn btn btn-secondary" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>
                                                <asp:LinkButton ID="btnNew" CssClass="btn btn btn-warning" runat="server" OnClick="btnNew_Click">   
                                                    <i class="fa fa-plus"></i>&nbsp<span class="kanit">NEW</span> 
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body pt-1 pb-1">
                                    <div class="row kanit">
                                        <div class="col-md-11">
                                            <asp:UpdatePanel ID="uptfilterby" runat="server">
                                                <ContentTemplate>
                                                    <div class="row pb-0">

                                                        <div class="col-md-4" style="display:none;" id="divdateFilter" runat="server">
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <span>Date fr.</span>
                                                                    <dx:ASPxDateEdit ID="dtBegin" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" runat="server" Theme="SoftOrange" CssClass="form-control form-control-sm" Width="100%">
                                                                    </dx:ASPxDateEdit>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>Date to</span>
                                                                    <dx:ASPxDateEdit ID="dtEnd" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" runat="server" Theme="SoftOrange" CssClass="form-control  form-control-sm" Width="100%">
                                                                    </dx:ASPxDateEdit>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--<div class="col-md-2" style="display:none;">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <span>สาขา &nbsp</span>
                                                                    <dx:ASPxComboBox ID="cboCompany" runat="server" DropDownStyle="DropDown" Theme="Mulberry"
                                                                        ValueField="CompanyID" ValueType="System.String" ViewStateMode="Enabled" TextFormatString="{0}" Width="100%">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="CompanyID" Caption="รหัส" />
                                                                            <dx:ListBoxColumn FieldName="Name" Width="300px" Caption="ชื่อสาขา" />
                                                                        </Columns>
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                        </div>--%>
                                                        <div class="col-md-2 pl-2" id="divsearch" runat="server">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <span>Search &nbsp</span>
                                                                    <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" Theme="SoftOrange"></dx:ASPxTextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <br />
                                                            <asp:LinkButton ID="btnSearch" CssClass="btn btn btn-secondary btn-sm" runat="server" Text="Load" OnClick="btnSearch_Click"></asp:LinkButton>
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
                    <div class="kanit" style="overflow-x: auto; width: 100%">
                        <dx:ASPxGridView ID="grdDetail" runat="server" Width="100%"
                            EnableTheming="True" Theme="SoftOrange"
                            AutoGenerateColumns="False"
                            KeyFieldName="CustomerID"
                            OnDataBinding="grdDetail_DataBinding"
                            OnRowCommand="grdDetail_RowCommand" KeyboardSupport="True">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="" Caption="Select" Width="80px">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                            CommandName="Select" CommandArgument='<%# Eval("CustomerID") %>'>
                                             <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>
                                    <HeaderStyle CssClass="kanit"></HeaderStyle>
                                </dx:GridViewDataTextColumn>                               
                                 <dx:GridViewDataTextColumn Caption="Customer code" FieldName="CustomerID"  >
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle CssClass="kanit" Wrap="False" />
                                    <CellStyle CssClass="kanit" Wrap="False" />
                                </dx:GridViewDataTextColumn>
              
                                <dx:GridViewDataTextColumn Caption="Name" FieldName="NameDisplay" Width="100%">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle CssClass="kanit" Wrap="False" />
                                    <CellStyle CssClass="kanit" Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="Birthday" FieldName="Birthdate">
                                    <HeaderStyle CssClass="kanit" Wrap="False" />
                                    <CellStyle CssClass="kanit" Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn> 
                                <dx:GridViewDataTextColumn Caption="Mobile" FieldName="Mobile">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle CssClass="kanit" Wrap="False" />
                                    <CellStyle CssClass="kanit" Wrap="False" />
                                </dx:GridViewDataTextColumn>    
                                    <dx:GridViewDataTextColumn FieldName="CrmPoint" Caption="Point">
                                                    <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                    <CellStyle CssClass="kanit" Wrap="False"></CellStyle>
                                                    <HeaderStyle CssClass="kanit" />
                                                    <GroupFooterCellStyle BackColor="#6699FF">
                                                    </GroupFooterCellStyle>
                                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
