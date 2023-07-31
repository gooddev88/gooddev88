﻿<%@ Page Title="Upload File" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="UploadFileItemPrice.aspx.cs" Inherits="Robot.Master.UploadFileItemPrice" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hdddocFrom" runat="server" />
    <asp:HiddenField ID="hddsource" runat="server" />

    <%--begin of Loading callback script--%>
    <script>
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

    <div class="row pb-1">
        <div class="col-md-12">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header pt-1 pb-1">
                            <div class="row">
                                <div class="col">
                                    <asp:LinkButton ID="btnBackList"
                                        CssClass="btn btn-default"
                                        runat="server"
                                        OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-4 pt-2 text-center">
                                    <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ID="lblHeaderCaption"></asp:Label>
                                </div>
                                <div class="col text-right">
                                </div>
                            </div>
                        </div>

                        <div class="card-body pt-2 pb-2">
                            <asp:UpdatePanel ID="upitemprice" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="row pt-1 ">
                                        <div class="col-md-12">
                                            <asp:Label runat="server">Choose excel file &nbsp</asp:Label>
                                            <div class="form-group">
                                                <div class="input-group">
                                                    <asp:FileUpload ID="fileuploadx" runat="server" CssClass="border" />
                                                    <label id="filename"></label>
                                                    <asp:LinkButton ID="btnUpload" Font-Bold="true" runat="server" CssClass="btn btn-warning rounded-right" OnClick="btnUpload_Click">
                                                                                 <i class="fas fa-upload"></i>&nbsp<span >อัพโหลด</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row ">
        <div class="col-md-12">
            <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                <strong>
                    <span id="myalertHead" ></span></strong>
                <br />
                <span id="myalertBody" ></span>
                <asp:Literal ID="lblAlertBody" runat="server"></asp:Literal>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <br />
                <asp:Label ID="lblError" ForeColor="Black" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </div>



</asp:Content>