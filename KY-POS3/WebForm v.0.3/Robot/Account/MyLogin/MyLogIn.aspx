<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyLogIn.aspx.cs" Inherits="Robot.Account.MyLogin.MyLogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--===============================================================================================-->
    <link rel="icon" type="image/png" href="images/icons/favicon.ico" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/bootstrap/css/bootstrap.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="fonts/iconic/css/material-design-iconic-font.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/animate/animate.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/css-hamburgers/hamburgers.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/animsition/css/animsition.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/select2/select2.min.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="vendor/daterangepicker/daterangepicker.css" />
    <!--===============================================================================================-->
    <link rel="stylesheet" type="text/css" href="css/util.css" />
    <link rel="stylesheet" type="text/css" href="css/main.css" />
    <!--===============================================================================================-->
</head>
<body>

    <script type="text/javascript">
        function RefreshIt(selectObj) {
            __doPostBack('<%= Page.ClientID %>', selectObj.name);
        }
    </script>

    <div class="limiter">
         <div class="container-login100" style="background-color:gold" <%--style="background-image: url('../../Image/BG/bubble.svg')"--%>;>
             <div class="wrap-login100 p-l-55 p-r-55 p-t-40 p-b-40" style="background-color:white;">
                <form class="login100-form validate-form" id="form1" runat="server">
              <asp:HiddenField ID="hdduser" runat="server" />
                    <asp:HiddenField ID="hddid" runat="server" />
                    <asp:HiddenField ID="hddpagename" runat="server" />
                    <asp:HiddenField ID="hddmenu" runat="server" />
                    <asp:HiddenField ID="hddmode" runat="server" />
                    <div class="text-center pb-1">
                        <asp:Image runat="server" ID="imgLogo" Width="200px"   />
                            <%--<asp:Image runat="server" Width="200px" ImageUrl="../../Image/Logo/somtum_change_face_logo.png" />--%>
                    </div>
                     
                    <div class="wrap-input100 validate-input m-b-23" data-validate="Username is reauired">
                        <span class="label-input100">Username</span>
                        <input runat="server" style="color:black;" class="input100" type="text" name="txtusername" id="txtusername" placeholder="Type your username" />
                        <span class="focus-input100" data-symbol="&#xf206;"></span>
                    </div>

                    <div class="wrap-input100 validate-input" data-validate="Password is required">
                        <span class="label-input100">Password</span>
                        <input runat="server" style="color:black;" class="input100" type="password" id="txtpassword" name="txtpassword" placeholder="Type your password" />
                        <span class="focus-input100" data-symbol="&#xf190;"></span>
                    </div>
                    
                  
                    <div class="text-right p-t-8">
                        
                        <asp:LinkButton ID="btnChangePassword" runat="server" OnClick="btnChangePassword_Click">Change password?</asp:LinkButton>
                    </div>
                      
          
                    <asp:Label ID="lbluser" Visible="false" runat="server" Text="Label"></asp:Label>

                      <div class="container-login100-form-btn">
                        <div class="wrap-login100-form-btn">
                            <div class="login100-form-bgbtn"></div>
                            <button class="login100-form-btn" id="btnlogin" name="btnlogin" onclick="javascript:RefreshIt(this);">
                                Login</button>
                        </div>
                            <div class="text-center p-t-8 p-b-31" runat="server" id="divLoginInfo">
                        <asp:LinkButton ID="lnkLoginInfo" ForeColor="Red" runat="server">Username or password Incurrect</asp:LinkButton>
                    </div>
                                       	<div class="text-right p-t-8 p-b-31">
                                               <br />						
                        <asp:LinkButton ID="btnDownLoad" runat="server" OnClick="btnDownLoad_Click">Download?</asp:LinkButton>
					</div>
                    </div>

                 
                </form>
            </div>
        </div>
    </div>


    <div id="dropDownSelect1"></div>

    <!--===============================================================================================-->
    <script src="vendor/jquery/jquery-3.2.1.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/animsition/js/animsition.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/bootstrap/js/popper.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/select2/select2.min.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/daterangepicker/moment.min.js"></script>
    <script src="vendor/daterangepicker/daterangepicker.js"></script>
    <!--===============================================================================================-->
    <script src="vendor/countdowntime/countdowntime.js"></script>
    <!--===============================================================================================-->
    <script src="js/main.js"></script>


</body>
</html>
