<%@ Page Title="AR Receipt" Language="C#" MasterPageFile="~/POS/SiteA.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="RCDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.RCDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <asp:HiddenField ID="hddmenu" runat="server" />


    <style>
        .size-button {
            padding-left: 40px;
            padding-right: 40px;
            padding-bottom: 10px;
            padding-top: 10px;
            border-radius: 5px;
        }

        .size-button-save {
            padding-left: 85px;
            padding-right: 85px;
            padding-bottom: 12px;
            padding-top: 12px;
            border-radius: 5px;
        }

        .width-button-ok {
            padding-left: 125px;
            padding-right: 125px;
        }

        .btn-circle.btn-md {
            width: 55px;
            height: 54px;
            padding: 12px 9px;
            border-radius: 28px;
            font-size: 19px;
            text-align: center;
        }
    </style>

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

    <script type="text/javascript">
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%--    <asp:UpdatePanel ID="updShowFile" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popFile2" runat="server" AllowDragging="True" AllowResize="True" Theme="Material"
                CloseAction="OuterMouseClick" ShowCloseButton="true" ShowMaximizeButton="true"
                EnableViewState="False"
                PopupHorizontalAlign="Center"
                PopupVerticalAlign="Middle"
                ShowFooter="false"
                Maximized="True"
                ShowOnPageLoad="True"
                FooterText="my file"
                HeaderText="My files" ClientInstanceName="popFile" EnableHierarchyRecreation="True" FooterStyle-Wrap="True">
                <ContentStyle Paddings-Padding="0" />
                <ClientSideEvents Shown="onPopupShown" />
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>--%>



    <div class="row " runat="server" id="divmain">
        <div class="col-lg-11 col-md-12 col-sm-12 col-12 mx-auto">
            <div class="row pt-1  ">
                <div class="col-md-12 ">
                    <div class="card bg-light">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-7">
                                    <asp:LinkButton ID="btnBackList" Font-Size="Large"
                                        CssClass="btn btn-default" runat="server"
                                        OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>  
                                        <strong>
                                        <asp:Literal ID="lblInfo" runat="server"></asp:Literal>
                                    </strong>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-md-5 text-right">
                                    <div class="btn-group" role="group" runat="server">
                                        <button id="btnGroupDrop1" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <span>พิมพ์</span>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="btnGroupDrop1" runat="server">
                                            <asp:LinkButton ID="btnPrintORC1" runat="server" Width="240px" CssClass="btn btn-default text-left" OnClick="btnPrintORC1_Click">
                                    <i class="far fa-print"></i>&nbsp<span >พิมพ์ใบเสร็จรับเงินขายสินค้า</span> 
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnPrintORC2" runat="server" Width="240px" CssClass="btn btn-default text-left" OnClick="btnPrintORC2_Click">
                                    <i class="far fa-print"></i>&nbsp<span >พิมพ์ใบเสร็จรับเงินงานบริการ</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="btn-group" role="group">
                                        <button id="btnGroupDrop2" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <i class="fas fa-bars"></i>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                            <asp:LinkButton ID="btnDeleteRV" runat="server" Text="" CssClass="dropdown-item " OnClick="btnDeleteRV_Click">
                                <i class="far fa-trash-alt"></i>
                                <span >ลบ </span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                </div>



                            </div>
                        </div>

                    </div>
                </div>
            </div>


            <div class="row pb-1 ">
                <div class="col-md-12  pt-1 ">
                    <div class="card">
                        <div class="card-header p-2" style="background-color: rgba(0,0,0,.06);">
                            <div class="row ">
                                <div class="col-md-8  ">
                                    <i class="far fa-clipboard-check"></i>
                                    &nbsp<asp:Label ID="lblSumInvPayAmt" runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Green"> </asp:Label>
                                </div>
                                <div class="col-md-4 text-right">
                                    <asp:LinkButton ID="btnselectinv" CssClass="btn btn-secondary btn-sm" runat="server" OnClick="btnselectinv_Click">   
                                                <i class="fas fa-check-double"></i>&nbsp<span >เลือกอินวอยซ์</span> 
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <dx:ASPxPopupControl ID="popFile1" runat="server" AllowDragging="True" AllowResize="True"
                            CloseAction="CloseButton"
                            ShowMaximizeButton="true"
                            EnableViewState="False"
                            PopupHorizontalAlign="WindowCenter"
                            PopupVerticalAlign="WindowCenter"
                            ShowCloseButton="false"
                            ShowFooter="True"
                            ShowOnPageLoad="True"
                            Maximized="True"
                            HeaderText="" ClientInstanceName="popFile"
                            EnableHierarchyRecreation="True"
                            FooterStyle-Wrap="True" CloseOnEscape="True" Modal="True" Theme="Mulberry">
                            <ContentStyle Paddings-Padding="0" />
                            <ClientSideEvents Shown="onPopupShown" />
                        </dx:ASPxPopupControl>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-6 pr-1 pl-1">
                                    <div class="form-group">
                                        <asp:ListView ID="grdInvline" runat="server" OnItemCommand="grdInvline_ItemCommand">
                                            <EmptyDataTemplate>
                                                <div style="text-align: center"><span style="color: darkgray">There are no invoice selected.</span>  </div>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <div class="col-md-0" style="display: none">
                                                    <asp:Label ID="lblLineNum" runat="server" Text='<%# Eval("LineNum") %>'></asp:Label>
                                                    <asp:Label ID="lblRCID" runat="server" Text='<%# Eval("RCID") %>'></asp:Label>
                                                </div>
                                                <div class="row pt-1">
                                                    <div class="col-md-12">
                                                        <div class="card">
                                                            <div class="card-header">
                                                                <div class="row pb-0">
                                                                    <div class="col-md-6">
                                                                        <div class="row pt-0">
                                                                            <div class="col-md-12">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="width: 300px; text-align: left">
                                                                                            <span style="font-size: large; color: crimson">
                                                                                                <asp:Label ID="lblINV_ID" Font-Bold="true" runat="server" Text='<%# Eval("INVID") %>'></asp:Label>
                                                                                            </span>
                                                                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                                                                CommandArgument='<%# Eval("LineNum") %>' Width="50px" CausesValidation="False" CommandName="Del" Text="Delete"
                                                                                                CssClass="btn btn-icons btn-defualt"> 
                                                                                        <i class="far fa-trash-alt"></i>
                                                                                            </asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                        <div class="row pb-0" style="font-size: small">
                                                                            <div class="col-md-12">
                                                                                <table style="border-spacing: 5em">
                                                                                    <tr style="padding-bottom: 10em">
                                                                                        <td style="width: 400px; font-size: small"><span>​​​ยอดเต็ม: &nbsp</span></td>
                                                                                        <td style="width: 250px; font-size: small"><%#  Convert.ToDecimal( Eval("InvTotalAmt")).ToString("N2") %> </td>
                                                                                    </tr>

                                                                                    <tr>
                                                                                        <td style="width: 400px; font-size: small"><span>​​​กำหนด: &nbsp</span></td>
                                                                                        <td style="width: 250px; font-size: small"><%#  Convert.ToDateTime(Eval("InvDueDate")).ToString("dd/MM/yyyy") %> </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>


                                                                    </div>
                                                                    <div class="col-md-6 text-right">
                                                                        <table style="padding-bottom: 10em">
                                                                            <tr>
                                                                                <td style="width: 400px; font-size: small"><span>​​​งวดที่:&nbsp </span></td>
                                                                                <td style="width: 250px; font-size: medium; padding-bottom: 5px">
                                                                                    <asp:TextBox ID="txtPayNo"
                                                                                        class="form-control form-control-sm" Width="100%"
                                                                                        Text='<%#  Eval("PayNo")%>'
                                                                                        runat="server" Style="text-align: center" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="padding-bottom: 10em">
                                                                                <td style="width: 400px; font-size: small"><span>​​​ชำระ:&nbsp</span></td>
                                                                                <td style="width: 400px; font-size: medium">
                                                                                    <asp:TextBox ID="txtAmount" Font-Size="Large" Font-Bold="true"
                                                                                        Style="text-align: right"
                                                                                        class="form-control form-control-sm" runat="server"
                                                                                        Text='<%#  Convert.ToDecimal( Eval("PayTotalAmt")).ToString("N2")%>'
                                                                                        onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>
                                                                <div class="row pb-1">
                                                                    <div class="col-md-12">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 400px; font-size: small"><span>​​​หมายเหตุ:&nbsp</span></td>
                                                                                <td style="width: 100%; font-size: small">
                                                                                    <asp:TextBox ID="txtRemark1" class="form-control form-control-sm" Text='<%# Eval("Remark1") %>' runat="server"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                                <div class="col-md-6 pr-1 pl-1">
                                    <div class="row pt-1 ">
                                        <div class="col-md-12 ">
                                            <div class="card bg-light">
                                                <div class="card-body">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="row ">
                                                                <div class="col-md-4">

                                                                    <span style="font-size: small">​​​วันที่จ่าย </span>
                                                                    <dx:ASPxDateEdit ID="dtPayDate" runat="server" Theme="Material" Width="100%"
                                                                        DisplayFormatString="dd-MM-yyyy"
                                                                        EditFormatString="dd-MM-yyyy">
                                                                        <TimeSectionProperties Visible="False">
                                                                            <TimeEditCellStyle HorizontalAlign="Right">
                                                                            </TimeEditCellStyle>
                                                                        </TimeSectionProperties>
                                                                    </dx:ASPxDateEdit>

                                                                </div>


                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">สกุลเงิน </span>
                                                                    <dx:ASPxComboBox ID="cboCurrency" runat="server"
                                                                        OnSelectedIndexChanged="cboCurrency_SelectedIndexChanged" AutoPostBack="true"
                                                                        DropDownStyle="DropDownList" Theme="Material"
                                                                        Font-Size="Small"
                                                                        ValueField="CurrencyID" ValueType="System.String"
                                                                        ViewStateMode="Enabled"
                                                                        TextFormatString="{0}" Width="100%">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="CurrencyID" Caption="Currency code" />
                                                                            <dx:ListBoxColumn FieldName="Name" Width="150px" Caption="Description" />
                                                                            <dx:ListBoxColumn FieldName="CountryCode" Width="250px" Caption="Country" />
                                                                        </Columns>
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">อัตราแลกเปลี่ยน</span>
                                                                    <asp:TextBox ID="txtRateExchange" CssClass="form-control form-control-sm" runat="server" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pt-2 pb-2 " id="divgladjust" runat="server">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="udpgl" runat="server">
                                                <ContentTemplate>
                                                    <div class="row pb-1 ">
                                                        <div class="col-md-12  pt-1 ">
                                                            <div class="card">
                                                                <div class="card-header p-2" style="background-color: rgba(0,0,0,.06);">
                                                                    <div class="row ">
                                                                        <div class="col-md-8 pt-2 text-left ">
                                                                            &nbsp
                                    <asp:Label ID="lblShowDiffAmt" runat="server" Font-Size="Large" Font-Bold="true" ForeColor="Green"> </asp:Label>
                                                                        </div>
                                                                        <div class="col-md-4 text-right" runat="server" id="divbtnAddpayment">
                                                                            <asp:LinkButton ID="btnAddpayment" CssClass="btn btn-success btn-sm" runat="server" OnClick="btnaddpayment_Click">   
                                                <i class="fas fa-plus-circle"></i>&nbsp<span style="font-size:small" >เพิ่มวิธีการจ่ายเงิน</span> 
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="card-body">
                                                                    <div class="row ">
                                                                        <div class="col-md-12">

                                                                            <asp:ListView ID="grdPaymentMethod"
                                                                                runat="server"
                                                                                OnItemCommand="grdPaymentMethod_ItemCommand"
                                                                                OnItemDataBound="grdPaymentMethod_ItemDataBound">
                                                                                <EmptyDataTemplate>
                                                                                    <div style="text-align: center"><span style="color: darkgray">There are no payment mehtod.</span>  </div>
                                                                                </EmptyDataTemplate>
                                                                                <ItemTemplate>
                                                                                    <div class="col-md-0" style="display: none">
                                                                                        <asp:Label ID="lblLineNum" runat="server" Text='<%# Eval("LineNum") %>'></asp:Label>
                                                                                    </div>

                                                                                    <div class="row">
                                                                                        <div class="col-md-8 text-left">
                                                                                            <div class="row" style="color: gray;">
                                                                                                <div class="col-md-12">
                                                                                                    <strong><%# Eval("PayByDesc") %> </strong>
                                                                                                    <span style="font-size: x-large; color: crimson"><strong>&nbsp   <%# Convert.ToDecimal( Eval("PayAmt")).ToString("N2") %></strong></span>&nbsp บาท
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-12">
                                                                                                    <asp:Literal ID="lblRCStatus" runat="server"></asp:Literal>

                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="col-md-4 text-right">
                                                                                            <asp:LinkButton ID="btnDelete" runat="server"
                                                                                                CommandArgument='<%# Eval("LineNum") %>' Width="50px" CausesValidation="False" CommandName="Del" Text="Delete"
                                                                                                CssClass="btn btn-icons btn-defualt"> 
                                                                                        <i class="far fa-trash-alt"></i>
                                                                                            </asp:LinkButton>&nbsp
                                                                                            <asp:LinkButton ID="btnEdit"
                                                                                                runat="server"
                                                                                                Visible='<%# ShowHideGridButton()%>'
                                                                                                CommandArgument='<%# Eval("LineNum") %>'
                                                                                                CausesValidation="False" CommandName="editrow" CssClass="btn btn-defult">  
                                                                                                <i class="fas fa-pencil-alt" style="color:gray;"></i>
                                                                                                <span  style="color:gray">แก้ไขวิธีชำระ</span>
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div style='<%# Convert.ToString( Eval("PayBy")) == "TRANSFER"   ? "display:block;": "display:none;" %>; font-size: smaller'>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันที่เงินใช้ได้ : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("StatementDate") == null ? "-" : Convert.ToDateTime(Eval("StatementDate")).ToString("dd/MM/yyyy")%></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​โอนจากธนาคาร : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("CustBankCode") %></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​โอนจากบัญชีเลข : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("PayToBookID") %></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​สาขาธนาคารที่โอน : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Crimson" Font-Size="Small" Font-Bold="true"> <%# Eval("CustBankBranch") %></asp:Label>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>

                                                                                    <div style='<%# Convert.ToString( Eval("PayBy")) == "CHEQUE"   ? "display:block;": "display:none;" %>; font-size: smaller'>
                                                                                        <div class="row">
                                                                                            <div class="col-md-12">
                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันรับเช็ค : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%#  Convert.ToDateTime(Eval("PayDate")).ToString("dd/MM/yyyy") %></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​จ่ายเข้าบัญชี : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("PayToBookID") %></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​เช็คธนาคาร : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("CustBankName") %></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​เลขที่เช็ค : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("PaymentRefNo") %></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​เช็คสาขาธนาคาร : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("CustBankBranch") %></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​เช็คหมดอายุ : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("ChqExpired") == null ? "-" : Convert.ToDateTime(Eval("ChqExpired")).ToString("dd/MM/yyyy")%></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันที่หน้าเช็ค : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("ChqDate") == null ? "-" : Convert.ToDateTime(Eval("ChqDate")).ToString("dd/MM/yyyy")%></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันที่ฝากเช็ค : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("ChqDepositDate") == null ? "-" : Convert.ToDateTime(Eval("ChqDepositDate")).ToString("dd/MM/yyyy") %></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันที่ Statement : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("StatementDate") == null ? "-" : Convert.ToDateTime(Eval("StatementDate")).ToString("dd/MM/yyyy")%></asp:Label>

                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​วันที่เช็คคืน : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("ChqReturnDate") == null ? "-" : Convert.ToDateTime(Eval("ChqReturnDate")).ToString("dd/MM/yyyy")%></asp:Label>
                                                                                                </div>

                                                                                                <div class="row pt-1">
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Gray">​​​เหตุผลเช็คคืน : &nbsp</asp:Label>
                                                                                                    <asp:Label runat="server" CssClass="col-md-3" ForeColor="Blue" Font-Size="Small" Font-Bold="true"><%# Eval("ChqReturnReason") %></asp:Label>
                                                                                                </div>

                                                                                            </div>
                                                                                        </div>

                                                                                    </div>
                                                                                    <hr />
                                                                                </ItemTemplate>
                                                                            </asp:ListView>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="card" hidden    ="hidden">
                                                        <div class="card-header p-2" style="background-color: rgba(0,0,0,.06);">
                                                            <div class="row" style="font-size: small">
                                                                <div class="col-md-10"><i class="fas fa-info"></i>&nbsp<span style="color: gray">ปรับปรุง</span></div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body pt-1 pb-2">
                                                            <div class="row pt-2">
                                                                <div class="col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-md-4">
                                                                            <span style="font-size: small">รายการ</span>
                                                                            <dx:ASPxComboBox ID="cboGLCode" runat="server"
                                                                                CssClass=" "
                                                                                DropDownStyle="DropDownList"
                                                                                Theme="Material"
                                                                                ValueField="Code" ValueType="System.String" ViewStateMode="Enabled"
                                                                                TextFormatString="{0} {1}" Width="100%">
                                                                                <Columns>
                                                                                    <dx:ListBoxColumn FieldName="Code" Caption="Code ID" />
                                                                                    <dx:ListBoxColumn FieldName="Name" Width="300px" Caption="Name" />
                                                                                </Columns>
                                                                            </dx:ASPxComboBox>
                                                                        </div>
                                                                        <div class="col-md-4" hidden="hidden">
                                                                            <span style="font-size: small">จำนวน</span>
                                                                            <asp:TextBox ID="txtDrAmt" CssClass="form-control form-control-sm" Font-Size="Small" runat="server"></asp:TextBox>
                                                                        </div>

                                                                        <div class="col-md-4 pt-4 text-left">
                                                                            <asp:LinkButton ID="btnAddGLAdjust" runat="server" CssClass="btn btn-default" OnClick="btnAddGLAdjust_Click">
                                                   <span  style="color:orangered"><i class="fas fa-comment-alt-plus"></i>&nbsp</span><span> เพิ่มรายการปรับปรุง</span> 
                                                                            </asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row pt-2">
                                                                <div class="col-md-12">
                                                                    <div class="row ">
                                                                        <div class="col-md-12">
                                                                            <div style="overflow-x: auto; width: 100%">
                                                                                <dx:ASPxGridView ID="grdAdjust" Width="100%"
                                                                                    runat="server" AutoGenerateColumns="False"
                                                                                    KeyFieldName="LineNum"
                                                                                    ClientInstanceName="grdAdjust"
                                                                                    Theme="Moderno"
                                                                                    OnRowCommand="grdAdjust_RowCommand">
                                                                                    <Settings ShowFooter="True"
                                                                                        ShowGroupFooter="Hidden" ShowFilterBar="Hidden" />
                                                                                    <Columns>
                                                                                        <dx:GridViewDataTextColumn VisibleIndex="0">
                                                                                            <DataItemTemplate>
                                                                                                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("LineNum") %>' CausesValidation="False"
                                                                                                    CommandName="del" Text="" CssClass="btn btn-icons btn-default">
                                                                           <span style="color:gray"> <i class="far fa-trash-alt"></i></span></asp:LinkButton>
                                                                                            </DataItemTemplate>
                                                                                        </dx:GridViewDataTextColumn>

                                                                                        <dx:GridViewDataTextColumn FieldName="GLCode" Caption="GL Account">
                                                                                            <EditFormSettings Visible="False" />
                                                                                            <HeaderStyle Wrap="False" Font-Size="Small" />
                                                                                            <CellStyle Wrap="False" Font-Size="Small" />
                                                                                        </dx:GridViewDataTextColumn>
                                                                                        <dx:GridViewDataTextColumn FieldName="GLNam" Caption="Name">
                                                                                            <EditFormSettings Visible="False" />
                                                                                            <HeaderStyle Wrap="False" Font-Size="Small" />
                                                                                            <CellStyle Wrap="False" Font-Size="Small" />
                                                                                        </dx:GridViewDataTextColumn>
                                                                                        <dx:GridViewDataTextColumn FieldName="DrAmt" Name="DrAmt" Caption="Dr Amt.">
                                                                                            <DataItemTemplate>
                                                                                                <dx:ASPxTextBox ID="txtDrAmt" runat="server" Width="100%" CssClass="form-control form-control-sm" Value='<%# Eval("DrAmt","{0:n2}") %>'></dx:ASPxTextBox>
                                                                                            </DataItemTemplate>
                                                                                            <HeaderStyle Wrap="False" Font-Size="Small" />
                                                                                            <CellStyle Wrap="False" Font-Size="Small" />
                                                                                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                                                                        </dx:GridViewDataTextColumn>
                                                                                        <dx:GridViewDataTextColumn FieldName="CrAmt" Name="CrAmt" Caption="Cr Amt.">
                                                                                            <DataItemTemplate>
                                                                                                <dx:ASPxTextBox ID="txtCrAmt" runat="server" Width="100%" CssClass="form-control form-control-sm" Value='<%# Eval("CrAmt","{0:n2}") %>'></dx:ASPxTextBox>
                                                                                            </DataItemTemplate>
                                                                                            <HeaderStyle Wrap="False" Font-Size="Small" />
                                                                                            <CellStyle Wrap="False" Font-Size="Small" />
                                                                                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                                                                        </dx:GridViewDataTextColumn>


                                                                                    </Columns>
                                                                                    <TotalSummary>

                                                                                        <dx:ASPxSummaryItem FieldName="CrAmt" ShowInColumn="CrAmt" ShowInGroupFooterColumn="CrAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                                                                        <dx:ASPxSummaryItem FieldName="DrAmt" ShowInColumn="DrAmt" ShowInGroupFooterColumn="DrAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />


                                                                                    </TotalSummary>
                                                                                </dx:ASPxGridView>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>


                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-body bg-light">
                    <div class="row pt-1  ">
                        <div class="col-md-12">
                            <div class="row pb-1">
 <div class="col-md-8">
     <div class="alert alert-danger" role="alert" id="divDiffPay" runat="server">
  ยอดชำระไม่ครบตามยอดอินวอยซ์
