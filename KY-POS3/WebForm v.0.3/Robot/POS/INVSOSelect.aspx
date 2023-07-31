﻿<%@ Page Title="Get SaleOrder" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteB.Master" AutoEventWireup="true" CodeBehind="INVSOSelect.aspx.cs" Inherits="Robot.POS.INVSOSelect" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmasterid" runat="server" />
    <asp:HiddenField ID="hddopenby" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

    <%--begin script for loadding panel --%>
    <script>  
        function OnBeginCallback(s, e) {
            LoadingPanel.Show();
        };
        function OnEndCallback(s, e) {
            LoadingPanel.Hide();
        };
    </script>
    <%--end script for loadding panel --%>

    <%--Begin ddl--%>
    <script type="text/javascript">
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
    </script>

    <script type="text/javascript">

        //begin select check in grid
        function Check_Click(objRef) {
            var row = objRef.parentNode.parentNode;
            var GridView = row.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        if (row.rowIndex % 2 == 0) {
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
        }
        //end select check in grid


    </script>

    <script type="text/javascript">
        //begin script grid dev
        function OnSelectAllRowsLinkClick() {
            grid.SelectRows();
        }
        function OnUnselectAllRowsLinkClick() {
            grid.UnselectRows();
        }
        function OnGridViewInit() {
            UpdateTitlePanel();
        }
        function OnGridViewSelectionChanged() {
            UpdateTitlePanel();
        }
        function OnGridViewEndCallback() {
            UpdateTitlePanel();
        }
        function UpdateTitlePanel() {
            var selectedFilteredRowCount = GetSelectedFilteredRowCount();
            if (selectAllMode.GetValue() != "AllPages") {
                lnkSelectAllRows.SetVisible(grid.cpVisibleRowCount > selectedFilteredRowCount);
                lnkClearSelection.SetVisible(grid.GetSelectedRowCount() > 0);
            }

            var text = "Total rows selected: <b>" + grid.GetSelectedRowCount() + "</b>. ";
            var hiddenSelectedRowCount = grid.GetSelectedRowCount() - GetSelectedFilteredRowCount();
            if (hiddenSelectedRowCount > 0)
                text += "Selected rows hidden by the applied filter: <b>" + hiddenSelectedRowCount + "</b>.";
            text += "<br />";
            info.SetText(text);
        }
        function GetSelectedFilteredRowCount() {
            return grid.cpFilteredRowCountWithoutPage + grid.GetSelectedKeysOnPage().length;
        }
        function ClosePopup() {
            window.close();
        }
    </script>
    <script type="text/javascript">
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
        //end script grid dev
    </script>

    <div class="row">
        <div class="col-md-12">
            <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                <asp:LinkButton ID="btnOK" runat="server" CssClass="btn btn-default" OnClick="btnOk_Click">
                <i class="fas fa-save"></i>&nbsp<span >OK</span> 
                </asp:LinkButton>
                <asp:LinkButton ID="btnClose" runat="server" href="#" OnClientClick="return window.parent.OnClosePopupEventHandler('OK-Line');" CssClass="btn btn-default">
               <i class="fas fa-sign-out-alt"></i>&nbsp<span >Close</span> 
                </asp:LinkButton>
                <asp:Label ID="lblInfoSave" runat="server" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
            </div>
        </div>
    </div>
    <div class="row pb-1">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-10">
                         <i class="fas fa-check-circle fa-2x"></i>
                            &nbsp
                            <asp:Literal ID="litStatus" runat="server"></asp:Literal>
                        </div>
                        <div class="col-md-2 text-right">
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="row pb-1">
        <div class="col-md-12">
            <div class="row">
                <div class="col-md-12">
                    <div runat="server" id="divSelectOption" style="display: none">
                        <dx:ASPxComboBox ID="selectAllMode" ClientInstanceName="selectAllMode" Caption="SELECT MODE:" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="SelectAllMode_SelectedIndexChanged">
                            <RootStyle CssClass="OptionsBottomMargin"></RootStyle>
                        </dx:ASPxComboBox>
                    </div>
                </div>

            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <div style="overflow-x: auto; width: 100%">
                                <dx:ASPxGridView ID="grid"
                                    ClientInstanceName="grid" runat="server"
                                    DataSourceID="ds_linex"
                                    CssClass="Sarabun"
                                    KeyFieldName="OrdID" Width="100%"
                                    OnCustomJSProperties="GridView_CustomJSProperties"
                                    EnableRowsCache="False" AutoGenerateColumns="False"
                                    Theme="Material">
                                    <Settings ShowTitlePanel="true" ShowFilterRow="true"
                                        ShowFilterBar="Auto" />
                                    <Styles>
                                    
                                        <AlternatingRow Enabled="true" />

                                        <TitlePanel CssClass="titleContainer"></TitlePanel>
                                    </Styles>
                                    <ClientSideEvents Init="OnGridViewInit" SelectionChanged="OnGridViewSelectionChanged" EndCallback="OnGridViewEndCallback" />
                                    <Templates>
                                        <TitlePanel>
                                            <dx:ASPxLabel ID="lblInfo" ClientInstanceName="info" runat="server" />
                                            <dx:ASPxHyperLink ID="lnkSelectAllRows" ClientInstanceName="lnkSelectAllRows" OnLoad="lnkSelectAllRows_Load"
                                                Text="Select all rows" runat="server" Cursor="pointer" ClientSideEvents-Click="OnSelectAllRowsLinkClick" />
                                            &nbsp;
                                            <dx:ASPxHyperLink ID="lnkClearSelection" ClientInstanceName="lnkClearSelection" OnLoad="lnkClearSelection_Load"
                                                Text="Clear selection" runat="server" Cursor="pointer" ClientVisible="false" ClientSideEvents-Click="OnUnselectAllRowsLinkClick" />
                                        </TitlePanel>
                                    </Templates>
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Visible" />
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" ShowClearFilterButton="true" Width="60px" VisibleIndex="0" SelectAllCheckboxMode="Page" />
                                        <dx:GridViewDataTextColumn FieldName="OrdID" Caption="Ord No.">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataDateColumn Caption="OrdDate" FieldName="OrdDate">
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False" />
                                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                        </dx:GridViewDataDateColumn>
                                        <dx:GridViewDataTextColumn FieldName="ComID" Caption="ComID">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CustID" Caption="Customer">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="CustName" Caption="CustomerName">
                                            <Settings AutoFilterCondition="Contains" />
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="OrdQty" Caption="OrdQty">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="OrdAmt" Caption="OrdAmt">
                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                            <CellStyle CssClass="Sarabun" Wrap="False"></CellStyle>
                                        </dx:GridViewDataTextColumn>

                                    </Columns>
                              
                                </dx:ASPxGridView>
                                <asp:ObjectDataSource ID="ds_linex" runat="server" SelectMethod="DSLineX" TypeName="Robot.POS.INVSOSelect">
                                    <%--    <SelectParameters>
                                        <asp:SessionParameter Name="cusid" SessionField="cusid_sel" Type="String" />
                                        <asp:SessionParameter Name="company" SessionField="company_sel" Type="String" />
                                    </SelectParameters>--%>
                                </asp:ObjectDataSource>
                            </div>
                            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grid"></dx:ASPxGridViewExporter>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="row text-center">
        <div class="col-md-12">
            <span style="color: dimgray; font-size: 10px">Invoice-Select-SaleOrder</span>
        </div>
    </div>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>