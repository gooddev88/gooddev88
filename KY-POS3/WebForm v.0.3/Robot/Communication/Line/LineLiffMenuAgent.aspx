﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineLiffMenuAgent.aspx.cs" Inherits="Robot.Communication.Line.LineLiffMenuAgent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
      <script>
        var vConsole = new VConsole()
        console.log("Hello World!")
  </script>

    <script src="https://static.line-scdn.net/liff/edge/versions/2.2.0/sdk.js"></script>
    <script>
        function createUniversalLink() {
            const link1 = liff.permanentLink.createUrl()
            document.getElementById("universalLink1").append(link1)

            liff.permanentLink.setExtraQueryParam('param=9')
            const link2 = liff.permanentLink.createUrl()
            document.getElementById("universalLink2").append(link2)
        }

        async function shareMsg() {
            await liff.shareTargetPicker([
                {
                    type: "text",
                    text: "This message was sent by ShareTargetPicker"
                }
            ])
        }

        function logOut() {
            liff.logout()
            window.location.reload()
        }

        function closed() {
            liff.closeWindow()
        }

        async function scanCode() {
            const result = await liff.scanCode()
            document.getElementById("scanCode").append(result.value)
        }

        function openWindow() {
            liff.openWindow({
               // url: "http://modernhrdev.gooddev.net/LineHelper/LineMenuAgent?id=" + profile.userId,
                url: "../../Communication/Line/LineMenuAgent?id=" + profile.userId,
                external: true
            })
        }

        async function getFriendship() {
            const friend = await liff.getFriendship()
            document.getElementById("friendship").append(friend.friendFlag)
            if (!friend.friendFlag) {
                if (confirm("คุณยังไม่ได้เพิ่ม Bot เป็นเพื่อน จะเพิ่มเลยไหม?")) {
                    window.location = "https://line.me/R/ti/p/@YOUR-BOT-ID"
                }
            }
        }

        async function sendMsg() {
            if (liff.getContext().type !== "none") {
                await liff.sendMessages([
                    {
                        "type": "sticker",
                        "stickerId": 1,
                        "packageId": 1
                    },
                    {
                        "type": "text",
                        "text": "hello kitty"
                    }
                ])
                alert("Message sent")
            }
        }

        function getContext() {
            if (liff.getContext() != null) {
                document.getElementById("type").append(liff.getContext().type)
                document.getElementById("viewType").append(liff.getContext().viewType)
                document.getElementById("utouId").append(liff.getContext().utouId)
                document.getElementById("roomId").append(liff.getContext().roomId)
                document.getElementById("groupId").append(liff.getContext().groupId)
            }
        }

        async function getUserProfile() {
            const profile = await liff.getProfile()
                
         
          //  window.location.replace("http://modernhrdev.gooddev.net/LineHelper/Test?lineid=" + profile.userId);
       
            window.location.replace("../../Communication/Line/LineMenuAgent?id=" + profile.userId +"&photo="+profile.pictureUrl);
         
            
        }

        function getEnvironment() {
            document.getElementById("os").append(liff.getOS())
            document.getElementById("language").append(liff.getLanguage())
            document.getElementById("version").append(liff.getVersion())
            document.getElementById("accessToken").append(liff.getAccessToken())
            document.getElementById("isInClient").append(liff.isInClient())
            if (liff.isInClient()) {
                document.getElementById("btnLogOut").style.display = "none"
            } else {
                document.getElementById("btnMsg").style.display = "none"
                document.getElementById("btnScanCode").style.display = "none"
                document.getElementById("btnClose").style.display = "none"
            }
        }

        async function main() {
            liff.ready.then(() => { 
                if (liff.isLoggedIn()) { 
                    getUserProfile() 
                } else {
                    liff.login()
                }
            })
            await liff.init({ liffId: "1656353977-nKD7JVOX" })//possy
        }
        main()
    </script>
        <style>
            .centered {
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
}
        </style>
    <div class="row">

        <div class="col-md-8 mx-auto centered">
            <asp:Image ID="Image1" runat="server" ImageUrl="../../Image/Little/loading1.gif" />
         
            <asp:Label runat="server" Font-Bold="true" Font-Size="50px" ID="lblMsg" Visible="false"> </asp:Label>

        </div>
    </div>
    </form>
</body>
</html>