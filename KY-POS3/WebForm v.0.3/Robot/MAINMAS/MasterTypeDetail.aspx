﻿<%@ Page Title="Option" Language="C#" MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" CodeBehind="MasterTypeDetail.aspx.cs" Inherits="Robot.MAINMAS.MasterTypeDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <%--    <asp:HiddenField ID="hddid" runat="server" />--%>
    <%--    <asp:HiddenField ID="hddoption" runat="server" />--%>
    <asp:HiddenField ID="hddmenu" runat="server" />
    <%--  <asp:HiddenField ID="hddrefid" runat="server" />--%>

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

        function mypopprofile() {
            btnPostProfile.DoClick();
        }

        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
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

            <div class="card-header">
                <div class="row">
                    <div class="col-md-4">
                        <asp:LinkButton ID="btnBackList" Font-Size="Small"
                            CssClass="btn btn-default" runat="server"
                            OnClick="btnBackList_Click">                                          
                            <span style="color:black"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:black"> Back</span>
                                             
                        </asp:LinkButton>
                    </div>
                    <div class="col-md-4 text-center">
                        <asp:Label runat="server" Font-Size="XX-Large" Font-Bold="true" ForeColor="black" ID="lblinfohead"></asp:Label>
                    </div>
                    <div class="col-md-4 text-right">
                        <asp:LinkButton ID="btnNew" CssClass="btn btn-info" runat="server"
                            OnClick="btnNew_Click">   
                            <span >***NEW***</span> 
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="row pb-2">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row ">
                                <div class="col-md-5">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <h4>
                                                <asp:Label runat="server" ID="lblMasterID"></asp:Label>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <div style="overflow-x: auto; width: 100%">
                                                <asp:UpdatePanel ID="updgrd" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <dx:ASPxGridView ID="grd" runat="server" 
                                                            ClientInstanceName="grd"
                                                            KeyFieldName="ValueTXT"
                                                            AutoGenerateColumns="False"
                                                            Width="100%"
                                                            OnDataBinding="grd_DataBinding"
                                                            EnableTheming="True"
                                                            KeyboardSupport="true"                                                            
                                                            OnHtmlRowPrepared="grd_HtmlRowPrepared"
                                                            Theme="Moderno"
                                                            CssClass="Sarabun"
                                                            OnRowCommand="grd_RowCommand">
                                                            <SettingsPager PageSize="100"/>
                                                            
                                                            <Columns>
                                                                <dx:GridViewDataTextColumn FieldName="ValueTXT" Caption="ตัวเลือก" Width="160px">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                    <DataItemTemplate>
                                                                        <asp:LinkButton ID="btnOpen" Width="100px" runat="server"   CssClass="Sarabun" 
                                                                            CommandName="editrow" CommandArgument='<%# Eval("ValueTXT") %>'>
                                                                                            <%# Eval("Description1") %>
                                                                        </asp:LinkButton>
                                                                    </DataItemTemplate>
                                                                    <HeaderStyle  Wrap="False"></HeaderStyle>
                                                                    <CellStyle Wrap="False" ></CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Description2" Width="200px" Caption="รายละเอียด 2" >
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                    <HeaderStyle   CssClass="Sarabun"  Wrap="False"></HeaderStyle>
                                                                    <CellStyle Wrap="False" ></CellStyle>
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn FieldName="Sort" Caption="ลำดับที่" Width="60px" >
                                                                    <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                    <HeaderStyle CssClass="Sarabun" ></HeaderStyle>
                                                                    <CellStyle Wrap="False">
                                                                    </CellStyle>
                                                                   
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataCheckColumn FieldName="IsActive" Caption="ใช้งาน">
                                                                    <HeaderStyle ></HeaderStyle>
                                                                    <CellStyle Wrap="False" ></CellStyle>
                                                                </dx:GridViewDataCheckColumn>
                                                            </Columns>
                                                        </dx:ASPxGridView>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="grd" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-7">
                                    <div class="row ">
                                        <div class="col-md-12">
                                            <div class="card bg-light">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">รหัสตัวเลือก </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtID" CssClass="form-control" 
                                                           Placeholder="***NEW****" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>
                                                        <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">รายละเอียด1 </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtdesc1" CssClass="form-control" Placeholder="" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>
                                                        <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">รายละเอียด2 </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtdesc2" CssClass="form-control" Placeholder="" runat="server"></asp:TextBox>           
                                                    </div>
                                                </row>
                                                        <row class="form-inline form-horizontal pt-2">
                                                    <label class="control-label col-md-2 pl-1">ลำดับ </label>
                                                    <div class="input-group col-md-10">
                                                       <asp:TextBox ID="txtSort" CssClass="form-control " runat="server" Style="text-align: right"
                                                             onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                    </div>
                                                </row> 
                                                    </div>
                                                </div>
                                                <div class="row pt-2  pr-3" >
                                                            <div class="col-md-2">
                                                                </div>
                                                            <div class="col-md-4">
                                                                 <asp:CheckBox ID="chkIsActive" runat="server"   Text="ใช้งาน"></asp:CheckBox>
                                                                </div>
                                                    <div class="col-md-6 text-right">
                                                           <asp:Button ID="btnOK"
                                                            Width="200"
                                                            CssClass="btn btn-success btn-lg "
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
                                    </div>

                                    <div class="row pt-2 text-center" style="display: none;">
                                        <div class="col-md-12">
                                            <div class="card bg-light py-2">
                                                <asp:Literal ID="lblInfo" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pt-3 " runat="server" id="divProfile">
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
                                                            <asp:UpdatePanel ID="udpProfile" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="row">
                                                                        <div class="col-md-2">
                                                                            <asp:Label runat="server" Font-Size="Larger" >รูปภาพ</asp:Label>
                                                                        </div>
                                                                        <div class="col-md-8 text-center">
                                                                            <asp:Image ID="imgProfile" runat="server"
                                                                                ImageAlign="Middle" Style="width: 150px" />
                                                                        </div>
                                                                        <div class="col-md-2"></div>
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