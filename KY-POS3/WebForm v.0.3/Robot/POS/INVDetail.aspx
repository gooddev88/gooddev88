<%@ Page Title="O Invoice" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="INVDetail.aspx.cs" Inherits="Robot.POS.INVDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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


        //popup show 
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }
        //begin Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK-Line':
                    popFile.Hide();

                    // btnRefreshX.DoClick();
                    btnLoadLine.DoClick();
                    break;
                case 'Cancel-Line':
                    popFile.Hide();
                    break;
                case 'OK-List':
                    btnLoadLine.DoClick();
                    break;
            }
        }

        //begin Popup Postback
        function OnClosePopupAlert() {
            popAlert.Hide();
        }

    </script>
</asp:Content>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddInvId" runat="server" />
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAddline" />
        </Triggers>
    </asp:UpdatePanel>



    <asp:UpdateProgress ID="udppPost" runat="server" AssociatedUpdatePanelID="udpPost">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>


    <dx:ASPxButton ID="btnLoadLine" runat="server" ClientInstanceName="btnLoadLine" ClientVisible="false"
        OnClick="btnLoadLine_Click">
    </dx:ASPxButton>

    <div class="row pb-2  pt-2 pb-1" style="background-color: darkkhaki">
        <div class="col-md-2">
            <asp:LinkButton ID="btnBack" runat="server" Width="100%" Text="" CssClass="btn btn-default" OnClick="btnBack_Click">
     <i class="fas fa-reply-all"  ></i>
           <span >O Invoice</span>
            </asp:LinkButton>
        </div>
        <div class="col-md-6">
            <asp:UpdatePanel ID="udpPost" runat="server">
                <ContentTemplate>
                    <div class="btn-group" runat="server" id="divDataFunction" role="group" aria-label="Button group with nested dropdown">
                        <asp:Button ID="btnSave"
                            CssClass="btn btn-default"
                            OnClick="btnSave_Click"
                            OnClientClick="this.disabled='true';"
                            UseSubmitBehavior="false"
                            runat="server"
                            Text="บันทึก"></asp:Button>

                        <div class="btn-group" role="group">
                            <button id="btnGroupDrop1" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span>เพิ่มเติม</span>
                            </button>
                            <div class="dropdown-menu" aria-labelledby="btnGroupDrop1" runat="server">
                                <asp:LinkButton ID="btnPrintOINV1" runat="server" Width="250px" CssClass="btn btn-default text-left" OnClick="btnPrintOINV1_Click">
                                    <i class="far fa-print"></i>&nbsp<span >พิมพ์อินวอยซ์</span> 
                                </asp:LinkButton>
                            </div>
                            <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                <asp:LinkButton ID="btnRefreshDoc" runat="server" class="dropdown-item" OnClick="btnRefreshDoc_Click" Visible="false">
                                     <span style="color:chocolate"> <i class="fas fa-sync-alt"></i>  </span>Refresh  </asp:LinkButton>
                            </div>
                        </div>
                        <div class="btn-group" role="group">
                            <button id="btnGroupDrop2" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span>ลบ</span>
                            </button>
                            <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-default" OnClick="btnDel_Click">
                                    <i class="fas fa-trash"></i>&nbsp<span >ลบ</span> 
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="col-md-4 text-right">
            <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                <div class="btn-group" role="group">
                    <button id="btnGroupmore" type="button" class="btn btn-dark dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Task
                    </button>
                    <div class="dropdown-menu " aria-labelledby="btnGroupmore" runat="server" hidden="hidden">
                        <asp:LinkButton ID="btnNewDoc" runat="server" class="dropdown-item" OnClick="btnNewDoc_Click"> สร้าง</asp:LinkButton>
                        <asp:LinkButton ID="btnCopy" runat="server" class="dropdown-item" OnClick="btnCopy_Click"> ก็อปปี้ </asp:LinkButton>
                    </div>
                </div>
                <dx:ASPxComboBox ID="cboDocLine" runat="server"
                    CssClass="Sarabun"
                    Theme="Material"
                    EnableCallbackMode="true"
                    AutoPostBack="true"
                    Font-Size="Medium"
                    DropDownWidth="600" DropDownHeight="300"
                    OnSelectedIndexChanged="cboDocLine_SelectedIndexChanged"
                    ValueType="System.String" ValueField="ID"
                    OnItemsRequestedByFilterCondition="cboDocLine_OnItemsRequestedByFilterCondition_SQL"
                    OnItemRequestedByValue="cboDocLine_OnItemRequestedByValue_SQL" TextFormatString="{0}{1}{2}"
                    DropDownStyle="DropDownList">
                    <Columns>
                        <dx:ListBoxColumn FieldName="SOINVID" Caption="เลขอินวอยซ์" />
                        <dx:ListBoxColumn FieldName="CustomerID" Caption="รหัสลูกค้า" />
                        <dx:ListBoxColumn FieldName="CustomerName" Caption="ชื่อลูกค้า" />
                    </Columns>
                    <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                </dx:ASPxComboBox>

                <asp:LinkButton ID="btnBackward" runat="server" CssClass="btn btn-secondary" OnClick="btnBackward_Click"> <i class="fas fa-step-backward"></i></asp:LinkButton>
                <asp:LinkButton ID="btnforward" runat="server" CssClass="btn btn-secondary" OnClick="btnforward_Click"> <i class="fas fa-step-forward"></i></asp:LinkButton>

            </div>
        </div>
    </div>



    <div class="card-header pt-1 pb-1">
        <div class="row pl-1 pr-1">
            <div class="col-md-6">
                <ul class="nav nav-pills card-header-pills" role="tablist">
                    <li class="nav-item"></li>
                    <li class="nav-item ">
                        <a class="nav-link active " id="a_tab_home" data-toggle="tab" href="#tab_home" role="tab" aria-controls="c_tab_home" aria-selected="true"><span>ข้อมูล</span></a>
                    </li>
                    <li class="nav-item" runat="server" id="tab_moreinfoX">
                        <a class="nav-link" id="a_tab_moreinfo" data-toggle="tab" href="#tab_moreinfo" role="tab" aria-controls="c_tab_moreinfo" aria-selected="false"><span>ข้อมูลเพิ่มเติม</span></a>
                    </li>
                    <li class="nav-item" runat="server" id="tab_dochistoryX">
                        <a class="nav-link" id="a_tab_dochistory" data-toggle="tab" href="#tab_dochistory" role="tab" aria-controls="c_tab_dochistory" aria-selected="false"><span>ประวัติ</span></a>
                    </li>
                </ul>
            </div>
            <div class="col-md-6 text-right ">
                <h4>
                    <asp:Literal ID="litStatus" runat="server"></asp:Literal></h4>
            </div>

        </div>
    </div>

    <div class="row pt-2">
        <div class="col-md-12">
            <div class="tab-content tab-content-solid">
                <%--begin tab general--%>
                <div class="tab-pane fade show active" id="tab_home" role="tabpanel" aria-labelledby="c_tab_home">
                    <div class="row pl-1 ">
                        <div class="col-md-9 pl-2">
                            <div class="row">
                                <div class="col-md-6 pr-0">
                                    <div class="card">
                                        <div class="card-header pt-2 pb-1">
                                            <div class="row">
                                                <div class="col-md-10">
                                                    <span><i class="far fa-folder-open"></i>&nbsp รายละเอียดเอกสาร</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body pt-1 pb-2">

                                            <div class="row ">
                                                <div class="col-md-6">
                                                    <span style="font-size: small">เลขอินวอยซ์</span>
                                                    <asp:TextBox ID="txtID"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server" placeholder="++NEW++"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <span style="font-size: small">วันที่อินวอยซ์</span>
                                                    <dx:ASPxDateEdit ID="dtDocumentDate"
                                                        CssClass="Sarabun"
                                                        Theme="Material"
                                                        DisplayFormatString="dd-MM-yyyy"
                                                        EditFormatString="dd-MM-yyyy"
                                                        runat="server"
                                                        Width="100%">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>

                                            <div class="row pt-1">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">สาขา  </span>
                                                    <dx:ASPxComboBox ID="cboCompany" runat="server"
                                                        DropDownStyle="DropDownList"
                                                        AutoPostBack="true"
                                                        CssClass="Sarabun"
                                                        Theme="Material" 
                                                        ValueField="CompanyID" ValueType="System.String" DropDownWidth="500"
                                                        ViewStateMode="Enabled" TextFormatString="{0} {1}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="CompanyID" Caption="รหัส" />
                                                            <dx:ListBoxColumn FieldName="Name" Caption="ชื่อ" Width="300" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>

                                            <div class="row pt-1">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">รหัสลูกค้า </span>
                                                    <dx:ASPxComboBox ID="cboDocTo"
                                                        runat="server"
                                                        Theme="Material"
                                                        CssClass="Sarabun"
                                                        AutoPostBack="true"
                                                        EnableCallbackMode="true"
                                                        DropDownWidth="600"
                                                        DropDownHeight="300"
                                                        OnSelectedIndexChanged="cboDocTo_SelectedIndexChanged"
                                                        ValueType="System.String" ValueField="CustomerID"
                                                        OnItemsRequestedByFilterCondition="cboDocTo_OnItemsRequestedByFilterCondition_SQL"
                                                        OnItemRequestedByValue="cboDocTo_OnItemRequestedByValue_SQL" TextFormatString="{0} {1}"
                                                        Width="100%"
                                                        DropDownStyle="DropDownList">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="CustomerID" Caption="Customer" />
                                                            <dx:ListBoxColumn FieldName="NameDisplay" Caption="Name" Width="400px" />
                                                        </Columns>
                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="row pt-1">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">ชื่อลูกค้า</span>
                                                    <asp:TextBox ID="txtCustomerName"
                                                        CssClass="form-control form-control-sm" Font-Size="Small" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">เลขใบสั่งซื้อ</span>
                                                    <asp:TextBox ID="txtPONo" CssClass="form-control form-control-sm"
                                                        Font-Size="Smaller" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row pt-1">
                                                <div class="col-md-5">
                                                    <span style="font-size: small">เลขประจำตัวผู้เสียภาษี</span>
                                                    <asp:TextBox ID="txtCustTaxID" CssClass="form-control form-control-sm" Font-Size="Smaller" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <span style="font-size: small">Credit Term</span>
                                                    <dx:ASPxComboBox ID="cboCreditTerm"
                                                        runat="server"
                                                        Theme="Material"
                                                        CssClass="Sarabun"
                                                        DropDownStyle="DropDownList"
                                                        ValueField="TermID" ValueType="System.String"
                                                        ViewStateMode="Enabled" TextFormatString="{1}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="TermID" Caption="Term code" />
                                                            <dx:ListBoxColumn FieldName="TermDesc" Width="150px" Caption="Description" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <span style="font-size: small">Vat </span>
                                                    <dx:ASPxComboBox ID="cboVatType" runat="server"
                                                        DropDownStyle="DropDownList"
                                                        Theme="Material"
                                                        CssClass="Sarabun"
                                                        ValueField="TaxID"
                                                        ValueType="System.String"
                                                        ViewStateMode="Enabled"
                                                        TextFormatString="{0}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="TaxID" Caption="Vat code" />
                                                            <dx:ListBoxColumn FieldName="TaxName" Width="150px" Caption="Description" />
                                                            <dx:ListBoxColumn FieldName="TaxValue" Width="100px" Caption="Rate" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-6 pr-0" id="divDeliveryInfo" runat="server">
                                    <asp:UpdatePanel ID="udpdeliveryinfo" runat="server">
                                        <ContentTemplate>
                                            <div class="card">
                                                <div class="card-header  pt-1 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <span style="color: black">
                                                                <i class="fas fa-info"></i></span>&nbsp<span> เพิ่มเติม</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body  pt-1 pb-2">
                                                    <div class="row pt-1">
                                                        <div class="col-md-12">

                                                            <div class="row pt-1">
                                                                <div class="col-md-12">
                                                                    <span style="font-size: small">ที่อยู่เปิดบิล 1 &nbsp</span>
                                                                    <asp:TextBox ID="txtBillAddr1" TextMode="MultiLine" Height="55" Font-Size="Small" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row pt-1">
                                                                <div class="col-md-12">
                                                                    <span style="font-size: small">ที่อยู่เปิดบิล 2 &nbsp</span>
                                                                    <asp:TextBox ID="txtBillAddr2" TextMode="MultiLine" Height="55" Font-Size="Small" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row pt-1">
                                                                <div class="col-md-12">
                                                                    <span style="font-size: small">หมายเหตุ </span>
                                                                    <asp:TextBox ID="txtRemark2" TextMode="MultiLine" Height="50" CssClass="form-control form-control-sm" Font-Size="Smaller" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row pt-1">
                                                                <div class="col-md-12">
                                                                    <span style="font-size: small">เงื่อนไขการชำระ </span>
                                                                    <asp:TextBox ID="txtPaymentMemo" TextMode="MultiLine" Height="70" CssClass="form-control form-control-sm" Font-Size="Smaller" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="btnLoadLine" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3 pr-2 ">
                            <asp:UpdatePanel ID="uptotalpriceInfo" runat="server">
                                <ContentTemplate>
                                    <div class="row pt-1">
                                        <div class="col-md-12">
                                            <div class="card">

                                                <div class="card-header pt-2 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <i class="fas fa-tags"></i>&nbsp Price info </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row text-right">
                                                        <div class="col-md-12">

                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">ก่อนส่วนลด</label>
                                                                <div class="col-sm-4">
                                                                    <asp:Literal ID="lblBaseTotalAmt" runat="server"></asp:Literal>
                                                                </div>
                                                            </div>


                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">ส่วนลดโดย</label>
                                                                <div class="col-sm-4">
                                                                    <asp:DropDownList runat="server" ID="cboDiscBy"
                                                                        AutoPostBack="true"
                                                                        CssClass="form-control form-control-sm Sarabun"
                                                                        Font-Size="Smaller"
                                                                        DropDownStyle="DropDownList" OnSelectedIndexChanged="cboDiscBy_SelectedIndexChanged">
                                                                        <asp:ListItem Text="%" Value="P"></asp:ListItem>
                                                                        <asp:ListItem Text="$" Value="A"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>

                                                            </div>
                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">ลดท้ายบิล(%)</label>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtOntopDisPer"
                                                                        name="txtOntopDisPer" Font-Size="Smaller"
                                                                        CssClass="form-control form-control-sm"
                                                                        runat="server" Style="text-align: right"
                                                                        onkeypress="return DigitOnly(this,event)">
                                                                    </asp:TextBox>
                                                                </div>

                                                            </div>
                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">ลดท้ายบิล(฿)</label>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtOntopDisAmt" name="txtOntopDisAmt" Font-Size="Smaller" CssClass="form-control form-control-sm" runat="server" Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">สุทธิ</label>
                                                                <div class="col-sm-4">
                                                                    <asp:Literal ID="lblTotalAmt" runat="server"></asp:Literal>

                                                                </div>
                                                            </div>
                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">ภาษี</label>
                                                                <div class="col-sm-4">
                                                                    <asp:Literal ID="lblVatAmt" runat="server"></asp:Literal>
                                                                </div>
                                                            </div>
                                                            <div class="row pt-1">
                                                                <label class="col-sm-4" style="font-size: small">สุทธิรวมภาษี</label>
                                                                <div class="col-sm-4">
                                                                    <asp:Literal ID="lblTotalAmtIncVat" runat="server"></asp:Literal>

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
                                    <asp:AsyncPostBackTrigger ControlID="btnLoadLine" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row pl-1 pr-1 pt-2 ">
                        <div class="col-md-12">
                            <div class="card-header pt-1 pb-1">
                                <div class="row">
                                    <div class="col-md-6">
                                        <span><i class="fas fa-cube fa-2x"></i>&nbsp รายการ</span>
                                    </div>
                                    <div class="col-md-6 text-right">
                                        <asp:LinkButton ID="btnAddSaleOrder" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnAddSaleOrder_Click">
                                           <i class="fas fa-check-circle  fa-2x"></i>&nbsp<span >เพิ่มออเดอร์</span> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnAddline" runat="server" CssClass="btn btn-default btn-sm" OnClick="btnAddline_Click">
                                            <i class="fas fa-plus-circle fa-2x"></i>&nbsp<span >เพิ่มรายการ</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdateProgress ID="udpppoline" runat="server" AssociatedUpdatePanelID="udppoline">
                                <ProgressTemplate>
                                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                                        <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:UpdatePanel ID="udppoline" runat="server">
                                <ContentTemplate>

                                    <dx:ASPxPopupControl ID="popFile" runat="server" AllowDragging="True" AllowResize="True"
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

                                    <div class="row pt-3">
                                        <div class="col-md-12">
                                            <div style="overflow-x: auto; width: 100%">
                                                <asp:ObjectDataSource ID="ds_linex"
                                                    runat="server" SelectMethod="DSLineX"
                                                    TypeName="Robot.POS.INVDetail"></asp:ObjectDataSource>
                                                <dx:ASPxGridView ID="grdlinex" Width="100%"
                                                    runat="server"
                                                    CssClass="Sarabun"
                                                    AutoGenerateColumns="False"
                                                    DataSourceID="ds_linex"
                                                    KeyFieldName="LineNum"
                                                    ClientInstanceName="grdlinex"
                                                    Theme="Moderno"
                                                    OnRowCommand="grdlinex_RowCommand">
                                                    <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn VisibleIndex="0" Visible="false">
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("LineNum") %>' CausesValidation="False"
                                                                    CommandName="del" Text="Show" CssClass="btn btn-icons btn-default">
                                                                           <span style="color:gray"> <i class="far fa-trash-alt"></i></span></asp:LinkButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn VisibleIndex="1">
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%# Eval("LineNum") %>' CausesValidation="False"
                                                                    CommandName="show" Text="Show" CssClass="btn btn-icons btn-default">
                                                                           <span style="color:black"> <i class="fas fa-edit"></i></span></asp:LinkButton>
                                                            </DataItemTemplate>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="ItemID" Caption="รหัสสินค้า" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="ItemName" Caption="ชื่อสินค้า" ReadOnly="True" Width="100%">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Remark1" Caption="ชื่อเปิดบิล" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Price" Caption="ราคา" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>

                                                        <dx:GridViewDataTextColumn FieldName="QtyInvoice" Name="QtyInvoice" Caption="จำนวน" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="TotalAmt" Name="TotalAmt" Caption="รวม (Exc Vat)" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="VatAmt" Name="VatAmt" Caption="Vat" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="TotalAmtIncVat" Name="TotalAmtIncVat" Caption="รวม Vat" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="VatTypeID" Caption="Vat Rate" ReadOnly="True">
                                                            <EditFormSettings Visible="False" />
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Width="60" Caption="SO No.">
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="lnkSoID" runat="server" ForeColor="Blue" CausesValidation="False"
                                                                    CommandName="so" Text='<%# Eval("SOID") %>'> </asp:LinkButton>
                                                            </DataItemTemplate>
                                                            <HeaderStyle Wrap="False" />
                                                            <CellStyle Wrap="False" />
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <TotalSummary>

                                                        <dx:ASPxSummaryItem FieldName="QtyInvoice" ShowInColumn="QtyInvoice" ShowInGroupFooterColumn="QtyInvoice" SummaryType="Sum" DisplayFormat="{0:n0}" />
                                                        <dx:ASPxSummaryItem FieldName="TotalAmt" ShowInColumn="TotalAmt" ShowInGroupFooterColumn="TotalAmt" SummaryType="Sum" DisplayFormat="{0:n0}" />
                                                        <dx:ASPxSummaryItem FieldName="DiscAmt" ShowInColumn="DiscAmt" ShowInGroupFooterColumn="DiscAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                                        <dx:ASPxSummaryItem FieldName="VatAmt" ShowInColumn="VatAmt" ShowInGroupFooterColumn="VatAmt" SummaryType="Sum" DisplayFormat="{0:n2}" />
                                                        <dx:ASPxSummaryItem FieldName="TotalAmtIncVat" ShowInColumn="TotalAmtIncVat" ShowInGroupFooterColumn="TotalAmtIncVat" SummaryType="Sum" DisplayFormat="{0:n2}" />

                                                    </TotalSummary>
                                                </dx:ASPxGridView>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnLoadLine" />
                                    <asp:AsyncPostBackTrigger ControlID="grdlinex" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <%--end tab general--%>

                <%--begin tab more info--%>
                <div class="tab-pane fade" id="tab_moreinfo" role="tabpanel" aria-labelledby="c_tab_moreinfo">

                    <asp:UpdatePanel ID="udppriceinfo" runat="server">
                        <ContentTemplate>
                            <dx:ASPxPopupControl ID="popFile2" runat="server" AllowDragging="True" AllowResize="True"
                                CloseAction="CloseButton"
                                ShowMaximizeButton="true"
                                EnableViewState="False"
                                PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter"
                                ShowCloseButton="True"
                                ShowFooter="True"
                                ShowOnPageLoad="True"
                                Maximized="True"
                                HeaderText="" ClientInstanceName="popFile2"
                                EnableHierarchyRecreation="True"
                                FooterStyle-Wrap="True" CloseOnEscape="True" Modal="True" Theme="Mulberry">
                                <ContentStyle Paddings-Padding="0" />
                                <ClientSideEvents Shown="onPopupShown" />
                            </dx:ASPxPopupControl>

                            <div class="row ">
                                <div class="col-md-6">

                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <div class="card">
                                                <div class="card-header pt-1 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <span style="color: gray">
                                                                <i class="fab fa-cc-amazon-pay fa-2x"></i>&nbsp รายละเอียดการจ่าย</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row">
                                                        <div class="col-md-12">

                                                            <div class="row">
                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">สกุลเงิน</span>
                                                                    <dx:ASPxComboBox ID="cboCurrency" runat="server"
                                                                        CssClass="Sarabun"
                                                                        OnSelectedIndexChanged="cboCurrency_SelectedIndexChanged"
                                                                        DropDownStyle="DropDownList"
                                                                        Theme="Material"
                                                                        AutoPostBack="true"
                                                                        ValueField="CurrencyID" ValueType="System.String" ViewStateMode="Enabled" TextFormatString="{0}" Width="100%">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="CurrencyID" Caption="รหัส สกุลเงิน" />
                                                                            <dx:ListBoxColumn FieldName="Name" Width="150px" Caption="รายละเอียด" />
                                                                            <dx:ListBoxColumn FieldName="CountryCode" Width="250px" Caption="เมือง" />
                                                                        </Columns>
                                                                    </dx:ASPxComboBox>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">อัตราแลกเปลี่ยน</span>
                                                                    <asp:TextBox ID="txtRateExchange"
                                                                        CssClass="form-control form-control-sm"
                                                                        runat="server"></asp:TextBox>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">วันที่อัตราแลกเปลี่ยน</span>
                                                                    <dx:ASPxDateEdit ID="dtDateRate"
                                                                        AutoPostBack="true"
                                                                        CssClass="Sarabun"
                                                                        OnDateChanged="dtDateRate_DateChanged"
                                                                        Theme="Material" runat="server"
                                                                        Width="100%"
                                                                        DisplayFormatString="dd-MM-yyyy"
                                                                        EditFormatString="dd-MM-yyyy">
                                                                    </dx:ASPxDateEdit>
                                                                </div>
                                                            </div>
                                                            <div class="row ">
                                                           <%--     <div class="col-md-4">
                                                                    <span style="font-size: small">Accout Group </span>
                                                                    <dx:ASPxComboBox ID="cboGLGroup"
                                                                        runat="server"
                                                                        CssClass="Sarabun"
                                                                        DropDownStyle="DropDownList"
                                                                        Theme="Material"
                                                                        ValueField="GroupID"
                                                                        ValueType="System.String"
                                                                        ViewStateMode="Enabled" TextFormatString="{0} {1}" Width="100%">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="GroupID" Caption="GL Group" />
                                                                            <dx:ListBoxColumn FieldName="Name" Width="250px" Caption="Description" />
                                                                        </Columns>
                                                                    </dx:ASPxComboBox>
                                                                </div>--%>

                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">วันที่จ่าย</span>
                                                                    <dx:ASPxDateEdit ID="dtPaymentDate"
                                                                        CssClass="Sarabun"
                                                                        Theme="Material"
                                                                        DisplayFormatString="dd-MM-yyyy"
                                                                        EditFormatString="dd-MM-yyyy"
                                                                        runat="server" Width="100%">
                                                                    </dx:ASPxDateEdit>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <span style="font-size: small">เซล</span>
                                                                    <dx:ASPxComboBox ID="cboSaleman" runat="server"
                                                                        Theme="Material"
                                                                        EnableCallbackMode="true"
                                                                        CallbackPageSize="10"
                                                                        CssClass="Sarabun"
                                                                        DropDownWidth="600" DropDownHeight="300"
                                                                        ValueType="System.String" ValueField="Username"
                                                                        OnItemsRequestedByFilterCondition="cboSaleman_OnItemsRequestedByFilterCondition_SQL"
                                                                        OnItemRequestedByValue="cboSaleman_OnItemRequestedByValue_SQL" TextFormatString="{0} ( {1}{2} )"
                                                                        Width="100%" DropDownStyle="DropDown">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="Username" Caption="Username" />
                                                                            <dx:ListBoxColumn FieldName="FullName" Caption="Name" Width="300px" />
                                                                            <dx:ListBoxColumn FieldName="DepartmentID" Caption="Department" Width="300px" />
                                                                        </Columns>
                                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pt-1">
                                        <div class="col-md-12">
                                            <div class="card">
                                                <div class="card-header  pt-1 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10">
                                                            <span style="color: gray">
                                                                <i class="fas fa-walking fa-2x"></i>
                                                            </span>&nbsp<span>สถานะ</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="card-body  pt-1 pb-2">
                                                    <div class="row">
                                                        <div class="col-md-12 ">
                                                            <span style="font-size: x-small;">
                                                                <asp:Literal ID="lblStatusInfo" runat="server"></asp:Literal>
                                                            </span>
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
                            <asp:AsyncPostBackTrigger ControlID="cboCurrency" />
                            <asp:AsyncPostBackTrigger ControlID="btnLoadLine" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <%--end tab more info--%>
                </div>

                <%--begin tab history--%>
                <div class="tab-pane fade" id="tab_dochistory" role="tabpanel" aria-labelledby="c_tab_dochistory">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>

                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <div class="row ">
                                        <div class="col-md-11">
                                            <h4>
                                                <span style="color: gray">ประวัติ</span>
                                            </h4>
                                        </div>
                                        <div class="col-md-1 text-right">
                                        </div>
                                    </div>
                                    <div class="row ">
                                        <div class="col-md-12">
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
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="btnAddDocMemo" />--%>
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
                <%--end tab history--%>

                <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
            </div>
        </div>
    </div>
    <div class="row text-center">
        <div class="col-md-12">
            <span style="color: dimgray; font-size: 10px">SOINV-DETAIL</span>
        </div>
    </div>
</asp:Content>
