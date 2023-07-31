<%@ Page Title="ยืนยันลงทะเบียน Line" Language="C#" MasterPageFile="~/Communication/SiteA.Master" AutoEventWireup="true" CodeBehind="LineLogInRequestDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.Communication.Line.LineLogInRequestDetail" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />

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
    </script>


    <div class="row pt-4">
        <div class="col-8 mx-auto">
            <div class="row text-center">
                <div class="col-12">
                    <h4>
                        <asp:Label ID="lblTopic" runat="server" Font-Size="Larger"> </asp:Label>
                    </h4>
                    <br />
                    <asp:Label ID="lblDescription" runat="server" Font-Size="Large"> </asp:Label>
                     <br />
                    <asp:Label ID="lblDescriptio2" runat="server" Font-Size="Small"> </asp:Label>
              
                </div>
            </div>
               <div class="row pt-2 ">
                <div class="col-md-12">
                    <span style="font: large">ฝากถึงแอดมินว่า...</span><br />
         <strong>  <asp:Literal ID="lblRequestMeomo" runat="server"></asp:Literal></strong>
                </div>
            </div>
            <hr />

            <div class="row pt-2 ">
                <div class="col-md-12">
                    <asp:Literal ID="lblUserInfo" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="row pt-2 ">
                <div class="col-md-12">
                    <span style="font: large">ฝากถึงผู้ขอว่า...</span>
                    <asp:TextBox ID="txtMomo"  TextMode="MultiLine" Height="50" CssClass="form-control " runat="server"></asp:TextBox>
                </div>
            </div>
     <div class="row text-center pt-1">
               <div class="col-12  ">
                    <asp:Button ID="btnBack"
                        CssClass="btn btn-default " ForeColor="#3366ff" Width="100" runat="server" Text="กลับ" OnClick="btnBack_Click"   />
                    <asp:Button ID="btnApprove"
                        CssClass="btn btn-default " ForeColor="#009933" Width="100" runat="server" Text="อนุมัติ"  OnClick="btnApprove_Click"/>
                    <asp:Button ID="btnReject"
                        CssClass="btn btn-default " ForeColor="#ff3399" Width="100" runat="server" Text="ปฏิเสธ"  OnClick="btnReject_Click"/>
                     
                </div>
         </div>
            <div class="row text-center ">
                <div class="col-12">
                    <asp:Label ID="lblAlertmsg" runat="server">  </asp:Label>
                </div> 
                 

            <hr />


            <div class="row pt-1">
                <div class="col-md-12">
                    <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                        <strong>
                            <span id="myalertHead"></span></strong>
                        <br />

                        <span id="myalertBody"></span>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>
            </div>

        </div>
    </div>



        </div>



    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
