<%@ Page Title="เลือกสาขา" Language="C#" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSSaleNewDoc.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSSaleNewDoc" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
 

  

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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row pt-5 pb-5" runat="server" id="divmain">
        <div class="col-lg-5 col-md-8 col-sm-12 col-12 mx-auto">
            <div class="card">
                <div class="card-header">
                    <div class="row  pt-3 pr-4">
                        <div class="col-md-12 pl-4 pt-1 ">
                            <div class="row">
                                <asp:Label CssClass="col-md-4 col-sm-6 col-6 pt-1 text-right" runat="server">เลือกสาขา &nbsp</asp:Label>
                                <asp:DropDownList ID="cboCompany" runat="server" CssClass="col-md-8 col-sm-6 col-6 form-control form-control-sm border-primary " DataTextField="Name" DataValueField="CompanyID"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row pt-1 pr-4 ">
                        <div class="col-md-12 pl-4">
                            <div class="row">
                                <asp:Label CssClass="col-md-4 col-sm-6 col-6 pt-1 text-right" runat="server">เลือกวันที่ &nbsp</asp:Label>
                                <dx:ASPxDateEdit ID="dtInvDate" CssClass="col-md-8 col-sm-6 col-6 border-primary " runat="server"
                                    Theme="Material" Width="100%"
                                    DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                    <TimeSectionProperties Visible="False">
                                        <TimeEditCellStyle HorizontalAlign="Right">
                                        </TimeEditCellStyle>
                                    </TimeSectionProperties>
                                </dx:ASPxDateEdit>
                            </div>
                         
                        </div>
                    </div>
                 
                    <div class="row pt-2  pb-3">
                        <div class="col-md-12 pl-4 pt-1 ">
                            <div class="row">
                          
                                <div class="col-md-12 pt-1 text-right">
                                      
                                    <asp:LinkButton ID="btnsaveto" runat="server" CssClass="btn btn-success" OnClick="btnsaveto_Click">
                                        <i class="far fa-file-invoice-dollar"></i>&nbsp<span >เข้าระบบขาย</span> 
                                    </asp:LinkButton>&nbsp   
                                       <asp:LinkButton ID="btnList" runat="server" Text="" CssClass="btn btn-dark" OnClick="btnList_Click">
                                      <i class="far fa-scroll"></i>
                                        <span >ประวัติขาย </span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                
                    </div>
                    
            <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                <hr />
                <strong>
                    <span id="myalertHead" ></span></strong>
                <br />
                <span id="myalertBody" ></span>
                        <asp:Label ID="lblAlertBody" runat="server" ForeColor="Red" ></asp:Label>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
