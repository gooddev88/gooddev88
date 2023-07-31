<%@ Page Title="สต็อก เคลื่อนไหว" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="StockMovementList.aspx.cs" Inherits="Robot.POS.StockMovementList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />


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
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
        </triggers>
    </asp:UpdatePanel>

    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header">
                    <div class="row">
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnBackList"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>                                            
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-6 text-center">
                            <asp:Label runat="server" Font-Size="X-Large" Font-Bold="true" ForeColor="Black" ID="lblinfohead"></asp:Label>
                        </div> 
                        <div class="col-md-4 text-right pb-2">
                                       <div class="btn-group" role="group" aria-label="Basic example">
  
                                  <asp:LinkButton ID="btnRecalStock" CssClass="btn btn-dark" runat="server"
                                OnClick="btnRecalStock_Click">
                                                <span  style="color:white">คำนวณสต๊อกใหม่</span> 
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnExcel" CssClass="btn btn-info" runat="server"
                                OnClick="btnExcel_Click">
                                                <span  style="color:white">Excel</span> 
                            </asp:LinkButton>
                                           </div>
                        </div>
                    </div>
                </div>
                <div class="card-body py-2">
                    <asp:UpdatePanel ID="UpdCompany" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="row" runat="server" id="divhide">
                        <div class="col-md-3">
                            <asp:Label runat="server">สาขา </asp:Label>
                            <asp:DropDownList ID="cboCompany" runat="server"
                                OnSelectedIndexChanged="cboCompany_SelectedIndexChanged"
                                AutoPostBack="true"
                                CssClass="form-control form-control-sm" DataTextField="Name"
                                DataValueField="CompanyID">
                            </asp:DropDownList>
                        </div>
                        <div class="col-2">
                            <asp:Label runat="server">ที่เก็บ </asp:Label>
                            <asp:DropDownList ID="cboLocation" runat="server" CssClass="form-control form-control-sm" DataTextField="LocCode" DataValueField="LocID"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <span style="font-size: medium">วันที่เริ่ม </span>
                            <dx:ASPxDateEdit ID="dtBegin" DisplayFormatString="dd-MM-yyyy" Theme="Material"
                                EditFormatString="dd-MM-yyyy" runat="server" CssClass=" " Width="100%">
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-2">
                            <span style="font-size: medium">ถึง </span>
                            <dx:ASPxDateEdit ID="dtEnd" DisplayFormatString="dd-MM-yyyy" Theme="Material"
                                EditFormatString="dd-MM-yyyy" runat="server" CssClass=" " Width="100%">
                            </dx:ASPxDateEdit>
                        </div>
                        <div class="col-md-3 pt-4">
                            <div class="input-group ">
                                <asp:TextBox runat="server" ID="txtSearch" Class="form-control "
                                    placeholder="ค้นหา" />
                                <div class="input-group-append bg-success border-success">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-success" OnClick="btnSearch_Click">
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
    <br />

    <div class="row">
        <div class="col-md-12">

            <div style="overflow-x: auto; width: 100%">
                <dx:ASPxGridView ID="grdDetail" runat="server" EnableTheming="True"
                    Theme="Material"
                    OnDataBinding="grdDetail_DataBinding"
                    AutoGenerateColumns="False"
                    KeyFieldName="DocID"
                    CssClass="Sarabun"
                    KeyboardSupport="True" Width="100%">
                    <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                    <Columns>
               
                     
                        <dx:GridViewDataTextColumn Caption="รหัสสินค้า" FieldName="ItemID" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ชื่อสินค้า" FieldName="ItemName" Width="100%">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                           <dx:GridViewDataTextColumn Caption="ที่เก็บ" FieldName="LocID" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                               <dx:GridViewDataTextColumn Caption="เข้า" FieldName="InQty" Width="120px">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <PropertiesTextEdit DisplayFormatString="n2" />
                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ออก" FieldName="OutQty" Width="120px">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <PropertiesTextEdit DisplayFormatString="n2" />
                            <CellStyle Wrap="False" HorizontalAlign="Right" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="เลขเอกสารเอกสาร" FieldName="DocID" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn Caption="วันที่เคลื่อนไหว" FieldName="StkDate" Width="120">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="ประเภทเอกสาร" FieldName="DocType" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>


                        <dx:GridViewDataTextColumn Caption="ประเภทสินค้า" FieldName="TypeID" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                 
                                 <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID" Width="130">
                            <HeaderStyle Wrap="False" CssClass="Sarabun" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>

                    </Columns>
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="InQty" ShowInColumn="InQty" ShowInGroupFooterColumn="InQty" SummaryType="Sum" DisplayFormat="{0:n0}" />
                        <dx:ASPxSummaryItem FieldName="OutQty" ShowInColumn="OutQty" ShowInGroupFooterColumn="OutQty" SummaryType="Sum" DisplayFormat="{0:n0}" />
                    </TotalSummary>
                </dx:ASPxGridView>
                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>
            </div>

        </div>
    </div>

</asp:Content>
