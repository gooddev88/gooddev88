<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewerSO102.aspx.cs" Inherits="PrintMaster.PrintFile.OMS.SO102.ViewerSO102" %>

<%@ Register assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Master</title>
</head>
<body>
    <form id="form1" runat="server"> 
        <div>
            <dx:ASPxWebDocumentViewer ID="docviewer" SettingsExport-ShowPrintNotificationDialog="false" runat="server"></dx:ASPxWebDocumentViewer>
        </div>
    </form>
</body>
</html>
