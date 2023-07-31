<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Pur.Master" AutoEventWireup="true" CodeBehind="DashBoard1011.aspx.cs" Inherits="Robot.DashBoard.DashBoard1011" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-10 kanit">
                                <i class="fas fa-user-crown"></i>&nbsp
                                <asp:Label ID="lblTopic" runat="server" Text="DashBoard1011"> </asp:Label>
                            </div>
                            <div class="col-md-2" style="text-align: right">
                                <asp:LinkButton ID="btnNextBoard" runat="server" Font-Bold="true" OnClick="btnNextBoard_click" ForeColor="Black">Next</asp:LinkButton>
                                <i class="fa fa-forward" aria-hidden="true"></i>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                            <%--   <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("6101")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />                                                                                                                                                      
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnCreateOrder_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                                <i class="fa fa-id-card fa-9x" aria-hidden="true"></i>
                                            </span>
                                        </asp:LinkButton><br/><br />
                                        <h4 class="card-title">เปิดออเดอร์</h4>                                        
                                    </div>
                                </div>
                            </div>--%>
                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1101")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPRList_Click">
                                            <span style="font-size: 1em; color: goldenrod;">
                                           <i class="fas fa-tags fa-5x"></i>                                        
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1101 PR</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1103")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOList_Click">
                                            <span style="font-size: 1em; color: goldenrod;">
                                             <i class="fas fa-shopping-cart fa-5x"></i>                                          
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1103 PO</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1201")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnDeliveryHead_Click">
                                            <span style="font-size: 1em; color: cornflowerblue;">
                                               <i class="fas fa-ship  fa-5x" ></i>                                           
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1201 GR List</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1111")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnDeliveryTracking_Click">
                                            <span style="font-size: 1em; color: crimson;">
                                             <i class="fas fa-map-marker-alt  fa-5x"  ></i>                                             
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1111 Packaging Tracking</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1211")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOTracking_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             <i class="fas fa-map-marker-alt  fa-5x" ></i>                                             
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1211 PO Tracking</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("1251")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnLicense_Click">
                                            <span style="font-size: 1em; color: green;">
                                               <i class="fas fa-file-certificate  fa-5x" ></i>                                              
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">1251 License</h6>
                                        <p></p>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <br />


    </div>


</asp:Content>
