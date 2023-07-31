<%@ Page Title="" Language="C#" MasterPageFile="~/POS/SiteA.Master" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="MyPrint.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.Print.MyPrint" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

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


    <asp:HiddenField ID="hddReportName" runat="server" />
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header pt-1 pb-1">
                    <div class="row">
                        <div class="col-md-10 kanit">
                            <i class="fa fa-backward" aria-hidden="true"></i>
                            <asp:LinkButton ID="btnBack" CssClass="btn btn-defult" runat="server" Font-Bold="true" OnClick="btnBack_Click">Back</asp:LinkButton>
                        </div>
                        <div class="col-md-2">
                        </div>
                    </div>
                </div>
                <div class="card-body pt-2">
                    <div class="row pb-2 pt-1">
                        <div class="col-md-12">
                            <div class="row" runat="server" id="divfilter">
                                <div class="col-md-3" runat="server" id="divdropdown">
                                    <span style="font-size:large;">เลือกผู้ขาย &nbsp</span>
                                    <dx:ASPxComboBox ID="cboVendor" runat="server"
                                        Theme="Material" DropDownStyle="DropDown"
                                        ValueField="VendorID"
                                        ValueType="System.String" TextFormatString="{0} {1}" Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="VendorID" Caption="ผู้ขาย" Width="150px" />
                                            <dx:ListBoxColumn FieldName="FullNameTh" Caption="ชื่อผู้ขาย" Width="250px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-2 pt-3">
                                    <asp:LinkButton ID="btnLoad" class="btn btn btn-success" runat="server" OnClick="btnLoad_Click"> 
                                        <i class="fas fa-search fa-lg"></i> &nbsp <span>Load</span>                                             
                                    </asp:LinkButton>
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <dx:ASPxWebDocumentViewer ID="docviewer" Width="100%" runat="server" DisableHttpHandlerValidation="False">
                            </dx:ASPxWebDocumentViewer>
                        </div>
                    </div>
                </div>
                <br />
            </div>

        </div>
    </div>



</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
