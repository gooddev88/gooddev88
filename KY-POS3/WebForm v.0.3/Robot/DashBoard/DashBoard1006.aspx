<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard1006.aspx.cs" Inherits="Robot.DashBoard.DashBoard1006" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">

            <div class="col-md-12">
                <div class="card">
                    <div class="card-header" >
                        <div class="row">
                            <div class="col-md-10 kanit">
                                <asp:Label ID="lblTopic" runat="server" Text=""> <i class="fa fa-codepen" aria-hidden="true"></i></asp:Label>
                            </div>
                            <div class="col-md-2" style="text-align: right">
                                <asp:LinkButton ID="btnNextBoard" runat="server" Font-Bold="true" OnClick="btnNextBoard_click" ForeColor="white">Next</asp:LinkButton>
                                <i class="fa fa-forward" aria-hidden="true"></i>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">

                                    <img class="card-img-left" style="width: 150px; height: 150px" src="../../Image/Menu/g007.png" alt="Card image">
                                    <div class="card-body kanit">
                                        <h4 class="card-title">รับพัสดุ DP/WH</h4>
                                        <p class="card-text">
                                            <asp:Label ID="lblD1" runat="server" Text="20 DP/WH"></asp:Label>
                                        </p>
                                      
                                        <asp:LinkButton ID="btnDropPoint" runat="server" CssClass="btn btn-warning" OnClick="btnDropPoint_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">เปิด</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">

                                    <img class="card-img-left" style="width: 150px" src="../../Image/Menu/g006.png" alt="Card image">
                                    <div class="card-body kanit">
                                        <h4 class="card-title">ส่งพัสดุ ณ จุดกระจายสินค้า</h4>
                                        <p class="card-text">
                                            <asp:Label ID="lblD2" runat="server" Text="250 รายการ"></asp:Label>
                                        </p>
                                        <asp:LinkButton ID="btnWarehouse" runat="server" CssClass="btn btn-warning" OnClick="btnWarehouse_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">เปิด</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">

                                    <img class="card-img-left" style="width: 150px" src="../../Image/Menu/g005.png" alt="Card image">
                                    <div class="card-body kanit">
                                        <h4 class="card-title">ส่งพัสดุปลายทาง</h4>
                                        <p class="card-text">
                                            <asp:Label ID="lblD3" runat="server" Text="150 ปลายทาง"></asp:Label>
                                        </p>
                                        <asp:LinkButton ID="btnDestination" runat="server" CssClass="btn btn-warning" OnClick="btnDestination_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">เปิด</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <br />
                </div>




            </div>
        </div>

        <br />


    </div>


</asp:Content>
