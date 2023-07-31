<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Viewer.aspx.cs" Inherits="PrintMaster.Viewer.Leave.Viewer" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Print Master</title>
</head>
<body>
    <form id="form1" runat="server">
      <%--      <asp:HiddenField ID="hddid" runat="server" />--%>
        <div>
            <dx:ASPxWebDocumentViewer ID="docviewer" runat="server"></dx:ASPxWebDocumentViewer>
        </div>
    </form>
</body>
</html>
