﻿<%@ Page Title="สต็อก คงเหลือ" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="StockBalanceList.aspx.cs" Inherits="Robot.POS.StockBalanceList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />

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
                            <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="Black" ID="lblinfohead"></asp:Label>
                        </div>
                        <div class="col-md-4 text-right pb-2">
                            <asp:LinkButton ID="btnExcel" CssClass="btn btn-info" runat="server"
                                OnClick="btnExcel_Click">
                                                <span  style="color:white">Excel</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnPrintStockBalance" CssClass="btn btn-secondary" runat="server"
                                OnClick="btnPrintStockBalance_Click">   
                                              <span>พิมพ์ สต็อกคงเหลือ</span> 
                            </asp:LinkButton>
                                
                                    <asp:LinkButton ID="btnStockBalByDate" CssClass="btn btn-info" runat="server" OnClick="btnStockBalByDate_Click">   
                                        <span >คงเหลือแบบรายวัน</span> 
                                    </asp:LinkButton>
                                
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <asp:UpdatePanel ID="UpdCompany" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:Label runat="server">สาขา </asp:Label>
                                    <asp:DropDownList ID="cboCompany" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="CompanyID" AutoPostBack="true"
                                        OnSelectedIndexChanged="cboCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:Label runat="server">ที่เก็บ </asp:Label>
                                    <asp:DropDownList ID="cboLocation" runat="server" CssClass="form-control" DataTextField="LocCode" DataValueField="LocID"></asp:DropDownList>
                                </div>
                                         <div class="col-2">
                                    <asp:Label runat="server">ประเภท </asp:Label>
                                    <asp:DropDownList ID="cboItemType" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                </div>
                                <div class="col-md-4 pt-4">
                                    <div class="input-group ">
                                        <asp:TextBox runat="server" ID="txtSearch" Class="form-control "
                                            placeholder="ค้นหา" />
                                        <div class="input-group-append bg-success border-success">
                                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i>&nbsp<span >Load</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row pt-1">
                                   <div class="col-md-2">
                                    <asp:CheckBox ID="chkShowAvl" Text="แสดงที่คงเหลือ" Checked="true" runat="server" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                            OnRowCommand="grdDetail_RowCommand"
                            OnDataBinding="grdDetail_DataBinding"
                            AutoGenerateColumns="False"
                            KeyFieldName="ItemID"
                            CssClass="Sarabun"
                            KeyboardSupport="True" Width="100%">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>

                                <dx:GridViewDataTextColumn Caption="ที่เก็บ" FieldName="LocID" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                                   <dx:GridViewDataTextColumn Width="60" Caption="รหัสสินค้า" FieldName="ItemID">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="lnkItemID" runat="server" ForeColor="Blue" CausesValidation="False"
                                            CommandName="itemid" Text='<%# Eval("ItemID") %>'> </asp:LinkButton>
                                    </DataItemTemplate>
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
<%--                                <dx:GridViewDataTextColumn Caption="รหัสสินค้า" FieldName="ItemID" Width="140px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>--%>
                                <%--<dx:GridViewDataTextColumn Caption="รหัสสินค้า" FieldName="ItemID" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>--%>
                                <dx:GridViewDataTextColumn Caption="ชื่อสินค้า" FieldName="ItemName" Width="140px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="สั่ง" FieldName="OrdQty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ส่ง" FieldName="InstQty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="คงเหลือ" FieldName="BalQty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชนิด" FieldName="TypeID" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="หน่วย" FieldName="UnitID" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                            <TotalSummary>
                                <dx:ASPxSummaryItem FieldName="BalQty" ShowInColumn="BalQty" ShowInGroupFooterColumn="BalQty" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                <dx:ASPxSummaryItem FieldName="OrdQty" ShowInColumn="OrdQty" ShowInGroupFooterColumn="OrdQty" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                <dx:ASPxSummaryItem FieldName="InstQty" ShowInColumn="InstQty" ShowInGroupFooterColumn="InstQty" SummaryType="Sum" DisplayFormat="{0:n2}" />
                            </TotalSummary>
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>