<%@ Page Title="เลือกสาขา" Language="C#" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSSaleDeletePermance.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSSaleDeletePermance" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddrefid" runat="server" />
    <asp:HiddenField ID="hddcaption" runat="server" />
    <asp:HiddenField ID="hdddoctype" runat="server" />


    <script type="text/javascript">
        function SendCommentCallback(s, e) {
            CallbackPanel.PerformCallback();
        };

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
    <div class="row pt-5 pb-5" runat="server" id="divmain">
        <div class="col-lg-5 col-md-8 col-sm-12 col-12 mx-auto">
            <div class="card">
                <div class="card-header">
                    <div class="row  pt-3 pr-4">
                        <div class="col-md-12  ">
                            <span>สาขา </span>

                            <asp:DropDownList ID="cboCompany" runat="server" 
                                CssClass="form-control  border-danger" 
                                AutoPostBack="true"
                                OnSelectedIndexChanged="cboCompany_SelectedIndexChanged"
                                DataTextField="Name" 
                                DataValueField="CompanyID"></asp:DropDownList>
                        </div>
                    </div>
            
                <div class="row  pt-3 pr-4">
                    <div class="col-md-12  ">
                        <span>ขายให้ </span>
                        <asp:DropDownList ID="cboShipTo" runat="server"  
                            CssClass="form-control  border-danger" 
                            AutoPostBack="true"
                            OnSelectedIndexChanged="cboShipTo_SelectedIndexChanged"
                            DataTextField="ShipToName"
                            DataValueField="ShipToID"></asp:DropDownList>

                    </div>
                </div>
                <div class="row  pt-3 pr-4">
                    <div class="col-md-12  ">
                        <span>เครื่อง </span>
                        <asp:DropDownList ID="cboMac" runat="server" 
                            AutoPostBack="true"
                            OnSelectedIndexChanged  ="cboMac_SelectedIndexChanged"
                            CssClass="form-control  border-danger" 
                            DataTextField="MacName" DataValueField="MacID"></asp:DropDownList>

                    </div>
                </div>
                <div class="row pt-2 pr-4 ">
                    <div class="col-md-12 ">
                          <span>เลือกวันที่ </span> 
                        <dx:ASPxDateEdit ID="dtInvDate" 
                            runat="server" Theme="Material"
                            Width="100%"
                            AutoPostBack="true"
                            OnDateChanged="dtInvDate_DateChanged"
                            DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                            <TimeSectionProperties Visible="False">
                                <TimeEditCellStyle HorizontalAlign="Right">
                                </TimeEditCellStyle>
                            </TimeSectionProperties>
                        </dx:ASPxDateEdit>


                    </div>
                </div>

                   <div class="row pt-2 pb-2 pr-4 ">
                    <div class="col-md-12  text-right ">
                     

                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-secondary" OnClick="btnDelete_Click">
                                      <i class="fas fa-times-circle fa-2x" style="color:red"></i>&nbsp<span >ลบบิลไร้ร่องรอย</span> 
                                </asp:LinkButton>&nbsp   
                                 
                         
                    </div>

                </div>
                        </div>
                <div id="divalert" style="display: none" class="alert alert-success" role="alert">
                    <hr />
                    <strong>
                        <span id="myalertHead"></span></strong>
                    <br />
                    <span id="myalertBody"></span>
                    <asp:Label ID="lblAlertBody" runat="server" ForeColor="Red"></asp:Label>
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            </div>
        </div>
    </div>



    <div class="row">
        <div class="col-12">
            <div style="overflow-x: auto; width: 100%">

                <dx:ASPxGridView ID="grdDetail" runat="server"
                    EnableTheming="True"
                    Theme="MaterialCompact"
                    AutoGenerateColumns="False"
                    KeyFieldName="BillID"
                    OnDataBinding="grdDetail_DataBinding" KeyboardSupport="True">

                    <Settings ShowFilterRow="True" ShowFooter="True" ShowGroupFooter="VisibleAlways" ShowFilterBar="Visible" ShowHeaderFilterButton="True" />

                    <Columns>
                     
                        <dx:GridViewDataTextColumn Caption="เลขบิล" FieldName="INVID" Width="160px">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="สาขา" FieldName="ComID">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                          <dx:GridViewDataTextColumn Caption="เครื่อง" FieldName="MacNo">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ขายให้" FieldName="ShipToLocID">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="วันที่บิล" FieldName="BillDate">
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataTextColumn Caption="จำนวนรายการ" FieldName="Qty">
                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="เบอร์โต๊ะ" FieldName="TableName">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="จำนวนเงิน" FieldName="NetTotalAmt">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ภาษี" FieldName="NetTotalVatAmt">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="จำนวนเงิน(Inc.VAT)" FieldName="NetTotalAmtIncVat">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Disc Amt" FieldName="ItemDiscAmt">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="จำนวนเงินสด" FieldName="PayByCash">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="จำนวนเงินโอน" FieldName="PayByOther">
                            <PropertiesTextEdit DisplayFormatString="N2"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ชื่อลูกค้า" FieldName="CustomerName">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="สาขาลูกค้า" FieldName="CustBranchName">
                            <Settings AutoFilterCondition="Contains" />
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="เลขที่เสียภาษี" FieldName="CustTaxID">
                            <PropertiesTextEdit DisplayFormatString="N0"></PropertiesTextEdit>
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Created by" FieldName="CreatedBy">
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Created date" FieldName="CreatedDate">
                            <HeaderStyle Wrap="False" />
                            <CellStyle Wrap="False" />
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy HH:mm"></PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>

                    </Columns>
                </dx:ASPxGridView>
                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grdDetail"></dx:ASPxGridViewExporter>

            </div>





        </div>
    </div>

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
