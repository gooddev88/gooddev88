<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site2.Master" MaintainScrollPositionOnPostback="true" CodeBehind="ShowMessage.aspx.cs" Inherits="Robot.Feature.ShowMessage" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <asp:HiddenField ID="hddid" runat="server" />
    <div class="row">
        <div class="col-md-4">
            <asp:Image ID="imgErrorLogo" Visible="false" runat="server" ImageUrl="~/Images/A1/lock1.png" />
            <asp:Image ID="imgSuccesLogo" Visible="false" runat="server" ImageUrl="~/Images/A1/lock1.png" />
        </div>
        <div class="col-md-8">
            <asp:Label ID="lblMessage" runat="server" CssClass="kanit" Text="Label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <dx:ASPxButton ID="btnClose" runat="server" CssClass="btn btn-dark btn-lg" AutoPostBack="false" Text="Cancel"
                OnClick="btnClose_Click">
            </dx:ASPxButton>
        </div>
    </div>
    <div class="row" hidden="hidden">
        <div class="col-md-12">
            <dx:ASPxButton ID="btnOk" runat="server" AutoPostBack="false" Text="OK" OnClick="btnOk_Click"></dx:ASPxButton>

            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel"
                OnClick="btnCancel_Click">
            </dx:ASPxButton>
        </div>
    </div>


</asp:Content>

