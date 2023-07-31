<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Fin.Master" AutoEventWireup="true" CodeBehind="DashBoard1005.aspx.cs" Inherits="Robot.DashBoard.DashBoard1005" %>

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

                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3301")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStatatmentImport_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             
                                                <i class="far fa-file-csv" style="font-size: 4rem !important"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">Import STM</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3302")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStmFinMemo_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             
                                                <i class="fas fa-envelope-open-dollar" style="font-size: 4rem !important"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">STM Fin. Memo</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3303")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStmFinSupMemo_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             
                                              <i class="fas fa-envelope-open-dollar" style="font-size: 4rem !important"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">STM Fin sup. Memo</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3304")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStmAccMemo_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             
                                              <i class="far fa-hand-holding-usd" style="font-size: 4rem !important"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">STM Acc. Memo</h4>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3305")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStmAccSupMemo_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             
                                              <i class="far fa-hand-holding-usd" style="font-size: 4rem !important"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">STM Acc sup. Memo</h4>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-4 col-sm-12 col-12 pb-3"  <%= (SetMenu("3310")=="0")?"style='display: none;'":"" %> >
                                <div class="card">
                                    <br />
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStmAll_Click">
                                            <span style="font-size: 1em; color: dimgray;">
                                             <i class="far fa-sack-dollar" style="font-size: 4rem !important"></i> 
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h4 class="card-title">STM History</h4>
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
