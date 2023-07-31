<%@ Page Title="User Group" Language="C#" MasterPageFile="~/MAINMAS/SiteA.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="MyUserGroupDetail.aspx.cs" Inherits="Robot.MAINMAS.MyUserGroupDetail" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<asp:HiddenField ID="hddid" runat="server" />--%>
    <asp:HiddenField ID="hddmenu" runat="server" />

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
        function OnClosePopupAlert() {
            popAlert.Hide();
        }
        //end show msg

        function Check_Click(objRef) {

            //Get the Row based on checkbox

            var row = objRef.parentNode.parentNode;


            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
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
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        //row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            //row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
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
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="udpPopAlert" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="400" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popAlert"
                HeaderText="Information" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
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
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="Close" OnClientClick="return OnClosePopupAlert();" />

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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>


    <div class="row">
        <div class="col-md-8 mx-auto">
            <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                        <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="udpmain" runat="server" UpdateMode="Conditional">
                <ContentTemplate>


                    <div class="row pt-2">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-header bg-dark">
                                    <div class="row ">
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                                CssClass="btn btn-default" runat="server"
                                                OnClick="btnBackList_Click">                                          
                            <span style="color:white"> <i class="fas fa-reply-all fa-2x"></i></span>
                            <span  style="font-size:medium;color:white"> Back</span>                                            
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-4 text-center">
                                            <span style="font-size: x-large; color: white">
                                                <i class="fas fa-users-cog fa-2x"></i>
                                                <strong>
                                                    <asp:Literal ID="lblTopic" runat="server"></asp:Literal>
                                                </strong>
                                            </span>
                                        </div>
                                        <div class="col-md-4 text-right ">
                                            <div class="btn-group" role="group" aria-label="Basic example">
                                                <asp:LinkButton ID="btnNew" CssClass="btn btn-info " runat="server" OnClick="btnNew_Click">   
                                      <span >***NEW***</span> 
                                                </asp:LinkButton>
                                                <asp:Button ID="btnSave"
                                                    CssClass="btn btn-success  "
                                                    OnClick="btnSave_Click"
                                                    OnClientClick="this.disabled='true';"
                                                    UseSubmitBehavior="false"
                                                    runat="server"
                                                    Text="บันทึก"></asp:Button>

                                            </div>

                                        </div>

                                    </div>
                                </div>
                                <div class="card-body bg-light">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row " runat="server" id="divHome">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <span>รหัสกลุ่มผู้ใช้งาน</span>
                                                            <asp:TextBox runat="server" ID="txtUserGroupId" CssClass="form-control " />

                                                        </div>
                                                        <div class="col-md-4">
                                                            <span>ชื่อกลุ่ม</span>
                                                            <asp:TextBox runat="server" ID="txtGroupName" CssClass="form-control " />
                                                        </div>
                                                        <div class="col-md-2">
                                                            <span>จัดเรียง</span>
                                                            <asp:TextBox runat="server" ID="txtSort" onkeypress="return DigitOnly(this,event)" CssClass="form-control" />
                                                        </div>

                                                        <div class="col-md-2 pt-4">

                                                            <asp:CheckBox ID="chkActive" runat="server" Text="ใช้งาน" />
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
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
            <div class="row pt-2" runat="server" id="divPermission">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header bg-dark">
                            <div class="row " style="color: white">
                                <div class="col-md-6">
                                    <i class="fas fa-door-open fa-2x"></i>&nbsp
                                    <span>Permission Info</span>
                                </div>
                                <div class="col-md-6 text-right ">
                                </div>

                            </div>
                        </div>
                        <div class="card-body bg-light">
                            <div class="row" runat="server" id="divCompanyAccess">
                                <div class="col-md-12">
                                    <div id="accordion">
                                        <div class="row pt-2">
                                            <div class="col-md-12">
                                                <div class="card">
                                                    <div class="card-header" style="background-color: cornflowerblue !important">
                                                        <div class="row">
                                                            <div class="col">
                                                                <a class="card-link" data-toggle="collapse" href="#collapseOne">
                                                                    <span style="color: black">สาขาที่รับผิดชอบ</span>
                                                                </a>
                                                            </div>

                                                            <div class="col text-right">
                                                                <asp:CheckBox ID="chkselectCompany" runat="server" AutoPostBack="true"
                                                                    OnCheckedChanged="chkselectCompany_CheckedChanged"
                                                                    Text="เลือกสาขา ทั้งหมด" />
                                                                <asp:LinkButton ID="btnCopyCom" runat="server"
                                                                    CssClass="btn btn-outline-dark" OnClick="btnCopyCom_Click">
                                                                    <span style="color:red">     <i class="fas fa-copy"></i></span>   
                                                                    <span >Copy สาขา</span> 
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div id="collapseOne" class="collapse" data-parent="#accordion">
                                                        <asp:UpdatePanel ID="udpgrdstore" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <div class="card-body bg-light">
                                                                    <div style="width: 100%; overflow: auto;">
                                                                        <asp:GridView ID="grdUserInCompany" runat="server" AutoGenerateColumns="False"
                                                                            OnRowDataBound="grdUserInCompany_RowDataBound"
                                                                            Font-Size="Smaller"
                                                                            ShowHeaderWhenEmpty="True"
                                                                            EmptyDataText="Data Not Found"
                                                                            CssClass="table table-striped table-bordered table-hover">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Select">
                                                                                    <HeaderTemplate>
                                                                                        <%--<asp:CheckBox ID="chkcheckAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkcheckAll_CheckedChanged" />--%>
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsInCompany" runat="server" Checked='<%# Bind("X") %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Type" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCompanyTypeName" runat="server" Text='<%# Bind("CompanyTypeName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Wrap="false" />
                                                                                    <HeaderStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="สาขา">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCompanyID" runat="server" Text='<%# Bind("CompanyID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Wrap="false" />
                                                                                    <HeaderStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="ชื่อ">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCompanyName" runat="server" Text='<%# Bind("CompanyName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Wrap="false" />
                                                                                    <HeaderStyle Wrap="false" Width="100%" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="chkselectCompany" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row pt-2">
                                            <div class="col-md-12">
                                                <div class="card">
                                                    <div class="card-header" style="background-color: cornflowerblue !important">
                                                        <a class="collapsed card-link" data-toggle="collapse" href="#collapseTwo">
                                                            <span style="color: black">สิทธิ์ใช้ Dashboard</span>
                                                        </a>
                                                    </div>
                                                    <div id="collapseTwo" class="collapse" data-parent="#accordion">
                                                        <div class="card-body bg-light">
                                                            <div class="row" runat="server" id="divDashBoard">
                                                                <div class="col-md-12">
                                                                    <div style="width: 100%; overflow: auto;">
                                                                        <asp:GridView ID="grdDashBoard" runat="server"
                                                                            Font-Size="Smaller"
                                                                            AutoGenerateColumns="False"
                                                                            ShowHeaderWhenEmpty="True"
                                                                            EmptyDataText="Data not found"
                                                                            CssClass="table table-striped table-bordered table-hover">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsInBoard" runat="server" Checked='<%# Bind("X") %>' />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="false" Width="60px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Board ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDashBoardID" runat="server" Text='<%# Bind("DashBoardID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Wrap="false" />
                                                                                    <HeaderStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Wrap="false" />
                                                                                    <HeaderStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row pt-2">
                                            <div class="col-md-12">
                                                <div class="card">
                                                    <div class="card-header" style="background-color: cornflowerblue !important">
                                                        <div class="row">
                                                            <div class="col">
                                                                <a class="card-link" data-toggle="collapse" href="#collapseThree">
                                                                    <span style="color: black">กำหนดสิทธิ์ ตามเมนู</span>
                                                                </a>
                                                            </div>

                                                            <div class="col text-right">
                                                                <asp:LinkButton ID="btnCopyRole" runat="server"
                                                                    CssClass="btn btn-outline-dark" OnClick="btnCopyRole_Click">
                                                                    <span style="color:red">     <i class="fas fa-copy"></i></span>   
                                                                    <span >Copy เมนู</span> 
                                                                </asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="collapseThree" class="collapse" data-parent="#accordion">
                                                        <div class="card-body bg-light">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <div id="grdCharges" runat="server" style="width: 100%; overflow: auto;">
                                                                        <asp:GridView ID="grdPermission"
                                                                            runat="server"
                                                                            AutoGenerateColumns="False"
                                                                            OnRowDataBound="grdPermission_RowDataBound"
                                                                            Font-Size="Smaller"
                                                                            ShowHeaderWhenEmpty="True"
                                                                            EmptyDataText="No data found"
                                                                            CssClass="table table-striped table-bordered table-hover">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="User Group" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblUserGroupId" runat="server" Text='<%# Bind("UserGroupId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="false" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:TemplateField>


                                                                                <asp:TemplateField HeaderText="Menu Code">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMenuCode" runat="server" Text='<%# Bind("MenuCode") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="false" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                     
                                                                                <asp:TemplateField HeaderText="Menu ID" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMenuId" runat="server" Text='<%# Bind("MenuId") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="false" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Menu Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMenuName" runat="server" Text='<%# Bind("MenuName") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle Wrap="false" />
                                                                                    <ItemStyle Wrap="false" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Open">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsOpen" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="lblIsOpen" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIsOpen" runat="server" Text='<%# Bind("IsOpen") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="NeedOpenPermission" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNeedOpen" runat="server" Text='<%# Bind("NeedOpenPermission") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Create">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsCreate" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="lblIsCreate" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIsCreate" runat="server" Text='<%# Bind("IsCreate") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="NeedCreatePermission" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNeedCreate" runat="server" Text='<%# Bind("NeedCreatePermission") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Edit">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsEdit" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="lblIsEdit" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIsEdit" runat="server" Text='<%# Bind("IsEdit") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="NeedEditPermission" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNeedEdit" runat="server" Text='<%# Bind("NeedEditPermission") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsDelete" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="lblIsDelete" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIsDelete" runat="server" Text='<%# Bind("IsDelete") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="NeedDeletePermission" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNeedDelete" runat="server" Text='<%# Bind("NeedDeletePermission") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Print">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkIsPrint" runat="server" />
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="lblIsPrint" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblIsPrint" runat="server" Text='<%# Bind("IsPrint") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="NeedPrintPermission" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblNeedPrint" runat="server" Text='<%# Bind("NeedPrintPermission") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle HorizontalAlign="Center" Wrap="false" />
                                                                                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>





                                         <div class="row pt-2">
                                            <div class="col-md-12">
                                                <div class="card">
                                                    <div class="card-header" style="background-color: cornflowerblue !important">
                                                        <div class="row">
                                                            <div class="col">
                                                                <a class="card-link" data-toggle="collapse" href="#collapseFour">
                                                                    <span style="color: black">ผู้ใชังานในกลุ่ม</span>
                                                                </a>
                                                            </div>

                                                            <div class="col text-right">
                                                               
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div id="collapseFour" class="collapse" data-parent="#accordion">
                                                        <div class="card-body bg-light">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                 <dx:ASPxGridView ID="grdUser" runat="server" 
                                                    ClientInstanceName="grdUser"
                                                    KeyFieldName="ID" 
                                                    AutoGenerateColumns="False" Width="100%"
                                                    OnDataBinding="grdUser_DataBinding"
                                                    EnableTheming="True"
                                                    CssClass="Sarabun"
                                                    KeyboardSupport="true"
                                                    OnHtmlRowPrepared="grdUser_HtmlRowPrepared"
                                                    Theme="Moderno"
                                                     OnRowCommand="grdUser_RowCommand"
                                                                     >
                                                    <Settings ShowGroupPanel="False"
                                                        ShowFooter="false" 
                                                        ShowGroupFooter="Hidden" />
                                                    <SettingsPager PageSize="100">
                                                    </SettingsPager>
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn FieldName="Username"  Caption="Username" Width="150px" VisibleIndex="0" Visible="false" >
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <DataItemTemplate>
                                                                <asp:LinkButton ID="btnOpen" Width="100px" runat="server" 
                                                                    CommandName="sel" CommandArgument='<%# Eval("Username") %>'>
                                                                    <%# Eval("Username") %>
                                                                </asp:LinkButton>
                                                            </DataItemTemplate>
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                               <CellStyle Wrap="False">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="Username"   Width="150px" Caption="Username" >
                                                            <Settings AutoFilterCondition="Contains" /> 
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn FieldName="FullName"  Caption="Name" Width="100%" >
                                                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <HeaderStyle CssClass="Sarabun" Wrap="False" />
                                                            <CellStyle Wrap="False">   </CellStyle> 
                                                        </dx:GridViewDataTextColumn>
                                                       
                                                    </Columns>
                                                    <TotalSummary>
                                                        <dx:ASPxSummaryItem FieldName="Username" SummaryType="Count" />
                                                    </TotalSummary>
                                                   
                                                    <Styles> 
                                                        <AlternatingRow Enabled="False"  />
                                                    </Styles>
                                                </dx:ASPxGridView>
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
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
