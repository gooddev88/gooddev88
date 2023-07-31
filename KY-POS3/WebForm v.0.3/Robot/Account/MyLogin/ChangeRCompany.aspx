﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Account/SiteB.Master" AutoEventWireup="true" CodeBehind="ChangeRCompany.aspx.cs" ClientIDMode="Static" Inherits="Robot.Account.MyLogin.ChangeRCompany" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  
    <asp:HiddenField ID="hddmenu" runat="server" />
   

    <asp:HiddenField ID="hddParentPage" runat="server" />

    <script type="text/javascript">
        //begin combobox bind data
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

        //end combobox bind data

        //BEGIN Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK':
                    btnPostCode.DoClick();
                    break;
                case 'Cancel':
                    popup.Hide();
                    popMessage.Hide();
                    break;
            }
        }
        //END Pop Postback


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
        //end show msg
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     


    <div class="row pt-3" runat="server" id="divmain">
        <div class="col-lg-10 col-md-10 col-sm-12 col-12 mx-auto">
            <div class="row">                         
                <div class="col-md-12 pt-1 ">
                    <asp:LinkButton ID="btnback" Width="100%" CssClass="btn btn-secondary" runat="server" 
                        Height="60"
                        Text="" OnClick="btnback_Click">
                                      <span style="color:white"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span class="" style="font-size:medium;color:white"> กลับ</span> 
                    </asp:LinkButton>
                </div>
            </div>
        </div>

    </div>

    <div class="row">
        <div class="col-md-10 mx-auto">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="" style="overflow-x: auto; width: 100%">
                        <asp:ListView ID="grdlist"
                            KeyFieldName="CompanyID"
                            OnPagePropertiesChanging="grdlist_PagePropertiesChanging"
                            OnItemCommand="grdlist_ItemCommand"
                            runat="server">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                <div class="row text-center" runat="server" hidden="hidden">
                                    <div class="col-md-12">
                                        <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdlist" PageSize="1000">
                                            <Fields>
                                                <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                <asp:NumericPagerField NumericButtonCssClass="accordion"></asp:NumericPagerField>
                                                <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </div>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="selrow" CommandArgument='<%# Eval("RComID")%>' class="btn p-0" ID="btnselect" Width="100%">
                                    <div class="card" style="border-radius: 30px; background-color:darkslateblue;">
                                        <div class="card-body">
                                            <div class="row  py-1 pl-3">
                                                <div class="col-md-12 text-center">
                                                    <span style="color: wheat; font-size: large"><%# Eval("RComName") %>   <%# Eval("RComID" ) %>  </span>   
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="row text-center pt-3 ">
                                    <div class="col-md-12">
                                        ...ไม่มีข้อมูล...
                                    </div>
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                         <div class="row pt-1">
                                <div class="col-md-12 ">
                                    <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                                        <strong>
                                            <span id="myalertHead" ></span></strong>
                                        <br />
                                        <span id="myalertBody" ></span>
                                        <asp:Label ID="lblAlertMsg" runat="server" Text=""></asp:Label>
                                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                </div>
                            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>