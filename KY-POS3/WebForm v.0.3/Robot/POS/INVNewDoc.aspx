<%@ Page Title="Create new Invoice" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="INVNewDoc.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.INVNewDoc" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddpopup" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

    <asp:HiddenField ID="hddParentPage" runat="server" />

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

    <asp:UpdatePanel ID="udpAlert" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="400" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
                HeaderText="Infomation" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsg"  runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg"  runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="OK" OnClientClick="return OnClosePopupAlert();" />
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  " runat="server" Text="CANCEL" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>
                                    </div>
                                    <div>&nbsp</div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>--%>
    </asp:UpdatePanel>

    <div class="row pt-1 pb-1" runat="server" id="div1">
        <div class="col-md-10 mx-auto">
            <div class="card" style="background-color:#9E9E9E;">
                <div class="card-body">
                    <div class="row pt-1 pb-1 ">
                        <div class="col-md-2 "> 
                            <asp:LinkButton ID="btnback" CssClass="btn btn-default" ForeColor="White" runat="server"
                                OnClick="btnback_Click">
                                   <span >   <i class="fas fa-reply-all fa-2x"></i> </span> 
                                        <span style="font-size:large">  กลับ   </span>
                            </asp:LinkButton>

                        </div>
                        <div class="col-md-10 text-right">
                            <h2 style="color:white;">สร้างเอกสาร  
                            </h2>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-10 mx-auto">
            <asp:ListView ID="grdlist"
                KeyFieldName="DocTypeID"
                OnPagePropertiesChanging="grdlist_PagePropertiesChanging"
                OnItemDataBound="grdlist_ItemDataBound"
                OnItemCommand="grdlist_ItemCommand"
                runat="server">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    <div class="row" runat="server" visible="false">
                        <div class="col-md-12">
                            <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdlist" PageSize="30">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField NumericButtonCssClass="accordion"></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="row pb-2 ">
                        <div class="col-md-12">
                            <div class="card" style="background-color:#f5f5f5;">
                                <div class="card-body">
                                    <asp:LinkButton runat="server" CommandName="selrow" CommandArgument='<%# Eval("DocTypeID")%>' class="btn p-0" ID="btnselect" Width="100%">
                                        <div class="row">
                                            <div class="col-md-12 text-left">
                                                <div class="row">
                                                    <div class="col-md-1 text-center">
                                                        <span style="color: limegreen; font-size: large">
                                                        <asp:Literal ID="lblstatus" runat="server"></asp:Literal></span>
                                                    </div>
                                                    <div class="col-md-11">                                                                                                                             
                                                        <span style="color: black; font-size: x-large"> <%# Eval("Name")%> </span><br />
                                                        <span style="color: gray; font-size: large"> <%# Eval("Remark")%> </span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="row ">
                        <div class="col-md-12">
                            ไม่พบข้อมูล...
                        </div>
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
    
     
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
