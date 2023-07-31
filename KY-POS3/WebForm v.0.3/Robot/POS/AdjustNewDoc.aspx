﻿<%@ Page Title="Create new" Language="C#" MasterPageFile="~/POS/SiteA.Master" ClientIDMode="AutoID" AutoEventWireup="true" CodeBehind="AdjustNewDoc.aspx.cs" Inherits="Robot.POS.AdjustNewDoc" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <asp:HiddenField ID="hddmenu" runat="server" />

    <script type="text/javascript">



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
                                                <asp:Label ID="lblHeaderMsg"  runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg"  runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " Visible="false" runat="server" Text="Close" OnClientClick="return OnClosePopupAlert();" />
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  " runat="server" Text="CANCEL" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>
                                    </div>
                                 
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-8 mx-auto">
            <div class="card bg-light">
                <div class="card-body">
                    <asp:LinkButton ID="btnBackList" Font-Size="Small"
                        CssClass="btn btn-default"
                        Width="100%"
                        runat="server"
                        OnClick="btnBackList_Click">                                          
                        <span> <i class="fas fa-reply-all fa-2x"></i></span>
                        <span   style="font-size:medium;"> กลับหน้ารายการ</span>                                            
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </div>


    <div class="row pt-1" runat="server" id="divmain">
        <div class="col-lg-8 col-md-8 col-sm-12 col-12 mx-auto">
            <asp:Panel ID="pn1" runat="server" DefaultButton="btnSearch" Visible="false">
                <div class="row ">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header">
                                <div class="row pt-2 ">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-9" id="divsearch" runat="server">
                                                <dx:ASPxTextBox ID="txtSearch"
                                                    ForeColor="deeppink"
                                                    placeholder="ค้นหาชื่อสาขา หรือ รหัสสาขา" runat="server"
                                                    CssClass="form-control form-control-lg " Width="100%">
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="col-md-3 pt-1">
                                                <asp:LinkButton ID="btnSearch" CssClass="btn btn-outline-success bn-lg" Width="100%" runat="server" Text="Search" OnClick="btnSearch_Click"></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>


            <asp:UpdateProgress ID="udppgrd" runat="server" AssociatedUpdatePanelID="udpgrd">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 99999999; background-color: #000000; opacity: 0.8;">
                        <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="row pt-1">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="udpgrd" runat="server">
                        <ContentTemplate>
                            
                                <asp:ListView ID="grdlist"
                                    KeyFieldName="CompanyID"
                                    OnPagePropertiesChanging="grdlist_PagePropertiesChanging"
                                    OnItemCommand="grdlist_ItemCommand"
                                    runat="server">
                                    <LayoutTemplate>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                        <div class="row text-center" hidden="hidden">
                                            <div class="col-md-12">
                                                <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdlist" PageSize="100">
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
                                        <div class="row pb-1">
                                            <div class="col-md-12">
                                                <div class="card bg-light" style="background-color:white;">
                                                    <div class="card-body" >
                                                        <div class="row pb-1 ">
                                                            <div class="col-md-12">
                                                                <asp:LinkButton runat="server" CommandName="ok" CommandArgument='<%# Eval("CompanyID")%>' class="btn p-0" ID="btnCompanyID" Width="100%">
                                                                    <div class="row">
                                                                        <div class="col-md-12 text-left">
                                                                            <strong>
                                                                                <asp:Label runat="server" Font-Size="Smaller" ID="lblCustomerID" Text='<%# Eval("CompanyID") %>'></asp:Label>
                                                                            </strong>
                                                                        </div>
                                                                        <%--<div class="col-md-4 text-right">
                                                                            <strong>
                                                                                <asp:Label runat="server" Font-Size="Smaller" ID="Label2" Text='<%# Convert.ToDecimal( Eval("Point") ).ToString("n2")  +" P" %>'></asp:Label>
                                                                            </strong>
                                                                        </div>--%>
                                                                        <div class="col-md-12 text-left">
                                                                            <asp:Label runat="server" ForeColor="Gray" Font-Size="Smaller" Text='<%# Eval("Name")  %>'></asp:Label>
                                                                        </div>
                                                                    </div>
                                                                </asp:LinkButton>
                                                                <asp:Label Visible="false" runat="server" ID="lblComID" Text='<%# Eval("CompanyID") %>'></asp:Label>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <div class="row  text-center">
                                            <div class="col-md-12">
                                                ...ไม่พบข้อมูล...
                                            </div>
                                        </div>

                                    </EmptyDataTemplate>
                                </asp:ListView>

                         
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>

        </div>

    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>