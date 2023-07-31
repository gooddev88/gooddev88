<%@ Page Title="Stock Adjust" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="POSStkAdjustList.aspx.cs" Inherits="Robot.POS.POSStkAdjustList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

    <script type="text/javascript">
        //begin Popup Postback
        function OnClosePopupAlert() {
            popAlert.Hide();
        }
    </script>

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
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  " runat="server" Text="CANCEL" OnClientClick="return OnClosePopupAlert();" />
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
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-8 text-center">
                            <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="Black" ID="lblinfohead"></asp:Label>
                        </div>
                        <div class="col-md-2 text-right pb-2">
                            <asp:LinkButton ID="btnNew" CssClass="btn btn-dark" runat="server" OnClick="btnNew_Click">   
                                <span >สร้างเอกสาร</span> 
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <div class="row">
                        <div class="col-2">
                            <span>สถานะ</span>
                            <asp:DropDownList ID="cboStatus" runat="server" CssClass="form-control" DataTextField="Desc" DataValueField="Value"></asp:DropDownList>
                        </div>
                        <div class="col-2">
                            <span>วันที่</span>
                            <dx:ASPxDateEdit ID="dtOrdDateBegin" DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-2">
                            <span>ถึง</span>
                            <dx:ASPxDateEdit ID="dtOrdDateEnd" DisplayFormatString="dd-MM-yyyy"
                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                            </dx:ASPxDateEdit>
                        </div>

                        <div class="col-md-3 pt-4">
                            <div class="input-group ">
                                <asp:TextBox runat="server" ID="txtSearch" Class="form-control"
                                    placeholder="ค้นหา" />
                                <div class="input-group-append bg-success border-success">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-success" OnClick="btnSearch_Click">
                                    <i class="fa fa-search"></i>&nbsp<span >Load</span> 
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 pt-4 text-left">
                            <asp:CheckBox ID="chkShowClose" Text="แสดงที่ยกเลิก" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowClose_CheckedChanged" />
                        </div>
                        <div class="col-md-6 text-right">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />

    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel ID="udp_list" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div style="overflow-x: auto; width: 100%">
                        <dx:ASPxGridView ID="grdDetail" runat="server" EnableTheming="True"
                            Theme="MaterialCompact"
                            OnDataBinding="grdDetail_DataBinding"
                            AutoGenerateColumns="False"
                            KeyFieldName="ID"
                            CssClass="Sarabun"
                            OnRowCommand="grdDetail_RowCommand"
                            KeyboardSupport="True" Width="100%">
                            <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="" Caption="" Width="60">
                                    <DataItemTemplate>
                                        <asp:LinkButton ID="btnOpen"
                                            runat="server"
                                            CssClass="btn btn-icons btn-default"
                                            CommandName="sel" CommandArgument='<%# Eval("ID") %>'>
                                             <i class="fa fa-folder-open"></i> 
                                        </asp:LinkButton>
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="เลขที่" FieldName="AdjID" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataDateColumn Caption="วันที่ปรับสต็อก" FieldName="AdjDate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="จำนวน" FieldName="AdjQty" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="ยอด" FieldName="AdjAmt" Width="130px">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <PropertiesTextEdit DisplayFormatString="n2" />
                                    <CellStyle Wrap="False" HorizontalAlign="Right" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn Caption="หมายเหตุ" FieldName="Remark" Width="300">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="สถานะ" FieldName="Status" Width="150">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="วันที่อนุมัติ" FieldName="ApprovedDate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="อนุมัติโดย" FieldName="ApprovedBy" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataDateColumn Caption="วันที่สร้าง" FieldName="CreatedDate" Width="120">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                                </dx:GridViewDataDateColumn>
                                <dx:GridViewDataTextColumn Caption="สร้างโดย" FieldName="CreatedBy" Width="130">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataCheckColumn Caption="ใช้งาน" FieldName="IsActive" Width="80">
                                    <HeaderStyle Wrap="False" CssClass="Sarabun" />
                                    <CellStyle Wrap="False" />
                                </dx:GridViewDataCheckColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
