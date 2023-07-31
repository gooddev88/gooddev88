<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Menu1001.aspx.cs" Inherits="Robot.DashBoard.Menu1001" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-10 kanit">
                               <i class="fas fa-user-crown"></i>&nbsp <asp:Label ID="lblTopic" runat="server" Text="รายงานรวม"> </asp:Label>
                            </div>
                        
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                            <div class="col-md-4 col-sm-12 col-12 pb-3" >
                               <div class="list-group">
                                    <button type="button" class="list-group-item list-group-item-action active">ออเดอร์ในสาขา</button>
                                    <asp:LinkButton runat="server" OnClick="btnTruck_postcode_Click" CssClass="list-group-item list-group-item-action">รถในรหัสไปรณีย์</asp:LinkButton>
                                    <asp:LinkButton runat="server" OnClick="btn_UserIn_OrderPerDay" CssClass="list-group-item list-group-item-action">จำนวนออเดอร์ที่เปิดของแต่ละคน</asp:LinkButton>
                                    <asp:LinkButton runat="server" OnClick="btn_CountOrder_InBrn" CssClass="list-group-item list-group-item-action" >ออเดอร์ที่ค้างในสาขา</asp:LinkButton>
                                </div>
                            </div>
                                        

                        </div>

                    </div>
                </div>
            </div>
        </div>

 

    </div>


</asp:Content>
