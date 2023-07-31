﻿<%@ Page Title="เข้าใช้งาน" Language="C#" MasterPageFile="~/Account/SiteB.Master" AutoEventWireup="true" CodeBehind="MyLogInRCompany.aspx.cs" ClientIDMode="Static" Inherits="Robot.Account.MyLogin.MyLogInRCompany" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddpopup" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

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

 
    <div class="row pt-5">
        <div class="col-md-12 mx-auto">
            <div class="row text-center">
                <div class="col-md-12">
                    <h2>
                        <span style="color:gray">เข้าใช้งาน</span>
                    </h2>
                </div>
            </div>
            <hr />
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
                                    <div class="card" style="border-radius: 30px; background-color:darkcyan;">
                                        <div class="card-body">
                                            <div class="row ">
                                                <div class="col-md-12 text-center">
                                                    <span style="color: white; font-size:xx-large"><%# Eval("RComName") %>  </span>   
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>