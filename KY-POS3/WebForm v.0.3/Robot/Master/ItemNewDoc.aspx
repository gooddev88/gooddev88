﻿<%@ Page Title="สร้างสินค้าและบริการ" Language="C#" MasterPageFile="~/Master/SiteB.Master" AutoEventWireup="true" CodeBehind="ItemNewDoc.aspx.cs" ClientIDMode="Static" Inherits="Robot.Master.ItemNewDoc" %>

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
                                                <asp:Label ID="lblHeaderMsg" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" runat="server" Text=""></asp:Label>
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


    <div class="row pt-3 pb-3" runat="server" id="divmain">
        <div class="col-lg-10 col-md-10 col-sm-12 col-12 mx-auto">
            <div class="row">
                <div class="col-md-12 ">
                    <asp:LinkButton ID="btnback" Width="100%" CssClass="btn btn-light" runat="server"
                        Height="60"
                        OnClick="btnback_Click">
                    <span ><i class="fas fa-arrow-circle-left fa-2x"></i>   </span>
                              
               <span  style="font-size:large" class="Sarabun">เลือกประเภทสินค้าและบริการ</span>
                          
                    </asp:LinkButton>
                </div>

            </div>
        </div>

    </div>

    <div class="row">
        <div class="col-md-6 mx-auto">
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div style="overflow-x: auto; width: 100%">
                        <dx:ASPxGridView ID="grddoctype" runat="server" Width="100%"
                            EnableTheming="True"
                            Theme="Moderno"
                            CssClass="Sarabun"
                            AutoGenerateColumns="False"
                            OnDataBinding="grddoctype_DataBinding"
                            KeyFieldName="ValueTXT"
                            KeyboardSupport="True"
                            OnRowCommand="grddoctype_RowCommand">

                            <SettingsPager Mode="ShowAllRecords">
                            </SettingsPager>

                            <Columns>
                                <dx:GridViewDataTextColumn Width="100%" CellStyle-HorizontalAlign="Left">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" Width="100%"
                                            CommandArgument='<%# Eval("ValueTXT") %>' CausesValidation="False"
                                            CommandName="Select" CssClass="btn btn-default">
                                            <div class="row text-left">
                                                <div class="col-12">
                                                     <i class="fas fa-check-circle fa-3x" style="color:red"></i>     
   <span style="color:black">
       <%# Eval("Description1") %>
                                                    </span>
                                                </div>
                                            </div>
                                                 
                                        </asp:LinkButton>

                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <%--        <dx:GridViewDataTextColumn Caption="ประเภทสินค้าและบริการ" FieldName="ValueTXT" Visible="false">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="รายละเอียด" FieldName="Description1" Width="100%">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />

                                </dx:GridViewDataTextColumn>--%>
                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>