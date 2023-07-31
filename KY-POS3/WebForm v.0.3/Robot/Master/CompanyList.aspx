<%@ Page Title="Company info" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="CompanyList.aspx.cs" Inherits="Robot.Master.CompanyList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddid" runat="server" />
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


    <div class="row pb-1">
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

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body pt-1 pb-1">
                            <div class="row ">
                                <div class="col"></div>
                                <div class="col-md-7 text-right pl-0 pr-0">
                                    <div class="input-group mb-12">
                                        <asp:TextBox ID="txtSearch" CssClass="form-control"
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
            <dx:ASPxCallbackPanel ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                runat="server"
                OnCallback="CallbackPanel_Callback">
                <SettingsLoadingPanel Enabled="false" />
                <ClientSideEvents BeginCallback="OnBeginCallback"
                    EndCallback="OnEndCallback" />
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxGridView ID="grdDetail" runat="server" Width="100%"
                            EnableTheming="True"
                            Theme="MaterialCompact"
                            AutoGenerateColumns="False"
                            KeyFieldName="CompanyID"
                            CssClass="Sarabun"
                            OnDataBinding="grdDetail_DataBinding"
                            OnRowCommand="grdDetail_RowCommand" KeyboardSupport="True">
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
                                            CommandName="Select" CommandArgument='<%# Eval("CompanyID") %>'>
                                             <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>
                                    <HeaderStyle ></HeaderStyle>
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="สาขา" FieldName="CompanyID" Width="140px">
                                    <HeaderStyle  Wrap="False"    CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="บริษัท" FieldName="ParentID" Width="140px">
                                    <HeaderStyle  Wrap="False"    CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อร้าน" FieldName="Name1" Width="250px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อสาขา" FieldName="Name2" Width="200px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn Caption="ชื่อย่อ" FieldName="ShortCode" Width="100px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                       <dx:GridViewDataTextColumn Caption="เลขสาขา" FieldName="BrnCode" Width="100px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="TAX ID" FieldName="TaxID" Width="150px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="เลขที่" FieldName="AddrNo" Width="130px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ตำบล" FieldName="AddrTumbon">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="อำเภอ" FieldName="AddrAmphoe">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="จังหวัด" FieldName="AddrProvince">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="รหัสไปรษณีย์" FieldName="AddrPostCode">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="ที่อยู่เปิดบิล 1" FieldName="BillAddr1" Width="350px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ที่อยู่เปิดบิล 2" FieldName="BillAddr2" Width="350px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="มือถือ" FieldName="Mobile" Width="120px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="เบอร์โทร" FieldName="Tel1" Width="120px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun"/>
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataCheckColumn FieldName="IsActive" Caption="ใช้งาน" Width="70px"> 
                                </dx:GridViewDataCheckColumn>

                                <dx:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy" Width="100px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="Created Date" FieldName="CreatedDate" Width="110px">
                                    <HeaderStyle  Wrap="False"   CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
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


</asp:Content>
