<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard1007.aspx.cs" Inherits="Robot.DashBoard.DashBoard1007" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
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
                        <div class="row text-center"> 
                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                                   <br />
                                    <span style="font-size: 1em; color: dimgray;">
                                        <i class="far fa-gifts  fa-9x"></i>
                                    </span> 

                                    <div class="card-body kanit">
                                        <h4 class="card-title">เปิดออเดอร์ (DP)</h4>
                                        <p class="card-text" style="display:none;"> 
                                        <i class="fas fa-link"></i>
                                                 <asp:LinkButton ID="btnListOrder" Font-Underline="false" ForeColor="Crimson" runat="server" Text="20 ออเดอร์ วันนี้"  OnClick="btnListOrder_Click">                                
                                                     </asp:LinkButton>
                                        </p>
                                        <asp:LinkButton ID="btnNewOrder" runat="server" CssClass="btn btn-warning" OnClick="btnNewOrder_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">สร้างออเดอร์</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                               <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                                   <br />
                                       <span style="font-size: 1em; color: dimgray;">
                                    <i class="fas fa-shipping-fast fa-9x"></i>
                                    </span> 
                                    <div class="card-body kanit">
                                        <h4 class="card-title">ทำใบคุมสาขา (WH)</h4>
                                        <p class="card-text" style="display:none;">
                                             <i class="fas fa-link"></i>
                                                 <asp:LinkButton ID="btnListPacking" Font-Underline="false" ForeColor="Crimson" runat="server" Text="20 ออเดอร์ วันนี้"  OnClick="btnListPacking_Click">                                
                                                     </asp:LinkButton>
                                        </p>
                                        <asp:LinkButton ID="btnNewPacking" runat="server" CssClass="btn btn-warning" OnClick="btnNewPacking_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">สร้างใบคุม</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>


                     
                        </div>
                        <div class="row text-center">
                                    <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                                   <br />
                                     <span style="font-size: 1em; color: gray;"> 
                                         <i class="far fa-scroll fa-9x"></i>
                                    </span> 
                                    <div class="card-body kanit">
                                        <h4 class="card-title">รายงานค่าคอมมิชชั่น (DP)</h4>
                                     
                                        <asp:LinkButton ID="btnShowComm" runat="server" CssClass="btn btn-warning" OnClick="btnShowComm_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">เปิด</span> 
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                                <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                                   <br />
                                     <span style="font-size: 1em; color: gray;"> 
                                      <i class="far fa-map-marker-alt fa-9x"></i>
                                    </span> 
                                    <div class="card-body kanit">
                                        <h4 class="card-title">ติดตามสถานะพัสดุ</h4>
                                     
                                        <asp:LinkButton ID="btnTracking" runat="server" CssClass="btn btn-warning" OnClick="btnTracking_Click">
                                        <i class="fa fa-folder-open"></i>&nbsp<span class="kanit">เปิด</span> 
                                        </asp:LinkButton>
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
