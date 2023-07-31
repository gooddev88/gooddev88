<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyQrScan.aspx.cs" Inherits="Robot.Feature.MyQrScan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QR Scan</title>
  
    <link href="Asset/qr.min.css" rel="stylesheet" />

        <script>qrstuffAppData = null;</script>
        <script>document.docReady = function (f) {
                if (document.readyState === 'complete')
                    f();
                else
                    document.addEventListener('DOMContentLoaded', f);
            }</script>
        <script src="Asset/qr.min.js" defer></script>

        <script>document.docReady(function () {
                var setViewport = function () {
                    document.getElementById('meta-viewport').setAttribute('content', 'width=' + Math.max(screen.width, 700));
                }
                window.addEventListener("orientationchange", setViewport);
                window.addEventListener("resize", _.debounce(setViewport, 200));
                setViewport();
            })</script>

        <style> .constrain, .content-width { min-width: 700px; max-width: 950px; }</style>


        <script type="text/javascript" src="https://cdn.qrstuff.com/include/modules/custom/qrcodescan/instascan/custom/instascan.min.js"></script>

        <script src="https://cdn.qrstuff.com/build/scripts/scan-d83e092ab6.min.js"></script>
  
      <script src="Asset/main.js" async></script>
</head>
<body >
   <form id="form1" runat="server">
           <asp:HiddenField ID="hddid" runat="server" />
        <asp:HiddenField ID="hddmenu" runat="server" />
        <asp:HiddenField ID="hddPreviouspage" runat="server" />
        <asp:HiddenField ID="hddFileType" runat="server" />
            <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-dark btn-lg" OnClick="btnBack_Click">
                                        <i class="fa fa-time"></i>&nbsp<span class="kanit">BACk</span> 
                                        </asp:LinkButton> 
        <div class="select">
            <label for="videoSource">Video source: </label>
            <select id="videoSource"></select>
        </div>
     <div class="constrain constrain-scan top">
             <div class="qrcodescan">
                <div class="preview-wrap loading">
                    <video playsinline></video>
                </div>
                <div class="result">
                </div>


                <script class="result-template" type="x-tmpl-mustache">
                    <div>
                    <h2>Scan Result</h2>
                    <div class="result-content{{#scan.displayMultiline}} multiline{{/scan.displayMultiline}}">
                    {{#scan.isLink}}{{scan.linkIntro}} <a href="{{scan.link}}" target="_blank">{{&scan.html}}</a>{{/scan.isLink}}
                    {{^scan.isLink}}{{&scan.html}}{{/scan.isLink}}
                    </div>
                    <form class="ng"><button class="scan-button button">Scan Another QR Code</button></form>
                    </div>
                </script>


            </div>

        </div>
    </form>
   
</body>
</html>
