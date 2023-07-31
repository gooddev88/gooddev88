<%@ Page Title="BOM" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="POSBOMDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.POSBOMDetail" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddmenu" runat="server" />

    <script type="text/javascript">

            <%--Begin ddl--%>
        //var startTime;
        //function OnBeginCallback() {
        //    LoadingPanel.Show();
        //    startTime = new Date();
        //}
        //function OnEndCallback() {
        //    LoadingPanel.Hide();
        //    var result = new Date() - startTime;
        //    result /= 1000;
        //    result = result.toString();
        //    if (result.length > 4)
        //        result = result.substr(0, 4);
        //    time.SetText(result.toString() + " sec");
        //    label.SetText("Time to retrieve the last data:");
        //}

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
    <dx:aspxloadingpanel id="LoadingPanel" clientinstancename="LoadingPanel"
        theme="Material"
        runat="server"
        modal="true"
        horizontalalign="Center"
        verticalalign="Middle">
    </dx:aspxloadingpanel>
    <asp:UpdatePanel ID="udpAlert" runat="server">
        <contenttemplate>
            <dx:aspxpopupcontrol id="popAlert" runat="server" width="400" closeaction="OuterMouseClick" closeonescape="true" modal="True"
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

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <progresstemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <contenttemplate>
            <div class="row ">
                <div class="col-md-4 pr-1">

                    <div class="row">
                        <div class="col-md-12">
                            <div class="card bg-light">
                                <div class="card-header" style="font-size: small;">
                                    <ul class="nav nav-pills card-header-pills" role="tablist">
                                        <li class="nav-item">
                                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                                CssClass="btn btn-default" runat="server"
                                                OnClick="btnBackList_Click">
                                                <span style="color: black"><i class="fas fa-reply-all fa-2x"></i></span>
                                                <span style="font-size: medium; color: black">Back</span>
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                                <div class="card-body p-2">
                                    <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnSearch">
                                        <div class="row pb-1">
                                            <div class="col-md-6 pt-1 pr-0">
                                                <dx:aspxtextbox id="txtSearch" runat="server" width="100%"
                                                    cssclass="form-control form-control-sm" theme="Material">
                                                </dx:aspxtextbox>
                                            </div>
                                            <div class="col-md-6 text-right pt-1">
                                                <div class="btn-group" role="group" aria-label="Basic example">
                                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-dark" OnClick="btnSearch_Click">
                                                        <i class="fa fa-search"></i>&nbsp<span>ค้นหา</span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNew" runat="server" CssClass="btn btn-success" OnClick="btnNew_Click">
                                                        <span>++NEW++</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>

                            </div>
                            <div class="row pt-2">
                                <div class="col-md-12">
                                    <div style="overflow-x: auto; width: 100%">
                                        <dx:aspxgridview id="grd" runat="server" clientinstancename="grd"
                                            keyfieldname="BomID"
                                            autogeneratecolumns="False"
                                            width="100%"
                                            cssclass="Sarabun"
                                            ondatabinding="grd_DataBinding"
                                            enabletheming="True"
                                            keyboardsupport="true"
                                            onhtmlrowprepared="grd_HtmlRowPrepared"
                                            theme="Moderno"
                                            onrowcommand="grd_RowCommand">
                                            <settings showgrouppanel="False"
                                                showfooter="false" showgroupfooter="Hidden" />
                                            <settingspager pagesize="40">
                                            </settingspager>
                                            <columns>
                                                <dx:gridviewdatatextcolumn fieldname="ItemID"
                                                    caption="Bom No." width="100px" visibleindex="0">
                                                    <settings autofiltercondition="Contains" />
                                                    <dataitemtemplate>
                                                        <asp:LinkButton ID="btnOpen" Width="100px" runat="server"
                                                            CommandName="Select" CommandArgument='<%# Eval("BomID") %>'>
                                                            <%# Eval("BomID") %>
                                                        </asp:LinkButton>
                                                    </dataitemtemplate>
                                                    <cellstyle wrap="False"></cellstyle>
                                                </dx:gridviewdatatextcolumn>
                                                <dx:gridviewdatatextcolumn fieldname="Description" width="100%"
                                                    caption="รายละเอียด">
                                                    <settings autofiltercondition="Contains" />
                                                    <cellstyle wrap="False"></cellstyle>
                                                </dx:gridviewdatatextcolumn>
                                                <dx:gridviewdatatextcolumn caption="ประเภท" fieldname="UserForModule" width="100">
                                                    <headerstyle wrap="False" cssclass="Sarabun" />
                                                    <cellstyle wrap="False" />
                                                </dx:gridviewdatatextcolumn>
                                                <dx:gridviewdatacheckcolumn fieldname="IsDefault" width="100"
                                                    caption="เป็นสูตรเริ่มต้น">
                                                    <settings autofiltercondition="Contains" />
                                                    <cellstyle wrap="False"></cellstyle>
                                                </dx:gridviewdatacheckcolumn>
                                            </columns>
                                        </dx:aspxgridview>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-8 pl-1">
                    <div class="row">
                        <div class="col-md-12">
                            <%--TAB Bom--%>
                            <div class="row pb-1" style="font-size: smaller;">
                                <div class="col-md-12">
                                    <div class="card bg-light">
                                        <div class="card-header">
                                            <div class="row">
                                                <div class="col-md-8">
                                                    <ul class="nav nav-pills card-header-pills" role="tablist">
                                                        <li class="nav-item">
                                                            <a class="nav-link active" style="font-size: medium;" id="tab-5-1" data-toggle="tab" href="#home-5-1" role="tab" aria-controls="home-5-1" aria-selected="true"><span>&nbsp ข้อมูลสูตรผลิต</span></a>
                                                        </li>

                                                        <li class="nav-item">
                                                            <a class="nav-link" style="font-size: medium;" id="a_tab_dochistory" data-toggle="tab" href="#tab_dochistory" role="tab" aria-controls="c_tab_dochistory" aria-selected="false"><span>TransactionLog</span></a>
                                                        </li>
                                                    </ul>
                                                </div>
                                                <div class="col-md-4 text-right">
                                                    <%--<h3>
                                                        <span class="badge badge-pill badge-info"><%= ShowItemType() %></span>
                                                    </h3>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <div class="tab-content tab-content-solid">
                                                <div class="tab-pane fade show active" id="home-5-1" role="tabpanel" aria-labelledby="tab-5-1">
                                                    <div class="row pt-1">
                                                        <div class="col-6">
                                                            <h4>ข้อมูลสูตรผลิต</h4>
                                                        </div>
                                                        <div class="col-6 text-right">
                                                            <asp:CheckBox ID="chkIsDefault" Text="เป็นสูตรเริ่มต้น" Checked="true" runat="server" />
                                                            <asp:CheckBox runat="server" Font-Size="Larger" ID="ckIsActive" Text="เปิดใช้งาน" />
                                                        </div>
                                                    </div>

                                                    <div class="row pb-1 pt-2">
                                                        <div class="col-md-3">
                                                            <span style="color: gray;">ประเภท </span>
                                                            <asp:DropDownList ID="cboTypeBom" Font-Size="Small" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboTypeBom_SelectedIndexChanged"
                                                                CssClass="form-control" DataTextField="Description1" DataValueField="ValueTXT">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <span style="color: gray;">Bom No.</span>
                                                            <asp:TextBox Font-Size="Small" runat="server" ID="txtBomID" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-md-4">
                                                            <span style="color: gray;">สินค้า </span>
                                                            <dx:aspxcombobox id="cboItemIDFG" runat="server"
                                                                theme="Material"
                                                                enablecallbackmode="true"
                                                                callbackpagesize="10"
                                                                autopostback="true"
                                                                dropdownwidth="600"
                                                                dropdownheight="300"
                                                                cssclass="Sarabun"
                                                                onselectedindexchanged="cboItemIDFG_SelectedIndexChanged"
                                                                valuetype="System.String" valuefield="ItemID"
                                                                onitemsrequestedbyfiltercondition="cboItemIDFG_OnItemsRequestedByFilterCondition_SQL"
                                                                onitemrequestedbyvalue="cboItemIDFG_OnItemRequestedByValue_SQL" textfield="ItemID" textformatstring="{0}-{2}"
                                                                width="100%" dropdownstyle="DropDownList">
                                                                <columns>
                                                                    <dx:listboxcolumn fieldname="ItemID" caption="สินค้า" />
                                                                    <dx:listboxcolumn fieldname="TypeID" caption="ประเภท" width="70" />
                                                                    <dx:listboxcolumn fieldname="Name1" caption="รายละเอียด" width="300px" />
                                                                </columns>
                                                                <clientsideevents begincallback="function(s, e) { OnBeginCallback(); }" endcallback="function(s, e) { OnEndCallback(); } " />
                                                            </dx:aspxcombobox>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <span style="color: gray;">Unit </span>
                                                            <asp:DropDownList ID="cboUnitFG" Font-Size="Small" runat="server" AutoPostBack="true"
                                                                CssClass="form-control" DataTextField="ToUnit" DataValueField="ToUnit">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <span style="color: gray;">รายละเอียด</span>
                                                            <asp:TextBox ID="txtDescription" Font-Size="Small" CssClass="form-control" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <span style="color: gray;">หมายเหตุ 1 &nbsp</span>
                                                            <asp:TextBox ID="txtRemark1" Font-Size="Small" CssClass="form-control" TextMode="MultiLine" Height="60px" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <hr />

                                                    <asp:UpdatePanel ID="upitemprice" runat="server" UpdateMode="Conditional">
                                                        <contenttemplate>

                                                            <div class="row pb-1">
                                                                <div class="col-md-12">

                                                                    <div class="row pt-1">
                                                                        <div class="col-md-12">
                                                                            <h4>รายการวัตถุดิบ</h4>
                                                                        </div>
                                                                    </div>

                                                                    <div class="row pt-3 ">
                                                                        <div class="col-md-12">
                                                                            <div class="row ">
                                                                                <div class="col-md-4">
                                                                                    <span style="color: gray;">วัตถุดิบ </span>
                                                                                    <dx:aspxcombobox id="cboItemIDRM" runat="server"
                                                                                        theme="Material"
                                                                                        enablecallbackmode="true"
                                                                                        callbackpagesize="10"
                                                                                        autopostback="true"
                                                                                        dropdownwidth="600"
                                                                                        dropdownheight="300"
                                                                                        cssclass="Sarabun"
                                                                                        onselectedindexchanged="cboItemIDRM_SelectedIndexChanged"
                                                                                        valuetype="System.String" valuefield="ItemID"
                                                                                        onitemsrequestedbyfiltercondition="cboItemIDRM_OnItemsRequestedByFilterCondition_SQL"
                                                                                        onitemrequestedbyvalue="cboItemIDRM_OnItemRequestedByValue_SQL" textfield="ItemID" textformatstring="{0}-{2}"
                                                                                        width="100%" dropdownstyle="DropDownList">
                                                                                        <columns>
                                                                                            <dx:listboxcolumn fieldname="ItemID" caption="สินค้า" />
                                                                                            <dx:listboxcolumn fieldname="TypeID" caption="ประเภท" width="60" />
                                                                                            <dx:listboxcolumn fieldname="Name1" caption="รายละเอียด" width="300px" />
                                                                                        </columns>
                                                                                        <clientsideevents begincallback="function(s, e) { OnBeginCallback(); }" endcallback="function(s, e) { OnEndCallback(); } " />
                                                                                    </dx:aspxcombobox>


                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <span style="color: gray;">จำนวน </span>
                                                                                    <asp:TextBox ID="txtRmQty" CssClass="form-control form-control-sm  text-right" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <span style="color: gray;">ผลิตได้จำนวน </span>
                                                                                    <asp:TextBox ID="txtFgQty" CssClass="form-control form-control-sm  text-right" Text="1" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                                                                </div>

                                                                                <div class="col-md-2">
                                                                                    <span style="color: gray;">Unit </span>
                                                                                    <asp:DropDownList ID="cboUnitRm" Font-Size="Small" runat="server" AutoPostBack="true"
                                                                                        CssClass="form-control" DataTextField="ToUnit" DataValueField="ToUnit">
                                                                                    </asp:DropDownList>
                                                                                </div>
                                                                                <div class="col-md-2 pt-3 text-right">
                                                                                    <span></span>
                                                                                    <asp:LinkButton ID="btnAddBomLine" Width="100%" runat="server" CssClass="btn btn-outline-success" OnClick="btnAddBomLine_Click">
                                                                                        <i class="fas fa-plus-circle fa-lg"></i>&nbsp<span>เพิ่ม</span>
                                                                                    </asp:LinkButton>
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
                                                                            <dx:aspxgridview id="grdBomLine"
                                                                                runat="server" width="100%"
                                                                                enabletheming="True"
                                                                                theme="Moderno"
                                                                                cssclass="Sarabun"
                                                                                keyfieldname="LineNum"
                                                                                onrowcommand="grdBomLine_RowCommand"
                                                                                autogeneratecolumns="False"
                                                                                keyboardsupport="true">
                                                                                <settings showfilterrow="True" showfooter="false" showgroupfooter="Hidden"
                                                                                    showfilterbar="Hidden" showheaderfilterbutton="True" />
                                                                                <columns>
                                                                                    <dx:gridviewdatatextcolumn caption="วัตถุดิบ" fieldname="ItemIDRM" width="150px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <cellstyle wrap="False" />
                                                                                    </dx:gridviewdatatextcolumn>
                                                                                    <dx:gridviewdatatextcolumn caption="รายละเอียด" fieldname="RMDescription" width="200px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <cellstyle wrap="False" />
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                    <dx:gridviewdatatextcolumn caption="จำนวน" fieldname="RmQty" width="130px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <propertiestextedit displayformatstring="n4" />
                                                                                        <cellstyle wrap="False" horizontalalign="Right" />
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                    <dx:gridviewdatatextcolumn caption="หน่วย" fieldname="RmUnit" width="130px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <cellstyle wrap="False" />
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                    <dx:gridviewdatatextcolumn caption="ผลิตได้จำนวน" fieldname="FgQty" width="130px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <propertiestextedit displayformatstring="n4" />
                                                                                        <cellstyle wrap="False" horizontalalign="Right" />
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                    <dx:gridviewdatatextcolumn caption="หน่วย" fieldname="FgUnit" width="130px">
                                                                                        <headerstyle wrap="False" />
                                                                                        <cellstyle wrap="False" />
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                    <dx:gridviewdatatextcolumn fieldname="" caption="ลบ" width="80px">
                                                                                        <dataitemtemplate>
                                                                                            <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-icons btn-default"
                                                                                                CommandName="Del" CommandArgument='<%# Eval("LineNum") %>'>
                                                                                                <i class="fas fa-trash"></i>
                                                                                            </asp:LinkButton>
                                                                                        </dataitemtemplate>
                                                                                        <headerstyle></headerstyle>
                                                                                    </dx:gridviewdatatextcolumn>

                                                                                </columns>
                                                                            </dx:aspxgridview>

                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-3 pt-4">
                                                                </div>
                                                                <div class="col-md-3">
                                                                </div>
                                                                <div class="col-md-6 text-center pt-4">
                                                                    <asp:UpdatePanel ID="udpmain" runat="server" UpdateMode="Conditional">
                                                                        <contenttemplate>
                                                                            <asp:Button ID="btnSave" Width="100%" Height="40"
                                                                                CssClass="btn btn-success "
                                                                                OnClick="btnSave_Click"
                                                                                OnClientClick="this.disabled='true';"
                                                                                UseSubmitBehavior="false"
                                                                                runat="server"
                                                                                Text="บันทึก"></asp:Button>
                                                                            <asp:LinkButton ID="btnDel" class="dropdown-item" runat="server" Visible="false" OnClick="btnDel_Click"><span>Delete</span> </asp:LinkButton>
                                                                        </contenttemplate>
                                                                        <triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                                                        </triggers>
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
                                                            </div>
                                                        </contenttemplate>
                                                        <triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cboItemIDRM" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddBomLine" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdBomLine" />
                                                        </triggers>
                                                    </asp:UpdatePanel>
                                                </div>



                                                <%--TAB HISTORY--%>
                                                <div class="tab-pane fade" id="tab_dochistory" role="tabpanel" aria-labelledby="c_tab_dochistory">

                                                    <div class="row pt-2">
                                                        <div class="col-md-12">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <button type="button" class="btn btn-secondary btn-block btn-fw  fa-2x">
                                                                        <i class="far fa-clock"></i><span>&nbsp ประวัติการแก้ไข</span></button>
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div style="overflow-x: auto; width: 100%">
                                                                        <asp:GridView ID="grd_transaction_log" runat="server"
                                                                            AutoGenerateColumns="False" Width="100%"
                                                                            Font-Size="Small" OnPageIndexChanging="grd_transaction_log_PageIndexChanging"
                                                                            CssClass="table table-striped table-bordered table-hover Sarabun"
                                                                            EmptyDataText="No transaction log data.."
                                                                            EmptyDataRowStyle-HorizontalAlign="Center"
                                                                            ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10">
                                                                            <columns>
                                                                                <asp:BoundField DataField="CreatedBy" HeaderText="Action By">
                                                                                    <headerstyle wrap="False" />
                                                                                    <itemstyle wrap="False" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="CreatedDate" HeaderText="Action Date" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy HH:mm}">
                                                                                    <headerstyle wrap="False" />
                                                                                    <itemstyle wrap="False" />
                                                                                </asp:BoundField>

                                                                                <asp:BoundField DataField="Action" HeaderText="Action">
                                                                                    <headerstyle wrap="False" />
                                                                                    <itemstyle wrap="False" />
                                                                                </asp:BoundField>
                                                                            </columns>
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
            </div>
        </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="grd" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
