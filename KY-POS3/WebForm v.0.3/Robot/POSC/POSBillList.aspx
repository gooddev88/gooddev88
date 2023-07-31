﻿<%@ Page Title="เช็คบิล" Language="C#" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSBillList.aspx.cs" Inherits="Robot.POSC.POSBillList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddcompany" runat="server" />
    <asp:HiddenField ID="hddParentPage" runat="server" />

    <%--begin of Loading callback script--%>
    <script>
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };

        function OnBeginCallback(s, e) {
            LoadingPanel.Show();
        };

        function OnEndCallback(s, e) {
            LoadingPanel.Hide();
        };
    </script>


    <dx:ASPxLoadingPanel ID="LoadingPanel" ClientInstanceName="LoadingPanel"
        Theme="Material"
        runat="server"
        Modal="true"
        HorizontalAlign="Center"
        VerticalAlign="Middle">
    </dx:ASPxLoadingPanel>
    <asp:UpdateProgress ID="udppPost" runat="server" AssociatedUpdatePanelID="udpPost">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="udpPost" runat="server">
        <ContentTemplate>
            <div class="row pt-2">
                <div class="col-md-12">
                    <div class="row  ">
                        <div class="col-12">
                            <div class="card">
                                <div class="card-header pt-1 pb-1">
                                    <div class="row ">
                                        <div class="col-md-8">
                                            <i class="fa fa-filter"></i>
                                            &nbsp<span ><%=hddTopic.Value %></span>
                                        </div>
                                        <div class="col-md-4 text-right">
                                            
                                             <div class="btn-group" role="group" aria-label="Basic example">
                             <asp:LinkButton ID="btnShowCancelOrder" CssClass="btn btn btn-secondary" runat="server" Text="แสดงยกเลิกออเดอร์" OnClick="btnShowCancelOrder_Click"></asp:LinkButton>
                                                 <asp:LinkButton ID="btnExcel" CssClass="btn btn btn-secondary" runat="server" Text="Excel" OnClick="btnExcel_Click"></asp:LinkButton>

                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body pt-1 pb-1">
                                    <div class="row ">
                                        <div class="col-md-11">
                                            <asp:UpdatePanel ID="uptfilterby" runat="server">
                                                <ContentTemplate>
                                                    <div class="row">
                                                        <div class="col-4">
                                                            <span>สาขา </span>
                                                            <asp:DropDownList ID="cboCompany" runat="server" CssClass="form-control form-control-sm " DataTextField="Name" DataValueField="CompanyID"></asp:DropDownList>

                                                        </div>
                                                        <div class="col-3">
                                                            <br />
                                                            <asp:LinkButton ID="btnSearch" CssClass="btn btn btn-secondary btn-sm" runat="server" Text="Load" OnClick="btnSearch_Click"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="row pb-0" style="display: none;">
                                                        <div class="col-2">
                                                            <span>เลขโต๊ะ</span>
                                                            <asp:DropDownList ID="cboTable" runat="server" CssClass="form-control form-control-sm " DataTextField="TableName" DataValueField="TableID"></asp:DropDownList>
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                        <div class="row">
                                            <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="row pt-1">
                <div class="col-lg-12 col-md-12">
                    <div  style="overflow-x: auto; width: 100%">
                        <dx:ASPxGridView ID="grdDetail" runat="server"
                            EnableTheming="True"
                            Theme="MaterialCompact"
                            AutoGenerateColumns="False"
                            KeyFieldName="BillID"
                            OnDataBinding="grdDetail_DataBinding"
                            OnRowCommand="grdDetail_RowCommand"
                            KeyboardSupport="True">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="" Caption="เลือก" Width="80px">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnOpen" runat="server" CssClass="btn btn-icons btn-default"
                                            CommandName="sel" CommandArgument='<%# Eval("BillID") %>'>
                                            <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>
                              
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="เบอร์โต๊ะ" FieldName="TableName">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ออเดอร์" FieldName="BillID" Width="160px">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataDateColumn Caption="วันที่บิล" FieldName="BillDate">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="จำนวนรายการ" FieldName="Qty">
                                    <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="จำนวนเงิน" FieldName="NetTotalAmt">
                                    <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ภาษี" FieldName="NetTotalVatAmt">
                                    <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="จำนวนเงิน(Inc.VAT)" FieldName="NetTotalAmtIncVat">
                                    <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ชื่อลูกค้า" FieldName="CustomerName">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สาขาที่ขาย" FieldName="ComID">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สาขาลูกค้า" FieldName="CustBranchName">
                                    <Settings AutoFilterCondition="Contains" />
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="เลขที่เสียภาษี" FieldName="CustTaxID">
                                    <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Created by" FieldName="CreatedBy">
                                    <HeaderStyle  Wrap="False" />
                                    <CellStyle  Wrap="False" />
                                </dx:GridViewDataTextColumn>

                            </Columns>
                        </dx:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
                    </div>

                    



                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>