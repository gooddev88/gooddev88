﻿<%@ Page Title="PO" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="POSPOList.aspx.cs" Inherits="Robot.POS.POSPOList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    
    <!-- Print slip via RawBT -->
    <script>

        function sendUrlToPrint(url) {
            var beforeUrl = 'intent:';
            var afterUrl = '#Intent;';
            // Intent call with component
            afterUrl += 'component=ru.a402d.rawbtprinter.activity.PrintDownloadActivity;'
            afterUrl += 'package=ru.a402d.rawbtprinter;end;';
            document.location = beforeUrl + encodeURI(url) + afterUrl;
            return false;
        }
        // jQuery: set onclick hook for css class print-file
        $(document).ready(function () {
            $('.print-file').click(function () {
                return sendUrlToPrint($(this).attr('href'));
            });
        });
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
                                                <asp:Label ID="lblHeaderMsg" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" runat="server" Text=""></asp:Label>
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
                        <div class="col-md-3 text-center">
                            <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="Black" ID="lblinfohead"></asp:Label>
                        </div>
                        <div class="col-md-5 text-right pb-2">
                            <asp:LinkButton ID="btnnotifyPo" CssClass="btn btn-info" runat="server" OnClick="btnnotifyPo_Click">   
                                <span>ส่งไลท์เบิกเงิน</span> 
                            </asp:LinkButton>
                             <asp:LinkButton ID="btnPOReport" CssClass="btn btn-secondary" runat="server" OnClick="btnPOReport_Click">   
                                <span>รายงาน</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnPrintPO" CssClass="btn btn-secondary" runat="server"
                                OnClick="btnPrintPO_Click">   
                                              <span>พิมพ์ใบสั่งซื้อ</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnNew" CssClass="btn btn-success" runat="server" OnClick="btnNew_Click">   
                                <span >สร้างใหม่</span> 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <div class="row">
                        <div class="col-2">
                            <span>สถานะ</span>
                            <asp:DropDownList ID="cboStatus" runat="server" CssClass="form-control form-control-sm" DataTextField="Desc" DataValueField="Value"></asp:DropDownList>
                        </div>
                        <div class="col-2">
                            <span>วันที่</span>
                            <dx:ASPxDateEdit ID="dtPODateBegin" DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-2">
                            <span>ถึง</span>
                            <dx:ASPxDateEdit ID="dtPODateEnd" DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-2">
                            <span>เลือกผู้ขาย (สั่งพิมพ์)</span>
                            <dx:ASPxComboBox ID="cboVendor" runat="server"
                                Theme="Material" DropDownStyle="DropDown"
                                ValueField="VendorID"
                                ValueType="System.String" TextFormatString="{0} {1}" Width="100%">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="VendorID" Caption="ผู้ขาย" Width="150px" />
                                    <dx:ListBoxColumn FieldName="FullNameTh" Caption="ชื่อผู้ขาย" Width="250px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-2 pt-3">
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
                        <div class="col-md-2 pt-3 text-left">
                            <asp:CheckBox ID="chkShowClose" Text="แสดงที่ปิดใช้งาน" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowClose_CheckedChanged" />
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
                        <dx:ASPxGridView ID="grdDetail" runat="server" EnableTheming="True"
                            Theme="MaterialCompact"
                            OnDataBinding="grdDetail_DataBinding"
                            AutoGenerateColumns="False"
                            KeyFieldName="POID"
                            CssClass="Sarabun"
                            OnRowCommand="grdDetail_RowCommand"
                            KeyboardSupport="True" Width="100%">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="" Caption="" Width="60">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnOpen"
                                            runat="server"
                                            CssClass="btn btn-icons btn-default"
                                            CommandName="Select" CommandArgument='<%# Eval("POID") %>'>
                                             <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>

                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="PO No." FieldName="POID" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                   <dx:GridViewDataTextColumn Caption="สถานะ" FieldName="Status" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="วันที่ PO" FieldName="PODate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                          
                                 <dx:GridViewDataTextColumn Caption="เลขสาขา" FieldName="ComID" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                      <dx:GridViewDataTextColumn Caption="เลขสั่ง" FieldName="OrdID" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="สาขาที่สั่ง" FieldName="OrdFrComName" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="จำนวนซื้อ" FieldName="Qty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ยอดซื้อ" FieldName="Amt" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                            <%--    <dx:GridViewDataTextColumn Caption="จำนวนที่ส่ง" FieldName="ShipQty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n0" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ยอดที่ส่ง" FieldName="ShipdAmt" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n0" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>--%>
                                <%--<dx:GridViewDataTextColumn Caption="ผู้ขาย" FieldName="VendorName" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>--%>
                                <dx:GridViewDataTextColumn Caption="หมายเหตุ" FieldName="Remark1" Width="300">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="วันที่สร้าง" FieldName="CreatedDate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="สร้างโดย" FieldName="CreatedBy" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn Caption="ใช้งาน" FieldName="IsActive" Width="80">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataCheckColumn>
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