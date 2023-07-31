<%@ Page Title="MasterType Info" Language="C#" EnableEventValidation="false"  MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" CodeBehind="MasterTypeList.aspx.cs" Inherits="Robot.MAINMAS.MasterTypeList" %>

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

    <div class="row pb-1 ">
        <div class="col-md-12">
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
                        <asp:Label runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="black" ID="lblHeaderCaption"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <div class="btn-group" role="group" aria-label="Basic example">
                            <asp:LinkButton ID="btnExcel" CssClass="btn btn-dark" runat="server"
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
                        <div class="card-body pt-1 pb-1">
                            <div class="row ">
                                <div class="col">
                                </div>
                                <div class="col-md-7 text-right pl-0 pr-0">
                                    <div class="input-group mb-12">
                                        <asp:TextBox ID="txtSearch2" CssClass="form-control"                                      
                                            Placeholder="ค้นหา" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="row">
                                        <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-sm-12 col-md-1 pl-0 pr-0">
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
                                    KeyFieldName="MasterTypeID"
                                    OnDataBinding="grdDetail_DataBinding"
                                    OnRowCommand="grdDetail_RowCommand"
                                    CssClass="Sarabun"
                                    KeyboardSupport="True" Width="100%">
                                    <SettingsPager PageSize="80">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                                    </SettingsPager>
                                    <SettingsPager Mode="EndlessPaging" PageSize="80"></SettingsPager>
                                    <SettingsResizing ColumnResizeMode="Control" />
                                    <Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto"
                                        HorizontalScrollBarMode="Auto"
                                        VerticalScrollableHeight="400"
                                        VerticalScrollBarMode="Auto" />
                                    <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" 
                                        ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                                    <Columns>
                                        <dx:GridViewDataTextColumn Width="60">
                                            <DataItemTemplate>
                                                <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                                    CommandName="Select" CommandArgument='<%# Eval("MasterTypeID") %>'>
                                                             <i class="fa fa-folder-open"></i> 
                                                </asp:LinkButton>
                                            </DataItemTemplate>
                                            <HeaderStyle CssClass="Sarabun" ></HeaderStyle>
                                        </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="รหัสตัวเลือก" FieldName="MasterTypeID" Width="200">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle  Wrap="False" CssClass="Sarabun"  />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ชื่อตัวเลือก" FieldName="Name" Width="300">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle  Wrap="False" CssClass="Sarabun"  />
                                            <CellStyle  Wrap="False" CssClass="Sarabun"  />
                                        </dx:GridViewDataTextColumn>
                                    
                                        <dx:GridViewDataTextColumn Caption="หมายเหตุ" FieldName="Remark" Width="100%">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle  Wrap="False" CssClass="Sarabun"  />
                                            <CellStyle  Wrap="False" />
                                        </dx:GridViewDataTextColumn>
                             
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
            <span style="color: dimgray; font-size: 10px">HOPI-LAB</span>
        </div>
    </div>
</asp:Content>
