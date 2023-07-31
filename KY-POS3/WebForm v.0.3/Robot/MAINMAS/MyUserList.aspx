<%@ Page Title="MyUser" Language="C#" EnableEventValidation="false" MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" CodeBehind="MyUserList.aspx.cs" Inherits="Robot.MAINMAS.MyUserList" %>

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
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span class="Sarabun" style="font-size:medium;color:black"> Back</span>                                            
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-4 text-center">
                            <span style="font-size: x-large; color: black">
                                  <i class="fas fa-user-secret fa-2x"></i>
                                <strong> 
                                <asp:Literal ID="lblTopic" runat="server"></asp:Literal>
                                    </strong>
                            </span>
                        </div>
                        <div class="col-md-4 text-right pb-2">
                            <div class="btn-group" role="group" aria-label="Basic example">
                                <asp:LinkButton ID="btnNew" class="btn btn-info" runat="server" OnClick="btnNew_Click">   
                                      <span class="Sarabun">***NEW***</span> 
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnExcel" class="btn btn-warning" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card-body bg-ligth pt-1 pb-1 ">
                    <div class="row " style="color:black">
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
                            <asp:CheckBox ID="chkShowDisable" 
                                Text="Show disable user" runat="server"
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
                                EnableTheming="True"
                                Theme="MaterialCompact"
                                AutoGenerateColumns="False"
                                Width="100%"
                                CssClass="Sarabun"
                                KeyFieldName="Username"
                                OnDataBinding="grdDetail_DataBinding"
                                OnRowCommand="grdDetail_RowCommand">
                                <SettingsBehavior AllowFocusedRow="true" />
                                <Settings ShowFilterRow="True" ShowFooter="True" ShowHeaderFilterButton="True" />
                                <Columns>
                                    <dx:GridViewDataTextColumn  Caption="OPEN" Width="60"      HeaderStyle-CssClass="Sarabun">
                                        <DataItemTemplate>
                                            <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default "
                                                CommandName="Select" CommandArgument='<%# Eval("Username") %>'>
                                                        <i class="fa fa-folder-open"></i> 
                                            </asp:LinkButton>
                                        </DataItemTemplate>
                                        <HeaderStyle ></HeaderStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Username" FieldName="Username" Width="150">
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun"/>
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="FullName">
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun" />
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>

                 
                               
                                    <dx:GridViewDataTextColumn Caption="Department" FieldName="DepartmentID" Visible="false">
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun"/>
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                        
                                    <dx:GridViewDataTextColumn Caption="Position" FieldName="PositionID" >
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun"/>
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Email" FieldName="Email">
                                        <HeaderStyle  Wrap="False"      CssClass="Sarabun"/>
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mobile" FieldName="Mobile" Width="120">
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun"/>
                                        <CellStyle  Wrap="False" />
                                    </dx:GridViewDataTextColumn>
 
                                    <dx:GridViewDataCheckColumn Caption="Active" FieldName="IsActive" Width="60">
                                        <HeaderStyle  Wrap="False"     CssClass="Sarabun" />
                                        <CellStyle  Wrap="False"  />
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
