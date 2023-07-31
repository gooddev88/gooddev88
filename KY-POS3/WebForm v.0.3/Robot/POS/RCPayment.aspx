<%@ Page Title="Payment" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="RCPayment.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.RCPayment" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row pt-4 pb-3">
        <div class="col-lg-8 mx-auto">
            <div class="row pb-2">
                <div class="col-md-12">
                    <asp:LinkButton ID="btnback1" runat="server" Text="" Width="100%"
                        CssClass="btn btn-danger" OnClick="btnBack_Click">
                        <i class="fas fa-backward"></i>
                        <span >กลับ </span>
                    </asp:LinkButton>
                </div>
            </div>
            <div class="row" style="font-size: smaller">
                <div class="col-md-6"> 
                    <div class="card">
                        <div class="card-header "> 
                            <asp:Label runat="server">วิธีการชำระเงิน </asp:Label>
                        </div>
                        <div class="card-body">
                            <div class="row  pt-1 " hidden="hidden">
                                <div class="col-md-3">
                                    <span>เลขอ้างอิงรับชำระ</span>
                                    <asp:TextBox ID="txtID" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <span>เลขสำคัญรับ</span>
                                    <asp:TextBox ID="txtRCID" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <span>บริษัท</span>
                                    <asp:TextBox ID="txtCompanyID" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row  pt-1 ">
                                <div class="col-md-12">
                                    <span>วิธีชำระ</span>                 
                                    <asp:DropDownList ID="cboPayBy" runat="server" 
                                        CssClass="form-control form-control-sm" 
                                        AutoPostBack="true" 
                                        OnSelectedIndexChanged="cboPayBy_SelectedIndexChanged"
                                        DataTextField="Name" 
                                        DataValueField="Code">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row ">
                                <div class="col-md-12">
                                    <span>หมายเหตุชำระ</span>
                                    <asp:TextBox ID="txtMemo" CssClass="form-control" TextMode="MultiLine" Height="40" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <asp:UpdatePanel ID="udpReceiveStatus" runat="server">
                        <ContentTemplate>
                            <div class="card">
                                <div class="card-header  "> 
                            <asp:Label runat="server">สถานะชำระ</asp:Label>
                                </div>
                                <div class="card-body" id="divStatementStatus" runat="server">
                                    <div class="row pt-1 ">
                                        <div class="col-md-12">
                                            <span>สถานะ</span>
                                            <asp:DropDownList ID="cboStatementStatus" 
                                                runat="server" AutoPostBack="true" 
                                                OnSelectedIndexChanged="cboStatementStatus_SelectedIndexChanged"
                                                CssClass="form-control form-control-sm"
                                                DataTextField="Desc" 
                                                DataValueField="Code">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row  pt-1 " id="divchqreturn" runat="server">
                                        <div class="col-md-12">
                                            <span>Return Reason</span>
                                            <asp:DropDownList ID="cboChqReturnReson" runat="server" CssClass="form-control form-control-sm" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row pt-1 ">
                                        <div class="col-md-12">
                                            
                                            <asp:label runat="server" ID="Statusdate_en">วันที่การชำระสมบูรณ์</asp:label>
                                            <dx:ASPxDateEdit ID="dtCompletedDate" runat="server"
                                                Theme="Material" Width="100%"
                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                <TimeSectionProperties Visible="False">
                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                    </TimeEditCellStyle>
                                                </TimeSectionProperties>
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>

                                    <div runat="server" id="divChqReturnDate">                                        

                                        <div class="row ">
                                            <div class="col-md-12">
                                                <asp:label runat="server" ID="Returndate_th">​​​วันที่เคลียร์เช็คคืน</asp:label>
                                                <asp:label runat="server" ID="Returndate_en">​​​Return Date</asp:label>
                                                <dx:ASPxDateEdit ID="dtChqReturnDate" runat="server" Theme="Material" Width="100%"
                                                    DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                    <TimeSectionProperties Visible="False">
                                                        <TimeEditCellStyle HorizontalAlign="Right">
                                                        </TimeEditCellStyle>
                                                    </TimeSectionProperties>
                                                </dx:ASPxDateEdit>
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

    <div class="row pb-2" runat="server" id="divpaycash" style="font-size: smaller">
        <div class="col-lg-8 mx-auto">
            <div class="card">
                <div class="card-header " >
                    <asp:Label runat="server" Font-Bold="true" ID="lblShowTotalinvoice_m"></asp:Label>
                </div>

                <div class="card-body">
                    <div class="row  pt-1 pb-1">
                        <div class="col-md-3">
                            <span><%= SetCaption("PAYMENT") %></span>
                            <asp:TextBox ID="txtPayAmt_CASH" runat="server"
                                BackColor="LemonChiffon"
                                style="text-align:right"
                                class="form-control form-control-lg" Font-Size="Large"
                                onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                        </div>
                    
                        <div class="col-md-3">
                            <span>วันที่เงินใช้ได้</span>
                            <dx:ASPxDateEdit ID="dtStatementDate_CASH" runat="server" Theme="Material" Width="100%"
                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="row " runat="server" id="divTrasnfer">
                        <div class="col-md-12">
                            <div class="row  pt-1 pb-1">
                                <div class="col-md-6">
                                    <span>จ่ายเงินเข้าบัญชี</span>
                                    <dx:ASPxComboBox ID="cboPayToBook_TR" 
                                        Theme="Material" 
                                        runat="server"
                                        
                                        DropDownStyle="DropDownList" 
                                        DropDownHeight="300" 
                                        DropDownWidth="600"
                                        ValueField="BookID" 
                                        ValueType="System.String" 
                                        TextFormatString="{0} {1} {2}" 
                                        Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="CompanyID" Caption="Company" Width="100px" />
                                            <dx:ListBoxColumn FieldName="BookNo" Caption="เลขบัญชี" Width="250px" />
                                            <dx:ListBoxColumn FieldName="BankName" Caption="ธนาคาร" Width="100px" />
                                        </Columns>
                                        <ItemStyle>
                                            <SelectedStyle BackColor="Red">
                                            </SelectedStyle>
                                        </ItemStyle>
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-6" runat="server" id="divRefNo_m">
                                    <span>เลขบัญชีผู้จ่าย</span>
                                    <asp:TextBox ID="txtRefNo_TR" class="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row ">
                                <div class="col-md-6"  >
                                    <span>ธนาคารบัญชีผู้จ่าย</span>
                                    <dx:ASPxComboBox ID="cboCustomerBank_TR" 
                                        runat="server" 
                                        Theme="Material" 
                                         
                                        DropDownStyle="DropDownList" 
                                        DropDownHeight="300" 
                                        DropDownWidth="600"
                                        ValueField="BankCode" 
                                        ValueType="System.String" 
                                        TextFormatString="{0} {1}" Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="BankCode" Caption="รหัสธนาคาร" Width="100px" />
                                            <dx:ListBoxColumn FieldName="Name_TH" Caption="ชือธนาคาร" Width="250px" />
                                        </Columns>
                                        <ItemStyle>
                                            <SelectedStyle BackColor="Red">
                                            </SelectedStyle>
                                        </ItemStyle>
                                    </dx:ASPxComboBox>
                                </div>
                                <div class="col-md-6" >
                                    <span>สาขาธนาคารบัญชีผู้จ่าย</span>
                                    <asp:TextBox ID="txtCustomerBankBranch_TR" class="form-control form-control-sm" runat="server" placeholder=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row pb-2" runat="server" id="divpaychq" >
        <div class="col-lg-8 mx-auto">
            <div class="card">
                <div class="card-header ">
                    <asp:Label runat="server" ID="lblShowTotalinvoice_chq"></asp:Label>
                </div>

                <div class="card-body">
                    <div class="row  pt-1 pb-1" style="font-size: smaller">
                        <div class="col-md-3">
                            <span><%= SetCaption("PAYMENT") %></span>
                            <asp:TextBox ID="txtPayAmt_CHQ" 
                                runat="server"
                                style="text-align:right"
                                BackColor="LemonChiffon"
                                Font-Size="X-Large"
                                class="form-control form-control-lg"
                                onkeypress="return DigitOnly(this,event)">
                            </asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <span>​​​วันรับเช็ค</span>
                            <dx:ASPxDateEdit ID="dtPayDate_CHQ" 
                                runat="server" 
                                Theme="Material" 
                                Width="100%"                            
                                DisplayFormatString="dd-MM-yyyy" 
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div> 
                    </div>

                    <div class="row  pt-1 pb-1" style="font-size: smaller">
                        <div class="col-md-6">
                            <span>ฝากเช็คเข้าบัญชี</span>
                            <dx:ASPxComboBox ID="cboPayToBook_CHQ" 
                                Theme="Material" runat="server"  
                                DropDownStyle="DropDownList" 
                                     CssClass="Sarabun"
                                DropDownHeight="300"
                                DropDownWidth="600"
                                ValueField="BookID" 
                                ValueType="System.String" 
                                TextFormatString="{0} {1} {2}" 
                                Width="100%">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="CompanyID" Caption="รหัสธนาคาร" Width="100px" />
                                    <dx:ListBoxColumn FieldName="BookNo" Caption="เลขบัญชี" Width="250px" />
                                    <dx:ListBoxColumn FieldName="BankName" Caption="ธนาคาร" Width="100px" />
                                </Columns>
                                <ItemStyle>
                                    <SelectedStyle BackColor="Red">
                                    </SelectedStyle>
                                </ItemStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-6">
                            <span>เลขที่เช็ค</span>
                            <asp:TextBox ID="txtRefNo_CHQ" class="form-control form-control-sm" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row  pt-1 pb-1" style="font-size: smaller">
                        <div class="col-md-6">
                            <span>เช็คธนาคาร</span>
                            <dx:ASPxComboBox ID="cboCustomerBank_CHQ"
                                runat="server" 
                                Theme="Material" 
                                 
                                DropDownStyle="DropDownList" 
                                DropDownHeight="300" 
                                DropDownWidth="600"
                                ValueField="BankCode" 
                                ValueType="System.String" 
                                TextFormatString="{0} {1}" 
                                Width="100%">
                                <Columns>
                                    <dx:ListBoxColumn FieldName="BankCode" Caption="รหัสธนาคาร" Width="100px" />
                                    <dx:ListBoxColumn FieldName="Name_TH" Caption="ชือธนาคาร" Width="250px" />
                                </Columns>
                                <ItemStyle>
                                    <SelectedStyle BackColor="Red">
                                    </SelectedStyle>
                                </ItemStyle>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4">
                            <span>เช็คสาขาธนาคาร</span>
                            <asp:TextBox ID="txtCustomerBankBranch_CHQ"
                                class="form-control " 
                                runat="server" 
                                placeholder="เช็คสาขาธนาคาร">
                            </asp:TextBox>
                        </div>
                   
                    </div>

                    <div class="row  pt-1 pb-1" style="font-size: smaller">
                        <div class="col-md-3">
                            <span>วันที่หน้าเช็ค</span>
                            <dx:ASPxDateEdit ID="dtChqDate_CHQ" 
                                runat="server" 
                                Theme="Material" 
                                Width="100%"
                                DisplayFormatString="dd-MM-yyyy" 
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-3">
                            <span>วันที่ฝากเช็ค</span>
                            <dx:ASPxDateEdit ID="dtChqDepositDate_CHQ" 
                                runat="server" 
                                Theme="Material" 
                                Width="100%"
                                DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-3">
                            <span>วันที่ Statement​​​</span>
                            <dx:ASPxDateEdit ID="dtStatementDate_CHQ" 
                                runat="server" Theme="Material" Width="100%"
                                DisplayFormatString="dd-MM-yyyy" 
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-3">
                            <span>วันที่เงินใช้ได้</span>
                            <dx:ASPxDateEdit ID="dtClearingDate_CHQ" 
                                runat="server"
                                Theme="Material" 
                                Width="100%"
                                DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                    </div>

                    <div class="row  pt-1 pb-1" id="divchq_expire" runat="server" style="font-size: smaller">
                        <div class="col-md-6">
                            <span>เช็คหมดอายุ</span>
                            <dx:ASPxDateEdit ID="dtChqExpired_CHQ" 
                                runat="server" 
                                Theme="Material" 
                                Width="100%"
                                DisplayFormatString="dd-MM-yyyy" 
                                EditFormatString="dd-MM-yyyy">
                                <TimeSectionProperties Visible="False">
                                    <TimeEditCellStyle HorizontalAlign="Right">
                                    </TimeEditCellStyle>
                                </TimeSectionProperties>
                            </dx:ASPxDateEdit>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>


    <div class="row pt-2  ">
        <div class="col-md-12 pl-4 pt-1 ">
            <div class="row">
                <div class="col-md-12 text-center">
                    <div class="btn-group" role="group" aria-label="Basic example">
                        <asp:LinkButton ID="btnBack" runat="server" Text="" CssClass="btn btn-danger" OnClick="btnBack_Click">
                        <i class="fas fa-backward"></i>
                        <span >กลับ </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnsave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click">
                                    <i class="fas fa-check-circle"></i>&nbsp<span >ตกลง</span> 
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row pt-4 pb-3">
        <div class="col-lg-8 mx-auto">
            <div id="divalert" style="display: none" class="alert alert-success" role="alert"> 
                <strong>
                    <span id="myalertHead" ></span></strong>
                <br />
                <span id="myalertBody" ></span>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
