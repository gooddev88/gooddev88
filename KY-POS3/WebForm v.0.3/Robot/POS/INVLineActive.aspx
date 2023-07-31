<%@ Page Title="Invoice Line" Language="C#" EnableEventValidation="false" MasterPageFile="~/POS/SiteB.Master" AutoEventWireup="true" CodeBehind="INVLineActive.aspx.cs" Inherits="Robot.POS.INVLineActive" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="hddTopic" runat="server" />

    <%--begin script for loadding panel --%>
    <script>  
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

    <script>
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



    <div class="row ">
        <div class="col-md-12">
            <div class="card">
                <div class="card-header text-black bg-info">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="col-md-6">
                                <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-default" OnClick="btnSave_Click">
                                  <i class="fas fa-save"></i>&nbsp<span >OK</span> 
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnClose" runat="server" href="#" OnClientClick="return window.parent.OnClosePopupEventHandler('OK-Line');" CssClass="btn btn-default">
                                           <i class="fas fa-sign-out-alt"></i>&nbsp<span >Close</span> 
                                    </asp:LinkButton>
                                    <div class="btn-group" role="group">
                                        <button id="btnGroupDrop2" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <span >Delete</span>
                                        </button>
                                        <div class="dropdown-menu" aria-labelledby="btnGroupDrop1">
                                            <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-default" OnClick="btnDel_Click">
                                          <i class="fas fa-trash"></i>&nbsp<span >Delete</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblInfoSave" runat="server" Font-Bold="true" Font-Size="Larger" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 text-right">
                            <div class="btn-group" role="group" aria-label="Button group">
                                <asp:LinkButton ID="btnNewLine" runat="server" CssClass="btn btn-secondary" OnClick="btnNewLine_Click">    <i class="fas fa-plus-circle"></i></asp:LinkButton>
                                <asp:DropDownList ID="cboDocLine" runat="server" Width="500px" CssClass="form-control" AutoPostBack="true" DataTextField="Description" DataValueField="LineNum" OnSelectedIndexChanged="cboDocLine_SelectedIndexChanged"></asp:DropDownList>
                                <asp:LinkButton ID="btnBackward" runat="server" CssClass="btn btn-secondary" OnClick="btnBackward_Click"> <i class="fas fa-step-backward"></i></asp:LinkButton>
                                <asp:LinkButton ID="btnforward" runat="server" CssClass="btn btn-secondary" OnClick="btnforward_Click"> <i class="fas fa-step-forward"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card-body">
                    <asp:UpdatePanel ID="udpinfo1" runat="server">
                        <ContentTemplate>
                            <div class="row pt-1">
                                <div class="col-md-12 ">
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
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="card ">
                                        <div class="card-header">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <span><i class="fas fa-cube fa-2x"></i>
                                                </div>
                                                <div class="col-md-6 text-right">
                                                    <asp:Label ID="lbldocid" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-12 ">
                                                    <span style="font-size: small">Item No. </span>
                                                    <dx:ASPxComboBox ID="cboItem" runat="server"
                                                        Theme="Material"
                                                        CssClass="Sarabun"
                                                        EnableCallbackMode="true"
                                                        CallbackPageSize="10"
                                                        AutoPostBack="true"
                                                        DropDownWidth="600"
                                                        DropDownHeight="300"
                                                        OnSelectedIndexChanged="cboItem_SelectedIndexChanged"
                                                        ValueType="System.String" ValueField="ItemID"
                                                        OnItemsRequestedByFilterCondition="cboItem_OnItemsRequestedByFilterCondition_SQL"
                                                        OnItemRequestedByValue="cboItem_OnItemRequestedByValue_SQL" TextField="ItemID" TextFormatString="{0} ( {1} )"
                                                        Width="100%" DropDownStyle="DropDownList">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="ItemID" Caption="Item No." />
                                                            <dx:ListBoxColumn FieldName="Name1" Caption="Description" Width="400px" />
                                                            <dx:ListBoxColumn FieldName="TypeID" Caption="Type" Width="70px" />
                                                            <dx:ListBoxColumn FieldName="UnitID" Caption="หน่วย" Width="70px" />
                                                        </Columns>
                                                        <ClientSideEvents BeginCallback="function(s, e) { OnBeginCallback(); }" EndCallback="function(s, e) { OnEndCallback(); } " />
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">Description</span>
                                                    <asp:TextBox ID="txtItemName"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row pb-0">
                                                <div class="col-md-6">
                                                    <span style="font-size: small">Qty</span>
                                                    <asp:TextBox ID="txtQtyInv"
                                                        OnTextChanged="txtQtyInv_TextChanged"
                                                        AutoPostBack="true"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server" Style="text-align: right"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <span style="font-size: small">Price</span>
                                                    <asp:TextBox ID="txtPrice"
                                                        OnTextChanged="txtPrice_TextChanged"
                                                        AutoPostBack="true"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server" Style="text-align: right"
                                                        onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row pb-0">
                                                <div class="col-md-6">
                                                    <span style="font-size: small">Total amt</span>
                                                    <asp:TextBox ID="txtBaseTotalAmt"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server"
                                                        Style="text-align: right"
                                                        onkeypress="return DigitOnly(this,event)"
                                                        OnTextChanged="txtBaseTotalAmt_TextChanged"
                                                        AutoPostBack="true"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <span style="font-size: small">Total After Disc.</span>
                                                    <asp:TextBox ID="txtTotalAmt" ReadOnly="true" onchange="setfourNumberDecimal(this)" CssClass="form-control form-control-sm" runat="server" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row pb-0">
                                                <div class="col-md-6">
                                                    <span style="font-size: small">VAT amt.</span>
                                                    <asp:TextBox ID="txtVatAmt"
                                                        ReadOnly="true"
                                                        CssClass="form-control form-control-sm"
                                                        runat="server"
                                                        Style="text-align: right"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <span style="font-size: small">Vat </span>
                                                    <dx:ASPxComboBox ID="cboVatType"
                                                        CssClass="Sarabun"
                                                        runat="server"
                                                        OnSelectedIndexChanged="cboVatType_SelectedIndexChanged"
                                                        AutoPostBack="true"
                                                        DropDownStyle="DropDownList"
                                                        Theme="Material"
                                                        ValueField="TaxID" ValueType="System.String" ViewStateMode="Enabled" TextFormatString="{0}" Width="100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="TaxID" Caption="Vat code" />
                                                            <dx:ListBoxColumn FieldName="TaxName" Width="300px" Caption="Description" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </div>
                                            </div>
                                            <div class="row pb-0">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">Total amt.(inc vat)</span>
                                                    <asp:TextBox ID="txtTotalAmtIncVat" onchange="setfourNumberDecimal(this)" ReadOnly="true" CssClass="form-control form-control-sm" runat="server" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row pb-0">
                                                <div class="col-md-12">
                                                    <span style="font-size: small">ชื่อเปิดบิล </span>
                                                    <asp:TextBox ID="txtRemark1" TextMode="MultiLine" Height="70" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
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
        </div>
    </div>
    <div class="row">
        <div class="col-md-12" runat="server" visible="false">
            <div class="card">
                <div class="card-header text-white bg-secondary">
                    <div class="row">
                        <div class="col-md-10">
                            <i class="fal fa-clipboard"></i>&nbsp 
                             <asp:Label ID="Label1" runat="server" Text="Remark"></asp:Label>
                        </div>
                        <div class="col-md-2 text-right">
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="udpinfo2" runat="server">
                        <ContentTemplate>

                            <div class="row">
                                <div class="col-md-12">
                                    <span style="font-size: small">Remark 2</span>
                                    <asp:TextBox ID="txtRemark2" TextMode="MultiLine" Height="60" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>
