﻿<%@ Page Title="สต็อกคงเหลือแบบเลือกวันที่" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="StockBalByDateList.aspx.cs" Inherits="Robot.POS.StockBalByDateList" %>

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
                            <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true"  ID="lblinfohead"></asp:Label>
                        </div>
                        <div class="col-md-4 text-right pb-2">
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <asp:UpdatePanel ID="UpdCompany" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-3">
                                    <asp:Label runat="server">สาขา </asp:Label>
                                    <asp:DropDownList ID="cboCompany" runat="server" CssClass="form-control"
                                        DataTextField="Name" DataValueField="CompanyID" AutoPostBack="true"
                                        OnSelectedIndexChanged="cboCompany_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-3">
                                    <asp:Label runat="server">ที่เก็บ </asp:Label>
                                    <asp:DropDownList ID="cboLocation" runat="server" CssClass="form-control form-control-sm" DataTextField="LocCode" DataValueField="LocID"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <span style="font-size: medium">วันที่ </span>
                                    <dx:ASPxDateEdit ID="dtDate" DisplayFormatString="dd-MM-yyyy" Theme="Material"
                                        EditFormatString="dd-MM-yyyy" runat="server" CssClass=" " Width="100%">
                                    </dx:ASPxDateEdit>
                                </div>

                                <div class="col-3 pt-4">
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

                                <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ที่เก็บ" FieldName="LocID" Width="140px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="วันที่" FieldName="StockAsDate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="สินค้า" FieldName="ItemID" Width="140px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อสินค้า" FieldName="ItemName" Width="200px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="คงเหลือ" FieldName="QtyBal" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                            <TotalSummary>
                                <dx:ASPxSummaryItem FieldName="BalQty" ShowInColumn="BalQty" ShowInGroupFooterColumn="BalQty" SummaryType="Sum" DisplayFormat="{0:n2}" />
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