﻿<%@ Page Title="" Language="C#" MasterPageFile="~/POS/SiteB.Master" AutoEventWireup="true" Async="true" CodeBehind="ChangeDate.aspx.cs" Inherits="Robot.POS.ChangeDate" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />

    <script>
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
    </script>

    <div class="row pt-2 kanit">
        <div class="col-xl-6 col-lg-8 col-md-12 col-sm-12 col-12 mx-auto">
            <div class="row">
                         <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mx-auto">
                    <dx:ASPxCalendar ID="dtProductNeedDate" Width="100%" Theme="Material" runat="server" />
                    <%--<dx:ASPxDateEdit ID="dtpayduedate" DisplayFormatString="dd-MM-yyyy" Theme="Material"
                        EditFormatString="dd-MM-yyyy" runat="server" CssClass="kanit" Width="100%">
                    </dx:ASPxDateEdit>--%>
                </div>
            </div>
            <div class="row pt-3 text-center">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 mx-auto">
                    <asp:LinkButton ID="btnOK" Width="100%" runat="server"
                        CssClass="btn btn-success py-2" Font-Size="Small"
                        OnClick="btnOK_Click">
                    <i class="fas fa-check-circle fa-lg"></i> ตกลง             
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </div>

    <div class="row pt-2 pb-2">
        <div class="col-md-11 mx-auto">
            <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                <strong>
                    <span id="myalertHead" class="kanit"></span></strong>
                <br />
                <span id="myalertBody" class="kanit"></span>
                <asp:Literal ID="lblMsgBody" runat="server"></asp:Literal>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        </div>
    </div>

</asp:Content>