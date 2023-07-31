<%@ Page Title="" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="SuccessPage.aspx.cs" Inherits="Robot.Communication.Misc.SuccessPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
    <div class="row">
        <div class="col-md-8 mx-auto">
            <div class="card py-5" style="border-color: white;">
                <div class="card-body py-5" style="font-size: smaller;">
                    <div class="row text-center pb-1">
                        <div class="col-md-8 mx-auto">
                            <asp:Label runat="server" ID="lblMsg" Font-Bold="true" Font-Size="35px"></asp:Label>
                            <br />
                            <br />
                            <span style="color: #27ae60"><i class="fas fa-check-circle fa-10x" style="font-size: 16em;"></i></span>
                            <br />
                            <br />
                            <asp:Label runat="server" Font-Bold="true" Font-Size="18px">กดปิดที่มุมบนด้านบนค่ะ</asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
