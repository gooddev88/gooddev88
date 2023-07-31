﻿<%@ Page Title="BookBank Info" Language="C#"  MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" CodeBehind="BookBankDetail.aspx.cs" Inherits="Robot.OMASTER.BookBankDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
 
    <asp:HiddenField ID="hddmenu" runat="server" />

    <script type="text/javascript">
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
    </script>


    <asp:UpdateProgress ID="udppPost" runat="server" AssociatedUpdatePanelID="udpPost">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="udpPost" runat="server">
        <ContentTemplate>

            <div class="card-header bg-success">
                <div class="row">
                    <div class="col-md-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:white"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span class="" style="font-size:medium;color:white"> กลับ</span>
                                             
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4 text-center">
                        <asp:Label runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="White" ID="lblinfohead"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                  
                    </div>
                </div>
            </div>

            <div class="row pt-1">
                <div class="col-md-12">

                    <div class="row ">
                        <div class="col-sm-12 col-md-5 pr-1">
                            <div class="card bg-light">
                                <div class="card-header" style="background-color:rgba(0, 0, 0, 0.03) !important;">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnSearch">
                                                <div class="row pb-1">
                                                    <div class="col-5 pt-1 pr-0">
                                                        <dx:ASPxTextBox ID="txtSearch" runat="server" Width="100%" CssClass="form-control form-control-sm" Theme="Material"></dx:ASPxTextBox>
                                                    </div>
                                                    <div class="col-3 pt-1">
                                                        <asp:LinkButton ID="btnSearch" Width="100%" runat="server" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                                                    <i class="fa fa-search"></i>&nbsp<span class="">ค้นหา</span> 
                                                        </asp:LinkButton>
                                                    </div>
                                                          <div class="col-4 pt-3 text-right">
                            <asp:CheckBox ID="chkShowClose" Text="แสดงที่ยกเลิก" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowClose_CheckedChanged" />
                        </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:UpdatePanel ID="updgrd" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate> 
                                                <dx:ASPxGridView ID="grd" runat="server" ClientInstanceName="grd"
                                                    KeyFieldName="BookID" 
                                                    CssClass="Sarabun"
                                                    AutoGenerateColumns="False" Width="100%"
                                                    OnDataBinding="grd_DataBinding"
                                                    EnableTheming="True"
                                                    KeyboardSupport="true" OnHtmlRowPrepared="grd_HtmlRowPrepared"
                                                    Theme="MaterialCompact" 
                                                    OnRowCommand="grd_RowCommand">
                                                    <SettingsPager PageSize="100">
                                                    </SettingsPager>
                                                          <SettingsResizing ColumnResizeMode="Control" /> 
                                                        <Settings ShowTitlePanel="true" ShowFilterRow="true" ShowFilterBar="Auto"
                                        HorizontalScrollBarMode="Auto"
                                        VerticalScrollableHeight="400"
                                        VerticalScrollBarMode="Auto" />
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="BookID" Caption="เลขบัญชี" Width="150px" VisibleIndex="0" HeaderStyle-CssClass="Sarabun">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="btnOpen" Width="120px" runat="server"  
                                                                    CommandName="editrow" CommandArgument='<%# Eval("BookID") %>'>
                                                                            <%# Eval("BookID") %>
                                                                </asp:LinkButton>
                                                            </DataItemTemplate>
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BookDesc" Width="200" Caption="ชื่อบัญชี" HeaderStyle-CssClass="Sarabun" CellStyle-CssClass="Sarabun">
                                                            <Settings AutoFilterCondition="Contains" /> 
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BankName" Caption="ธนาคาร" Width="100%">
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                       
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="BranchName" Caption="สาขา" Width="120px" >
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                           
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Sort" Caption="ลำดับ" Width="120">
                                                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                   
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataCheckColumn FieldName="IsActive" Caption="ใช้งาน" Width="80">
                                                            <HeaderStyle CssClass=""></HeaderStyle>
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                        </dx:GridViewDataCheckColumn>
                                                    </Columns>
                                              
                                                </dx:ASPxGridView>
                                                         
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="grd" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
      <asp:AsyncPostBackTrigger ControlID="chkShowClose" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-7 pl-1">
                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="card bg-light">
                                        <div class="card-header" style="background-color:rgba(0, 0, 0, 0.03) !important;">
                                            <div class="row ">
                                                <div class="col-6">
                                                       <h4>เพิ่มแก้ไข</h4>
                                                </div>
                                                   <div class="col-6 text-right">
                                                            <asp:LinkButton ID="btnNew" CssClass="btn btn-warning" runat="server"
                            OnClick="btnNew_Click">   
                                            <span class="" >***NEW***</span> 
                        </asp:LinkButton>
                                                   </div>
                                            </div>
                                         
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">เลขบัญชี </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtID" CssClass="form-control" Placeholder="***NEW***"
                                                           runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>
                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">ชื่อบัญชี </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtBookDesc" CssClass="form-control" Placeholder="" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>

                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">ธนาคาร </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:DropDownList ID="cboBankCode" runat="server" CssClass="form-control form-control-sm " DataTextField="Name_TH" DataValueField="BankCode"></asp:DropDownList>       
                                                    </div>
                                                </row>

                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">สาขา </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtBranchName" CssClass="form-control" Placeholder="" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>

                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">ลำดับ</label>
                                                    <div class="input-group col-md-10">
                                                        <asp:TextBox ID="txtSort" CssClass="form-control " runat="server" onkeypress="javascript:return isNumber(event)"></asp:TextBox>
                                                    </div>                                                    
                                                </row>
                                                <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1 ">หมายเหตุ </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtRemark1" CssClass="form-control" TextMode="MultiLine" Height="80" Placeholder="หมายเหตุ" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>
                                                <row class="form-inline form-horizontal pt-2">
                                                           <label class="control-label col-md-2 pl-1">  </label>
                                                    <div class="input-group col-md-10">
                                                     <asp:CheckBox ID="chkIsActive" runat="server" Text="ใช้งาน"></asp:CheckBox>
                                                    </div>
                                                </row>

                                      
                                            </div>
                                        </div>

                                        
                                             <div class="row pr-3">
                                                 <div class="col-6">

                                                 </div>
                                                        <div class="col-6">
                                                                <asp:Button ID="btnOK"
                                                            Width="100%"
                                                            CssClass="btn btn-info btn-lg "
                                                            OnClick="btnOK_Click"
                                                            OnClientClick="this.disabled='true';"
                                                            UseSubmitBehavior="false"
                                                            runat="server"
                                                            Text="บันทึก"></asp:Button>
                                                 </div>
                                               
                                               </div>
                                        <div class="row text-center pt-2 pl-2 pr-2">
                                            <div class="col-md-12">
                                                <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                                                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                                    <strong>
                                                        <span id="myalertHead" class=""></span></strong>
                                                    <br />
                                                    <span id="myalertBody" class=""></span>
                                                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>
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
            <asp:AsyncPostBackTrigger ControlID="btnOK" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>