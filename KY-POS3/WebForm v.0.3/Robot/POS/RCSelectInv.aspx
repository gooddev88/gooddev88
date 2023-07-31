﻿<%@ Page Title="Select Invoice" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="RCSelectInv.aspx.cs" Inherits="Robot.POS.RCSelectInv" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">



    <style>
        /* tammon begin of checkbox style    */
        /* The customcheck */
        .customcheck {
            display: block;
            position: relative;
            padding-left: 35px;
            margin-bottom: 12px;
            cursor: pointer;
            font-size: 22px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default checkbox */
            .customcheck input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
            }

        /* Create a custom checkbox */
        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 25px;
            width: 25px;
            background-color: #eee;
            border-radius: 5px;
        }

        /* On mouse-over, add a grey background color */
        .customcheck:hover input ~ .checkmark {
            background-color: #ccc;
        }

        /* When the checkbox is checked, add a blue background */
        .customcheck input:checked ~ .checkmark {
            background-color: #02cf32;
            border-radius: 5px;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the checkmark when checked */
        .customcheck input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the checkmark/indicator */
        .customcheck .checkmark:after {
            left: 9px;
            top: 5px;
            width: 5px;
            height: 10px;
            border: solid white;
            border-width: 0 3px 3px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }



        /*tammon end of checkbox style    */
    </style>

    <style type="text/css">
        hr.style4 {
            background-color: #fff;
            border-top: 2px dashed #8c8b8b;
        }

    </style>

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

    <%--begin of Loading callback script--%>
    <script>
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };

        function OnBeginCallbackLoadingPanel(s, e) {
            LoadingPanel.Show();
        };

        function OnEndCallbackLoadingPanel(s, e) {
            LoadingPanel.Hide();
        };
    </script>

          <asp:UpdateProgress ID="udppPost" runat="server" AssociatedUpdatePanelID="udpPost">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 99999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
                          <asp:UpdatePanel ID="udpPost" runat="server">
        <ContentTemplate>
         <asp:Panel ID="pn1" runat="server"  DefaultButton="btnSearch">
    <div class="row pt-3 pb-2">
        <div class="col-md-7 pt-2">
            <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
        
                         <asp:LinkButton ID="btnSave" runat="server" Font-Size="Large" CssClass="btn btn-default" OnClick="btnSave_Click">
               <span style="color:forestgreen">    <i class="fas fa-check-circle fa-2x"></i></span> &nbsp<span >OK</span> 
                </asp:LinkButton>
                <asp:LinkButton ID="btnClose" runat="server" Font-Size="Large" CssClass="btn btn-default" OnClick="btnClose_Click">
               <span style="color:red">   <i class="fas fa-times-circle fa-2x"></i></span> &nbsp<span >Close</span> 
                </asp:LinkButton>

                <asp:Label ID="lblInfoSave" runat="server" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
            </div>
        </div>
        <div class="col-md-2">
            <span   style="font-size:small">วันที่อินวอยซ์(เริ่ม)</span>
            <dx:ASPxDateEdit ID="dtBegin" runat="server"  Theme="Material" Width="100%"
                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                <TimeSectionProperties Visible="false">
                    <TimeEditCellStyle HorizontalAlign="Right">
                    </TimeEditCellStyle>
                </TimeSectionProperties>
            </dx:ASPxDateEdit>
        </div>
        <div class="col-md-2">
            <span  style="font-size:small">ถึงวันที่</span>
            <dx:ASPxDateEdit ID="dtEnd" runat="server"   Theme="Material" Width="100%"
                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                <TimeSectionProperties Visible="false">
                    <TimeEditCellStyle HorizontalAlign="Right">
                    </TimeEditCellStyle>
                </TimeSectionProperties>
            </dx:ASPxDateEdit>
        </div>
        <div class="col-md-1 pt-4 text-left"> 
           <asp:LinkButton ID="btnSearch" 
               CssClass="btn btn-outline-success btn-sm bn-lg"
               Width="100%" runat="server"  OnClick="btnSearch_Click" >โหลด</asp:LinkButton>
                                 
    </div>
        </div>
             </asp:Panel>
    
    <div class="row pb-1">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-10 ">
                        
                            <i class="far fa-file-alt"></i>&nbsp 
                             <asp:Label ID="lblCustomer" Font-Size="Large" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="card-body">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:ListView ID="grdLine2" runat="server">
                                <ItemTemplate>
                                    <div class="row ">
                                        <div class="col-md-6">
                                            <div class="row">
                                                <div class="col-md-12">

                                                </div>
                                            </div>
                                                 <div class="row">
                                                <div class="col-md-12">
                                                        <label class="customcheck" style="color: deeppink">
                                                <%# Eval("SOINVID") %>
                                                <input type="checkbox" runat="server" id="ckSelect" />
                                                <span class="checkmark"></span>
                                            </label>
                                        <div class="col-md-3" style="display: none;"><span style="color: gray; font-weight: bold; font-size: smaller">อินวอยซ์ : &nbsp</span><asp:Label ID="lblINV_ID" runat="server" Text='<%# Eval("SOINVID") %>'></asp:Label></div>
                                                </div>
                                            </div>
                                                <div class="row ">
                                                     
                                        <div class="col-md-12">
                                            <span style="color: gray; font-size: small">ค้างชำระ: &nbsp</span> <span style="color: black; font-weight: bold"><%#  Convert.ToDecimal(Eval("INVPendingAmt")).ToString("N2") %> &nbsp  </span><span style="color: gray; font-size: small">บาท</span>
                                        </div> 

                                    </div>
                                        </div>
                                        
                                 
                                        <div class="col-md-6 text-right">
                          
                                    <div class="row   pt-1" style="font-size: smaller">
                                        <div class="col-md-12">
                                            <span style="color: gray;">กำหนด: &nbsp</span> <strong><%# Convert.ToDateTime( Eval("PayDueDate")).ToString("dd/MM/yyyy") %></strong>  &nbsp
                                       <span style="color: gray;">ยอดเต็ม: &nbsp</span> <span><strong><%#  Convert.ToDecimal(Eval("NetTotalAmtIncVat")).ToString("N2") %> </strong>&nbsp บาท </span>
                                        </div>
                                    </div>

                                        </div>
                                    </div>
                     
                                    <div>
                                        <hr class="style4" />
                                    </div>
                                </ItemTemplate>
                            </asp:ListView>

                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>
            </ContentTemplate> 
                   
            </asp:UpdatePanel>
         
           
    <div class="row text-center">
        <div class="col-md-12">
            <span style="color: dimgray; font-size: 10px">SO-RCSELECTINV</span>
        </div>
    </div>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>