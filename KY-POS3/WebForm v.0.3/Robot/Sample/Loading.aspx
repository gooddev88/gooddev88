<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loading.aspx.cs" Inherits="Robot.Sample.Loading" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   
</head>
<body>
    <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--begin popup--%>
        <script type="text/javascript">

</script>
        <%--end popup--%>
       
  <style type="text/css">
  

        .modalloading {
            position: fixed;
            z-index: 999;
            height: 100%;
            width: 100%;
            top: 0;
            background-color: Black;
            filter: alpha(opacity=60);
            opacity: 0.6;
            -moz-opacity: 0.8;
        }

        .center {
            z-index: 1000;
            margin: 300px auto;
            padding: 10px;
            width: 130px;
            background-color: White;
            border-radius: 10px;
            filter: alpha(opacity=100);
            opacity: 1;
            -moz-opacity: 1;
        }

            .center img {
                height: 128px;
                width: 128px;
            }
    </style>

        <asp:UpdateProgress ID="udpPOK" runat="server" AssociatedUpdatePanelID="udpOK">
            <ProgressTemplate> 
                <div class="modalloading">
                    <div class="center">
                      
                             <img src="../Image/Little/working.gif" />
                    </div>
               
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="udpOK" runat="server">
            <ContentTemplate>  
                  <div   >
             
                    <h1>Click the button to see the UpdateProgress!</h1>
                    <asp:Button ID="Button1" Text="Submit" runat="server" OnClick="Button1_Click" />
                    <asp:Label ID="lblR" runat="server" Text="Label"></asp:Label> 
                </div> 
            </ContentTemplate> 
        </asp:UpdatePanel> 
    </form>
</body>
</html>
