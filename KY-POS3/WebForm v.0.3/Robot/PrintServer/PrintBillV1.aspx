<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintBillV1.aspx.cs" Inherits="Robot.PrintServer.PrintBillV1" %>

<%@ Register assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>

        <script src="../Asset/js/ga1.js"></script>
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
    <form id="form1" runat="server">
         <div class="row">
                        <div class="col-12">
                              <dx:ASPxWebDocumentViewer ID="docviewer" runat="server"   Width="100%"  DisableHttpHandlerValidation="False">
        </dx:ASPxWebDocumentViewer>
                        </div>
                    </div>
    
    </form>
</body>
</html>
