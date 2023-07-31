<%@ Page Title="User Group" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="MyUserGroupList.aspx.cs" Inherits="Robot.Master.MyUserGroupList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddrole" runat="server" />
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

    <div class="row pb-3">
        <div class="col-md-12">

            <div class="card pb-3">
                     <div class="card-header bg-dark   pt-1 pb-1">
                    <div class="row ">
       <div class="col-md-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:white"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:white"> Back</span>                                            
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4 text-center">
                            <span style="font-size: x-large; color: white">
                                   <i class="fas fa-users-cog fa-2x"></i>
                                <strong> 
                                <asp:Literal ID="lblTopic" runat="server"></asp:Literal>
                                    </strong>
                            </span>
                        </div>
                        <div class="col-md-4 text-right pb-2">
                            <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnNew"    CssClass="btn btn-success " runat="server" OnClick="btnNew_Click">   
                                      <span >++NEW++</span> 
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExcel" CssClass="btn btn-warning" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>
                            </div> 
                        </div>
                    </div>
                </div>

                <div class="card-body bg-light pt-1 pb-1"> 
                            <div class="row "> 
                                <div class="col-md-4">
                                    <span style="font-size: small">Search </span>
                                    <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" 
                                        Theme="Material">
                                    </dx:ASPxTextBox>
                                </div>

                                <div class="col-md-4 pt-4">
                                    <dx:ASPxButton ID="btnSearch"  runat="server" Text="Load" BackColor="#009933" AutoPostBack="false" Theme="Material">
                                        <ClientSideEvents Click="SendCommentCallback" />
                                    </dx:ASPxButton>

                                </div>
                                <div class="col-md-4 pt-4 text-right">
                                    <asp:CheckBox ID="chkShowDisable"  Text="Show disable group" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="chkShowDisable_CheckedChanged" />
                                </div>  
                            </div> 
                            <div class="row">
                                <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                            </div> 
                </div> 
            </div>
          
        </div>
        </div>
        <div class="row">
            <div class="col-md-12">


                <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                    runat="server"
                    OnCallback="CallbackPanel_Callback">
                    <SettingsLoadingPanel Enabled="false" />
                    <ClientSideEvents BeginCallback="OnBeginCallback" EndCallback="OnEndCallback" />
                    <PanelCollection>
                        <dx:PanelContent>
                     
                                <dx:ASPxGridView ID="grdDetail" runat="server"
                                    EnableTheming="True" Theme="SoftOrange"
                                    AutoGenerateColumns="False"
                                    KeyFieldName="UserGroupID"
                                    Width="100%"
                                    OnDataBinding="grdDetail_DataBinding"
                                    OnRowCommand="grdDetail_RowCommand">
                                    <SettingsBehavior AllowFocusedRow="true" />
                                    <Settings ShowFilterRow="True" ShowFooter="True" ShowHeaderFilterButton="True" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn  Width="60"  Caption="OPEN"  >
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                    CommandName="Select" CommandArgument='<%# Eval("UserGroupID") %>'>
                                                        <i class="fa fa-folder-open"></i> 
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                            <HeaderStyle ></HeaderStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Group Code" FieldName="UserGroupID" Width="160">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle  Wrap="False" />
                                            <CellStyle  Wrap="False" Font-Size="Smaller" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Name" FieldName="GroupName"  >
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle  Wrap="False" />
                                            <CellStyle  Wrap="False" Font-Size="Smaller" />
                                        </dx:GridViewDataTextColumn>
                                               <dx:GridViewDataTextColumn Caption="Sort" FieldName="Sort" Width="60">
                                            <HeaderStyle  Wrap="False" />
                                            <CellStyle  Wrap="False" Font-Size="Smaller" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataCheckColumn Caption="Active" FieldName="IsActive" Width="60">
                                            <HeaderStyle  Wrap="False" />
                                            <CellStyle  Wrap="False" Font-Size="Smaller" />
                                        </dx:GridViewDataCheckColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                     
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>

            </div>
        </div>
</asp:Content>
