<%@ Page Title="สินค้าและบริการ" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="ItemDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.Master.ItemDetail" %>


<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <asp:HiddenField ID="hddmenu" runat="server" />

    <script type="text/javascript">

            <%--Begin ddl--%>
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

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row ">
                <div class="col-md-4 pr-1">

                    <div class="row">
                        <div class="col-md-12">
                            <div class="card bg-light">
                                <div class="card-header" style="font-size: small;">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <ul class="nav nav-pills card-header-pills" role="tablist">
                                                <li class="nav-item">
                                                    <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                                        CssClass="btn btn-default" runat="server"
                                                        OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> กลับหน้ารายการ</span>                                            
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </div>
                                        <div class="col-md-6 text-right">
                                            <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-success" OnClick="btnNew_Click">
                                                <span >เพิ่มใหม่</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body p-2">
                                    <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnSearch">
                                        <div class="row pb-1">
                                            <div class="col-md-6 pr-0">
                                                <span>ค้นหา</span>
                                                <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%"
                                                    CssClass="form-control form-control-sm" Theme="Material">
                                                </dx:ASPxTextBox>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="row">
                                                    <div class="col-md-7">
                                                        <span>ประเภท</span>
                                                        <asp:DropDownList ID="cboTypeFilter" runat="server" CssClass="form-control"
                                                            DataTextField="Description1" DataValueField="ValueTXT">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-5 pt-4 text-right">
                                                        <div class="btn-group" role="group" aria-label="Basic example">
                                                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-dark" OnClick="btnSearch_Click">
                                                                <span >ค้นหา</span> 
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </div>

                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <div style="overflow-x: auto; width: 100%">
                                        <dx:ASPxGridView ID="grd" runat="server" ClientInstanceName="grd"
                                            KeyFieldName="ItemID"
                                            AutoGenerateColumns="False"
                                            Width="100%"
                                            OnDataBinding="grd_DataBinding"
                                            EnableTheming="True"
                                            KeyboardSupport="true"
                                            CssClass="Sarabun"
                                            OnHtmlRowPrepared="grd_HtmlRowPrepared"
                                            Theme="Moderno"
                                            OnRowCommand="grd_RowCommand">
                                            <SettingsBehavior AllowSort="false" AllowGroup="false" />
                                            <Settings ShowGroupPanel="False"
                                                ShowFooter="false" ShowGroupFooter="Hidden" />
                                            <SettingsPager PageSize="15">
                                            </SettingsPager>
                                            <Columns>
                                                <dx:GridViewDataTextColumn FieldName="ItemID"
                                                    Name="col_cus_id" Caption="Product" Width="100px" VisibleIndex="0">
                                                    <Settings AutoFilterCondition="Contains" />
                                                    <DataItemTemplate>
                                                        <asp:LinkButton ID="btnOpen" Width="100px" runat="server"
                                                            CommandName="Select" CommandArgument='<%# Eval("ItemID") %>'>
                                                                    <%# Eval("ItemID") %>
                                                        </asp:LinkButton>
                                                    </DataItemTemplate>

                                                    <CellStyle Wrap="False"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="TypeID" Width="120"
                                                    Caption="ประเภท">
                                                    <Settings AutoFilterCondition="Contains" />
                                                    <CellStyle Wrap="False"></CellStyle>
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="หมวด" FieldName="CateID" Width="130">
                                                    <HeaderStyle Wrap="False" />
                                                    <CellStyle Wrap="False" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn FieldName="Name1" Width="100%"
                                                    Caption="Description">
                                                    <Settings AutoFilterCondition="Contains" />
                                                    <CellStyle Wrap="False"></CellStyle>
                                                </dx:GridViewDataTextColumn>

                                            </Columns>
                                        </dx:ASPxGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-8 pl-1">
                    <div class="row">
                        <div class="col-md-12">
                            <%--TAB Order--%>
                            <div class="row pb-1" style="font-size: smaller;">
                                <div class="col-md-12">
                                    <div class="card bg-light">
                                        <div class="card-header">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <ul class="nav nav-pills card-header-pills" role="tablist">
                                                        <li class="nav-item">
                                                            <a class="nav-link active " id="tab-5-1" data-toggle="tab" href="#home-5-1" role="tab" aria-controls="home-5-1" aria-selected="true"><span>&nbsp ข้อมูลสินค้า</span></a>
                                                        </li>
                                                        <li class="nav-item" runat="server" id="divitemprice">
                                                            <a class="nav-link" id="a_tab_itemprice" data-toggle="tab" href="#tab_itemprice" role="tab" aria-controls="c_tab_itemprice" aria-selected="false"><span>ข้อมูลราคาสินค้า</span></a>
                                                        </li>
                                                        <li class="nav-item" runat="server" id="divitemInPointRate">
                                                            <a class="nav-link" id="a_tab_itemInPointRate" data-toggle="tab" href="#tab_itemInPointRate" role="tab" aria-controls="c_tab_itemInPointRate" aria-selected="false"><span>ตั้งค่าแต้มสะสมสินค้า</span></a>
                                                        </li>

                                                        <li class="nav-item">
                                                            <a class="nav-link" id="a_tab_dochistory" data-toggle="tab" href="#tab_dochistory" role="tab" aria-controls="c_tab_dochistory" aria-selected="false"><span>TransactionLog</span></a>
                                                        </li>
                                                    </ul>

                                                </div>
                                                <div class="col-md-4 text-right">
                                                    <h3>
                                                        <span class="badge badge-pill badge-info"><%= ShowItemType() %></span>
                                                    </h3>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="card-body">
                                            <div class="tab-content tab-content-solid">
                                                <div class="tab-pane fade show active" id="home-5-1" role="tabpanel" aria-labelledby="tab-5-1">
                                                    <div class="row pt-2">
                                                        <div class="col-md-12">
                                                            <h4>ข้อมูลสินค้าและบริการ</h4>
                                                        </div>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-md-8">

                                                            <div class="row pb-1">
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">รหัสสินค้าและบริการ</span>
                                                                    <asp:TextBox Font-Size="Small" runat="server" ID="txtItemID" CssClass="form-control" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">รหัสอ้างอิง </span>
                                                                    <asp:TextBox ID="txtRefID" Font-Size="Small" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <span style="color: gray;">รายละเอียด 1</span>
                                                                    <asp:TextBox ID="txtName1" Font-Size="Small" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6" runat="server" hidden="hidden">
                                                                    <span style="color: gray;">รายละเอียด 2</span>
                                                                    <asp:TextBox ID="txtName2" Font-Size="Small" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="row" runat="server" hidden="hidden">
                                                                <div class="col-md-12">
                                                                    <span style="color: gray;">เปิดบิลในบริษัท</span>
                                                                    <asp:DropDownList ID="cboCompany" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Name1" DataValueField="CompanyID"></asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">ราคาขาย </span>
                                                                    <asp:TextBox ID="txtPrice" Font-Bold="true" CssClass="form-control" Style="text-align: right"
                                                                        onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span style="color: navy;">หน่วย &nbsp</span>
                                                                    <asp:DropDownList ID="cboUnitID" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">ประเภท </span>
                                                                    <asp:DropDownList ID="cboType" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>

                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">หมวด</span>
                                                                    <asp:DropDownList ID="cboCate" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>

                                                                <div class="col-md-3" runat="server" hidden="hidden">
                                                                    <span style="color: gray;">กลุ่ม </span>
                                                                    <asp:DropDownList ID="cboGroup1" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">เลือกผู้ขาย </span>
                                                                    <dx:ASPxComboBox ID="cboVendor" runat="server"
                                                                        CssClass="Sarabun"
                                                                        EnableCallbackMode="true"
                                                                        CallbackPageSize="10"
                                                                        Theme="Material"
                                                                        Width="100%"
                                                                        DropDownWidth="600"
                                                                        DropDownHeight="300"
                                                                        ValueType="System.String" ValueField="VendorID"
                                                                        OnItemsRequestedByFilterCondition="cboVendor_OnItemsRequestedByFilterCondition_SQL"
                                                                        OnItemRequestedByValue="cboVendor_OnItemRequestedByValue_SQL" TextFormatString="{0}-{1}"
                                                                        DropDownStyle="DropDownList">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn FieldName="VendorID" Caption="รหัส" />
                                                                            <dx:ListBoxColumn FieldName="FullNameTh" Caption="ชื่อ" Width="300px" />
                                                                        </Columns>
                                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">ราคาทุน </span>
                                                                    <asp:TextBox ID="txtCost" Font-Bold="true" CssClass="form-control" Style="text-align: right"
                                                                        onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="row pb-1">
                                                                <div class="col-md-6">
                                                                    <span style="color: gray;">จัดเรียง</span>
                                                                    <asp:TextBox runat="server" ID="txtSort" onkeypress="return DigitOnly(this,event)" CssClass="form-control" />
                                                                </div>
                                                            </div>

                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <span style="color: gray;">ชื่อเปิดบิล &nbsp</span>
                                                                    <asp:TextBox ID="txtRemark1" Font-Size="Small" CssClass="form-control" TextMode="MultiLine" Height="60px" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-12">
                                                                    <span style="color: gray;">หมายเหตุ &nbsp</span>
                                                                    <asp:TextBox ID="txtRemark2" Font-Size="Small" CssClass="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="col-md-4">
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

                                                    <div class="row">
                                                        <div class="col-md-4 text-left pt-4">
                                                            <p>
                                                                <asp:CheckBox runat="server" Font-Bold="true" Font-Size="Larger" ID="ckIsHold" Visible="false" Text="ระงับการใช้" />
                                                                <asp:CheckBox runat="server" Font-Bold="true" Font-Size="Larger" ID="ckIsActive" Text="เปิดใช้งาน" />
                                                            </p>
                                                        </div>
                                                        <div class="col-md-4 text-center pt-4">
                                                            <asp:UpdatePanel ID="udpmain" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <asp:Button ID="btnSave" Width="100%" Height="40"
                                                                        CssClass="btn btn-success "
                                                                        OnClick="btnSave_Click"
                                                                        OnClientClick="this.disabled='true';"
                                                                        UseSubmitBehavior="false"
                                                                        runat="server"
                                                                        Text="บันทึก"></asp:Button>
                                                                    <asp:LinkButton ID="btnDel" class="dropdown-item" runat="server" Visible="false" OnClick="btnDel_Click"><span>Delete</span> </asp:LinkButton>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                            <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                                                                <strong>
                                                                    <span id="myalertHead"></span></strong>
                                                                <br />
                                                                <span id="myalertBody"></span>
                                                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                                    <span aria-hidden="true">&times;</span>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4"></div>
                                                    </div>
                                                </div>

                                                <div class="tab-pane fade" id="tab_itemprice" role="tabpanel" aria-labelledby="c_tab_itemprice">
                                                    <asp:UpdatePanel ID="upitemprice" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>

                                                            <div class="row pb-1">
                                                                <div class="col-md-12">
                                                                    <div class="card">
                                                                        <div class="card-header text-white bg-secondary">
                                                                            <i class="fa fa-address-card"></i>&nbsp<span>ราคาสินค้าในสาขา</span>
                                                                        </div>

                                                                        <div class="card-body">
                                                                            <div class="row ">
                                                                                <div class="col-md-10 ">
                                                                                    <div class="row ">
                                                                                        <div class="col-md-4">
                                                                                            <span>สาขา</span>
                                                                                            <asp:DropDownList ID="cboCompanyID2" runat="server" CssClass="form-control form-control-sm " DataTextField="Name1" DataValueField="CompanyID"></asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <span>ขายผ่าน</span>
                                                                                            <asp:DropDownList ID="cboShipTo" runat="server" Width="100%"
                                                                                                CssClass="form-control  form-control-sm "
                                                                                                DataTextField="ShipToName" DataValueField="ShipToID">
                                                                                            </asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <span>วันที่เริ่ม</span>
                                                                                            <dx:ASPxDateEdit ID="dtDateBegin" runat="server" Theme="iOS" Width="100%"
                                                                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                                                <TimeSectionProperties Visible="False">
                                                                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                                                                    </TimeEditCellStyle>
                                                                                                </TimeSectionProperties>
                                                                                            </dx:ASPxDateEdit>
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <span>ถึงวันที่</span>
                                                                                            <dx:ASPxDateEdit ID="dtDateEnd" runat="server" Theme="iOS" Width="100%"
                                                                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                                                <TimeSectionProperties Visible="False">
                                                                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                                                                    </TimeEditCellStyle>
                                                                                                </TimeSectionProperties>
                                                                                            </dx:ASPxDateEdit>
                                                                                        </div>

                                                                                    </div>
                                                                                    <div class="row">
                                                                                        <div class="col-md-4">
                                                                                            <span style="color: navy;">คำนวณภาษี </span>
                                                                                            <asp:DropDownList ID="cboPriceTaxcon" runat="server" CssClass="form-control form-control-sm" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <span style="color: navy;">ราคา </span>
                                                                                            <asp:TextBox ID="txtItemPrice" CssClass="form-control form-control-sm" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <span style="color: navy;">ลำดับใช้งาน </span>
                                                                                            <asp:DropDownList runat="server" ID="cboUseLevel" CssClass="form-control form-control-sm" DropDownStyle="DropDownList">
                                                                                                <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                                                                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </div>

                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <div class="row pt-3">
                                                                                        <div class="col-md-12 text-right">
                                                                                            <span style="color: navy;"></span>
                                                                                            <asp:LinkButton ID="btnSaveItemPrice" Width="100%" runat="server" CssClass="btn btn-outline-dark" OnClick="btnSaveItemPrice_Click">
                                                                                                <i class="fa fa-save"></i>&nbsp<span >เพิ่ม</span> 
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>


                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row pt-2 pb-1">
                                                                <div class="col-md-12">
                                                                    <div class="row">
                                                                        <div class="col-md-12 ">
                                                                            <dx:ASPxGridView ID="grdItemPrice"
                                                                                runat="server" Width="100%"
                                                                                EnableTheming="True"
                                                                                Theme="Moderno"
                                                                                CssClass="Sarabun"
                                                                                KeyFieldName="ID"
                                                                                OnDataBinding="grdItemPrice_DataBinding"
                                                                                OnRowCommand="grdItemPrice_RowCommand"
                                                                                AutoGenerateColumns="False"
                                                                                KeyboardSupport="true">
                                                                                <SettingsPager PageSize="500">
                                                                                    <PageSizeItemSettings Visible="false" ShowAllItem="true" />
                                                                                </SettingsPager>
                                                                                <Settings ShowFilterRow="false" ShowFooter="false" ShowGroupFooter="Hidden"
                                                                                    ShowFilterBar="Hidden" ShowHeaderFilterButton="True" />
                                                                                <Columns>
                                                                                       <dx:GridViewDataTextColumn FieldName="" Caption="Delete" Width="80px">
                                                                                        <DataItemTemplate>
                                                                                            <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-icons btn-default"
                                                                                                CommandName="Del" CommandArgument='<%# Eval("ItemID") %>'>
                                                                                <i class="fas fa-trash"></i>
                                                                                            </asp:LinkButton>
                                                                                        </DataItemTemplate>
                                                                                        <HeaderStyle></HeaderStyle>
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="สาขา" FieldName="CompanyID" Width="150px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="ชื่อสาขา" FieldName="CompanyName" Width="200px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                           <dx:GridViewDataTextColumn Caption="ลำดับใช้งาน" FieldName="UseLevel" Width="130px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="ขายผ่าน" FieldName="CustID" Width="130px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataTextColumn Caption="คำนวณภาษี" FieldName="PriceTaxCondType" Width="130px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                    </dx:GridViewDataTextColumn>

                                                                                    <dx:GridViewDataTextColumn Caption="ราคา" FieldName="Price" Width="130px">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <PropertiesTextEdit DisplayFormatString="n2" />
                                                                                        <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                                                    </dx:GridViewDataTextColumn>
                                                                                    <dx:GridViewDataDateColumn Caption="เริ่ม" FieldName="DateBegin">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                                                    </dx:GridViewDataDateColumn>
                                                                                           <dx:GridViewDataDateColumn Caption="สิ้นสุด" FieldName="DateEnd">
                                                                                        <HeaderStyle Wrap="False" />
                                                                                        <CellStyle Wrap="False" />
                                                                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                                                    </dx:GridViewDataDateColumn>
                                                                                 
                                                                                </Columns>
                                                                            </dx:ASPxGridView>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSaveItemPrice" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdItemPrice" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>

                                                <div class="tab-pane fade" id="tab_itemInPointRate" role="tabpanel" aria-labelledby="c_tab_itemInPointRate">
                                                    <asp:UpdatePanel ID="UpditemInPointRate" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row pb-1">
                                                                <div class="col-md-12">
                                                                    <div class="card">
                                                                        <div class="card-header text-white bg-secondary">
                                                                            <i class="fa fa-address-card"></i>&nbsp<span>ตั้งค่าแต้มสะสมสินค้า</span>
                                                                        </div>
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col-md-12">
                                                                                    <div class="row">
                                                                                        <div class="col-md-3">
                                                                                            <span>จำนวนเงินต่อแต้ม </span>
                                                                                            <asp:TextBox ID="txtAmtPerPointRate" CssClass="form-control form-control-sm" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <span>เริ่มต้น</span>
                                                                                            <dx:ASPxDateEdit ID="dtPointDateBegin" runat="server" Theme="Material" Width="100%"
                                                                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                                                <TimeSectionProperties Visible="False">
                                                                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                                                                    </TimeEditCellStyle>
                                                                                                </TimeSectionProperties>
                                                                                            </dx:ASPxDateEdit>
                                                                                        </div>
                                                                                        <div class="col-md-2">
                                                                                            <span>สิ้นสุด</span>
                                                                                            <dx:ASPxDateEdit ID="dtPointDateEnd" runat="server" Theme="Material" Width="100%"
                                                                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                                                <TimeSectionProperties Visible="False">
                                                                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                                                                    </TimeEditCellStyle>
                                                                                                </TimeSectionProperties>
                                                                                            </dx:ASPxDateEdit>
                                                                                        </div>
                                                                                        <div class="col-md-3">
                                                                                            <span>อายุแต้ม(เดือน)</span>
                                                                                            <asp:TextBox ID="txtExpireInMont" CssClass="form-control form-control-sm" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                                        </div>
                                                                                        <div class="col-md-2 text-right pt-3">
                                                                                            <span style="color: navy;"></span>
                                                                                            <asp:LinkButton ID="btnItemInPointRate" Width="100%" runat="server" CssClass="btn btn-outline-dark" OnClick="btnItemInPointRate_Click">
                                                                                                <i class="fa fa-save"></i>&nbsp<span >เพิ่ม</span> 
                                                                                            </asp:LinkButton>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>

                                                                        </div>


                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row pt-2 pb-1">
                                                                <div class="col-md-12">
                                                                    <dx:ASPxGridView ID="grditemInPointRate"
                                                                        runat="server" Width="100%"
                                                                        EnableTheming="True"
                                                                        Theme="Moderno"
                                                                        CssClass="Sarabun"
                                                                        KeyFieldName="RateID"
                                                                        OnDataBinding="grditemInPointRate_DataBinding"
                                                                        OnRowCommand="grditemInPointRate_RowCommand"
                                                                        AutoGenerateColumns="False"
                                                                        KeyboardSupport="true">
                                                                        <SettingsPager PageSize="500">
                                                                            <PageSizeItemSettings Visible="false" ShowAllItem="true" />
                                                                        </SettingsPager>
                                                                        <Settings ShowFilterRow="false" ShowFooter="false" ShowGroupFooter="Hidden"
                                                                            ShowFilterBar="Hidden" ShowHeaderFilterButton="True" />
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn FieldName="" Caption="Delete" Width="80px">
                                                                                <DataItemTemplate>
                                                                                    <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-icons btn-default"
                                                                                        CommandName="Del" CommandArgument='<%# Eval("RateID") %>'>
                                                                                <i class="fas fa-trash"></i>
                                                                                    </asp:LinkButton>
                                                                                </DataItemTemplate>
                                                                                <HeaderStyle></HeaderStyle>
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="รหัสสินค้า" FieldName="ItemID" Width="130px">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <CellStyle Wrap="False" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="จำนวนเงินต่อแต้ม" FieldName="AmtPerPointRate" Width="120px">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <PropertiesTextEdit DisplayFormatString="n2" />
                                                                                <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataDateColumn Caption="เริ่มต้น" FieldName="DateBegin">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <CellStyle Wrap="False" />
                                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                                            </dx:GridViewDataDateColumn>
                                                                            <dx:GridViewDataDateColumn Caption="สิ้นสุด" FieldName="DateEnd">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <CellStyle Wrap="False" />
                                                                                <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                                                            </dx:GridViewDataDateColumn>
                                                                            <dx:GridViewDataTextColumn Caption="อายุแต้ม(เดือน)" FieldName="ExpireInMont" Width="150px">
                                                                                <HeaderStyle Wrap="False" />
                                                                                <PropertiesTextEdit DisplayFormatString="n0" />
                                                                                <CellStyle Wrap="False" HorizontalAlign="Right" />
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                    </dx:ASPxGridView>
                                                                </div>
                                                            </div>

                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnItemInPointRate" />
                                                            <asp:AsyncPostBackTrigger ControlID="grditemInPointRate" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </div>


                                                <%--TAB HISTORY--%>
                                                <div class="tab-pane fade" id="tab_dochistory" role="tabpanel" aria-labelledby="c_tab_dochistory">
                                                    <div class="row pt-1">
                                                        <div class="col-md-12">
                                                            <div>
                                                                <div class="col-md-12">
                                                                    <div class="row">
                                                                        <button type="button" class="btn btn-light btn-block btn-fw">
                                                                            <i class="far fa-shoe-prints"></i>
                                                                            <span>&nbsp Tansaction History</span></button>
                                                                    </div>
                                                                    <div class="row">
                                                                        <div class="col-md-12">

                                                                            <div class="row">
                                                                                <div style="overflow-x: auto; width: 100%">
                                                                                    <asp:GridView ID="grd_transaction_log" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                                        CssClass="table table-striped table-bordered table-hover"
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
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="row" runat="server" id="divOther">
                                <div class="col-md-12">
                                    <div class="card">
                                        <div class="card-header">
                                            <i class="fas fa-otter fa-2x"></i>&nbsp<span>อื่นๆ</span>
                                        </div>
                                        <div class="card-body">


                                            <div class="row pb-1">

                                                <div class="col-md-3 ">
                                                    <span style="color: gray;">Group 2 &nbsp</span>
                                                    <asp:DropDownList ID="cboGroup2" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 ">
                                                    <span style="color: gray;">Group 3 &nbsp</span>
                                                    <asp:DropDownList ID="cboGroup3" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row pb-1">
                                                <div class="col-md-3 ">
                                                    <span style="color: gray;">หน่วยจัดเก็บสินค้า </span>
                                                    <asp:DropDownList ID="cboPacking" Font-Size="Small" runat="server" CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-3 ">
                                                    <span style="color: gray;">Volumn </span>
                                                    <asp:TextBox ID="txtVol" Font-Size="Small" CssClass="form-control" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grd" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
