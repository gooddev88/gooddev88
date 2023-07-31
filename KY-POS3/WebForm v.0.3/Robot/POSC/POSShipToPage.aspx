﻿<%@ Page Title="Ship to" Language="C#" MasterPageFile="~/POSC/SiteA.Master" AutoEventWireup="true" CodeBehind="POSShipToPage.aspx.cs" ClientIDMode="Static" Inherits="Robot.POSC.POSShipToPage" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">   
 

        <div class="row">
                <div class="col-8 mx-auto">
                    <asp:ListView ID="grdlist"
                        KeyFieldName="ShipToID"
                        OnPagePropertiesChanging="grdlist_PagePropertiesChanging"
                        OnItemCommand="grdlist_ItemCommand"
                        runat="server">
                <%--        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceholder" 
                                runat="server" />
                            <div class="row text-center">
                                <div class="col-12">
                                  
                                    <asp:DataPager ID="grdlistPager1" runat="server" PagedControlID="grdlist" PageSize="30">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                            <asp:NumericPagerField NumericButtonCssClass="accordion"></asp:NumericPagerField>
                                            <asp:NextPreviousPagerField ButtonType="Button" ButtonCssClass="btn-dark" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </div>
                        </LayoutTemplate>--%>
                        <ItemTemplate>
                            <div class="row pt-2 ">
                                <div class="col-12">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="row">
                                                <div class="col-12">
                                                    <asp:LinkButton runat="server" CommandName="sel" CommandArgument='<%# Eval("ShipToID")%>' class="btn p-0" ID="btnselect" Width="100%">
                                                        <div class="row">  
                                                              <div class="col-2">
                                                                  <img src='<%#Eval("ImageUrl") %>' alt="" style="width: 90px;" />
                                                                  </div>
                                                                       <div class="col-10">
                                                                              
                                                                           <span  style="color: cadetblue; font-size: 30px;"><strong><%# Eval("ShipToName")%>  </strong>    </span>
                                                                   
                                                                 </div>
                                                            </div>
                                                   

                                                    </asp:LinkButton>
                                                </div>
                                            </div> 

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <div class="row ">
                                <div class="col-12">
                                    ไม่พบข้อมูล...
                                </div>
                            </div>
                        </EmptyDataTemplate>
                    </asp:ListView> 
                </div>
            </div>
         

</asp:Content>
<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>