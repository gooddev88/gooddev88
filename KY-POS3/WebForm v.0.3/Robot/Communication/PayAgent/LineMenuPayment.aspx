<%@ Page Title="แจ้งชำระเงิน" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="LineMenuPayment.aspx.cs" ClientIDMode="Static" Inherits="Robot.PayAgent.LineMenuPayment" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddphoto" runat="server" />


    <div class="row pt-4">
        <div class="col-12 mx-auto">
            <div class="row text-center pb-3">
                <div class="col-12">
                    <img src="../../Image/Logo/QWLogo.png" style="width: 150px" />
                    <br />
                    <asp:Label ID="lblMenuPayment" runat="server" Font-Bold="true" Font-Size="X-Large"> </asp:Label>
                </div>
            </div>


            <div class="row pt-2">
                <div class="col-10 text-center mx-auto">
                    <asp:LinkButton runat="server" Width="100%" CssClass="btn btn-info text-decoration-none" Font-Size="Larger" 
                        ForeColor="White" ID="btnStepPaymentList"
                        OnClick="btnStepPaymentList_Click">
                        <asp:Label runat="server" Font-Size="X-Large">สาขาโอนชำระเงินใบคุม</asp:Label>
                    </asp:LinkButton>
                </div>
            </div>
                   <div class="row pt-2">
                <div class="col-10 text-center mx-auto">
                    <asp:LinkButton runat="server" Width="100%" CssClass="btn btn-info text-decoration-none" 
                        Font-Size="Larger" ForeColor="White" ID="btnDriverSendMoney"
                        OnClick="btnDriverSendMoney_Click">
                        <asp:Label runat="server" Font-Size="X-Large">คนรถส่งเงิน</asp:Label>
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
