<%@ Page Title="UploadDocument" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POSA1.Master" AutoEventWireup="true" CodeBehind="UploadDocument.aspx.cs" Inherits="Robot.UploadTemplate.UploadDocument" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddcompany" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
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
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };

        function OnBeginCallback(s, e) {
            LoadingPanel.Show();
        };

        function OnEndCallback(s, e) {
            LoadingPanel.Hide();
        };
    </script>


    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="LoadingPanel"
        Theme="Material"
        runat="server"
        Modal="true"
        HorizontalAlign="Center"
        VerticalAlign="Middle">
    </dx:ASPxLoadingPanel>

    <asp:UpdatePanel ID="udpPost" runat="server">
        <ContentTemplate>
            <div class="row pb-1">
                <div class="col-md-12">
                    <div class="row grid-margin">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-header pt-1 pb-1">
                                    <div class="row kanit">
                                        <div class="col-md-8">
                                            <ul class="nav nav-pills card-header-pills" role="tablist">
                                                <li class="nav-item">
                                                    <asp:LinkButton ID="btnBackList" Font-Size="Small" CssClass="nav-link active" runat="server" OnClick="btnBackList_Click" meta:resourcekey="btnBackListResource1"> 
                                                        <i class="fa fa-chevron-circle-left"></i>&nbsp<span class="kanit">Back</span> 
                                                    </asp:LinkButton>
                                                </li>
                                                <li class="nav-item" runat="server">
                                                    <a class="nav-link " id="a_tab_home"><span class="kanit"><i class="fa fa-filter"></i>&nbsp<%=hddTopic.Value %></span></a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body pt-2 pb-2">
                                    <div class="row kanit pb-3">
                                        <div class="col-md-4">
                                            <span>Upload Type &nbsp</span>
                                            <asp:DropDownList ID="cboUploadType" runat="server" CssClass="form-control form-control-sm" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                        </div>
                                    </div>
                                      <div class="row kanit pb-3">
                                        <div class="col-md-1 text-left">
                                            <asp:Button ID="btnNext" runat="server" Font-Bold="true" class="btn btn-warning" Text="Next" OnClick="btnNext_Click" />
                                        </div>
                                    </div>
                                                                 

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="row pt-3">
                <div class="col-md-12">
                    <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                        <strong>
                            <span id="myalertHead" class="kanit"></span></strong>
                        <br />
                        <span id="myalertBody" class="kanit"></span>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>
            </div>

</asp:Content>
