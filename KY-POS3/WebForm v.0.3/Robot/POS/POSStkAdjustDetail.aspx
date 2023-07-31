<%@ Page Title="นับสต็อก" Language="C#" MasterPageFile="~/POS/SiteA.Master" AutoEventWireup="true" CodeBehind="POSStkAdjustDetail.aspx.cs" ClientIDMode="Static" Inherits="Robot.POS.POSStkAdjustDetail" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddmenu" runat="server" />
    <style>

        .btn-main {
            display: inline-block;
            font-weight: 400;
            color: #212529;
            text-align: center;
            vertical-align: middle;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-color: transparent;
            border: 1px solid transparent;
            padding: .375rem .75rem;
            font-size: 1rem;
            line-height: 1.5;
            border-radius: .25rem;
            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }
        
        .btn-10ac84 {
            color: #fff;
            background-color: #10ac84;
            border-color: #10ac84;
        }
        
    </style>

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

        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";
        }

        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";
        }

        function mypopprofile() {
            btnPostProfile.DoClick();
        }
        //end show msg

        //popup show 
        function onPopupShown(s, e) {
            var windowInnerWidth = window.innerWidth;
            if (s.GetWidth() > windowInnerWidth) {
                s.SetWidth(windowInnerWidth - 4);
                s.UpdatePosition();
            }
        }

        function OnClosePopupAlert() {
            popAlert.Hide();
        }

        function OnClosePopupDeleteAlert() {
            popDeleteAlert.Hide();
        }
    </script>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="udpAlert" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popAlert" runat="server" Width="500" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
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
                                                <asp:Label ID="lblHeaderMsg" CssClass="" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsg" CssClass="" runat="server" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:Button ID="btnOK" CssClass="btn btn-success btn-sm " runat="server" Text="ตกลง" OnClientClick="return OnClosePopupAlert();" />
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

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="popDeleteAlert" runat="server" Width="600" CloseAction="OuterMouseClick" CloseOnEscape="true" Modal="True"
                Theme="Mulberry"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popDeleteAlert"
                HeaderText="Infomation" AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AutoUpdatePosition="true">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="PanelDelete" runat="server" DefaultButton="btOK">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div class="card Sarabun">
                                        <div class="card-body">
                                            <h5 class="card-title"><strong>
                                                <asp:Label ID="lblHeaderMsgDelete" CssClass="Sarabun" runat="server" Text=""></asp:Label></strong> </h5>
                                            <hr />
                                            <p class="card-text">
                                                <asp:Label ID="lblBodyMsgDelete" CssClass="Sarabun" runat="server" Font-Size="Small" Text=""></asp:Label>
                                            </p>
                                            <div style="text-align: right">
                                                <asp:LinkButton ID="btnConfirmDel" runat="server" CssClass="btn btn-success" OnClick="btnConfirmDel_Click"> 
                                                    <i class="fas fa-check-circle fa-lg"></i> ตกลง
                                                 </asp:LinkButton>
                                                <asp:LinkButton ID="btnCancle" runat="server" CssClass="btn btn-danger" Text="" OnClientClick="return OnClosePopupDeleteAlert();"> 
                                                    <i class="fas fa-times-circle fa-lg"></i> ยกเลิก
                                                 </asp:LinkButton>
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
            <asp:AsyncPostBackTrigger ControlID="btnDel" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="udppmain" runat="server" AssociatedUpdatePanelID="udpmain">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.8;">
                <span style="border-width: 0px; position: fixed; padding: 50px; font-size: 40px; left: 40%; top: 40%;">Working ...</span>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="udpmain" runat="server">
        <ContentTemplate>
            <div class="row ">
                <div class="col-xl-10 col-lg-11 col-md-11 mx-auto">
                    <div class="row pt-1 pb-2">
                        <div class="col-md-6">
                          <asp:LinkButton ID="btnBackList" Font-Size="Small"
                                CssClass="btn btn-default" runat="server"
                                OnClick="btnBackList_Click">                                                                          
                              <h4 style="font-weight: bold; color: black;">
                                 <span><i class="fas fa-reply fa-2x"></i></span>  &nbsp  
                                  <asp:label runat="server" ID="lblDocinfo"></asp:label>
                              </h4>
                            </asp:LinkButton>
                            
                        </div>
                        <div class="col-md-6 text-right pt-3">                                                          
                           <asp:Button ID="btnSave" Width="25%" Height="40"
                                CssClass="btn-main btn-10ac84 "
                                OnClick="btnSave_Click"
                                OnClientClick="this.disabled='true';"
                                UseSubmitBehavior="false"
                                runat="server"
                                Text="บันทึก"></asp:Button>
                            <asp:LinkButton ID="btnCloseDoc" runat="server" CssClass="btn btn-dark" Height="40" OnClick="btnCloseDoc_Click" Text=""> 
                               <%--<i class="fas fa-repeat-alt"></i>--%> ปิดเอกสาร
                             </asp:LinkButton> 
                             <asp:LinkButton ID="btnDel" runat="server" CssClass="btn btn-danger" Height="40" OnClick="btnDel_Click" Text=""> 
                                <i  class="far fa-trash-alt fa-lg"></i> ลบเอกสาร
                             </asp:LinkButton>  
                        </div>
                    </div>

                    <div class="row pb-1" style="font-size: smaller;">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row pb-1 pt-2">
                                        <div class="col-md-8">
                                            <asp:Label runat="server" Font-Size="X-Large" ID="lblAdjID"></asp:Label>&nbsp&nbsp<br />
                                            <asp:Label runat="server" ForeColor="Gray" Font-Size="Large" ID="lblStockDate"></asp:Label><br />
                                            <asp:Label runat="server" ForeColor="Gray" Font-Size="Medium" ID="lblCompany"></asp:Label>
                                        </div>
                                        <div class="col-md-4 text-right">
                                            <h3><asp:Literal ID="litStatus" runat="server"></asp:Literal></h3>
                                        </div>
                                    </div>

                                    <div class="row pt-1">
                                        <div class="col-md-3">
                                            <span>วันที่ปรับสต็อก</span>
                                            <dx:ASPxDateEdit ID="dtAdjDate" DisplayFormatString="dd-MM-yyyy"
                                                EditFormatString="dd-MM-yyyy" runat="server" Theme="Material" Width="100%">
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>

                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtRemark1" Font-Size="Small" CssClass="form-control" Placeholder="หมายเหตุ" TextMode="MultiLine" Height="70px" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row pt-4">
                        <div class="col-6">
                            <h4 style="font-weight: bold; color: black;">รายการสินค้า</h4>
                        </div>
 <div class="col-6 text-right">
     <div class="btn-group" role="group" aria-label="Basic example">
     <asp:LinkButton ID="btnLoadStockBalRMK" runat="server" CssClass="btn btn-warning" Height="40" 
                                OnClick="btnLoadStockBalRMK_Click" Text=""> 
                                <i class="fas fa-cloud-download-alt fa-lg"></i> โหลด RMK คงเหลือ
                            </asp:LinkButton>
     <asp:LinkButton ID="btnLoadStockBalFGK" runat="server" CssClass="btn btn-warning" Height="40" 
                                OnClick="btnLoadStockBalFGK_Click" Text=""> 
                                <i class="fas fa-cloud-download-alt fa-lg"></i> โหลด FGK คงเหลือ
                            </asp:LinkButton>
