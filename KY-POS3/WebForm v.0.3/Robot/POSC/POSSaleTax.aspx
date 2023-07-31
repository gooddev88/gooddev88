<%@ Page Title="POS Tax invoice" Language="C#" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSSaleTax.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSSaleTax" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
   

    <style>
        .size-button {
            padding-left: 40px;
            padding-right: 40px;
            padding-bottom: 10px;
            padding-top: 10px;
            border-radius: 5px;
        }

        .size-button-save {
            padding-left: 85px;
            padding-right: 85px;
            padding-bottom: 12px;
            padding-top: 12px;
            border-radius: 5px;
        }        

        .width-button-ok {
            padding-left: 125px;
            padding-right: 125px;
        }

        .btn-circle.btn-md { 
            width: 55px;
            height: 54px;
            padding: 12px 9px;
            border-radius: 28px;
            font-size: 19px;
            text-align: center;
        } 
    </style>

    
    <!-- Print slip via RawBT -->
    <script>

        function sendUrlToPrint(url) {
            var beforeUrl = 'intent:';
            var afterUrl = '#Intent;';
            // Intent call with component
            afterUrl += 'component=ru.a402d.rawbtprinter.activity.PrintDownloadActivity;'
            afterUrl += 'package=ru.a402d.rawbtprinter;end;';
            document.location = beforeUrl + encodeURI(url) + afterUrl;
            return false;
        }
        // jQuery: set onclick hook for css class print-file
        $(document).ready(function () {
            $('.print-file').click(function () {
                return sendUrlToPrint($(this).attr('href'));
            });
        });
    </script>

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

        //begin print pdf
         function printPDF(url) { 
            popupwindow(url,'print','7000','500'); 
        }

        function popupwindow(url, title, w, h) {
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var w = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            w.focus();
            w.print();
        }
        //end print dpf
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
       <dx:ASPxPopupControl ID="popPrint" runat="server" 
        CloseAction="OuterMouseClick" 
        ShowMaximizeButton="true" 
        ShowCloseButton="true" 
        CloseOnEscape="true" 
        Modal="True"
        Theme="Material"
        Width="800"
        Height="600"
        PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter"
        ClientInstanceName="popPrint"
        HeaderText="Print" 
        AllowDragging="True"
        PopupAnimationType="None"
        EnableViewState="False"
        AutoUpdatePosition="true">
    </dx:ASPxPopupControl>

    <div class="row pt-2" runat="server" id="divmain">
     
        <div class="col-lg-8 col-md-10 mx-auto">
            <div class="card">
                <div class="card-header">
                    <div class="row ">
                        <div class="col-md-6 text-left ">                            
                       <asp:LinkButton ID="btnBack" runat="server" Text="" CssClass="btn btn-defualt" OnClick="btnBack_Click">
                <i class="fas fa-step-backward"></i>
                <span >กลับ </span>
            </asp:LinkButton>
                        </div>                                                                       
                    </div>
                    
                </div> 
                <div class="card-body">
                     <div class="row ">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                            <div class="row">
                                <asp:label CssClass="col-md-3 pt-1 text-right" runat="server">ชื่อลูกค้า &nbsp</asp:label>
                                <asp:TextBox ID="txtCustomerName" CssClass="col-md-8 form-control " runat="server"></asp:TextBox>                                                   
                            </div>
                        </div>                                               
                    </div>

                    <div class="row ">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                            <div class="row">
                                <asp:label CssClass="col-md-3 pt-1 text-right" runat="server">สาขา &nbsp</asp:label>
                                <asp:TextBox ID="txtCustBranchName" CssClass="col-md-8 form-control " runat="server" ></asp:TextBox>                                                   
                            </div>
                        </div>                                               
                    </div>

                    <div class="row ">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                            <div class="row">
                                <asp:label CssClass="col-md-3 pt-1 text-right" runat="server">ที่อยู่ 1 &nbsp</asp:label>
                                <asp:TextBox ID="txtBillAddr1" CssClass="col-md-8 form-control " runat="server"></asp:TextBox>                                                   
                            </div>
                        </div>                                               
                    </div>

                    <div class="row ">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                            <div class="row">
                                <asp:label CssClass="col-md-3 pt-1 text-right" runat="server">ที่อยู่ 2 &nbsp</asp:label>
                                <asp:TextBox ID="txtBillAddr2" CssClass="col-md-8 form-control " runat="server"></asp:TextBox>                                                   
                            </div>
                        </div>                                               
                    </div>

                    <div class="row ">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                            <div class="row">
                                <asp:label CssClass="col-md-3 pt-1 text-right" runat="server">เลขผู้เสียภาษี &nbsp</asp:label>
                                <asp:TextBox ID="txtCustTaxID" CssClass="col-md-8 form-control " runat="server" onkeypress="return DigitOnly(this,event)"></asp:TextBox>                                                   
                            </div>
                        </div>                                               
                    </div>

                    <div class="row pt-2  ">
                        <div class="col-md-12 pl-4 pt-1 ">
                            <div class="row">
                          <div class="col-md-8 mx-auto text-center">
                                <asp:LinkButton ID="btnsave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click">
                                    <i class="fas fa-check-circle"></i>&nbsp<span >พิมพ์ใบกำกับ</span> 
                                </asp:LinkButton> 
 
                            </div>                            
                            </div>
                        </div>                                               
                    </div>
                    <div class="row">
                        <div class="col-md-12 mr-auto pl-4 pt-1 ">
                                <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                                <hr />
                                <strong>
                                    <span id="myalertHead" ></span></strong>
                                <br />
                                <span id="myalertBody" ></span>
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>

    </div>   

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
