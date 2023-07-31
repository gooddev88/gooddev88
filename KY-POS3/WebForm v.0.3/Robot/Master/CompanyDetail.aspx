<%@ Page Title="Company info" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="CompanyDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.Master.CompanyDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddcopy" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />
    <asp:HiddenField ID="hddtaxid" runat="server" />
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

        //BEGIN Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK':
                    btnPostCode.DoClick();
                    popFile.Hide();
                    btnShowListPost.DoClick();
                    break;
                case 'Cancel':
                    popup.Hide();
                    popMessage.Hide();
                    break;
            }
        }

        //END Pop Postback

        //popup show
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }

        //begin prevent link double click
        var isSubmitted = false;
        function preventMultipleSubmissions() {
            if (!isSubmitted) {
                $('#<%=btnSave2.ClientID %>').val('Submitting.. Plz Wait..');
                isSubmitted = true;
                return true;
            }
            else {
                return false;
            }
        }
        //end prevent link double click

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

        function mypopprofile() {
            btnPostProfile.DoClick();
        }

        function OnClosePopupAlert() {
            popAlert.Hide();
        }
        //end show msg
    </script>

    <script type="text/javascript">

        function SelectRadioButton(regexPattern, selectedRadioButton) {
            regex = new RegExp(regexPattern);
            for (i = 0; i < document.forms[0].elements.length; i++) {
                element = document.forms[0].elements[i];
                if (element.type == 'radio' && element.name.toString().endsWith(regexPattern)) {
                    element.checked = false;
                }
            }
            selectedRadioButton.checked = true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="udpmain" runat="server">
        <ContentTemplate>

            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="600" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
                HeaderText="Information" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
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
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="Close" OnClientClick="return OnClosePopupAlert();" />
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  " runat="server" Text="CANCEL" Visible="false" />
                                            </div>
                                        </div>
                                    </div>

                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>

            <dx:ASPxPopupControl ID="popFile" runat="server" AllowDragging="True" AllowResize="True"
                CloseAction="CloseButton"
                ShowMaximizeButton="true"
                EnableViewState="False"
                PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter"
                ShowCloseButton="True"
                Width="1400"
                Height="700"
                ShowOnPageLoad="True"
                HeaderText=""
                FooterText=""
                ShowFooter="False"
                ClientInstanceName="popFile"
                EnableHierarchyRecreation="True"
                FooterStyle-Wrap="True" CloseOnEscape="True" Modal="True" Theme="Mulberry">
                <ContentStyle Paddings-Padding="0" />
                <ClientSideEvents Shown="onPopupShown" />
            </dx:ASPxPopupControl>

            <div class="row">
                <div class="col-md-9 mx-auto">
                    <div class="row pt-2 pb-2">
                        <div class="col-md-12 px-0">
                            <asp:LinkButton ID="btnBackList" runat="server" Width="100%" CssClass="btn btn-outline-dark" Height="40"
                                OnClick="btnBackList_Click">
                       <i class="fas fa-chevron-square-left"></i>
                       <span >Back </span>
                            </asp:LinkButton>
                        </div>
                    </div>

                    <div class="row ">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row pb-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card">
                                                <div class="card-header pt-2 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10"><i class="fas fa-user-circle fa-2x"></i>&nbsp<span >ข้อมูล สาขา</span></div>
                                                        <div class="col-md-2 pt-1 text-right">
                                                            <span style="font-size: large;">
                                                                <asp:CheckBox ID="chkActive"  Text="Active" runat="server" AutoPostBack="true" OnCheckedChanged="chkActive_CheckedChanged" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row ">
                                                        <div class="col-md-8">
                                                            <div class="row " style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>รหัสสาขา</span>
                                                                    <asp:TextBox ID="txtCompanyID" CssClass="form-control form-control-sm" runat="server" placeholder="***NEW***"  style="text-align:center" ></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>Tax ID &nbsp</span>
                                                                    <asp:TextBox ID="txtTaxID" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row pt-2 " style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>ชื่อร้าน &nbsp</span><span style="color: red">*</span>
                                                                    <asp:TextBox ID="txtName1" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>ชื่อสาขา &nbsp</span>
                                                                    <asp:TextBox ID="txtName2" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row pt-2 " style="font-size: small;" runat="server" >
                                                                <div class="col-md-6">
                                                                    <span>อักษรย่อ &nbsp</span><span style="color: red">*</span>
                                                                    <asp:TextBox ID="txtShortCode" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                                   <div class="col-md-6">
                                                                    <span>เลขสาขา &nbsp</span><span style="color: red">*</span>
                                                                    <asp:TextBox ID="txtBrnCode" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                              <%--  <div class="col-md-6">
                                                                    <span>สำนักงานใหญ่</span>
                                                                    <asp:DropDownList ID="cboCompany" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Name" DataValueField="CompanyID"></asp:DropDownList>
                                                                </div>--%>
                                                            </div>

                                                            <div class="row  pb-1" style="font-size: small;">
                                                                <div class="col-md-12 ">
                                                                    <span>หมายเหตุ 1 &nbsp</span>
                                                                    <asp:TextBox ID="txtRemark1" CssClass="form-control form-control-sm" TextMode="MultiLine" Height="60px" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row  pb-1" style="font-size: small;">
                                                                <div class="col-md-12 ">
                                                                    <span>หมายเหตุ 2 &nbsp</span>
                                                                    <asp:TextBox ID="txtRemark2" CssClass="form-control form-control-sm" TextMode="MultiLine" Height="60px" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row pb-1" style="font-size: small;">
                                                                <div class="col-md-12">
                                                                    <asp:CheckBox ID="ckIsWH" Text="DC" runat="server" />
                                                                </div>
                                                            </div>

                                                            <div class="row pt-5 ">
                                                                <div class="col-md-6">
                                                                    <asp:LinkButton ID="btnSave2" Width="100%" runat="server"
                                                                        CssClass="btn btn-outline-success" Font-Size="Small"
                                                                        OnClick="btnSave2_Click">
                                                                    <i class="fas fa-check-circle fa-2x"></i>&nbsp
                                                                        <span >บันทึก</span>
                                                                    </asp:LinkButton>
                                                                    &nbsp                                                                    
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <asp:LinkButton ID="btnNew" Width="100%" runat="server"
                                                                        CssClass="btn btn-outline-info" Font-Size="Small"
                                                                        OnClick="btnNew_Click">
                                                                    <i class="fas fa-plus-circle fa-2x"></i>&nbsp
                                                                        <span >***NEW***</span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>

                                                        </div>

                                                        <div class="col-md-4 pt-3">
                                                            <div class="row pb-1 " runat="server" id="divProfile">
                                                                <div class="col-md-12">
                                                                    <div class="card">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <dx:ASPxButton ID="btnPostProfile" runat="server" ClientInstanceName="btnPostProfile" ClientVisible="false"
                                                                                        OnClick="btnPostProfile_Click">
                                                                                    </dx:ASPxButton>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <asp:UpdatePanel ID="udpProfile" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <div class="row">
                                                                                                <div class="col-md-12">
                                                                                                    <asp:Image ID="imgProfile" runat="server" ImageAlign="Middle" Style="width: 100%;" />
                                                                                                    <%--Style="max-height: 260px; max-width: 140px; height: auto; width: auto;"--%>
                                                                                                </div>
                                                                                            </div>
                                                                                            <div class="row">
                                                                                                <div class="col-md-12 text-right">
                                                                                                    <div class="btn-group" role="group" aria-label="Basic example">
                                                                                                        <asp:LinkButton ID="btnUploadProfile" runat="server" OnClick="btnUploadProfile_Click">
                                                                                                        <span style="color: dimgray;">   <i class="fas fa-upload"></i></span>
                                                                                                        </asp:LinkButton>
                                                                                                        &nbsp  &nbsp
                                                                                                        <asp:LinkButton ID="btnRemoveProfile" runat="server" OnClick="btnRemoveProfile_Click">
                                                                                                         <span style="color: dimgray;">  <i class="far fa-trash-alt"></i></span>
                                                                                                        </asp:LinkButton>
                                                                                                    </div>
                                                                                                </div>
                                                                                                <dx:ASPxPopupControl ID="popprofile" runat="server" Theme="Mulberry"
                                                                                                    CloseAction="CloseButton" ShowCloseButton="false"
                                                                                                    EnableViewState="False"
                                                                                                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                                                                                    AllowDragging="true"
                                                                                                    MinWidth="800px" MinHeight="600px"
                                                                                                    HeaderText="" ClientInstanceName="popprofile" EnableHierarchyRecreation="True">
                                                                                                    <ContentStyle Paddings-Padding="0" />
                                                                                                    <ClientSideEvents Shown="onPopupShown" />
                                                                                                </dx:ASPxPopupControl>
                                                                                            </div>
                                                                                        </ContentTemplate>
                                                                                        <Triggers>
                                                                                            <asp:AsyncPostBackTrigger ControlID="btnPostProfile" />
                                                                                        </Triggers>
                                                                                    </asp:UpdatePanel>
                                                                                </div>
                                                                            </div>

                                                                        </div>

                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card">
                                                <div class="card-header pt-2 pb-1" style="background-color: mediumslateblue; color: white">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <i class="fas fa-home fa-2x"></i>
                                                            &nbsp<span >ข้อมูลการติดต่อ</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <asp:UpdatePanel ID="udpAddrCustomer" UpdateMode="Conditional" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row " style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>ค้นหาที่อยู่</span>
                                                                    <dx:ASPxComboBox ID="cboComAddr" runat="server"
                                                                        
                                                                        AutoPostBack="true"
                                                                        EnableCallbackMode="true"
                                                                        CallbackPageSize="10"
                                                                        Theme="Material"
                                                                        Width="100%"
                                                                        DropDownWidth="600"
                                                                        DropDownHeight="300"
                                                                        OnSelectedIndexChanged="cboComAddr_SelectedIndexChanged"
                                                                        ValueType="System.String" ValueField="ID"
                                                                        OnItemsRequestedByFilterCondition="cboComAddr_OnItemsRequestedByFilterCondition_SQL"
                                                                        OnItemRequestedByValue="cboComAddr_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                                                                        DropDownStyle="DropDownList">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="FULLADDR" Caption="Address" Width="600px" />
                                                                        </Columns>
                                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="row  pt-1" style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>เลขที่ (House No.)</span>
                                                                    <asp:TextBox ID="txtAddrNo" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>หมู่/ถนน (Village No. / Road)</span>
                                                                    <asp:TextBox ID="txtAddrMoo" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row   pt-1" style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>รหัสไปรษณีย์ (Postal Code)</span>
                                                                    <asp:TextBox ID="txtAddrPostCode" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>ตำบล/แขวง (Sub-district / Sub-area)</span>
                                                                    <asp:TextBox ID="txtAddrTumbon" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row   pt-1" style="font-size: small;">
                                                                <div class="col-md-6">
                                                                    <span>เขต/อำเภอ (District / Area)</span>
                                                                    <asp:TextBox ID="txtAddrAmphoe" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>จังหวัด (Province)</span>
                                                                    <asp:TextBox ID="txtAddrProvince" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cboComAddr" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <div class="row " style="font-size: small;">
                                                        <div class="col-md-6">
                                                            <span>เบอร์โทรศัพท์</span><span style="color: red;">*</span>
                                                            <asp:TextBox ID="txtMobile" MaxLength="10" onkeypress="return DigitOnly(this,event)"
                                                                CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>Email</span>
                                                            <asp:TextBox ID="txtEmail" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row pt-1">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <asp:UpdatePanel ID="udpacc" runat="server">
                                                <ContentTemplate>
                                                    <div class="card">
                                                        <div class="card-header pt-2 pb-1" style="background-color: deepskyblue">
                                                            <div class="row">
                                                                <div class="col-md-8">
                                                                    <i class="fas fa-dollar-sign fa-2x"></i>
                                                                    &nbsp<span >ข้อมูลทางบัญชี</span>
                                                                </div>
                                                                <div class="col-md-4 text-right">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body">
                                                              <div class="row pb-1"  >
                                                                   <div class="col-12 ">
                                                                       <h2><strong>Bank</strong></h2>
                                                                       </div>
                                                                 </div>
                                                            <div class="row pb-1" style="font-size: small">
                                                                <div class="col-md-6 ">
                                                                    <span>ธนาคาร </span>
                                                                    <asp:DropDownList ID="cboBankCode" runat="server" CssClass="form-control form-control-sm " DataTextField="Name_TH" DataValueField="BankCode"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-6 ">
                                                                    <span>เลขที่บัญชี </span>
                                                                    <asp:TextBox ID="txtBookBankID" CssClass="form-control form-control-sm " runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                                  <div class="row pb-1" style="font-size: small">
                                                                <div class="col-md-12 ">
                                                                     <span>ชื่อบัญชี </span>
                                                                    <asp:TextBox ID="txtBookName" CssClass="form-control form-control-sm " runat="server"></asp:TextBox>
                                                                    </div>
                                                                      </div>
                                                             <div class="row pb-1 pt-2">
                                                                   <div class="col-12 ">
                                                                       <h2><strong>PromptPay</strong></h2>
                                                                       </div>
                                                                 </div>
                                                                  <div class="row pb-1" style="font-size: small">
                                                                <div class="col-md-6 ">
                                                                    <span>ประเภท </span>
                                                                    <asp:DropDownList ID="cboPromtpayType" runat="server" CssClass="form-control form-control-sm " DataTextField="Description" DataValueField="Value"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-6 ">
                                                                    <span>เบอร์มือถือ / เลขบัตรประชาชน / E-Wallet </span>
                                                                    <asp:TextBox ID="txtPromptpay" CssClass="form-control form-control-sm " runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row pb-1" style="font-size: small">
                                                                <div class="col-md-12">
                                                                    <span>QrPaymentData</span>
                                                                    <asp:TextBox ID="txtQrPaymentData"
                                                                        CssClass="form-control"
                                                                        Font-Size="Small"
                                                                        TextMode="MultiLine" Height="60"
                                                                        runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                              <div class="row pb-1 pt-2">
                                                                   <div class="col-12 ">
                                                                       <h2><strong>Vat</strong></h2>
                                                                       </div>
                                                                 </div>
                                                               <div class="row pb-1 pt-2" style="font-size: small">
                                                                <div class="col-md-6 pt-3 ">
                                                                    <asp:CheckBox runat="server" Font-Size="Larger" ID="ckIsVatRegister" Text="จดทะเบียน Vat" />
                                                                </div>
                                                                <div class="col-md-6 ">
                                                                    <span>คำนวณภาษี </span>
                                                                    <asp:DropDownList ID="cboPriceTaxcon" runat="server" CssClass="form-control form-control-sm " DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row pb-1" style="font-size: small">
                                                                <div class="col-md-6">
                                                                    <span>ที่อยู่เปิดบิล 1</span>
                                                                    <asp:TextBox ID="txtBillAddr1"
                                                                        CssClass="form-control"
                                                                        Font-Size="Small"
                                                                        TextMode="MultiLine" Height="60"
                                                                        runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span>ที่อยู่เปิดบิล 2</span>
                                                                    <asp:TextBox ID="txtBillAddr2"
                                                                        CssClass="form-control"
                                                                        Font-Size="Small"
                                                                        TextMode="MultiLine" Height="60"
                                                                        runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                         

                                                        </div>
                                                    </div>


                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>

                                    <div class="row pt-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card-header pt-2 pb-1"
                                                style="background-color: darkgoldenrod; color: white">
                                                <div class="row">
                                                    <div class="col-md-8"><i class="fas fa-home-heart fa-2x"></i>&nbsp<span>สถานที่เก็บ</span></div>
                                                    <div class="col-md-4 text-right">
                                                    </div>
                                                </div>
                                            </div>

                                            <dx:ASPxGridView ID="grdLocation"
                                                Width="100%"
                                                runat="server"
                                                AutoGenerateColumns="False"
                                                KeyFieldName="CutomerID"
                                                CssClass="Sarabun"
                                                Theme="Material">
                                                <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" />
                                                <Columns>
                                                    <dx:GridViewDataTextColumn FieldName="CompanyID" Caption="สาขา" Width="120px">
                                                        <EditFormSettings Visible="False" />
                                                        <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                                        <CellStyle Wrap="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="LocID" Caption="LocID" Width="120px">
                                                        <EditFormSettings Visible="False" />
                                                        <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                                        <CellStyle Wrap="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataTextColumn FieldName="Name" Caption="LocName" Width="200px">
                                                        <EditFormSettings Visible="False" />
                                                        <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                                        <CellStyle Wrap="False" />
                                                    </dx:GridViewDataTextColumn>
                                                    <dx:GridViewDataCheckColumn FieldName="IsActive" Caption="ใช้งาน" Width="100">
                                                        <HeaderStyle CssClass="Sarabun"></HeaderStyle>
                                                        <CellStyle Wrap="False">
                                                        </CellStyle>
                                                    </dx:GridViewDataCheckColumn>
                                                </Columns>
                                            </dx:ASPxGridView>

                                        </div>
                                    </div>

                                    <div class="row pt-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="card-header pt-2 pb-1"
                                                        style="background-color: cadetblue; color: white">
                                                        <div class="row">
                                                            <div class="col-md-8"><i class="fas fa-couch fa-2x"></i>&nbsp<span>โต๊ะ</span></div>
                                                            <div class="col-md-4 text-right">
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="card-body bg-white">
                                                        <div class="row">
                                                            <div class="col-md-2">
                                                                <span>หมายเลขโต๊ะ</span>
                                                                <asp:TextBox ID="txtTableID" Font-Size="Small" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <span>ชื่อโต๊ะ</span>
                                                                <asp:TextBox ID="txtTableName" Font-Size="Small" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <span>จัดเรียง</span>
                                                                <asp:TextBox ID="txtSort" CssClass="form-control form-control-sm" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2 text-left pt-3">
                                                                <span style="color: navy;"></span>
                                                                <asp:LinkButton ID="btnTable" Width="100%" runat="server" CssClass="btn btn-success" OnClick="btnAddTable_Click">
                                                             <i class="fa fa-save"></i>&nbsp<span >เพิ่ม</span> 
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>

                                                        <div class="row pt-2">
                                                            <div class="col-12">
                                                                <dx:ASPxGridView ID="grdPOSTable"
                                                                    Width="100%"
                                                                    runat="server"
                                                                    AutoGenerateColumns="False"
                                                                    KeyFieldName="TableID"
                                                                    CssClass="Sarabun"
                                                                    OnDataBinding="grdPOSTable_DataBinding"
                                                                    OnRowCommand="grdPOSTable_RowCommand"
                                                                    Theme="Material">
                                                                    <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" />
                                                                    <Columns>
                                                                        <dx:GridViewDataTextColumn FieldName="" Caption="ลบ" Width="80px">
                                                                            <DataItemTemplate>
                                                                                <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-icons btn-default"
                                                                                    CommandName="Del" CommandArgument='<%# Eval("TableID") %>'>
                                                                                <i class="fas fa-trash"></i>
                                                                                </asp:LinkButton>
                                                                            </DataItemTemplate>
                                                                            <HeaderStyle></HeaderStyle>
                                                                        </dx:GridViewDataTextColumn>
                                                                  
                                                                        <dx:GridViewDataTextColumn FieldName="TableID" Caption="หมายเลขโต๊ะ" Width="130px">
                                                                            <EditFormSettings Visible="False" />
                                                                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                                                            <CellStyle Wrap="False" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn FieldName="TableName" Caption="ชื่อโต๊ะ" Width="200px">
                                                                            <EditFormSettings Visible="False" />
                                                                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                                                            <CellStyle Wrap="False" />
                                                                        </dx:GridViewDataTextColumn>
                                                                        <dx:GridViewDataTextColumn Caption="ลำดับ" FieldName="Sort" Width="120px">
                                                                            <HeaderStyle Wrap="False" />
                                                                            <PropertiesTextEdit DisplayFormatString="n0" />
                                                                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                                        </dx:GridViewDataTextColumn>
                                                                    </Columns>
                                                                </dx:ASPxGridView>
                                                            </div>
                                                        </div>
                                                    </div>

                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                    </div>

                                    <div class="row pt-2 pb-2">
                                        <div class="col-md-11 mx-auto">
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

                                </div>
                            </div>

                            <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                        </div>
                    </div>

                    <div class="row pt-2" hidden="hidden">
                        <asp:LinkButton ID="btnDel" Visible="false" runat="server" CssClass="btn btn-outline-danger" OnClick="btnDel_Click">
                        <i class="fas fa-trash"></i>&nbsp<span >Delete</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="row pt-2">
                <div class="col-md-9 px-0 mx-auto">
                    <div class="row">
                        <div class="col-md-12">
                            <button type="button" class="btn btn-secondary btn-block btn-fw  fa-2x">
                                <i class="far fa-clock"></i><span >&nbsp ประวัติการแก้ไข</span></button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div style="overflow-x: auto; width: 100%">
                                <asp:GridView ID="grd_transaction_log" runat="server"
                                    AutoGenerateColumns="False" Width="100%"
                                    Font-Size="Small" OnPageIndexChanging="grd_transaction_log_PageIndexChanging"
                                    CssClass="table table-striped table-bordered table-hover"
                                    EmptyDataText="No transaction log data.."
                                    EmptyDataRowStyle-HorizontalAlign="Center"
                                    ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10">
                                    <Columns>
                                        <asp:BoundField DataField="CreatedBy" HeaderText="Action By" >
                                            <HeaderStyle Wrap="False"  />
                                            <ItemStyle Wrap="False"  />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="CreatedDate" HeaderText="Action Date" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}"  >
                                            <HeaderStyle Wrap="False"  />
                                            <ItemStyle Wrap="False"  />
                                        </asp:BoundField>

                                        <asp:BoundField DataField="Action" HeaderText="Action" >
                                            <HeaderStyle Wrap="False"  />
                                            <ItemStyle Wrap="False"  />
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
         <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
