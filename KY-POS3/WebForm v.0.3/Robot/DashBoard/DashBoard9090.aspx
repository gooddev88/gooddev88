<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/POSA1.Master" AutoEventWireup="true" CodeBehind="DashBoard9090.aspx.cs" Inherits="Robot.DashBoard.DashBoard9090" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-10 kanit">
                                <i class="fas fa-user-crown"></i>&nbsp
                                <asp:Label ID="lblTopic" runat="server" Text="DashBoard9090"> </asp:Label>
                            </div>
                            <div class="col-md-2" style="text-align: right">
                                <asp:LinkButton ID="btnNextBoard" runat="server" Font-Bold="true" OnClick="btnNextBoard_click" ForeColor="Black">Next</asp:LinkButton>
                                <i class="fa fa-forward" aria-hidden="true"></i>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                        

                          
                            
                            <div class="col-md-3 col-sm-12 col-12 pb-3" <%= (SetMenu("9211")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMailGroup_Click">
                                            <span style="font-size: 1em; color:gray">
                                                <i class="fas fa-mailbox fa-4x"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">9211 Mail Group</h6>
                                        <p>Back office</p>
                                    </div>
                                </div>
                            </div>  

                            <div class="col-md-3 col-sm-12 col-12 pb-3" <%= (SetMenu("9202")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="card-body kanit">
                                        <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnSetMailGroupInDocstep_Click">
                                            <span style="font-size: 1em; color:gray"> 
                                                <i class="fas fa-mailbox fa-4x"></i>
                                            </span>
                                        </asp:LinkButton><br />
                                        <br />
                                        <h6 class="card-title">9202 Mail Group In DocStep</h6>
                                        <p>Back office</p>
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
