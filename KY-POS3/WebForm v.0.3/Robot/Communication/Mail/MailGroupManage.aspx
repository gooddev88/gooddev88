﻿<%@ Page Title="Mail Group" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="MailGroupManage.aspx.cs" ClientIDMode="Static" Inherits="Robot.Communication.Mail.MailGroupManage" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

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
        function OnClosePopupAlert(command) {
            switch (command) {
                case 'OK':
                    popAlert.Hide();
                    btnCloseMailGroup.DoClick();
                    break;
            }
        }

        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK':
                    popAlert.Hide();
                    break;
            }
        }

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

    <dx:ASPxPopupControl ID="popAlert" runat="server" Width="1200" Height="400" CloseAction="OuterMouseClick" CloseOnEscape="false" Modal="True"
        Theme="Material" HeaderText="Add mail"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
        AllowDragging="True"
        PopupAnimationType="None"
        EnableViewState="False"
        
        AutoUpdatePosition="true">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <dx:ASPxPanel ID="Panel1" runat="server" DefaultButton="btOK">
                    <PanelCollection>
                        <dx:PanelContent runat="server">

                               <div class="row ">
                            <div class="col-8 mx-auto">
                            <div class="row text-center">
                                <div class="col-12"></div>
                                <h4>
                                    <asp:Label runat="server" Font-Size="Larger" >New mail</asp:Label>
                                </h4>
                            </div> 
                            <div class="row ">
                                <div class="col-md-12">
                                    <span style="font: large">Group No.</span>
                                    <dx:ASPxComboBox ID="cboMailGroupID"
                                        runat="server"
                                        
                                        DropDownStyle="DropDownList"
                                        Theme="Material"
                                        ValueField="ValueTXT"
                                        ValueType="System.String"
                                        ViewStateMode="Enabled" TextFormatString="{0} {1}"
                                        Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="ValueTXT" Caption="Group ID" />
                                            <dx:ListBoxColumn FieldName="Description1" Width="250px" Caption="Description" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row ">
                                <div class="col-md-12">
                                    <span style="font: large">Email</span>
                                    <asp:TextBox ID="txtemail" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row ">
                                <div class="col-md-12">
                                    <span style="font: large">Remark</span>
                                    <asp:TextBox ID="txtRemark" CssClass="form-control " runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row ">
                                   <div class="col-6 pt-1">
                                         <asp:Label ID="lblAlertmsg" runat="server">  </asp:Label> 
                                       </div>
                                <div class="col-6 pt-1 text-right ">
                                    <asp:Button  Width="100" 
                                        ID="btnClose" 
                                        ForeColor="Red"
                                        CssClass="btn btn-default " 
                                        runat="server" Text="Close" 
                                        OnClick="btnClose_Click" />
                                    <asp:Button ID="btnSave"
                                        CssClass="btn btn-default " ForeColor="#9966ff" Width="100" runat="server" Text="เพิ่ม" OnClick="btnSave_Click" />
                                </div>

                            </div>

                            <hr />
                             
                               
                            <div class="row pt-1">
                                <div class="col-md-12">
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
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl>

     
    <dx:ASPxButton ID="btnCloseMailGroup" runat="server" ClientInstanceName="btnCloseMailGroup" ClientVisible="false"
        OnClick="btnCloseMailGroup_Click">
    </dx:ASPxButton>

    <div class="row">
        <div class="col-md-12">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header">
                            <div class="row">
                                <div class="col-2">
                                    <asp:LinkButton ID="btnBackList"
                                        CssClass="btn btn-default" runat="server"
                                        OnClick="btnBackList_Click"> 
                                                <i class="fa fa-chevron-circle-left"></i>&nbsp
                                             <span >Back</span> 
                                    </asp:LinkButton>

                                </div>
                                <div class="col-7 text-center ">
                                    <h4>
                                        <%=hddTopic.Value %>
                                    </h4>
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
                        </div>
                        <div class="card-body pt-1 pb-2">
                            <div class="row">
                                <div class="col-2 ">
                                    <asp:Label runat="server">Mail Group </asp:Label>
                                    <dx:ASPxComboBox ID="cboMailGroupFilter"
                                        runat="server"
                                        
                                        DropDownStyle="DropDown"
                                        Theme="Material"
                                        ValueField="ValueTXT"
                                        ValueType="System.String"
                                        ViewStateMode="Enabled"
                                        TextFormatString="{0} {1}"
                                        Width="100%">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="ValueTXT" Caption="Group ID" />
                                            <dx:ListBoxColumn FieldName="Description1" Width="250px" Caption="Description" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </div>

                                <div class="col-3 pt-4">
                                    <div class="input-group">
                                        <asp:TextBox runat="server" ID="txtSearch"
                                            Class="form-control] " placeholder="Search" />
                                        <div class="input-group-append">
                                            <asp:LinkButton ID="btnSearch" runat="server"
                                                CssClass="btn btn-success "
                                                OnClick="btnSearch_Click">
                                            <i class="fa fa-search"></i>&nbsp<span >ค้นหา</span> 
                                            </asp:LinkButton>
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
                <SettingsPager Mode="ShowPager" PageSize="80"></SettingsPager>
                <SettingsResizing ColumnResizeMode="Control" />
                <Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto"
                    HorizontalScrollBarMode="Auto"
                    VerticalScrollableHeight="400"
                    VerticalScrollBarMode="Auto" />
                <Settings ShowFooter="True" ShowGroupFooter="VisibleAlways"
                    ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
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
                    <dx:GridViewDataTextColumn Caption="Mail Group" FieldName="MailGroupID" Width="140">
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
                    <dx:GridViewDataTextColumn Caption="สร้างโดย" FieldName="CreatedBy" Width="100">
                        <Settings AutoFilterCondition="Contains" />
                        <HeaderStyle  Wrap="True" />
                        <CellStyle  Wrap="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="สร้างเมื่อ" FieldName="CreatedDate" Width="120">
                        <HeaderStyle  Wrap="True" />
                        <CellStyle  Wrap="False" />
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>

                </Columns>
            </dx:ASPxGridView>


            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
        </div>
    </div>

    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>