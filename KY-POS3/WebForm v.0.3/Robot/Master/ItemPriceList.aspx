﻿<%@ Page Title="ราคาสินค้า" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="ItemPriceList.aspx.cs" Inherits="Robot.Master.ItemPriceList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />

        <%--Begin ddl--%>
    <script type="text/javascript">
        var startTime;
        function OnBeginCallback() {
            startTime = new Date();
        }
        function OnEndCallback() {
            var result = new Date() - startTime;
            result /= 1000;
            result = result.toString();
            if (result.length > 4)
                result = result.substr(0, 4);
            time.SetText(result.toString() + " sec");
            label.SetText("Time to retrieve the last data:");
        }
    </script>

    <script type="text/javascript">
        //begin Popup Postback
        function OnClosePopupAlert() {
            popAlert.Hide();
        }
    </script>

    <asp:UpdatePanel ID="udpAlert" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="400" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
                HeaderText="Infomation" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsg"  runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg"  runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="OK" OnClientClick="return OnClosePopupAlert();" />
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  " runat="server" Text="CANCEL" OnClientClick="return OnClosePopupAlert();" />
                                            </div>
                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>

            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
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
                            <asp:Label runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="Black" ID="lblinfohead"></asp:Label>
                        </div>
                        <div class="col-md-4 text-right pb-2">
                            <asp:LinkButton ID="btnExportExcel" CssClass="btn btn-defult text-left" Font-Size="Large" ForeColor="#9999ff" runat="server"
                                OnClick="btnExportExcel_Click">   
                            <i class="fas fa-cloud-download-alt" style="color:seagreen"></i>
                            <span style="color:darkcyan">Export Excel</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnImport" CssClass="btn btn-defult text-left" Font-Size="Large" ForeColor="#9999ff" runat="server"
                                OnClick="btnImport_Click">   
                            <i class="fas fa-cloud-upload-alt" style="color:palevioletred"></i>
                            <span style="color:darkcyan">Import Excel</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <div class="row">
                        <div class="col-md-2">
                            <span style="font-size: small">Item No. </span>
                            <dx:ASPxComboBox ID="cboItem" runat="server"
                                Theme="Material"
                                CssClass="Sarabun"
                                EnableCallbackMode="true"
                                CallbackPageSize="10"
                                DropDownWidth="600"
                                DropDownHeight="300"
                                ValueType="System.String" ValueField="ItemID"
                                OnItemsRequestedByFilterCondition="cboItem_OnItemsRequestedByFilterCondition_SQL"
                                OnItemRequestedByValue="cboItem_OnItemRequestedByValue_SQL" TextField="ItemID" TextFormatString="{0} ( {1} )"
                                Width="100%" DropDownStyle="DropDownList">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="ItemID" Caption="Item No." />
                                    <dx:ListBoxColumn FieldName="Name1" Caption="Description" Width="400px" />
                                    <dx:ListBoxColumn FieldName="TypeID" Caption="Type" Width="100px" />
                                </Columns>
                                <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                            </dx:ASPxComboBox>
                        </div>
                        <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                        <div class="col-md-2">
                            <span  style="font-size: small">สาขา </span>
                            <asp:DropDownList ID="cboCompanyID" runat="server" 
                                CssClass="form-control form-control-sm" DataTextField="Name1" DataValueField="CompanyID">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <span style="font-size: small">ขายผ่าน</span>
                            <asp:DropDownList ID="cboShipTo" runat="server" Width="100%"
                                CssClass="form-control form-control-sm"
                                DataTextField="ShipToName" DataValueField="ShipToID">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <span style="font-size: small">คำนวณภาษี </span>
                            <asp:DropDownList ID="cboPriceTaxcon" runat="server" CssClass="form-control form-control-sm" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <span style="font-size: small">ลำดับใช้งาน </span>
                            <asp:DropDownList runat="server" ID="cboUseLevel" CssClass="form-control form-control-sm" DropDownStyle="DropDownList">
                                <asp:ListItem Text="ทั้งหมด" Value=""></asp:ListItem>
                                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="col-md-3 pt-3">
                            <div class="input-group ">
                                <asp:TextBox runat="server" ID="txtSearch" Class="form-control"
                                    placeholder="ค้นหา" />
                                <div class="input-group-append bg-success border-success">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i>&nbsp<span >Load</span> 
                                    </asp:LinkButton>
                                </div>
                                </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />



    <div class="row">
        <div class="col-md-12">

            <asp:UpdatePanel ID="udp_list" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div style="overflow-x: auto; width: 100%">
                        <dx:ASPxGridView ID="grdDetail" 
                            runat="server" 
                            EnableTheming="True"
                            Theme="MaterialCompact" 
                            OnDataBinding="grdDetail_DataBinding"
                            AutoGenerateColumns="False"
                            KeyFieldName="ItemID"
                            CssClass="Sarabun"
                            KeyboardSupport="True" Width="100%">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="" Caption="" Width="60">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnOpen"
                                            runat="server"
                                            CssClass="btn btn-icons btn-default"
                                            CommandName="Select" CommandArgument='<%# Eval("ItemID") %>'>
                                             <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>
                                    <HeaderStyle ></HeaderStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="รหัสสินค้า" FieldName="ItemID" Width="130">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อสินค้า" FieldName="ItemName" Width="250px">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สาขา" FieldName="CompanyID" Width="130">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อสาขา" FieldName="CompanyName" Width="210px">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ลำดับใช้งาน" FieldName="UseLevel" Width="80px">
                                    <HeaderStyle Wrap="False" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="ขายผ่าน" FieldName="CustID" Width="120">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ราคา" FieldName="Price" Width="130px">
                                    <HeaderStyle Wrap="False" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="คำนวณภาษี" FieldName="PriceTaxCondType" Width="130px">
                                    <HeaderStyle Wrap="False" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="เริ่ม" FieldName="DateBegin" Width="120">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataDateColumn Caption="สิ้นสุด" FieldName="DateEnd" Width="120">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>