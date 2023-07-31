<%@ Page Title="รายงานสรุปยอดขาย" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="ReportSalesSummary.aspx.cs" ClientIDMode="Static" Inherits="Robot.Communication.Report.ReportSalesSummary" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />


    <style>
        .btn.btn-turquoise {
            color: #fff;
            background-color: #00CC6A;
            border-color: #00CC6A;
        }
    </style>

    <script type="text/javascript">

        //ddl load
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

        //BEGIN Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK':
                    btnPostCode.DoClick();
                    btnShowListPost.DoClick();
                    break;
                case 'Cancel':
                    popup.Hide();
                    popMessage.Hide();
                    break;
            }
        }

        //END Pop Postback

        //popup show
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }



        //begin show msg
        function ShowAlert(message, messagetype) {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "block";

            var cssclass;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert alert-success alert-dismissible fade show'
                    break;
                case 'Error':
                    cssclass = 'alert alert-danger alert-dismissible fade show'
                    break;
                case 'Warning':
                    cssclass = 'alert alert-warning alert-dismissible fade show'
                    break;
                default:
                    cssclass = 'alert alert-info alert-dismissible fade show'
            }
            document.getElementById("divalert").setAttribute("class", cssclass);
            document.getElementById("myalertHead").textContent = messagetype;
            document.getElementById("myalertBody").textContent = message;
            $('.alert').alert()
        }
        //function CloseAlert() {
        //    var divalert = document.getElementById("divalert");
        //    divalert.style.display = "none";

        //}

        function mypopprofile() {
            btnPostProfile.DoClick();
        }


        function OnClosePopupAlert() {
            popAlert.Hide();
        }
        //end show msg
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

            <div class="row pt-2">
                <div class="col-md-10 px-2 mx-auto">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row text-center">
                                <div class="col-12">
                                    <img src="../../Image/Logo/kylogo.png" style="width: 90px" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12 text-center">
                                    <h3 style="font-weight: bold; color: black;">
                                        <asp:Label  runat="server" ID="lblTopic"></asp:Label>
                                    </h3>
                                </div>
                                <div class="col-md-12 text-center">
                                    <asp:Label runat="server" Font-Size="Larger" ID="lblSaleName"></asp:Label>
                                </div>
                                <div class="col-md-12 text-center">
                                    <asp:LinkButton ID="btnBackList" CssClass="text-decoration" runat="server" Font-Size="Small"
                                    OnClick="btnBackList_Click">
                                <span>กลับสู่หน้าหลัก</span> &nbsp
                                </asp:LinkButton>&nbsp
                                    <asp:LinkButton ID="btnChangeDate" runat="server" ForeColor="DeepPink"
                                        OnClick="btnChangeDate_Click">
                                        <asp:Label CssClass="text-decoration-none" Font-Size="Medium" runat="server" ID="lblshowdate"></asp:Label>
                                    </asp:LinkButton>
                                </div>
                            </div>
                            
                            <div class="row pt-3">
                                <div class="col-11 mx-auto">
                                    <asp:ListView ID="grdline"
                                        KeyFieldName="PaymentID"
                                        OnDataBinding="grdline_DataBinding"
                                        OnPagePropertiesChanging="grdline_PagePropertiesChanging"
                                        OnItemCommand="grdLine_ItemCommand"
                                        OnItemDataBound="grdLine_ItemDataBound"
                                        pagesize="100"
                                        runat="server">
                                        <LayoutTemplate>
                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                            <div class="row text-center" runat="server" visible="false">
                                                <div class="col-md-12">
                                                    <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdline" PageSize="10">
                                                        <Fields>
                                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                            <asp:NumericPagerField NumericButtonCssClass="accordion"></asp:NumericPagerField>
                                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <div class="row pb-3">
                                                <div class="col-md-12">
                                                    <div class="card" style="box-shadow: 0 10px 20px rgba(0,0,0,0.19), 0 6px 6px rgba(0,0,0,0.23);">
                                                        <div class="card-header">
                                                            <div class="row">
                                                                <div class="col-12 text-center">
                                                                    <h5 style="font-weight:bold;"><%# Eval("BranchName")%></h5>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body">
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>ยอดสินค้า</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("NettotalAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>vat สินค้า</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("NettotalVatAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>รวมทั้งหมด</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("NettotalAmtIncVat")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>ปัดเศษ</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("RoundDown")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>ยอดรวมหลังปัดเศษ</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("NetTotalAfterRound")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>รวมบิล</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("TotalBill")).ToString("N0") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>รวมบิลที่ยกเลิก</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("TotalCancelBill")).ToString("N0") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>รวมที่จ่ายเงินสด</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("CashPayAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>รวมที่จ่ายโอน</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("TransferPayAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>ค่าเฉลี่ยต่อบิล</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("AVGPerBIll")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <hr class="my-3" style="border-top: 2px dotted;" />
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>GRAB</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("GrabAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>PANDA</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("PandaAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>LINEMAN</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("LineManAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-6 text-left">
                                                                    <span style="color: black; font-size: small;"><strong>GOJEK</strong> </span>
                                                                </div>
                                                                <div class="col-6 text-right">
                                                                    <span style="color: black; font-size: small;"><%# Convert.ToDecimal(Eval("GoJekAmt")).ToString("N2") %> </span>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="row py-3">
                                                <div class="col-md-12 text-center">
                                                    <hr style="border-top:dotted;" />
                                                    <asp:Label runat="server" Font-Size="" ForeColor="Black"> ยังไม่มีรายการยอดขาย </asp:Label>
                                                    <hr style="border-top:dotted;" />
                                                </div>
                                            </div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                        </div>
                    </div>      
                    <div class="row text-center pt-3">
                        <div class="col-12">
                               <asp:Label ID="lblSumPay" Font-Bold="true" Font-Size="Larger" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row pt-1">
                <div class="col-12 pt-1">
                    <asp:Label ID="lblAlertmsg" runat="server"></asp:Label>
                </div>
            </div>


</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
