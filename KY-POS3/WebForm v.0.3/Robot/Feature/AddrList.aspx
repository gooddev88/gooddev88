<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site2.Master" MaintainScrollPositionOnPostback="true" CodeBehind="AddrList.aspx.cs" Inherits="Robot.Feature.AddrList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddmenu" runat="server" />
    <asp:HiddenField ID="hddPreviouspage" runat="server" />
 
            <div class="row">
                     <div class="col-md-12">
                             
                                    <div class="input-group ">
                                        <asp:TextBox runat="server" ID="txtSearch" Class="form-control  jinput col-md-10 kanit" placeholder="ค้นหา" />
                                        <div class="input-group-append bg-primary border-primary">
                                            <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-info" OnClick="btnSearch_Click">
                                            <i class="fa fa-search"></i>&nbsp<span class="kanit">Load</span> 
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

              
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <div style="overflow-x: auto; width: auto">
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="True"
                            ShowHeaderWhenEmpty="true"
                            OnPageIndexChanging="grd_PageIndexChanging"
                            OnRowCommand="grd_RowCommand"                            
                            EmptyDataText="Input text for search"
                            PageSize="10"
                            OnRowDataBound="grd_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelect" runat="server" CausesValidation="False" CommandName="Select" Text="เลือก" class="btn btn-info" />
                                    </ItemTemplate>
                                 <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:TemplateField>
                                <asp:BoundField DataField="PostCode" HeaderText="รหัสไปรษณีย์" HeaderStyle-CssClass="kanit">
                                     <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:BoundField>
                                <asp:BoundField DataField="ProvinceID" HeaderText="จังหวัด" HeaderStyle-CssClass="kanit">
                         <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:BoundField>
                                <asp:BoundField DataField="AmphoeID" HeaderText="อำเภท/เขต" HeaderStyle-CssClass="kanit">
                        <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:BoundField>
                                <asp:BoundField DataField="DistrictID" HeaderText="ตำบล/แขวง" HeaderStyle-CssClass="kanit">
                         <HeaderStyle Wrap="False" CssClass="kanit" />
                                    <ItemStyle   Wrap="False" CssClass="kanit"  />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
    
        <div class="row" hidden="hidden">
        <div class="col-md-12">
            <dx:ASPxButton ID="btnOk" runat="server" AutoPostBack="false" Text="OK" OnClick="btnOk_Click"></dx:ASPxButton>
             
                        <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="false" Text="Cancel"
                        OnClick="btnCancel_Click">
                    </dx:ASPxButton>
        </div>
    </div>


</asp:Content>

