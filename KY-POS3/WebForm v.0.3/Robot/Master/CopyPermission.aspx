﻿<%@ Page Title="Copy menu" Language="C#" MasterPageFile="~/MainE.Master" AutoEventWireup="true" CodeBehind="CopyPermission.aspx.cs" ClientIDMode="Static" Inherits="Robot.Master.CopyPermission" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <asp:HiddenField ID="hddType" runat="server" />



    <script type="text/javascript">
        //begin combobox bind data
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

        //end combobox bind data




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

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="udpmain" runat="server">
        <ContentTemplate>
            <div class="row  pt-5" runat="server" id="divmain">
                <div class="col-lg-6 col-md-8 col-sm-12 col-12 mx-auto">
                    <div class="card">
                        <div class="card-header bg-info">
                            <div class="row kanit">
                                <div class="col">
                                    <span>
                                        <asp:Label ID="lblInfo" Font-Size="Large" ForeColor="Black" runat="server" Text=""></asp:Label>
                                    </span>
                                </div>
                            </div>

                        </div>
                        <div class="card-body bg-light">
                            <div class="row pb-3  kanit " runat="server" id="divroleUser">
                                <div class="col-md-12 kanit">
                                    <asp:ObjectDataSource ID="roleUserdatasource" runat="server" SelectMethod="LoadRoleUser" TypeName="Robot.Master.CopyPermission">
                                           <SelectParameters>
                                            <asp:SessionParameter Name="fromGroup" SessionField="fromroleid" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <div class="row">
                                        <div class="col-md-12">
                                          
                                                 <span style="color: dimgray">คัดลอกสิทธิ์จากผู้ใช้งาน
                                            </span>
                                            </span>
                                            <dx:ASPxComboBox ID="cboRoleUserSource"
                                                DataSourceID="roleUserdatasource" runat="server"
                                                CssClass="form-control kanit" DropDownStyle="DropDownList"
                                                Theme="Default"
                                                EnableCallbackMode="True"
                                                CallbackPageSize="30"
                                                ValueField="Username" ValueType="System.String" ViewStateMode="Enabled"
                                                TextFormatString="{0}-{1}" Width="100%">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="Username" Width="130px" Caption="Username" />
                                                    <dx:ListBoxColumn FieldName="FullName" Width="600px" Caption="Name" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row pb-3  kanit " runat="server" id="divRoleGroup">
                                <div class="col-md-12 kanit">
                                    <asp:ObjectDataSource ID="roleGroupdatasource" runat="server" SelectMethod="LoadRoleGroup" TypeName="Robot.Master.CopyPermission" OldValuesParameterFormatString="original_{0}">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="fromGroup" SessionField="fromroleid" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <span style="color: dimgray">คัดลอกสิทธิ์จากกลุ่มผู้ใช้งาน
                                            </span>
                                            <dx:ASPxComboBox ID="cboRoleGroupSource"
                                                DataSourceID="roleGroupdatasource" runat="server"
                                                CssClass="form-control kanit" DropDownStyle="DropDownList"
                                                Theme="Default"
                                                EnableCallbackMode="True"
                                                CallbackPageSize="30"
                                                ValueField="UserGroupID" ValueType="System.String" ViewStateMode="Enabled"
                                                TextFormatString="{0}{1}" Width="100%">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="UserGroupID" Width="130px" Caption="Group ID" />
                                                    <dx:ListBoxColumn FieldName="GroupName" Width="600px" Caption="Name" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row pt-2 kanit pt-3">
                                <div class="col-md-12  kanit">
                                    <div class="row">
                                        <div class="col-md-4 mx-auto pt-1 text-center">
                                            <div class="btn-group" role="group" aria-label="Basic example">
                                                <asp:LinkButton ID="btnBack" Width="120" runat="server"
                                                    CssClass="btn btn-dark" OnClick="btnBack_Click">
                                                    <span style="color:crimson"> <i class="fas fa-arrow-circle-left fa-2x"></i></span>Back
                                                </asp:LinkButton>
                                                <asp:Button ID="btnok"
                                                    CssClass="btn btn-success kanit"
                                                    OnClick="btnOk_Click"
                                                    OnClientClick="this.disabled='true';"
                                                    UseSubmitBehavior="false"
                                                    runat="server"
                                                    Text="Save"></asp:Button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                                <hr />
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
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>