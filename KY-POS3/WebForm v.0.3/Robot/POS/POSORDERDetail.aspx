<%@ Page Title="ออเดอร์" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="POSORDERDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.POSORDERDetail" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <asp:HiddenField ID="hddmenu" runat="server" />
    <style>
        .btn-main {
            display: inline-block;
            font-weight: 400;
            color: #212529;
            text-align: center;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-color: transparent;
            border: 1px solid transparent;
            padding: .375rem .75rem;
            font-size: 1rem;
            line-height: 1.5;
            border-radius: .25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }

        .btn-10ac84 {
            color: #fff;
            background-color: #10ac84;
            border-color: #10ac84;
        }
    </style>

    <script type="text/javascript">

        //ddl load
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

        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";
        }

        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";
        }

        function mypopprofile() {
            btnPostProfile.DoClick();
        }
        //end show msg

        //popup show 
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }

        function OnClosePopupAlert() {
            popAlert.Hide();
        }

        function OnClosePopupDeleteAlert() {
            popDeleteAlert.Hide();
        }
    </script>

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

    <script> 
        function printPDF(url) {
            popupwindow(url, 'print', '7000', '500');
        }

        function popupwindow(url, title, w, h) {
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var w = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
            w.focus();
            w.print();
        }

    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpAlert" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="500" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
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
                                                <asp:Label ID="lblHeaderMsg" CssClass="" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" CssClass="" runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="ตกลง" OnClientClick="return OnClosePopupAlert();" />
                                            </div>
                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>

            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popDeleteAlert" runat="server" Width="600" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popDeleteAlert"
                HeaderText="Infomation" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="PanelDelete" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div class="card Sarabun">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsgDelete" CssClass="Sarabun" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsgDelete" CssClass="Sarabun" runat="server" Font-Size="Small" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:LinkButton ID="btnConfirmDel" runat="server" CssClass="btn btn-success" OnClick="btnConfirmDel_Click"> 
                                                    <i class="fas fa-check-circle fa-lg"></i> ตกลง
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnCancle" runat="server" CssClass="btn btn-danger" Text="" OnClientClick="return OnClosePopupDeleteAlert();"> 
                                                    <i class="fas fa-times-circle fa-lg"></i> ยกเลิก
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>

            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="udpmain" runat="server">
        <ContentTemplate>

            <dx:ASPxPopupControl ID="popPrint" runat="server"
                CloseAction="OuterMouseClick"
                ShowMaximizeButton="true"
                ShowCloseButton="true"
                CloseOnEscape="true"
                Modal="True"
                Theme="Material"
                Width="800"
                Height="600"
                PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter"
                ClientInstanceName="popPrint"
                HeaderText="Print"
                AllowDragging="True"
                PopupAnimationType="None"
                EnableViewState="False"
                AutoUpdatePosition="true">
            </dx:ASPxPopupControl>

            <div class="row ">
                <div class="col-xl-10 col-lg-11 col-md-11 mx-auto">
                    <div class="row pt-1 pb-2">
                        <div class="col-md-6">
                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">
                                <h4 style="font-weight: bold; color: black;">
                                    <span><i class="fas fa-reply fa-2x"></i></span>&nbsp  
                                  <asp:Label runat="server" ID="lblDocinfo"></asp:Label>
                                </h4>
                            </asp:LinkButton>

                        </div>
                        <div class="col-md-6 text-right pt-3">

                            <asp:Button ID="btnSave" Width="34%" Height="40"
                                CssClass="btn-main btn-10ac84"
                                OnClick="btnSave_Click"
                                OnClientClick="this.disabled='true';"
                                UseSubmitBehavior="false"
                                runat="server"
                                Text="บันทึก"></asp:Button>
                            <asp:LinkButton ID="btnPrintOrder" runat="server" Visible="false" CssClass="btn btn-secondary" Height="40" OnClick="btnPrintOrder_Click" Text=""> 
                                 พิมพ์ใบสั่งออเดอร์
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnPrintShipOrder" runat="server" Visible="false" CssClass="btn btn-secondary" Height="40" OnClick="PrintShipOrder_Click" Text=""> 
                                 พิมพ์ใบส่งสินค้า
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-danger" Height="40" OnClick="btnDel_Click" Text=""> 
                                <i  class="far fa-trash-alt fa-lg"></i> ลบเอกสาร
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="row pb-1" style="font-size: smaller;">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row  pb-1 pt-2">
                                        <div class="col-md-8">
                                            <asp:Label runat="server" Font-Size="X-Large" ID="lblOrderID"></asp:Label>&nbsp&nbsp<br />
                                            <asp:Label runat="server" ForeColor="Gray" Font-Size="Large" ID="lblOrderDate"></asp:Label><br />
                                            <asp:Label runat="server" ForeColor="Gray" Font-Size="Medium" ID="lblCompany"></asp:Label>

                                        </div>
                                        <div class="col-md-4 text-right">
                                            <h3>
                                                <asp:Literal ID="litStatus" runat="server"></asp:Literal></h3>
                                        </div>
                                    </div>

                                    <div class="pt-2 pb-1">
                                        <div class="col-md-6 pl-0">
                                            <asp:Label Font-Size="Medium" runat="server">ส่งไปศูนย์กระจายสินค้า </asp:Label>
                                            <asp:DropDownList ID="cboCompany" runat="server" CssClass="form-control form-control-sm" DataTextField="FullName" DataValueField="CompanyID"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtRemark1" Font-Size="Small" CssClass="form-control" Placeholder="หมายเหตุ" TextMode="MultiLine" Height="70px" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row pt-4">
                        <div class="col-md-12">
                            <h4 style="font-weight: bold; color: black;">รายการสินค้า</h4>
                        </div>
                    </div>

                    <div class="row pt-1">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">

                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <div class="row" id="divAddItem" runat="server">
                                                <div class="col-md-4">
                                                    <span style="color: gray;">สินค้า </span>
                                                    <dx:ASPxComboBox ID="cboItem" runat="server"
                                                        Theme="Material"
                                                        Font-Size="Larger"
                                                        EnableCallbackMode="true"
                                                        CallbackPageSize="10"
                                                        DropDownWidth="500"
                                                        DropDownHeight="300"
                                                        CssClass="Sarabun"
                                                        AutoPostBack="true"
                                                        ValueType="System.String" ValueField="ItemID"
                                                        OnSelectedIndexChanged="cboItem_SelectedIndexChanged"
                                                        OnItemsRequestedByFilterCondition="cboItem_OnItemsRequestedByFilterCondition_SQL"
                                                        OnItemRequestedByValue="cboItem_OnItemRequestedByValue_SQL" TextField="ItemID" TextFormatString="{0}-{2}-{3}"
                                                        Width="100%" DropDownStyle="DropDownList">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="ItemID" Caption="สินค้า" />
                                                            <dx:ListBoxColumn FieldName="TypeID" Caption="ประเภท" Width="70" />
                                                            <dx:ListBoxColumn FieldName="Name1" Caption="รายละเอียด" Width="200px" />
                                                            <dx:ListBoxColumn FieldName="UnitID" Caption="หน่วย" Width="70px" />

                                                        </Columns>
                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <span style="color: gray;">จำนวน </span>
                                                    <asp:TextBox ID="txtQty" CssClass="form-control text-right" Font-Size="Larger" Font-Bold="true" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-2">
                                                    <span style="color: gray;">หน่วย</span>
                                                    <asp:DropDownList ID="cboUnitConvert" runat="server" Font-Size="Larger" CssClass="form-control" DataTextField="UnitToDesc" DataValueField="ToUnit"></asp:DropDownList>
                                                </div>

                                                <div class="col-md-2 pt-4 text-left">
                                                    <asp:LinkButton ID="btnAddLine" Width="100%" runat="server" ForeColor="Black" CssClass="" OnClick="btnAddLine_Click">
                                                 <i class="fas fa-plus-circle fa-2x"></i>&nbsp<span style="font-size:large">เพิ่ม</span> 
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cboItem" />
                                            <asp:AsyncPostBackTrigger ControlID="btnAddLine" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                    <div class="row pt-5">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="udpgrd" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:ListView ID="grdline"
                                                        KeyFieldName="LineNum"
                                                        OnPagePropertiesChanging="grdline_PagePropertiesChanging"
                                                        OnItemDataBound="grdlist_ItemDataBound"
                                                        OnItemCommand="grdline_ItemCommand"
                                                        pagesize="500"
                                                        runat="server">
                                                        <LayoutTemplate>
                                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            <div class="row" runat="server" hidden="hidden">
                                                                <div class="col-md-12">
                                                                    <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdline" PageSize="1000">
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
                                                            <div class="col-md-12" style="display: none">
                                                                <asp:Label ID="lblLineNum" runat="server" Text='<%# Eval("LineNum") %>'></asp:Label>
                                                            </div>
                                                            <div class="row pb-1">
                                                                <div class="col-md-12">
                                                                    <div class="row ">
                                                                        <div class="col-md-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-8 col-md-8 col-sm-8 col-8 pt-2 text-left">
                                                                                    <span style="color: black; font-size: 22px;"><%# Eval("Name")%>
                                                                                        <%--&nbsp   <strong><%# Convert.ToDecimal(Eval("Price")).ToString("N0") %> </strong>บาท--%>
                                                                                    </span>&nbsp
                                                                                    สั่ง : <strong><%# Convert.ToDecimal(Eval("OrdQty")).ToString("N2") %> </strong>&nbsp <%# Eval("Unit")%><br />
                                                                                    <span style="color: gray; font-size: small;">   
                                                                                        ส่ง : <strong><%# Convert.ToDecimal(Eval("ShipQty")).ToString("N2") %> </strong>
                                                                                        รับ : <strong><%# Convert.ToDecimal(Eval("GrQty")).ToString("N2") %> </strong>
                                                                                    </span>
                                                                                    <br />
                                                                                    <span style="color: gray; font-size: small;">เหลือ :
                                                                                        <asp:TextBox ID="txtBalQty"
                                                                                            Font-Size="Medium"
                                                                                            Font-Bold="true"
                                                                                            ForeColor="DeepPink"
                                                                                            class="form-control-f form-control-sm" runat="server"
                                                                                            Text='<%#  Convert.ToDecimal( Eval("BalQtyOrd")).ToString("N2")%>'
                                                                                            Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                                        <%--<strong><%# Convert.ToDecimal(Eval("BalQty")).ToString("N2") %> </strong>--%>
                                                                                        &nbsp <%# Eval("Unit")%> &nbsp&nbsp&nbsp <asp:DropDownList ID="ddlUnitID" Font-Size="Small" Visible="false" runat="server" CssClass="form-control-sm" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>                                                                                                                                                                                                                                                   
                                                                                        <%--สั่งแล้ว : <strong><%# Convert.ToDecimal(Eval("OnOrdQty")).ToString("N0") %> </strong>--%>

                                                                                    </span>
                                                                                </div>
                                                                                <div class="col-lg-4 col-md-4 col-sm-4 col-4 text-right">
                                                                                    <div class="row pb-2 pt-1">
                                                                                        <div class="col-md-7 pr-0 pt-2">
                                                                                            <asp:Label CssClass="text-right" Font-Size="Large" ForeColor="Black" runat="server" ID="lblorder">สั่งซื้อ</asp:Label>
                                                                                            <asp:Label CssClass="text-right" Font-Size="Large" ForeColor="Black" runat="server" ID="lblShip">ส่งสินค้า</asp:Label>
                                                                                            <asp:Label CssClass="text-right" Font-Size="Large" ForeColor="Black" runat="server" ID="lblGrQty">รับสินค้า</asp:Label>
                                                                                        </div>
                                                                                        <div class="col-md-5">
                                                                                            <asp:TextBox ID="txtQty"
                                                                                                Font-Size="Large"
                                                                                                Font-Bold="true"
                                                                                                ForeColor="DeepPink"
                                                                                                class="form-control-f form-control-sm" runat="server" Placeholder="จำนวน"
                                                                                                Text='<%#  Convert.ToDecimal( Eval("OrdQty")).ToString("N2")%>'
                                                                                                Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>

                                                                                            <asp:TextBox ID="txtShipQty"
                                                                                                Font-Size="Large"
                                                                                                Font-Bold="true"
                                                                                                ForeColor="DeepPink"
                                                                                                class="form-control-f form-control-sm" runat="server" Placeholder="จำนวนส่ง"
                                                                                                Text='<%#  Convert.ToDecimal( Eval("ShipQty")).ToString("N2")%>'
                                                                                                Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>

                                                                                            <asp:TextBox ID="txtGrQty"
                                                                                                Font-Size="Large"
                                                                                                Font-Bold="true"
                                                                                                ForeColor="DeepPink"
                                                                                                class="form-control-f form-control-sm" runat="server" Placeholder="จำนวนรับ"
                                                                                                Text='<%#  Convert.ToDecimal( Eval("GrQty")).ToString("N2")%>'
                                                                                                Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>

                                                                                        </div>
                                                                                    </div>
                                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("LineNum") %>'
                                                                                        CausesValidation="False" CommandName="Del" Text="ลบ"> 
                                                                                    <i style="color:gray;" class="far fa-trash-alt fa-lg"></i>
                                                                                    </asp:LinkButton>
                                                                                </div>
                                                                            </div>
                                                                         
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="row py-3 ">
                                                                <div class="col-md-12 text-center">
                                                                    <asp:Label runat="server" Font-Size="" ForeColor="deeppink">... ไม่มีรายการสินค้า ...</asp:Label>
                                                                </div>
                                                            </div>

                                                        </EmptyDataTemplate>
                                                    </asp:ListView>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdline" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddLine" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnAddLine" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>


            </div>
            </div>
                        </div>
                    </div>





                    <div class="row pt-2" id="divRM" runat="server">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <h4 style="font-weight: bold; color: black;">รายการวัตถุดิบ</h4>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row pt-1">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">


                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <div style="overflow-x: auto; width: 100%">
                                                <dx:ASPxGridView ID="grdBomLine" runat="server"
                                                    ClientInstanceName="grdBomLine"
                                                    KeyFieldName="LineNum"
                                                    AutoGenerateColumns="False"
                                                    Width="100%"
                                                    CssClass="Sarabun"
                                                    OnDataBinding="grdBomLine_DataBinding"
                                                    EnableTheming="True"
                                                    KeyboardSupport="true"
                                                    Theme="Moderno">
                                                    <SettingsBehavior AllowSort="false" SortMode="Value" />
                                                    <Settings ShowGroupPanel="False"
                                                        ShowFooter="false" ShowGroupFooter="Hidden" />
                                                    <SettingsPager PageSize="70">
                                                    </SettingsPager>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="FgItemID" Width="150" Caption="FgItemID" Visible="false">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <CellStyle Wrap="False"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="FgName" Width="150" Caption="ราการสั่ง(FGK)">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <CellStyle Wrap="False"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="จำนวนสั่ง" FieldName="FgQty" Width="100px">
                                                            <HeaderStyle Wrap="False" />
                                                            <PropertiesTextEdit DisplayFormatString="n2" />
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="FgUnit" Width="150" Caption="หน่วยสั่ง">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <CellStyle Wrap="False"></CellStyle>
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="RmItemName" Width="100%" Caption="รายการซื้อ(RMK)">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <CellStyle Wrap="False"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="RmQty" Name="txtRmQty" Caption="จำนวนซื้อ" Width="180px">
                                                            <DataItemTemplate>
                                                                <dx:ASPxTextBox ID="txtRmQty" runat="server"
                                                                    Font-Bold="true" Width="180px"
                                                                    CssClass="form-control text-right"
                                                                    onkeypress="return DigitOnly(this,event)"
                                                                    Value='<%# Eval("RmQty","{0:n2}") %>'>
                                                                </dx:ASPxTextBox>
                                                            </DataItemTemplate>
                                                            <HeaderStyle Wrap="False" Font-Size="Small" />
                                                            <CellStyle Wrap="False" Font-Size="Small" HorizontalAlign="Right" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn Caption="จำนวนซื้อ" FieldName="OrdID" Width="100px">
                                                            <DataItemTemplate>
                                                                <asp:Label runat="server" Text='<%# Eval("RmQty","{0:n2}") %>'></asp:Label>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="RmUnit" Width="150" Caption="หน่วยซื้อ">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <CellStyle Wrap="False"></CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="มูลค่า" FieldName="RmAmt" Width="150px" Visible="false">
                                                            <HeaderStyle Wrap="False" />
                                                            <PropertiesTextEdit DisplayFormatString="n2" />
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="ราคาต่อหน่วย" FieldName="Price" Width="150px" Visible="false">
                                                            <HeaderStyle Wrap="False" />
                                                            <PropertiesTextEdit DisplayFormatString="n2" />
                                                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                        </dx:GridViewDataTextColumn>


                                                    </Columns>
                                                </dx:ASPxGridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            <asp:AsyncPostBackTrigger ControlID="grdline" />
            <asp:AsyncPostBackTrigger ControlID="btnAddLine" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
