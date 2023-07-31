﻿<%@ Page Title="ขั้นตอนการจ่าย" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="StepPaymentList.aspx.cs" ClientIDMode="Static" Inherits="Robot.PayAgent.StepPaymentList" %>

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

        .color-deeppink{
            color:deeppink;
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

            <div class="pt-3" runat="server" id="divSelectCompany">

                <div class="row">
                    <div class="col-md-10 mx-auto pl-0 text-left">
                        <asp:LinkButton ID="btnBackList" Font-Size="Medium" CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click"> 
                                 <span> <i class="fas fa-arrow-alt-left fa-2x"></i></span> &nbsp&nbsp
                            <asp:Label runat="server" Font-Bold="true" ForeColor="Black" Font-Size="XX-Large">เลือกสาขา</asp:Label>
                        </asp:LinkButton>

                    </div>
                </div>

                <div class="row">
                    <div class="col-md-10 mx-auto text-left">
                        <div class="row">
                            <div class="col-7">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtSearch" Class="form-control"
                                        placeholder="ค้นหา" />
                                    <div class="input-group-append bg-success border-success">
                                    </div>
                                </div>
                            </div>
                            <div class="col-5 pl-0">
                                <asp:LinkButton ID="btnSearch" runat="server" Width="100%" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                                  <span >Load</span> 
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <hr />

                <div class="row py-2">
                    <div class="col-md-10 mx-auto px-0">
                        <%--<div class="card">
                            <div class="card-body p-3">--%>                                               
                        <div class="row">
                            <div class="col-md-12">
                                <asp:ListView ID="grdCompanyID"
                                    KeyFieldName="CompanyID"
                                    OnDataBinding="grdCompanyID_DataBinding"
                                    OnPagePropertiesChanging="grdCompanyID_PagePropertiesChanging"
                                    OnItemCommand="grdCompanyID_ItemCommand"
                                    pagesize="10"
                                    runat="server">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        <div class="row text-center pt-2" runat="server" style="font-size: large;">
                                            <div class="col-md-12">
                                                <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdCompanyID" PageSize="10">
                                                    <Fields>
                                                        <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                        <asp:NumericPagerField NumericButtonCssClass="color-deeppink" NextPreviousButtonCssClass="color-deeppink"></asp:NumericPagerField>
                                                        <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="sel" CommandArgument='<%# Eval("CompanyID")%>' class="btn p-0" ID="btnselect" Width="100%">
                                                <div class="card">
                                                    <div class="card-body" style="background-color:#fed330;">
                                                        <div class="row  py-1 pl-3">
                                                            <div class="col-md-12 text-center">
                                                                <span style="color: black; font-size: medium; font-weight:bold;"><%# Eval("Name1") %>&nbsp  <%# Eval("Name2" ) %>  </span>   
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="row py-3">
                                            <div class="col-md-12 text-center">
                                                <asp:Label runat="server" Font-Size="" ForeColor="deeppink">...ไม่มีข้อมูล...</asp:Label>
                                            </div>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </div>

                        <%--</div>
                        </div>--%>
                    </div>
                </div>

            </div>

            <div class="pt-3" runat="server" id="divSrlectBookBank">
                <div class="row">
                    <div class="col-md-10 mx-auto pl-0 text-left">
                        <asp:LinkButton ID="btnBackToCompany" Font-Size="Medium" CssClass="btn btn-default" runat="server"
                            OnClick="btnBackToCompany_Click"> 
                                 <span> <i class="fas fa-arrow-alt-left fa-2x"></i></span> &nbsp&nbsp
                            <asp:Label runat="server" Font-Bold="true" ForeColor="Black" Font-Size="XX-Large">เลือกธนาคาร</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="row py-2">
                    <div class="col-md-10 mx-auto px-0">
                        <%--<div class="card">
                            <div class="card-body p-3">--%>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:ListView ID="grdBookBank"
                                    KeyFieldName="BookID"
                                    OnDataBinding="grdBookBank_DataBinding"
                                    OnPagePropertiesChanging="grdBookBank_PagePropertiesChanging"
                                    OnItemCommand="grdBookBank_ItemCommand"
                                    pagesize="10"
                                    runat="server">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        <div class="row text-center pt-2" runat="server" style="font-size: large;">
                                            <div class="col-md-12">
                                                <asp:DataPager ID="grdlistPager2" runat="server" PagedControlID="grdBookBank" PageSize="10">
                                                    <Fields>
                                                        <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                        <asp:NumericPagerField NumericButtonCssClass="color-deeppink" NextPreviousButtonCssClass="color-deeppink"></asp:NumericPagerField>
                                                        <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-default" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                    </Fields>
                                                </asp:DataPager>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="sel" CommandArgument='<%# Eval("BookID")%>' class="btn p-0" ID="btnselect" Width="100%">
                                                <div class="card">
                                                    <div class="card-body" style="background-color:#fed330;">
                                                        <div class="row py-1 pl-3">
                                                            <div class="col-md-12 text-center">
                                                                <span style="color: black; font-size: medium; font-weight:bold;"><%# Eval("BookDesc") %>&nbsp  <%# Eval("BookNo" ) %> - (<%# Eval("BankCode") %>)</span>   
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                        </asp:LinkButton>

                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="row py-3">
                                            <div class="col-md-12 text-center">
                                                <asp:Label runat="server" Font-Size="" ForeColor="deeppink">...ไม่มีข้อมูล...</asp:Label>
                                            </div>
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                            </div>
                        </div>

                        <%--</div>
                        </div>--%>
                    </div>
                </div>


            </div>


            <div class="pt-3" runat="server" id="divtransfer">
                <div class="row pt-2">
                    <div class="col-md-10 mx-auto pl-0 text-left">
                        <asp:LinkButton ID="btnBackToTransfer" Font-Size="Medium" CssClass="btn btn-default" runat="server"
                            OnClick="btnBackToTransfer_Click"> 
                                 <span> <i class="fas fa-arrow-alt-left fa-2x"></i></span> &nbsp&nbsp
                            <asp:Label runat="server" Font-Bold="true" ForeColor="Black" Font-Size="XX-Large">โอนจากธนาคาร</asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="row pt-2">
                    <div class="col-md-12">
                        <asp:Label runat="server">ธนาคาร</asp:Label>
                        <asp:DropDownList ID="cboBankCode" runat="server" CssClass="form-control form-control-sm kanit" DataTextField="Name_TH" DataValueField="BankCode"></asp:DropDownList>
                    </div>
                </div>

                <div class="row pt-2">
                    <div class="col-md-12">
                        <asp:Label runat="server">ยอดเงินโอน</asp:Label>
                        <asp:TextBox runat="server" ID="txtPayAmt" Font-Size="Large"
                            CssClass="form-control form-control-sm"
                            Style="text-align: right" onkeypress="return DigitOnly(this,event)" />
                    </div>
                </div>

                <div class="row pt-2">
                    <div class="col-md-12">
                        <asp:Label runat="server">วันที่โอนเงิน</asp:Label>
                        <dx:ASPxDateEdit ID="dtReconcileDate"
                            DisplayFormatString="dd-MM-yyyy"
                            EditFormatString="dd-MM-yyyy"
                            runat="server"
                            Theme="Material"
                            Width="100%">
                        </dx:ASPxDateEdit>
                    </div>
                </div>

                <div class="row pt-2">
                    <div class="col-md-12">
                        <asp:Label runat="server">หมายเหตุ</asp:Label>
                        <asp:TextBox runat="server" ID="txtMemo" Height="50px"
                            CssClass="form-control form-control-sm" placeholder="หมายเหตุ" />
                    </div>
                </div>


                <div class="row pt-3">
                    <div class="col-md-12">
                        <asp:LinkButton ID="btnSave" runat="server" Width="100%" CssClass="btn btn-success"
                            OnClick="btnSave_Click"> 
                            <span>ยืนยัน</span> 
                        </asp:LinkButton>
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
            <asp:AsyncPostBackTrigger ControlID="grdCompanyID" />
            <asp:AsyncPostBackTrigger ControlID="grdBookBank" />
        </Triggers>
    </asp:UpdatePanel>



</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>