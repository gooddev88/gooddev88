﻿<%@ Page Title="Mail Group" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="MailGroupMini.aspx.cs" ClientIDMode="Static" Inherits="Robot.Communication.Mail.MailGroupMini" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <asp:HiddenField ID="hddmenu" runat="server" />
 

    <%--begin of Loading callback script--%>
    <script>
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };
    </script>

    <script type="text/javascript">  
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
    </script>

    
    
         <div class="row">
                                <div class="col-2">
                                

                                </div>
                                <div class="col-7 text-center ">
                                   <h4>
                                           
                                       <asp:Label ID="lblTopic" runat="server" Font-Size="Larger" > </asp:Label>
                                </h4><br />
                                <asp:Label ID="lblMailGroupInfo" runat="server" Font-Size="Larger" > </asp:Label>
                                </div>
                                <div class="col-3 text-right">
                                    <div class="btn-group" role="group" aria-label="Basic example">
                                        <asp:LinkButton ID="btnNewx"
                                            runat="server"
                                            CssClass="btn btn-default"
                                            ForeColor="#9966ff"
                                            OnClick="btnNew_Click">
                                             <span >เพิ่มใหม่</span> 
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnExcel"
                                            ForeColor="#9966ff"
                                            CssClass="btn btn-default"
                                            runat="server"
                                            OnClick="btnExcel_Click">
                                                     <span >Excel</span> 
                                        </asp:LinkButton>

                                    </div>

                                </div>
                            </div>
    

    <div class="row pt-0">
        <div class="col-md-12">
            <dx:ASPxGridView ID="grdDetail" runat="server"
                EnableTheming="True"
                Theme="Moderno"
                AutoGenerateColumns="False"
                KeyFieldName="ID"
                OnDataBinding="grdDetail_DataBinding"
                OnRowCommand="grdDetail_RowCommand"
                KeyboardSupport="True"
                Width="100%">
                   <SettingsPager PageSize="80">
                    <PageSizeItemSettings Visible="true" ShowAllItem="true" />
                </SettingsPager>
                <SettingsPager Mode="ShowAllRecords"  ></SettingsPager>
                <SettingsResizing ColumnResizeMode="Control" />
                <Settings ShowTitlePanel="false" ShowFilterRow="false" ShowFilterBar="Hidden"
                    HorizontalScrollBarMode="Auto"
                    VerticalScrollableHeight="400"
                    VerticalScrollBarMode="Auto" />
                <Settings ShowFooter="false" ShowGroupFooter="Hidden"
                    ShowFilterBar="Hidden" ShowHeaderFilterButton="false" />
                <Columns>
                    <dx:GridViewDataTextColumn Width="80px">
                        <DataItemTemplate>
                          
                                <asp:LinkButton ID="btnDel" runat="server" CommandArgument='<%# Eval("ID") %>'
                                    CausesValidation="True" CommandName="Del"
                                    CssClass="btn btn-icons btn-default">
                                                <i class="fas fa-trash"></i>
                                </asp:LinkButton>
                            
                        </DataItemTemplate> 
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Mail Group" FieldName="MailGroupID" Width="140" Visible="false">
                        <Settings AutoFilterCondition="Contains" />
                        <HeaderStyle  Wrap="True" />
                        <CellStyle  Wrap="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" Width="100%">
                        <Settings AutoFilterCondition="Contains" />
                        <HeaderStyle  Wrap="True" />
                        <CellStyle  Wrap="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="หมายเหตุ" FieldName="Remark" Width="150">
                        <Settings AutoFilterCondition="Contains" />
                        <HeaderStyle  Wrap="True" />
                        <CellStyle  Wrap="False" />
                    </dx:GridViewDataTextColumn>
               

                </Columns>
            </dx:ASPxGridView>


            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
        </div>
    </div>

    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
         
    <dx:ASPxButton ID="btnCloseMailGroup" runat="server" ClientInstanceName="btnCloseMailGroup" ClientVisible="false"
        OnClick="btnCloseMailGroup_Click">
    </dx:ASPxButton>
</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>