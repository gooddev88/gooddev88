<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/POS/SiteB.Master" AutoEventWireup="true" CodeBehind="MenuLineData.aspx.cs" Inherits="Robot.DashBoard.MenuLineData" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-8 mx-auto">
                <div class="row">
                    <div class="col-md-12 text-center">
                        <asp:LinkButton runat="server" ID="btnBack" Width="100%"
                            OnClick="btnBack_Click">
                            <img src="MenuLine/Image/persondata_menu.png" style="width: 100px" />
                            <br />
                            <asp:Label Font-Size="XX-Large" ID="lblTopic" runat="server"> </asp:Label>
                        </asp:LinkButton>
                    </div>
                </div>



                <div class="row pt-1 pb-1 text-left" <%= (SetMenu("7181")=="0")?"style='display: none;'":"" %>>
                    <div class="col-md-12">
                        <asp:LinkButton runat="server" CssClass="text-decoration-none" Font-Size="Larger" ForeColor="Black" ID="btnLineLoginReq"
                            OnClick="btnLineLoginReq_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                                   <img src="MenuLine/Image/menu_lineoa.png" style="width:100px" /> <br />
 
                                    </div>
                                </div>
                                คำขอใช้บริการ Line OA
                                <%--คำขอใช้บริการ Line Official--%>                      
                                <span style="font-size: large"><%=ShowMainMenuDesc("7181") %> </span>
                                <%--<p style="margin-bottom: 0px; font-size:smaller; color: gray;">7181 สำหรับตรวจสอบผู้ยืนคำขอใช้บริการ KY POS ผ่าน Line Official</p>--%>
                            </div>
                        </div>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="row pt-1 pb-1 text-left" <%= (SetMenu("7181")=="0")?"style='display: none;'":"" %>>
                    <div class="col-md-12">
                        <asp:LinkButton runat="server" CssClass="text-decoration-none" Font-Size="Larger" ForeColor="Black" ID="btnLineLogin"
                            OnClick="btnLineLogin_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                            <img src="MenuLine/Image/person_in_line.png" style="width:100px" /> <br />
                                    </div>
                                </div>
                                รายชื่อผู้ใช้บริการ Line OA
                                <%--รายชื่อผู้ใช้บริการ Line Offcial--%>
                                    <span style="font-size: large"><%=ShowMainMenuDesc("7181") %> </span>
                                  <%--<p style="margin-bottom: 0px; font-size:smaller; color: gray;">7181 สำหรับตรวจสอบและยกเลิก พนักงานที่กำลังใช้บริการ KY POS ผ่าน Line Official</p>--%>
                            </div>
                        </div>
                        </asp:LinkButton>
                    </div>
                </div>                


            </div>
        </div>
    </div>


</asp:Content>