</div>
 
     </div>
                    </div>

                    <div class="row pt-1">
                        <div class="col-md-12">
                            <div class="card">
                                <div class="card-body">
                                    <div class="row ">
                                        <div class="col-md-4">
                                            <span style="color: gray;">สินค้า </span>
                                            <dx:ASPxComboBox ID="cboItem" runat="server"
                                                CssClass="" DropDownStyle="DropDownList"
                                                Theme="Material"
                                                ValueField="ItemID" ValueType="System.String"
                                                ViewStateMode="Enabled"
                                                TextFormatString="{0}-{1}" Width="100%">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="ItemID" Caption="สินค้า" />
                                                    <dx:ListBoxColumn FieldName="Name1" Caption="รายละเอียด" Width="350px" />
                                                        <dx:ListBoxColumn FieldName="TypeID" Caption="ประเภท" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </div>
                                                                          <div class="col-md-2">
                            <asp:Label runat="server">ที่เก็บ </asp:Label>
                            <asp:DropDownList ID="cboLocation" runat="server" CssClass="form-control  form-control-sm" DataTextField="LocCode" DataValueField="LocID"></asp:DropDownList>
                        </div>
                                        <div class="col-md-2">
                                            <span style="color: gray;">จำนวน </span>
                                            <asp:TextBox ID="txtQty" CssClass="form-control form-control-sm  text-right" onkeypress="return DigitOnly(this,event)" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 pt-3 text-left">
                                            <asp:LinkButton ID="btnAddLine" Width="100%" runat="server" ForeColor="Black" CssClass="" OnClick="btnAddLine_Click">
                                                 <i class="fas fa-plus-circle fa-2x"></i>&nbsp<span class="">เพิ่ม</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="row pt-5">
                                        <div class="col-md-12">
                                            <asp:UpdatePanel ID="udpgrd"
                                                UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <asp:ListView ID="grdline"
                                                        KeyFieldName="LineNum"
                                                        OnPagePropertiesChanging="grdline_PagePropertiesChanging"
                                                        OnItemDataBound="grdlist_ItemDataBound"
                                                        OnItemCommand="grdline_ItemCommand"
                                                        pagesize="500"
                                                        runat="server">
                                                        <LayoutTemplate>
                                                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                            <div class="row" runat="server" hidden="hidden">
                                                                <div class="col-md-12">
                                                                    <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdline" PageSize="1000">
                                                                        <Fields>
                                                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                                            <asp:NumericPagerField NumericButtonCssClass="accordion"></asp:NumericPagerField>
                                                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </div>
                                                            </div>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <div class="col-md-12" style="display: none">
                                                                <asp:Label ID="lblLineNum" runat="server" Text='<%# Eval("LineNum") %>'></asp:Label>
                                                            </div>
                                                            <div class="row pb-1">
                                                                <div class="col-md-12">
                                                                    <div class="row ">
                                                                        <div class="col-md-12">
                                                                            <div class="row">
                                                                                <div class="col-lg-8 col-md-8 col-sm-8 col-8 pt-2 text-left">
                                                                                    <span style="color: black; "><%# Eval("ItemID")%> &nbsp <%# Eval("Name")%>  
                                                                                        &nbsp ตั้งต้น
                                                                                        <strong><%# Convert.ToDecimal(Eval("BeginQty")).ToString("N2") %> </strong> 
                                                                                            &nbsp ปรับปรุง
                                                                                        <strong><%# Convert.ToDecimal(Eval("AdjQty")).ToString("N2") %> </strong> 
                                                                                        &nbsp <%# Eval("Unit")%> 
                                                                                    </span>
                                                                                    <br />
                                                                                </div>
                                                                                <div class="col-lg-4 col-md-4 col-sm-4 col-4 text-right">
                                                                                     <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("LineNum") %>'
                                                                                        CausesValidation="False" CommandName="Del" Text="ลบ"> 
                                                                                        <i style="color:gray;" class="far fa-trash-alt fa-lg"></i>
                                                                                    </asp:LinkButton> 
                                                                                </div>
                                                                            </div>
                                                                               <div class="row pb-2 ">
                                                                                   <div class="col-2 pt-2 pr-0 pt-2">
                                                                                    <span> <%#   Eval("LocID") %></span> 
                                                                                   </div>
                                                                          
                                                                                   <div class="col-10 text-right">
                                                                                       <span>จำนวนหลังปรับปรุง</span>
                                                                                       <asp:TextBox ID="txtQty"
                                                                                           Font-Size="Large"
                                                                                           Font-Bold="true"
                                                                                           ForeColor="DeepPink"
                                                                                           class="form-control-f form-control-sm" runat="server" Placeholder="จำนวน"
                                                                                           Text='<%#  Convert.ToDecimal( Eval("ActualQty")).ToString("N2")%>'
                                                                                           Style="text-align: right" onkeypress="return DigitOnly(this,event)"></asp:TextBox>
                                                                                   </div>
                                                                               </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <hr />
                                                        </ItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <div class="row py-3 ">
                                                                <div class="col-md-12 text-center">
                                                                    <asp:Label runat="server" Font-Size="" ForeColor="deeppink">... ยังไม่มีรายการ ...</asp:Label>
                                                                </div>
                                                            </div>

                                                        </EmptyDataTemplate>
                                                    </asp:ListView>

                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="grdline" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
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
        </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="grdline" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:SqlDataSource ID="sqlSearch" runat="server" ConnectionString="<%$ ConnectionStrings:GAConnectionString %>"></asp:SqlDataSource>
</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