</div>
     </div>
                                <div class="col-md-4 text-right">
                                    <div class="btn-group" role="group">
                                        <asp:Button ID="btnsave"
                                            CssClass="btn btn-success " Width="120"
                                            OnClick="btnSave_Click"
                                            OnClientClick="this.disabled='true';"
                                            UseSubmitBehavior="false"
                                            runat="server"
                                            Text="บันทึกชำระ"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="row pt-2 pb-2 ">
                <div class="col-md-12">
                    <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                        <hr />
                        <strong>
                            <span id="myalertHead"></span></strong>
                        <br />
                        <span id="myalertBody"></span>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>
            </div>

            <div class="row pt-2 pb-2 " runat="server" id="divtransaction">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="card">
                                <div class="card-header" style="background-color: rgba(0,0,0,.06);">
                                    <div class="row" style="font-size: small">
                                        <div class="col-md-10"><i class="fas fa-history fa-2x"></i>&nbsp<span style="color: gray"> History</span></div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="row pt-3">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12 ">
                                                    <div style="overflow-x: auto; width: 100%">
                                                        <asp:GridView ID="grd_transaction_log" runat="server" AutoGenerateColumns="False" Width="100%"
                                                            CssClass="table table-striped table-bordered table-hover"
                                                            Font-Size="Small"
                                                            EmptyDataText="No transaction log data.."
                                                            EmptyDataRowStyle-HorizontalAlign="Center"
                                                            ShowHeaderWhenEmpty="true">
                                                            <Columns>
                                                                <asp:BoundField DataField="CreatedBy" HeaderText="Action By">
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="CreatedDate" HeaderText="Action Date" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="Action" HeaderText="Action">
                                                                    <HeaderStyle Wrap="False" />
                                                                    <ItemStyle Wrap="False" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
