﻿<%@ Page Title="QuestionDetail" Language="C#" MasterPageFile="~/POSA1.Master" AutoEventWireup="true" CodeBehind="QuestionDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.CRM.QuestionDetail" %>

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



        //popup show 
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }
        //begin Popup Postback
        function OnClosePopupEventHandler(command) {
            switch (command) {
                case 'OK-Line':
                    popFile.Hide();
                    btnShowListPost.DoClick();
                    break;
                case 'Cancel-Line':
                    popFile.Hide();
                    break;
                case 'OK-List':
                    btnShowListPost.DoClick();
                    break;
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

        //begin Popup Postback
        function OnClosePopupAlert() {
            popAlert.Hide();
        }
        //end Pop Postback

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

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
                                                <asp:Label ID="lblHeaderMsg" CssClass="kanit" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" CssClass="kanit" runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm kanit" runat="server" Text="OK" OnClientClick="return OnClosePopupAlert();" />
                                                <asp:Button ID="btnCancel" CssClass="btn btn-warning btn-sm  kanit" runat="server" Text="CANCEL" OnClick="btnCancel_Click" />
                                            </div>

                                        </div>
                                    </div>
                                    <div>&nbsp</div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ContentStyle>
                    <Paddings PaddingBottom="5px" />
                </ContentStyle>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
       <%-- <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSaveStockOrder" />
        </Triggers>--%>
    </asp:UpdatePanel>



    <asp:UpdateProgress ID="udppPost" runat="server" AssociatedUpdatePanelID="udpPost">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <div class="row pb-2">
        <div class="col-md-2">
            <asp:LinkButton ID="btnBack" runat="server" Width="100%" CssClass="btn btn-info" OnClick="btnBack_Click">
           <i class="fas fa-chevron-square-left"></i>
           <span class="kanit">แบบสอบถาม </span>
            </asp:LinkButton>
        </div>
        <div class="col-md-6">
            <asp:UpdatePanel ID="udpPost" runat="server">
                <ContentTemplate>

                    <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                        <asp:Button ID="btnSave"
                            CssClass="btn btn-default kanit"
                            OnClick="btnSave_Click"
                            OnClientClick="this.disabled='true';"
                            UseSubmitBehavior="false"
                            runat="server"
                            Text="Save"></asp:Button>


                        <div class="btn-group" role="group">
                            <button id="btnGroupDrop1" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span class="kanit">More</span>
                            </button>
                            <div class="dropdown-menu" aria-labelledby="btnGroupDrop1" runat="server">
                                <asp:LinkButton ID="btnExcel" CssClass="btn btn-default" runat="server">
                                    <i class="fas fa-file-excel"></i>&nbsp<span class="kanit">Excel</span>
                                </asp:LinkButton>&nbsp
                            </div>
                        </div>
                        <div class="btn-group" role="group">
                            <button id="btnGroupDrop2" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span class="kanit">Delete</span>
                            </button>
                            <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-default" OnClick="btnDel_Click">
                                    <i class="fas fa-trash"></i>&nbsp<span class="kanit">Delete</span> 
                                </asp:LinkButton>
                            </div>
                        </div>

                        <asp:Label ID="lblInfoSave" runat="server" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-md-4 text-right">
            <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                <div class="btn-group" role="group">
                    <button id="btnGroupmore" type="button" class="btn btn-dark dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        Task
                    </button>
                    <div class="dropdown-menu" aria-labelledby="btnGroupmore">
                        <asp:LinkButton ID="btnNewLine" runat="server" class="dropdown-item" OnClick="btnNewLine_Click"><i class="far fa-file-plus"></i>&nbsp New</asp:LinkButton>
                        <asp:LinkButton ID="btnCopy" runat="server" class="dropdown-item" OnClick="btnCopy_Click"><i class="far fa-layer-plus"></i> Copy </asp:LinkButton>
                    </div>
                </div>
                <dx:ASPxComboBox ID="cboDocLine" runat="server" CssClass="form-control form-control"
                    Theme="Mulberry"
                    EnableCallbackMode="true"
                    AutoPostBack="true"
                    CallbackPageSize="10"
                    DropDownWidth="600" DropDownHeight="300"
                    OnSelectedIndexChanged="cboDocLine_SelectedIndexChanged"
                    ValueType="System.String" ValueField="QID"
                    FilterMinLength="1"
                    OnItemsRequestedByFilterCondition="cboDocLine_OnItemsRequestedByFilterCondition_SQL"
                    OnItemRequestedByValue="cboDocLine_OnItemRequestedByValue_SQL" TextFormatString="{0}"
                    DropDownStyle="DropDownList">
                    <Columns>
                        <dx:ListBoxColumn FieldName="QID" Caption="QID" />
                    </Columns>
                    <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                </dx:ASPxComboBox>

                <asp:LinkButton ID="btnBackward" runat="server" CssClass="btn btn-secondary" OnClick="btnBackward_Click"> <i class="fas fa-step-backward"></i></asp:LinkButton>
                <asp:LinkButton ID="btnforward" runat="server" CssClass="btn btn-secondary" OnClick="btnforward_Click"> <i class="fas fa-step-forward"></i></asp:LinkButton>

            </div>
        </div>
    </div>


    <div class="card">
        <div class="card-header pt-1 pb-1">
            <div class="row kanit">

                <div class="col-md-6">
                    <ul class="nav nav-pills card-header-pills" role="tablist">
                        <li class="nav-item"></li>
                        <li class="nav-item ">
                            <a class="nav-link active " id="a_tab_home" data-toggle="tab" href="#tab_home" role="tab" aria-controls="c_tab_home" aria-selected="true"><span class="kanit">General</span></a>
                        </li>                        
                    </ul>
                </div>
                <div class="col-md-6 text-right ">
                    
                </div>

            </div>
        </div>
        <div class="card-body pt-1 pb-1">
            <div class="row kanit">
                <div class="col-md-12">
                    <div class="tab-content tab-content-solid">
                        <%--begin tab general--%>
                        <div class="tab-pane fade show active" id="tab_home" role="tabpanel" aria-labelledby="c_tab_home">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12 pl-1 pr-0">
                                            <asp:UpdatePanel ID="udppoinfo" runat="server">
                                                <ContentTemplate>
                                                    <div class="card">
                                                        <div class="card-header pt-2 pb-1">
                                                            <div class="row">
                                                                <div class="col-md-10"><i class="fas fa-list"></i>&nbsp<span class="kanit">Question info</span></div>
                                                            </div>
                                                        </div>
                                                        <div class="card-body pt-1 pb-2">
                                                            <div class="row kanit">                                                            
                                                                <div class="col-md-3">
                                                                    <span>QID</span>
                                                                    <asp:TextBox ID="txtQID" Enabled="false" CssClass="form-control form-control-sm" runat="server" placeholder="++NEW++"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span>ชนิดแบบสอบถาม &nbsp</span>
                                                                    <asp:DropDownList ID="cboQType" runat="server" CssClass="form-control form-control-sm kanit" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span>กลุ่มแบบสอบถาม &nbsp</span>
                                                                    <asp:DropDownList ID="cboQGroup" runat="server" CssClass="form-control form-control-sm kanit" DataTextField="Description1" DataValueField="ValueTXT"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span>ชนิดตัวเลือกแบบสอบถาม &nbsp</span>
                                                                    <asp:DropDownList runat="server" ID="cboChoiceType" CssClass="form-control form-control-sm" DropDownStyle="DropDownList">
                                                                        <asp:ListItem Text="FREE TEXT" Value="F"></asp:ListItem>
                                                                        <asp:ListItem Text="SELECT" Value="S"></asp:ListItem>
                                                                        <asp:ListItem Text="MULTI SELECT" Value="MS"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="row kanit">
                                                                <div class="col-md-6">
                                                                    <span>รายละเอียด</span>
                                                                    <asp:TextBox ID="txtQDescription" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span>​​​วันที่เริ่ม</span>
                                                                    <dx:ASPxDateEdit ID="dtDateBegin" runat="server" Theme="iOS" Width="100%"
                                                                        DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                        <TimeSectionProperties Visible="False">
                                                                            <TimeEditCellStyle HorizontalAlign="Right">
                                                                            </TimeEditCellStyle>
                                                                        </TimeSectionProperties>
                                                                    </dx:ASPxDateEdit>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <span>​​​วันที่ถึง</span>
                                                                    <dx:ASPxDateEdit ID="dtDateEnd" runat="server" Theme="iOS" Width="100%"
                                                                        DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                                        <TimeSectionProperties Visible="False">
                                                                            <TimeEditCellStyle HorizontalAlign="Right">
                                                                            </TimeEditCellStyle>
                                                                        </TimeSectionProperties>
                                                                    </dx:ASPxDateEdit>
                                                                </div>
                                                            </div>


                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>

                                    </div>
                                    <div class="row pt-2">
                                        <div class="col-md-12 pl-1 pr-0">

                                            <div class="card">
                                                <div class="card-header pt-1 pb-1">
                                                    <div class="row">
                                                        <div class="col-md-6"><i class="fas fa-list"></i>&nbsp<span class="kanit">รายการตัวเลือก </span></div>
                                                        <div class="col-md-3 text-right">
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="udpalertaddline" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <div class="row text-right">
                                                                <div class="col-md-12">
                                                                    <asp:Label ID="lblInfoSave2" runat="server" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>

                                                </div>
                                                <asp:UpdateProgress ID="udppprline" runat="server" AssociatedUpdatePanelID="udpprline">
                                                    <ProgressTemplate>
                                                        <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                                                            <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:UpdatePanel ID="udpprline" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <div class="card-body pt-1 pb-2">
                                                            <div class="row">
                                                                <div class="col-md-3 kanit">
                                                                   <asp:Label runat="server" CssClass="kanit">Choice ID &nbsp</asp:Label>
                                                                   <asp:DropDownList runat="server" ID="cboChoiceID" CssClass="form-control form-control-sm" DropDownStyle="DropDownList">
                                                                       <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                                                       <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                                                       <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                                                       <asp:ListItem Text="D" Value="D"></asp:ListItem>
                                                                       <asp:ListItem Text="E" Value="E"></asp:ListItem>
                                                                       <asp:ListItem Text="F" Value="F"></asp:ListItem>
                                                                       <asp:ListItem Text="G" Value="G"></asp:ListItem>
                                                                       <asp:ListItem Text="H" Value="H"></asp:ListItem>
                                                                       <asp:ListItem Text="I" Value="I"></asp:ListItem>
                                                                       <asp:ListItem Text="J" Value="J"></asp:ListItem>
                                                                       <asp:ListItem Text="K" Value="K"></asp:ListItem>
                                                                       <asp:ListItem Text="L" Value="L"></asp:ListItem>
                                                                       <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                                                       <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                                                       <asp:ListItem Text="O" Value="O"></asp:ListItem>
                                                                       <asp:ListItem Text="P" Value="P"></asp:ListItem>
                                                                       <asp:ListItem Text="Q" Value="Q"></asp:ListItem>
                                                                       <asp:ListItem Text="R" Value="R"></asp:ListItem>
                                                                       <asp:ListItem Text="S" Value="S"></asp:ListItem>
                                                                       <asp:ListItem Text="T" Value="T"></asp:ListItem>
                                                                       <asp:ListItem Text="U" Value="U"></asp:ListItem>
                                                                       <asp:ListItem Text="V" Value="V"></asp:ListItem>
                                                                       <asp:ListItem Text="W" Value="W"></asp:ListItem>
                                                                       <asp:ListItem Text="X" Value="X"></asp:ListItem>
                                                                       <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                                                       <asp:ListItem Text="Z" Value="Z"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </div>                                                                
                                                                <div class="col-md-3 kanit">
                                                                    <asp:Label runat="server">รายละเอียด &nbsp</asp:Label>
                                                                    <asp:TextBox ID="txtChoiceDescription" CssClass="form-control form-control-sm kanit" runat="server"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-2 pt-3 text-left">
                                                                    <asp:LinkButton ID="btnSaveChoice" ForeColor="White" runat="server" CssClass="btn btn-success" OnClick="btnSaveChoice_Click">
                                                                        <i class="fa fa-save"></i>&nbsp<span class="kanit">เพิ่ม</span> 
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>


                                                            <div class="row pt-3">
                                                                <div class="col-md-12">
                                                                    <div style="overflow-x: auto; width: 100%">
                                                                        <asp:GridView ID="grdLine" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                            CssClass="table table-striped table-bordered table-hover"
                                                                            EmptyDataText="No data.."
                                                                            EmptyDataRowStyle-HorizontalAlign="Center"
                                                                            ShowHeaderWhenEmpty="true"
                                                                            DataKeyNames="ID"
                                                                            OnRowCommand="grdLine_RowCommand">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="KEY" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>                                                                              
                                                                                <asp:TemplateField HeaderText="Choice ID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblChoiceID" runat="server" Text='<%# Eval("ChoiceID") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle CssClass="kanit" Wrap="false" />
                                                                                    <ItemStyle CssClass="kanit" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="รายละเอียด">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblChoiceDescription" runat="server" Text='<%# Eval("ChoiceDescription") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <HeaderStyle CssClass="kanit" Wrap="false" />
                                                                                    <ItemStyle CssClass="kanit" Wrap="false" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                        </div>
                                                    </ContentTemplate>                                              
                                                    <Triggers><asp:AsyncPostBackTrigger ControlID="grdLine" /></Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="row pt-3">
                                        <div class="col-md-12">
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

                        </div>
                        <%--end tab general--%>
                       
                        <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>