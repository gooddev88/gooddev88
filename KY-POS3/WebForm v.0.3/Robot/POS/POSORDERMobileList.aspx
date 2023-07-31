﻿<%@ Page Title="" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="POSORDERMobileList.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.POSORDERMobileList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

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
        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";

        }

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

    <asp:UpdatePanel ID="udpPost" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-10 px-2 mx-auto">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row pt-2">
                                        <div class="col-md-12 text-center">
                                            <h3>
                                                <asp:Label runat="server" Font-Bold="true" ForeColor="Black" ID="lblheadinfo"></asp:Label>
                                            </h3>
                                        </div>
                                        <div class="col-md-12 text-center">
                                            <asp:LinkButton ID="btnChangeDate" runat="server" Visible="false"
                                                OnClick="btnChangeDate_Click">
                                                <i class="fas fa-calendar-day"></i>
                                                <asp:Label CssClass="text-decoration-none" Font-Size="Small" runat="server" ID="lblshowdate"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-6">
                                            <span>วันที่สั่ง</span>
                                            <dx:ASPxDateEdit ID="dtOrdDateBegin" DisplayFormatString="dd-MM-yyyy"
                                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                                            </dx:ASPxDateEdit>
                                        </div>
                                        <div class="col-6">
                                            <span>ถึง</span>
                                            <dx:ASPxDateEdit ID="dtOrdDateEnd" DisplayFormatString="dd-MM-yyyy"
                                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-6">
                                            <span>สถานะ</span>
                                            <asp:DropDownList ID="cboStatus" Font-Size="Small" runat="server" CssClass="form-control form-control-sm" DataTextField="Desc" DataValueField="Value"></asp:DropDownList>
                                        </div>
                                        <div class="col-6 pt-3">
                                            <asp:LinkButton ID="btnSearch" runat="server" Width="100%" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                                  <span >Load</span> 
                                            </asp:LinkButton>
                                            <div class="input-group" runat="server" visible="false">
                                                <asp:TextBox runat="server" ID="txtSearch" Class="form-control"
                                                    placeholder="ค้นหา" />
                                                <div class="input-group-append bg-success border-success">
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pt-3">
                                        <div class="col-12 text-center">
                                            <asp:LinkButton ID="btnNew" ForeColor="#9999ff"
                                                CssClass="btn btn-defult" Font-Size="Large" runat="server"
                                                OnClick="btnNew_Click">  
                                                    <i class="fas fa-plus-circle" style="color:seagreen"></i>
                                                   <span   style="color:darkcyan">สร้างใหม่</span>
                                            </asp:LinkButton>
                                                                                        <asp:LinkButton ID="btnPrintFGK" CssClass="btn btn-secondary" runat="server" Visible="false"
                                                OnClick="btnPrintFGK_Click">   
                                              <span>พิมพ์ใบเบิกสินค้า</span> 
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPrintRMK" CssClass="btn btn-secondary" runat="server" Visible="false"
                                                OnClick="btnPrintRMK_Click">   
                                              <span>พิมพ์ใบซื้อสินค้า</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <hr />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:ListView ID="grdline"
                                                KeyFieldName="OrdID"
                                                OnDataBinding="grdline_DataBinding"
                                                OnPagePropertiesChanging="grdline_PagePropertiesChanging"
                                                OnItemCommand="grdLine_ItemCommand"
                                                OnItemDataBound="grdLine_ItemDataBound"
                                                pagesize="10"
                                                runat="server">
                                                <LayoutTemplate>
                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                    <div class="row text-center" runat="server">
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
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="row" style="flex-wrap: unset;">
                                                                <div class="col-12 pr-0 pt-1 text-left">
                                                                    <span style="color: black; font-size: small;">เลขออเดอร์ <strong><%# Eval("OrdID") %></strong> &nbsp  วันที่ <strong><%# Convert.ToDateTime(Eval("OrdDate")).ToString("dd/MM/yyyy") %></strong>
                                                                    </span>
                                                                    <br />
                                                                    <span style="color: black; font-size: small;">สั่ง <strong><%# Convert.ToDecimal(Eval("OrdQty")).ToString("N2") %></strong>  &nbsp ยอดเงิน <strong><%# Convert.ToDecimal(Eval("OrdAmt")).ToString("N2") %></strong> บาท
                                                                    </span>&nbsp
                                                                    
                                                                    <asp:LinkButton ID="btnShow" ForeColor="#6699ff"
                                                                        runat="server" CommandArgument='<%# Eval("OrdID") %>'
                                                                        CssClass="btn btn-default py-0 px-0" Font-Size="Small"
                                                                        CausesValidation="False" CommandName="show"> 
                                                                            รายละเอียดสินค้า
                                                                    </asp:LinkButton>
                                                                    <br />
                                                                    <asp:Label runat="server" ID="lblShip" ForeColor="Black" Font-Size="Small">ส่งแล้ว <strong ><%# Convert.ToDecimal(Eval("ShipQty")).ToString("N2") %></strong>  &nbsp รับแล้ว <strong><%# Convert.ToDecimal(Eval("GrQty")).ToString("N2") %></strong> 
                                                                    </asp:Label>
                                                                    &nbsp
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                </ItemTemplate>
                                                <EmptyDataTemplate>
                                                    <div class="row py-3">
                                                        <div class="col-md-12 text-center">
                                                            <asp:Label runat="server" Font-Size="" ForeColor="deeppink">... ไม่มีรายการรับสินค้า ...</asp:Label>
                                                        </div>
                                                    </div>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

            <div class="row pt-1">
                <div class="col-12 pt-1">
                    <asp:Label ID="lblAlertmsg" runat="server"></asp:Label>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdline" />
        </Triggers>
    </asp:UpdatePanel>



</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>