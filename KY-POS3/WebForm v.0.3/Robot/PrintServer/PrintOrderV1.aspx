<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintOrderV1.aspx.cs" Inherits="Robot.PrintServer.PrintOrderV1" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/css/bootstrap.min.css" integrity="sha384-zCbKRCUGaJDkqS1kPbPd7TveP5iyJE0EjAuZQTgFLD2ylzuqKfdKlfG/eSrtxUkn" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.slim.min.js" integrity="sha384-DfXdz2htPH0lsSSs5nCTpuj/zy4C+OGpamoFVy38MVBnE+IbbVYUew+OrCXaRkfj" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-fQybjgWLrvvRgtW6bFlB7jaZrFsaBXjsOMm/tB9LTS58ONXgqbR9W8oWht/amnpF" crossorigin="anonymous"></script>


    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>



    <script src="../Asset/js/ga1.js"></script>

    <style>
        #divElement{
    position: absolute;
    width:100%;
/*    top: 50%;
    left: 50%;
    margin-top: -50px;
    margin-left: -50px; */
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
</head>
<body>
    <form id="form1" runat="server" >
            <asp:HiddenField ID="hdddevice" runat="server" />
        <div  id="divElement">
        <div class="row pt-3">
            <div class="col-12 text-center">
                
                <asp:Button ID="btnPrintOrder" CssClass="btn btn-primary" runat="server" Text="พิมพ์รายการอาหาร" OnClick="btnPrintOrder_Click" Visible="false" />
                <asp:Button ID="btnPrintBill" CssClass="btn btn-info" runat="server" Text="พิมพ์ใบเสร็จ" OnClick="btnPrintBill_Click" Visible="false" />
                <asp:Button ID="btnPrintInv" CssClass="btn btn-secondary" runat="server" Text="พิมพ์ใบกำกับ" OnClick="btnPrintInv_Click" Visible="false" />
            </div>
        </div>
            <div class="row" hidden="hidden">
                <div class="col-12">
                    <asp:Label runat="server" ID="lblUrl"  Font-Size="Smaller">
                        
                    </asp:Label>
                
                </div>
            </div>

        <div class="row">
            <div class="col-12">
                <dx:ASPxWebDocumentViewer ID="docviewer" runat="server" Width="100%" DisableHttpHandlerValidation="False">
                </dx:ASPxWebDocumentViewer>
            </div>
        </div>
            </div>

    </form>
</body>
</html>
