<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/DashBoard/SiteA.Master" AutoEventWireup="true" CodeBehind="DashBoard5001.aspx.cs" Inherits="Robot.DashBoard.DashBoard5001" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card bg-light">
                    <div class="card-header">
                        <div class="row ">
                            <div class="col-6 ">
                                <i class="fas fa-user-crown"></i>&nbsp
                                <asp:Label ID="lblTopic" runat="server" Text="DashBoard5000"> </asp:Label>
                            </div>
                            <div class="col-6" style="text-align: right">
                                <asp:LinkButton ID="btnNextBoard" runat="server" Font-Bold="true" OnClick="btnNextBoard_click" ForeColor="Black">Next</asp:LinkButton>
                                <i class="fa fa-forward" aria-hidden="true"></i>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5001")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnPOS1" OnClick="btnPOSV1_Click">
                                            <%--<span style="font-size: 1em; color: limegreen;">                                                
                                       <i class="fas fa-store fa-5x"></i>
                                            </span>--%><img src="../Image/Logo/Sale-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5001") %> </span> 
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5001") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5002")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnPOSV2" OnClick="btnPOSV2_Click">
                                                <img src="../Image/Logo/Sale-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5002") %> </span>
                                           <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5002") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5100")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnCheckBillList_Click">
                                            <span style="font-size: 1em; color: cadetblue;">                                                 
                                       <i class="fas fa-cash-register fa-5x"></i>
                                            </span>
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5100") %> </span>
                                         <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5100") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5004")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnBilHistory" OnClick="btnBilHistory_Click">
                                             
                                                <i class="fas fa-history  fa-5x" style="color:burlywood"></i>
                                       
                                       
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5004") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5004") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5902")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 ">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnPOSReport" OnClick="btnPOSReport_Click">
                                            <%--<span style="font-size: 1em; color: cadetblue;">                                                 
                                       <i class="fas fa-chart-bar fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/รายงานขาย-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5902") %> </span>
                                             <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5902") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("2001")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSORD_Click">
                                            <%--<span style="font-size: 1em; color: red;">                                                 
                                       <i class="fas fa-shopping-cart fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/สั่งออเดอร์-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("2001") %> </span>
                                             <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("2001") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("2002")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSORD_ACCEP_Click">
                                            <%--<span style="font-size: 1em; color: #be4bdb;">                                                 
                                       <i class="fas fa-box-full fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/รับออเดอร์-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("2002") %> </span>
                                           <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("2002") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("2101")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSPO_Click">
                                            <%--<span style="font-size: 1em; color:  black;">                                                 
                                       <i class="fas fa-bread-slice fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/ซื้อสินค้า-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("2101") %> </span>
                                           <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("2101") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("2003")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSORDShip_Click">
                                           <%-- <span style="font-size: 1em; color: #fd7e14;">                                                 
                                       <i class="fas fa-shipping-fast fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/ส่งสินค้า-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("2003") %> </span>
                                             <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("2003") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("2004")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 ">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSORDGR_Click">
                                            <%--<span style="font-size: 1em; color: #82c91e;">                                                 
                                       <i class="fas fa-hand-receiving fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/รับสินค้า-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("2004") %> </span>
                                                 <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("2004") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("3001")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSBOM_Click">
                                            <span style="font-size: 1em; color: #f783ac;">                                                 
                                       <i class="fas fa-hamburger fa-5x"></i>
                                            </span>
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("3001") %> </span>
                                               <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("3001") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("4111")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnOInvoice_Click">
                                <span style="font-size: 1em; color: orangered;">                                                 
                                                
                                    <i class="fab fa-twitch fa-5x"></i>
                                </span>
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("4111") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("4111") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("4112")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnOReceipt_Click">
                                            <span style="font-size: 1em; color: orangered;">                                                 
                                                
                                          <i class="fab fa-blogger fa-5x"></i>
                                            </span>
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("4112") %> </span>
                                              <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("4112") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("6011")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 ">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnPOSStkAdjust_Click">
                                            <%--<span style="font-size: 1em; color: #4c6ef5;">                                                 
                                       <i class="fas fa-adjust fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/ปรับปรุงสินค้า-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("6011") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("6011") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("8001")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 ">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStockBalance_Click">
                                            <%--<span style="font-size: 1em; color: #82c91e;">                                                 
                                       <i class="fas fa-balance-scale-right fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/สินค้าคงเหลือ-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("8001") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("8001") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("8002")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 ">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnStockMovement_Click">
                                            <%--<span style="font-size: 1em; color: #be4bdb;">                                                 
                                       <i class="fas fa-balance-scale fa-5x"></i>
                                            </span>--%>
                                                <img src="../Image/Logo/รายการเคลื่อนไหว-icon.png" width="90" /> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("8002") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("8002") %> </p>
                                            
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5903")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnPOSORDERRM" OnClick="btnPOSORDERRM_Click">
                                            <span style="font-size: 1em; color: #ff922b;">                                                 
                                       <i class="fas fa-chart-bar fa-5x"></i>
                                            </span>
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5903") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5903") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnRawBT_Click">   
                                                <span style="font-size: 1em; color: hotpink;">                                                 
                                       <i class="fas fa-print-search fa-5x"></i>
                                            </span> 
                                            </asp:LinkButton><br />
                                            <span style="font-size: large">RAW BT3.9 </span>
                                            <p style="margin-bottom: 0rem;">&nbsp</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-12 col-12 pb-3">
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnAppDownLoad_Click">   
                                             
                                                    <i class="fab fa-app-store-ios fa-5x" style=" color: orangered;"></i>
                                         
                                            </asp:LinkButton><br />
                                            <span style="font-size: large">ดาวโหลดแอพขาย </span>
                                            <p style="margin-bottom: 0rem;">&nbsp</p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5028")=="0")?"style='display: none;'":"" %>>
                                <div class="card">
                                    <div class="row pt-2 pb-3">
                                        <div class="col-md-12">
                                            <asp:LinkButton runat="server" CssClass="btn btn" ID="btnDeletePermanace" OnClick="btnDeletePermanace_Click">
                                             <i class="fas fa-times-circle  fa-5x"  style="color:red"></i>
                                       
                                            </asp:LinkButton><br />
                                            <span style="font-size: large"><%=ShowMainMenuDesc("5028") %> </span>
                                            <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5028") %> </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>


                        <div <%= (SetMenu("9000")=="0")?"style='display: none;'":"" %>>

                            <div class="row ">
                                <div class="col-md-12">
                                    <h4><span style="color: gray"><%=ShowMainMenuDesc("9000") %>  </span></h4>
                                    <hr />
                                </div>
                            </div>

                            <div class="row text-center">

                                <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("5901")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnItemV2_Click">
                                            <span style="font-size: 1em; color: #ff922b;">                                                 
                                       <i class="fab fa-product-hunt fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("5901") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("5901") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("9412")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnItemPrice_Click">
                                            <span style="font-size: 1em; color: blueviolet;">  
                                                <i class="fas fa-tags fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9412") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9412") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12  pb-3" <%= (SetMenu("9201")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnCompany_Click">
                                            <span style="font-size: 1em; color: black;">                                                 
                                       <i class="fas fa-home-lg-alt fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9201") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9201") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9002")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMyUserInfo_Click">
                                            <span style="font-size: 1em; color: dodgerblue;">                                                 
                                                
                                                <i class="fas fa-user-md-chat fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9002") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9002") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9003")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMyUserGroupInfo_Click">
                                            <span style="font-size: 1em; color: mediumorchid;">                                                 
                                                <i class="fas fa-users-cog fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9003") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9003") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9017")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnVendor_Click">
                                            <span style="font-size: 1em; color: mediumpurple;">                                                 
                                                <i class="fas fa-venus-mars fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9017") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9017") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9911")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMasterTypeInfo_Click">
                                            <span style="font-size: 1em; color: black;">                                                 
                                                <i class="fas fa-cogs fa-5x"></i>  
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9911") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9911") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9999")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMenuLine_Click">
                                                    <i style="color:#02b002;" class="fab fa-line fa-5x"></i>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large"><%=ShowMainMenuDesc("9999") %> </span>
                                                <p style="margin-bottom: 0rem;font-size:smaller"><%=ShowMainMenuDesc2("9999") %> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4 col-sm-12 col-12 pb-3" <%= (SetMenu("9000")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnAPPV2_Click">
                                            <span style="font-size: 1em; color: red;"> 
                                                <i class="fas fa-rocket fa-5x"></i>
                                            </span>
                                                </asp:LinkButton><br />
                                                <span style="font-size: large">APP V2 </span>
                                                <p style="margin-bottom: 0rem;">&nbsp</p>
                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <%--                                <div class="col-md-2 col-sm-12 col-12 pb-3" <%= (SetMenu("9911")=="0")?"style='display: none;'":"" %>>
                                    <div class="card">
                                        <div class="row pt-2 ">
                                            <div class="col-md-12">
                                                <asp:LinkButton runat="server" CssClass="btn btn" OnClick="btnMasterTypeInfo_Click">
                                                            <i class="fas fa-cogs fa-4x" style="color:black;"></i>                                                           
                                                </asp:LinkButton><br />
                                                <span style="font-size: medium"><%=ShowMainMenuDesc("9911") %> </span>
                                                <p><span style="font-size: small"><%=ShowSubMenuDesc("9911") %></span> </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
</asp:Content>
