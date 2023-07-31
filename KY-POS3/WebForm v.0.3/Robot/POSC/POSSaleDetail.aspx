<%@ Page Title="POSV2" Language="C#" MasterPageFile="~/POSC/SiteA.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="POSSaleDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSSaleDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddPrintUrl" runat="server" />


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


    <!-- jQuery.NumPad -->
    <script src="../../Asset/js/jquery.numpad.js"></script>
    <link rel="stylesheet" href="../../Asset/css/jquery.numpad.css">
    <script type="text/javascript">
        // Set NumPad defaults for jQuery mobile. 
        // These defaults will be applied to all NumPads within this document!
        $.fn.numpad.defaults.gridTpl = '<table class="table modal-content"></table>';
        //$.fn.numpad.defaults.backgroundTpl = '<div class="modal-backdrop in"></div>';
        $.fn.numpad.defaults.displayTpl = '<input type="text" class="form-control" />';
        $.fn.numpad.defaults.buttonNumberTpl = '<button type="button" class="btn btn-default"></button>';
        $.fn.numpad.defaults.buttonFunctionTpl = '<button type="button" class="btn" style="width: 100%;"></button>';
        $.fn.numpad.defaults.onKeypadCreate = function () { $(this).find('.done').addClass('btn-success'); };

        // Instantiate NumPad once the page is ready to be shown
        $(document).ready(function () {
            $('#txtCustPayAmt-btn').numpad({
                target: $('#txtCustPayAmt')
            });

        });
    </script>
    <style type="text/css">
        .nmpd-grid {
            border: none;
            padding: 20px;
        }

            .nmpd-grid > tbody > tr > td {
                border: none;
            }

        /* Some custom styling for Bootstrap */
        .qtyInput {
            display: block;
            width: 100%;
            padding: 6px 12px;
            color: #555;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s;
        }
    </style>



    <style>
        .container {
            position: relative;
            max-width: 800px; /* Maximum width */
            margin: 0 auto; /* Center it */
        }

            .container .content {
                position: absolute; /* Position the background text */
                bottom: 0; /* At the bottom. Use top:0 to append it to the top */
                background: rgb(0, 0, 0); /* Fallback color */
                background: rgba(0, 0, 0, 0.5); /* Black background with 0.5 opacity */
                color: #f1f1f1; /* Grey text */
                width: 100%; /* Full width */
                padding: 20px; /* Some padding */
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
        //begin Popup Postback
        function OnClosePopupAlert() {
            popAlert.Hide();
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
    <dx:aspxpopupcontrol id="popInputQty" runat="server" width="800" height="200" closeaction="OuterMouseClick" closeonescape="true" modal="True"
        theme="Material"
        popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter" clientinstancename="popInputQty"
        headertext="Infomation" allowdragging="True" popupanimationtype="None" enableviewstate="False" autoupdateposition="true">
        <contentcollection>
            <dx:popupcontrolcontentcontrol runat="server">
                <dx:aspxpanel id="ASPxPanel1" runat="server" defaultbutton="btnDirectInputQty">
                    <panelcollection>
                        <dx:panelcontent runat="server">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row ">
                                        <div class="col-md-12">
                                            <asp:Label Font-Size="Large" runat="server">จำนวน &nbsp</asp:Label>

                                            <dx:aspxspinedit id="txtInputQty" runat="server" buttonstyle-font-size="XX-Large" font-size="XX-Large" theme="iOS" number="1" numbertype="Integer"
                                                minvalue="1" maxvalue="999999" width="100%" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 pt-3">
                                            <asp:Button ID="btnDirectInputQty" CssClass="btn btn-success btn-lg " runat="server" Text="บันทึก" OnClick="btnDirectInputQty_Click" />
                                        </div>
                                    </div>
                                    <hr />

                                </div>
                            </div>
                            <div>&nbsp</div>
                        </dx:panelcontent>
                    </panelcollection>
                </dx:aspxpanel>
            </dx:popupcontrolcontentcontrol>
        </contentcollection>
        <contentstyle>
            <paddings paddingbottom="5px" />
        </contentstyle>
    </dx:aspxpopupcontrol>


    <dx:aspxpopupcontrol id="popPrint" runat="server"
        closeaction="OuterMouseClick"
        showmaximizebutton="true"
        showclosebutton="true"
        closeonescape="true"
        modal="True"
        theme="Material"
        width="800"
        height="600"
        popuphorizontalalign="WindowCenter"
        popupverticalalign="WindowCenter"
        clientinstancename="popPrint"
        headertext="Print"
        allowdragging="True"
        popupanimationtype="None"
        enableviewstate="False"
        autoupdateposition="true">
    </dx:aspxpopupcontrol>


    <asp:UpdatePanel ID="udpAlert" runat="server">
        <contenttemplate>
            <dx:aspxpopupcontrol id="popAlert" runat="server" width="600" closeaction="OuterMouseClick" closeonescape="true" modal="True"
                theme="Mulberry"
                popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter" clientinstancename="popAlert"
                headertext="Infomation" allowdragging="True" popupanimationtype="None" enableviewstate="False" autoupdateposition="true">
                <contentcollection>
                    <dx:popupcontrolcontentcontrol runat="server">
                        <dx:aspxpanel id="Panel1" runat="server" defaultbutton="btOK">
                            <panelcollection>
                                <dx:panelcontent runat="server">
                                    <div class="card">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsg" runat="server" Text=""></asp:Label>
                                            </strong></h5>
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

                                </dx:panelcontent>
                            </panelcollection>
                        </dx:aspxpanel>
                    </dx:popupcontrolcontentcontrol>
                </contentcollection>

            </dx:aspxpopupcontrol>
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </triggers>
    </asp:UpdatePanel>


    <div class="row pb-1" runat="server" id="divmain">
        <div class="col-md-12">

            <div class="card">
                <div class="card-header">
                    <div class="row ">
                        <div class="col-4 ">
                            <div class="row">
                                <div class="col-12">
                                    <strong><span style="color: crimson">
                                        <asp:Label CssClass="text-center" ID="lblcompany" runat="server">&nbsp</asp:Label></span> </strong>
                                    <br />
                                    <asp:Label ID="lblID" ForeColor="DimGray" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-8 text-right">
                            <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                                <asp:LinkButton ID="btnnew" runat="server" CssClass="btn btn-success btn-sm" OnClick="btnnew_Click">
                                    <i class="far fa-file-invoice-dollar"></i>&nbsp<span>รายการใหม่</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnCheckBill" runat="server" CssClass="btn btn-dark  btn-sm" OnClick="btnCheckBill_Click">
                                    <i class="far fa-scroll"></i>&nbsp<span>เช็คบิล</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnviewlist" runat="server" CssClass="btn btn-dark  btn-sm" OnClick="btnviewlist_Click">
                                    <i class="far fa-scroll"></i>&nbsp<span>ประวัติบิล</span>
                                </asp:LinkButton>

                                <div class="btn-group" role="group">
                                    <button id="btnGroupDrop1" type="button" class="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                        <i class="fas fa-th-list"></i>
                                    </button>
                                    <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                        <asp:LinkButton ID="btnDelete" class="dropdown-item " runat="server" Text="ยกเลิก" OnClick="btnDelete_Click"></asp:LinkButton>
                                        <%--<asp:Button ID="btnDeletelPermanent" CssClass="dropdown-item  " runat="server" Text="ลบบิล" OnClick="btnDeletelPermanent_Click" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <%-- การจ่ายเงิน --%>
    <div class="row pt-2" runat="server" id="divpay">
        <div class="col-lg-10 col-md-12 mx-auto">
            <div class="card shadow mb-5 rounded">
                <div class="card-header pb-2 pt-2">
                    <div class="row ">
                        <div class="col-md-6 pt-2 ">
                            <i class="fas fa-comments-dollar fa-2x"></i>&nbsp
                            <asp:Label CssClass="" runat="server">ข้อมูลการจ่ายเงิน  &nbsp</asp:Label>
                        </div>

                    </div>
                </div>

                <div class="card-body">
                    <div class="row ">
                        <div class="col-md-12 mx-auto pt-4 pb-4">
                            <div class="row pb-3">
                                <asp:Label class="col-md-4 pt-1 text-right" runat="server">จ่ายโดย &nbsp</asp:Label>
                                <asp:DropDownList runat="server" ID="cboTenderType" CssClass="col-md-4 pt-1 form-control form-control-sm"
                                    DropDownStyle="DropDownList" OnSelectedIndexChanged="cboTenderType_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="เงินสด" Value="CASH"></asp:ListItem>
                                    <asp:ListItem Text="โอนเงิน/บัตรเครดิต" Value="TRANSFER"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label class="col-md-4 pt-1 text-left" runat="server">&nbsp</asp:Label>
                            </div>
                            <div class="row pb-3">
                                <asp:Label class="col-md-4 pt-1 text-right" runat="server">ยอดรวม &nbsp</asp:Label>
                                <input type="text" id="txtsum_price" runat="server" name="txtsum_price" class="col-md-4 form-control form-control-sm " style="text-align: right" onkeypress="return DigitOnly(this,event)">
                                <asp:Label class="col-md-4 pt-1 text-left" runat="server">บาท &nbsp</asp:Label>
                            </div>
                            <div class="row pb-3">
                                <div class="input-group">
                                    <asp:Label class="col-md-4 pt-1 text-right" runat="server">รับเงิน &nbsp</asp:Label>
                                    <asp:TextBox type="text" ID="txtCustPayAmt" runat="server"
                                        name="txtCustPayAmt" class="col-md-4 form-control form-control-sm " value=""
                                        placeholder="" aria-describedby="txtCustPayAmt-btn"
                                        onkeypress="return DigitOnly(this,event) " Style="text-align: right">
                                    </asp:TextBox>
                                    <span class="input-group-btn">
                                        <button class="btn btn-dark btn-sm" id="txtCustPayAmt-btn" type="button"><i class="far fa-calculator-alt"></i></button>
                                    </span>
                                    <asp:Label class="col-md-4 pt-1 text-left" runat="server">บาท &nbsp</asp:Label>
                                </div>
                            </div>
                            <div class="row ">
                                <div class="col-12  mx-auto">
                                    <asp:LinkButton ID="btnSubmitPayment" runat="server"
                                        CssClass="btn btn-success"
                                        OnClick="btnSubmitPayment_Click">
                                        <i class="fas fa-check-circle"></i>&nbsp&nbsp
                                        <span>ตกลง</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- เลือกสินค้า --%>


    <div class="row pb-1  " runat="server" id="divitem_select">
        <div class="col-md-7 pr-0">
            <div class="card">
                <div class="card-header  pb-1 pt-1">
                    <div class="row">
                        <div class="col-md-6 pl-0">
                            <div class="input-group mb-3">
                                <asp:DropDownList ID="cboTable" runat="server"
                                    CssClass="form-control form-control-sm  bg-warning"
                                    ForeColor="White" Font-Size="Large"
                                    DataTextField="TableName"
                                    OnSelectedIndexChanged="cboTable_SelectedIndexChanged"
                                    DataValueField="TableID">
                                </asp:DropDownList>
                                <asp:LinkButton ID="btnShipTo" runat="server" CssClass="btn btn-danger btn-sm"
                                    OnClick="btnShipTo_Click">
                                </asp:LinkButton>
                            </div>
                        </div>


                        <div class="col-md-6 pl-1 text-right" runat="server" id="divPrintDesktop">
                            <div class="btn-group" role="group" aria-label="Basic example" runat="server" id="divprint">

                                <asp:LinkButton ID="btnprintBill" runat="server" Width="60px" CssClass="btn btn-secondary btn-sm" OnClick="btnprintBill_Click">
                                    <span>บิล</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnprintInv" runat="server" Width="60px" CssClass="btn btn-secondary  btn-sm" OnClick="btnprintInv_Click">
                                    <span>ใบเสร็จ</span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnprintFInv" runat="server" Width="80px" CssClass="btn btn-secondary btn-sm" OnClick="btnprintFInv_Click">
                                    <span>ใบกำกับ</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <asp:DataList ID="grdlist" runat="server" RepeatColumns="3"
                        CellSpacing="2" RepeatDirection="Horizontal"
                        RepeatLayout="Table" Width="100%"
                        OnItemCommand="grdlist_ItemCommand">
                        <itemtemplate>
                            <div class="row pb-0">
                                <div class="col-md-12 border bg-light">
                                    <asp:Label ID="lblItem" Visible="false" Font-Size="Large" class="col-form-label " Text='<%# Eval("ItemID") %>' runat="server"></asp:Label>
                                    <asp:LinkButton runat="server" CommandName="sel" Width="100%" CommandArgument='<%# Eval("ItemID")%>' class="btn" ID="btnSeleltItem">
                                        <div class="row " style="height: 45px; width=100%">
                                            <div class="col-md-12 text-center">
                                                <span style="font-size: 13px"><%# Eval("Name") %></span>
                                                <br />
                                                <span style="font-size: 13px"><%# Eval("Price", "{0:n2}") +" บาท" %></span>
                                                <%-- <div hidden="hidden"><%# Eval("ItemID")%></div>
                                                <div class="container">
                                                    <asp:Image ID="img" ImageUrl='<%# Eval("ImageUrl").ToString()!=""?Eval("ImageUrl").ToString():"../Image/Logo/logo_codeyum.jpg" %>' runat="server" Style="width: 100%;" />
                                                    <div class="content">
                                                        <p>
                                                            <span ><strong><%# Eval("Name") %></strong> </span>
                                                            <span ><%#"ราคา "+ Eval("Price", "{0:n2}") %></span>
                                                        </p>
                                                    </div>
                                                </div>--%>
                                            </div>
                                        </div>


                                    </asp:LinkButton>
                                </div>
                            </div>
                        </itemtemplate>
                    </asp:DataList>

                </div>
            </div>
        </div>
        <div class="col-md-5 pl-1">
            <div class="row pb-1">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header pl-0 pb-1 pt-1">
                            <div class="row" style="font-size: small">

                                <asp:Label CssClass="col-md-4 pt-2  px-0 text-right" runat="server">วันที่ขาย &nbsp</asp:Label>
                                <div class="col-md-8 px-0" style="font-size: 15px;">
                                    <dx:aspxdateedit id="dtInvDate"
                                        theme="Material"
                                        autopostback="true"
                                        ondatechanged="dtInvDate_DateChanged"
                                        runat="server" width="100%"
                                        displayformatstring="dd-MM-yyyy" editformatstring="dd-MM-yyyy">
                                        <timesectionproperties visible="False">
                                            <timeeditcellstyle horizontalalign="Right">
                                            </timeeditcellstyle>
                                        </timesectionproperties>
                                    </dx:aspxdateedit>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row pb-1">
                <div class="col-md-12">
                    <div class="input-group">
                        <asp:TextBox ID="txtDisAmtPer" CssClass="form-control form-control-sm " BackColor="#ffffcc" runat="server" Style="text-align: right" placeholder="ระบุส่วนลด" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                        <div class="input-group-append">
                            <asp:LinkButton ID="btnDiscPer" runat="server" CssClass="btn btn-warning btn-sm" OnClick="btnDiscPer_Click">
                                <span>%</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDiscAmt" runat="server" CssClass="btn btn-danger btn-sm" OnClick="btnDiscAmt_Click">
                                <span>฿</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div style="overflow-x: auto; width: 100%">
                        <asp:GridView ID="grditemSelect" runat="server" AutoGenerateColumns="False" Width="100%"
                            CssClass="table table-striped table-bordered table-hover "
                            EmptyDataText="ยังไม่เลือกรายการ.."
                            EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowHeaderWhenEmpty="true"
                            ShowFooter="True"
                            OnRowCommand="grditemSelect_RowCommand"
                            OnRowDataBound="grditemSelect_RowDataBound"
                            DataKeyNames="LineNum">
                            <columns>
                                <asp:BoundField DataField="ItemName" HeaderText="เมนู">
                                    <headerstyle wrap="false" font-size="13px" />
                                    <itemstyle wrap="false" font-size="13px" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="true" HeaderText="จำนวน" HeaderStyle-Width="40">
                                    <itemtemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("LineNum") %>'
                                            CausesValidation="True" CommandName="editqty" Text='<%# Eval("Qty","{0:N0}") %>'
                                            CssClass="btn btn-info btn-sm">
                                        </asp:LinkButton>
                                    </itemtemplate>
                                    <headerstyle font-size="13px" />
                                    <itemstyle font-size="13px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="TotalAmt" HeaderText="รวม" DataFormatString="{0:n2}">
                                    <headerstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                    <itemstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                </asp:BoundField>
                                <asp:TemplateField ShowHeader="true" HeaderStyle-Width="60" Visible="false">
                                    <itemtemplate>
                                        <asp:LinkButton ID="btnDel" runat="server" CommandArgument='<%# Eval("LineNum") %>'
                                            CausesValidation="True" CommandName="Del"
                                            CssClass="btn btn-icons btn-default">
                                            <i class="fas fa-trash"></i>
                                        </asp:LinkButton>
                                    </itemtemplate>
                                </asp:TemplateField>
                            </columns>
                            <footerstyle />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="row text-right pb-2">
                <div class="col-md-12">
                    <h5>
                        <span style="font-size: 15px">VAT &nbsp</span>
                        <asp:Label runat="server" CssClass="badge badge-pill badge-primary " ID="lblSumVatAmt" Text="0.00"></asp:Label>
                        <span style="font-size: 15px">&nbsp บาท</span>
                        <br />
                        <span class=" " style="font-size: 15px">ยอดรวม VAT &nbsp</span>
                        <asp:Label runat="server" CssClass="badge badge-pill badge-primary " ID="lblSumTotalAmtIncVat" Text="0.00"></asp:Label>
                        <span style="font-size: 15px">&nbsp บาท</span>
                        <br />
                        <span class=" " style="font-size: 15px">หลังปัดเศษ &nbsp</span>
                        <asp:Label runat="server" CssClass="badge badge-pill badge-primary " ID="lblSumNetTotalAfterRound" Text="0.00"></asp:Label>
                        <span style="font-size: 15px">&nbsp บาท</span>
                    </h5>
                </div>

            </div>

            <div class="row pb-2">
                <div class="col-md-12">
                    <asp:LinkButton ID="btnpay" runat="server" Width="100%" ForeColor="White" CssClass="btn btn-success" OnClick="btnpay_Click">
                        <i class="far fa-comment-alt-dollar"></i><span>เลือกวิธีชำระ</span>
                    </asp:LinkButton>
                </div>
            </div>
            <div class="row pb-2" runat="server" id="divgrdpayment">
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <contenttemplate>
                            <div class="row">
                                <div class="col-md-12 ">
                                    <div style="overflow-x: auto; width: 100%">
                                        <asp:GridView ID="grdpay" runat="server" AutoGenerateColumns="False" Width="100%"
                                            CssClass="table table-striped table-bordered table-hover"
                                            EmptyDataText="ไม่มีรายการ.."
                                            EmptyDataRowStyle-HorizontalAlign="Center"
                                            ShowHeaderWhenEmpty="true"
                                            ShowFooter="true"
                                            OnRowCommand="grdpay_RowCommand"
                                            OnRowDataBound="grdpay_RowDataBound"
                                            DataKeyNames="PaymentType">
                                            <columns>
                                                <asp:BoundField DataField="PaymentType" HeaderText="จ่ายโดย">
                                                    <headerstyle wrap="False" font-size="13px" />
                                                    <itemstyle wrap="False" font-size="13px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="GetAmt" HeaderText="รับเงิน" DataFormatString="{0:n2}">
                                                    <headerstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                    <itemstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayAmt" HeaderText="จำนวนที่จ่าย" DataFormatString="{0:n2}">
                                                    <headerstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                    <itemstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ChangeAmt" HeaderText="เงินทอน" DataFormatString="{0:n2}">
                                                    <headerstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                    <itemstyle wrap="False" horizontalalign="Right" font-size="13px" />
                                                </asp:BoundField>
                                                <asp:TemplateField ShowHeader="true" HeaderStyle-Width="80">
                                                    <itemtemplate>
                                                        <asp:LinkButton ID="btnDel" runat="server" CommandArgument='<%# Eval("PaymentType") %>'
                                                            CausesValidation="True" CommandName="Del"
                                                            CssClass="btn btn-icons btn-default">
                                                            <i class="fas fa-trash"></i>
                                                        </asp:LinkButton>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                            </columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </contenttemplate>
                        <triggers>
                            <asp:AsyncPostBackTrigger ControlID="grdlist" />
                        </triggers>
                    </asp:UpdatePanel>
                </div>
            </div>


            <div class="row pb-2" runat="server" id="divbtnsave">
                <div class="col-md-12 mx-auto text-center">

                    <asp:Button ID="btnsave"
                        ForeColor="Black"
                        CssClass="btn btn-warning btn-lg"
                        Width="140px"
                        OnClick="btnSave_Click"
                        OnClientClick="this.disabled='true';"
                        UseSubmitBehavior="false"
                        runat="server"
                        Text="บันทึกออเดอร์"></asp:Button>
                    &nbsp
                                    <asp:Button ID="btnSaveInvoice"
                                        ForeColor="White"
                                        CssClass="btn btn-success btn-lg"
                                        Width="140px"
                                        OnClick="btnSaveInvoice_Click"
                                        OnClientClick="this.disabled='true';"
                                        UseSubmitBehavior="false"
                                        runat="server"
                                        Text="จ่ายเงิน"></asp:Button>
                </div>
            </div>

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

    <%-- เพิ่มสินค้าสินค้า --%>
    <div class="row pb-1" runat="server" id="divitem_price">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header pb-1 pt-1">
                    <div class="row">
                        <div class="col-md-9">
                            <asp:Label runat="server">สินค้า :</asp:Label>&nbsp&nbsp
                            <asp:Label runat="server" ID="lblnameItem">&nbsp</asp:Label>
                        </div>
                        <div class="col-md-3 text-right">
                            <asp:LinkButton ID="btncloseAddItem" runat="server" CssClass="btn btn-sm" OnClick="btncloseAddItem_Click">
                                <i class="fas fa-window-close fa-2x" style="color: red;"></i>&nbsp
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <div class="row ">
                        <div class="col-md-12 mx-auto pt-4 pb-4">
                            <div class="row pb-3">
                                <asp:Label class="col-md-4 pt-1 text-right" runat="server">ราคา &nbsp</asp:Label>
                                <asp:TextBox ID="txtPrice" CssClass="col-md-4 form-control form-control-sm" runat="server" Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                            </div>
                            <div class="row pb-3">
                                <asp:Label class="col-md-4 pt-1 text-right" runat="server">จำนวน &nbsp</asp:Label>
                                <asp:TextBox ID="txtqty" CssClass="col-md-4 form-control form-control-sm" Text="1" runat="server" Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                            </div>
                            <div class="row ">
                                <div class="col-md-4 mx-auto text-center">
                                    <asp:LinkButton ID="btnAddItem" runat="server" CssClass="btn btn-success size-button" OnClick="btnAddItem_Click">
                                        <i class="fas fa-check-circle"></i>&nbsp&nbsp<span>ตกลง</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>

    <div class="row" hidden="hidden">
        <div class="col-lg-4 col-md-5 pl-4 pt-0 ">
            <div class="row pb-1">
                <asp:Label CssClass="col-md-3 pt-1 text-center" runat="server">ลูกค้า &nbsp</asp:Label>
                <div class="col-md-9">
                    <dx:aspxcombobox id="cboCustomerID" runat="server" cssclass="form-control form-control-sm " enablecallbackmode="true" callbackpagesize="10" theme="Mulberry"
                        valuetype="System.String" valuefield="CustomerID"
                        filterminlength="1"
                        onitemsrequestedbyfiltercondition="cboCustomerID_OnItemsRequestedByFilterCondition_SQL"
                        onitemrequestedbyvalue="cboCustomerID_OnItemRequestedByValue_SQL" textformatstring="{0}"
                        width="100%" dropdownstyle="DropDown">
                        <columns>
                            <dx:listboxcolumn fieldname="NameDisplay" caption="ชื่อ" width="250" />
                        </columns>
                        <clientsideevents begincallback="function(s, e) { OnBeginCallback(); }" endcallback="function(s, e) { OnEndCallback(); } " />
                    </dx:aspxcombobox>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                </div>
            </div>
        </div>
    </div>




    <script>
        $('#txtCustPayAmt').keyup(function () {
            var sum_pay = $('#txtCustPayAmt').val() - $('#txtTotalAmt').val();
            $('#txtchange').val(sum_pay);
        });
    </script>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
