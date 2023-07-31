﻿<%@ Page Title="CustomerDetail" Language="C#" MasterPageFile="~/POSA1.Master" AutoEventWireup="true" CodeBehind="CustomerDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.CRM.CustomerDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddTopic" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
    <asp:HiddenField ID="hddcopy" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />



    <script type="text/javascript">

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

        //BEGIN Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK':
                    btnPostCode.DoClick();
                    break;
                case 'Cancel':
                    popup.Hide();
                    popMessage.Hide();
                    break;
            }
        }
        //END Pop Postback

        //popup show 
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }


        //begin prevent link double click
        var isSubmitted = false;
        function preventMultipleSubmissions() {
            if (!isSubmitted) {
                $('#<%=btnSave.ClientID %>').val('Submitting.. Plz Wait..');
                isSubmitted = true;
                return true;
            }
            else {
                return false;
            }
        }
        //end prevent link double click

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
        //end show msg

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <dx:ASPxPopupControl ID="popup1" AllowDragging="true" runat="server" ClientInstanceName="popup" ContentUrl="~/Feature/AddrList.aspx"
        PopupElementID="btnShowAddr" PopupAction="LeftMouseClick"
        Height="400px" Width="800px">
    </dx:ASPxPopupControl>

    <div class="row">
        <div class="col-md-8 mx-auto">
            <div class="row pl-1 pb-2">
                <div class="col-md-12 px-0">
                    <asp:LinkButton ID="btnBack" runat="server" width="100%" CssClass="btn btn-info" OnClick="btnBack_Click">
                       <i class="fas fa-chevron-square-left"></i>
                       <span class="kanit">Back </span>
                    </asp:LinkButton>
                </div>                 
            </div>

            <div class="row kanit">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:UpdatePanel ID="udppoinfo" runat="server">
                                <ContentTemplate>
                                    <div class="row pb-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card">
                                                <div class="card-header pt-2 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10"><i class="fas fa-user-circle"></i>&nbsp<span class="kanit">Basic info</span></div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>Customer ID</span>
                                                            <asp:TextBox ID="txtCustomerID" Enabled="false" CssClass="form-control form-control-sm" runat="server" placeholder="++NEW++"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>เลขประจำตัวผู้เสียภาษี (Tax ID)</span>
                                                            <asp:TextBox ID="txtTaxID" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                        <%--<div class="col-md-6">
                                                            <span>สาขา &nbsp</span>
                                                            <dx:ASPxComboBox ID="cboCompany" runat="server" CssClass="form-control form-control-sm" DropDownStyle="DropDownList" Theme="Mulberry"
                                                                ValueField="CompanyID" ValueType="System.String" ViewStateMode="Enabled" TextFormatString="{0}" Width="100%">
                                                                <Columns>
                                                                    <dx:ListBoxColumn FieldName="CompanyID" Caption="Compay code" />
                                                                    <dx:ListBoxColumn FieldName="Name" Width="350" Caption="Name" />
                                                                </Columns>
                                                            </dx:ASPxComboBox>
                                                        </div>--%>
                                                    </div>
                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>Name</span>
                                                            <asp:TextBox ID="txtNameTh1" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>Last Name</span>
                                                            <asp:TextBox ID="txtNameTh2" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    
                                                     <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>Mobile</span>
                                                            <asp:TextBox ID="txtMobile" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>Email</span>
                                                            <asp:TextBox ID="txtEmail" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>  

                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>Birthday</span>
                                                            <dx:ASPxDateEdit ID="dtBirthday" CssClass="kanit" runat="server" Theme="iOS" Width="100%"
                                                                DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                <TimeSectionProperties Visible="False">
                                                                    <TimeEditCellStyle HorizontalAlign="Right">
                                                                    </TimeEditCellStyle>
                                                                </TimeSectionProperties>
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                         <div class="col-md-6">
                                                               <span>Point</span>
                                                                 <span style="font-size: xx-large;color:brown;">
                                                                     <b>
                                                            <asp:Literal ID="lblPoint" runat="server"></asp:Literal>
                                                                         </b>
                                                        </span>
                                                             </div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card">
                                                <div class="card-header pt-2 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10"><i class="fas fa-home"></i>&nbsp<span class="kanit">Address</span></div>
                                                    </div>
                                                </div>
                                                <div class="card-body pt-1 pb-2">
                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>เลขที่ </span>
                                                            <asp:TextBox ID="txtAddrNo" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>หมู่/ถนน </span>
                                                            <asp:TextBox ID="txtAddrMoo" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>รหัสไปรษณีย์ </span>
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtAddrPostCode" Height="32px" runat="server" class="form-control form-control-sm kanit" ReadOnly="true" placeholder="เลือกที่อยู่จากรหัสไปรษณีย์"></asp:TextBox>
                                                                <div class="input-group-append">
                                                                    <button class="btn btn-secondary pb-0" id="btnShowAddr" type="button">
                                                                        <i class="fa fa-list"></i>
                                                                    </button>
                                                                </div>
                                                                <dx:ASPxButton ID="btnPostCodeRefresh" runat="server" ClientInstanceName="btnPostCode" ClientVisible="false"
                                                                    OnClick="btnPostCodeRefresh_Click">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>ตำบล/แขวง </span>
                                                            <asp:TextBox ID="txtAddrTumbon" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row kanit">
                                                        <div class="col-md-6">
                                                            <span>เขต/อำเภอ</span>
                                                            <asp:TextBox ID="txtAddrAmphoe" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <span>จังหวัด</span>
                                                            <asp:TextBox ID="txtAddrProvince" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    


                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row pb-2">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <div class="card">
                                                <div class="card-header pt-2 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-10"><i class="fas fa-home"></i>&nbsp<span class="kanit">Customer info</span></div>
                                                    </div>
                                                </div>

                                                <div class="card-body pb-2">
                                                    <div class="row pb-2 kanit">
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <asp:label runat="server" CssClass="col-md-6">1.ผิวของลูกค้ามีลักษณะอย่างไร</asp:label> 
                                                                <div class="col-md-6">
                                                                    <asp:DropDownList runat="server" ID="cboList1" CssClass="form-control form-control-sm kanit" DropDownStyle="DropDownList">
                                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="ผิวคลํ้า" Value="1"></asp:ListItem>
                                                                        <asp:ListItem Text="ผิวขาวใส" Value="2"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>                                                         
                                                        </div>   
                                                        <div class="col-md-6">
                                                            <div class="row">
                                                                <asp:label runat="server" CssClass="col-md-6">2.เคยใช้สินค้าของเราหรือไม่</asp:label> 
                                                                <div class="col-md-6">
                                                                    <asp:DropDownList runat="server" ID="cboList2" CssClass="form-control form-control-sm kanit" DropDownStyle="DropDownList">
                                                                        <asp:ListItem Text="" Value=""></asp:ListItem>
                                                                        <asp:ListItem Text="เคยใช้" Value="3"></asp:ListItem>
                                                                        <asp:ListItem Text="ไม่เคยใช้" Value="4"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>                                                         
                                                        </div>
                                                    </div> 
                                                    
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                   
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>

                    </div>

                    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                </div>
            </div>

            <div class="row pt-2">
                <div class="col-md-12 text-center">
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success" OnClick="btnSave_Click">
                        <i class="fas fa-save"></i>&nbsp<span class="kanit">Save</span> 
                    </asp:LinkButton>&nbsp
                        <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-danger" OnClick="btnDel_Click">
                        <i class="fas fa-trash"></i>&nbsp<span class="kanit">Delete</span> 
                        </asp:LinkButton>
                </div>
            </div>

            <br />
           
            <div class="row">
                 <div class="col-md-11 mx-auto">
                     <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                    <strong>
                        <span id="myalertHead" class="kanit"></span></strong>
                    <br />
                    <span id="myalertBody" class="kanit"></span>
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                 </div>
            </div>

        </div>
    </div>


</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>