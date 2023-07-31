﻿<%@ Page Title="รายงาน" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="LineMenuReport.aspx.cs" ClientIDMode="Static" Inherits="Robot.Report.LineMenuReport" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddphoto" runat="server" />


    <div class="row pt-4">
        <div class="col-12 mx-auto">
            <div class="row text-center pb-3">
                <div class="col-12">
                    <img src="../../Image/Logo/kylogo.png" style="width: 150px" />
                    <br />
                    <asp:Label ID="lblMenuPayment" runat="server" Font-Bold="true" Font-Size="X-Large"> </asp:Label>
                </div>
            </div>


            <div class="row pt-2">
                <div class="col-10 text-center mx-auto">
                    <asp:LinkButton runat="server" Width="100%" CssClass="btn btn-info text-decoration-none" Font-Size="Larger" 
                        ForeColor="White" ID="btnReportSummary"
                        OnClick="btnReportSummary_Click">
                        <asp:Label runat="server" Font-Size="X-Large">รายงานสรุปยอดขาย</asp:Label>
                    </asp:LinkButton>
                </div>
            </div>

                        <div class="row pt-2">
                <div class="col-10 text-center mx-auto">
                    <asp:LinkButton runat="server" Width="100%" CssClass="btn btn-info text-decoration-none" Font-Size="Larger" 
                        ForeColor="White" ID="btnReportSumPo"
                        OnClick="btnReportSumPo_Click">
                        <asp:Label runat="server" Font-Size="X-Large">รายงานสรุปสั่งซื้อสินค้า</asp:Label>
                    </asp:LinkButton>
                </div>
            </div>


            <div class="row pt-2">
                <div class="col-6 pt-1">
                    <asp:Label ID="lblAlertmsg" runat="server">  </asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>