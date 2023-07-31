﻿<%@ Page Title="" Language="C#" MasterPageFile="~/POS/SiteA.Master" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="MyPrint.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSPrint.MyPrint" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddlinenum" runat="server" />
 
    <asp:HiddenField ID="hddDocumentTypeGroup" runat="server" />



</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    <asp:HiddenField ID="hddReportName" runat="server" />
    <asp:HiddenField ID="HiddenField3" runat="server" />
    <asp:HiddenField ID="hddOption1" runat="server" />
    <asp:HiddenField ID="hddOption2" runat="server" />

              <dx:ASPxButton ID="ASPxButton1" runat="server" Text="ASPxButton" ClientVisible="false" AutoPostBack="true">
              <ClientSideEvents Click="function(s, e) {
 
    docviewer.Print(); 
    docviewer.ExportTo();	
}" />
    </dx:ASPxButton>
    <asp:Button ID="btnShow"  Visible="false" runat="server" Text="Show" OnClick="btnShow_Click" />

    <div class="row pt-2">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-10 kanit">
                            <i class="fa fa-backward" aria-hidden="true"></i>
                            <asp:LinkButton ID="btnBack" runat="server" Font-Bold="true" ForeColor="Black" OnClick="btnBack_Click">Back</asp:LinkButton>
                        </div>
                        <div class="col-md-2">
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row kanit">
                        <div class="col-md-3 text-center" style="display:none;">

                            <div class="row" runat="server" id="divbtnPrintBillposafull">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnPrintBillposafull" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnPrintBillposaFull_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมใบเสร็จ/ใบกำกับภาษี</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="row" runat="server" id="divbtnPrintBillposa">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnPrintBillposa" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnPrintBillposa_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมใบเสร็จ/ใบกำกับภาษีแบบย่อ</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="row" runat="server" id="divTotalBillInCom">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnTotalBillInCom" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnPrintTotalBillInCom_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมใบภาษีขาย</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="row" runat="server" id="divSumTotalBill">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnSumTotalBill" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnPrintSumTotalBill_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมรายงานสรุปยอดขาย</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="row" runat="server" id="divCancelBill">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnCancelBill" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnPrintbtnCancelBill_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมรายงานยกเลิดบิล</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="row" runat="server" id="divbtnTotalqtyItem">
                                <div class="col-md-12 pb-3">
                                    <asp:LinkButton ID="btnTotalqtyItem" class="btn btn" Width="100%" BackColor="#C0C0C0" BorderColor="Black" ForeColor="Black" runat="server" OnClick="btnTotalqtyItem_Click">
                                        <div class="text-left">
                                            <i class="fa fa-print fa-lg" aria-hidden="true"></i> &nbsp <span class="kanit">พิมรายงานสินค้ารายชิ้น</span> 
                                        </div>

                                    </asp:LinkButton>
                                </div>
                            </div>


                        </div>
                        <div class="col-lg-12 col-md-12">
                            <dx:ASPxWebDocumentViewer ID="docviewer" ToolbarMode="Ribbon" Height="1100px" Theme="Material" Width="100%" runat="server" DisableHttpHandlerValidation="False">
                            </dx:ASPxWebDocumentViewer>
                        </div>
                    </div>
                </div>
                <br />
            </div>

        </div>
    </div>



</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>