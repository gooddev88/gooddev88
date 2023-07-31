<%@ Page Title="ลงทะเบียนใช้งาน" Language="C#" MasterPageFile="~/Communication/SiteB.Master" AutoEventWireup="true" CodeBehind="LineMenuAgent.aspx.cs" ClientIDMode="Static" Inherits="Robot.Communication.Line.LineMenuAgent" %>

<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <asp:HiddenField ID="hddid" runat="server" />
    <asp:HiddenField ID="hddphoto" runat="server" />


    <div class="row pt-4">
        <div class="col-12 mx-auto">
            <div class="row text-center">
                <div class="col-12">
                    <img src="../../Image/Logo/kylogo.png" style="width: 150px" />
                    <br />
                    <asp:Label ID="lblUser" runat="server" Font-Size="X-Large"> </asp:Label>
                </div>
            </div>

            <div class="row pt-2">
                <div class="col-11 text-center mx-auto">
                    <i>
                        <asp:Literal ID="lblLineSay" runat="server"></asp:Literal></i>                   
                </div>
            </div>

            <div class="row pt-2" runat="server" visible="false">
                <div class="col-11 mx-auto ">
                    <asp:Image ID="Image1" ImageUrl="hdd" runat="server" />
                    <i>
                        <asp:Literal ID="lblphoto" runat="server"></asp:Literal></i>
                </div>
            </div>

            <div class="row pt-3">
                <div class="col-11 mx-auto">
                    <div class="row pt-1 pb-1 text-left">
                        <div class="col-md-12">
                            <asp:LinkButton runat="server" CssClass="text-decoration-none" Font-Size="Larger" ForeColor="Black" ID="btnReport"
                                OnClick="btnReport_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                            <img src="../Image/statistic_chart.png" style="width:100px" /> <br />
                                        </div>
                                </div> 
                                  <p style="margin-bottom: 0px; font-size:smaller; color: gray;">รายงานยอดขาย</p>
                            </div>
                        </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                     <%--    <div class="row pt-1 pb-1 text-left">
                        <div class="col-md-12">
                            <asp:LinkButton runat="server" CssClass="text-decoration-none"
                                Font-Size="Larger" ForeColor="Black" ID="btnOpenWeb"
                                OnClick="btnOpenWeb_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                            <img src="../Image/statistic_chart.png" style="width:100px" /> <br />
                                        </div>
                                </div>
                              
                                  <p style="margin-bottom: 0px; font-size:smaller; color: gray;">เปิดเว็บ</p>
                            </div>
                        </div>
                            </asp:LinkButton>
                        </div>
                    </div>
                        <div class="row pt-1 pb-1 text-left">
                        <div class="col-md-12">
                            <asp:LinkButton runat="server" CssClass="text-decoration-none"
                                Font-Size="Larger" ForeColor="Black" ID="btnDownLoadApp"
                                OnClick="btnDownLoadApp_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                            <img src="../Image/statistic_chart.png" style="width:100px" /> <br />
                                        </div>
                                </div>
                              
                                  <p style="margin-bottom: 0px; font-size:smaller; color: gray;">ดาวน์โหลดแอป</p>
                            </div>
                        </div>
                            </asp:LinkButton>
                        </div>
                    </div>--%>
                  

<%--                    <div class="row pt-1 pb-1 text-left">
                        <div class="col-md-12">
                            <asp:LinkButton runat="server" CssClass="text-decoration-none" Font-Size="Larger" ForeColor="Black" ID="btnActivityReq"
                                OnClick="btnActivityReq_Click">
                        <div class="card">
                            <div class="card-body py-3" style="padding-left: 2rem !important;">
                                <div class="row pb-2">
                                    <div class="col-md-12">
                            <img src="../../HRTimeAttendance/Image/Unemployed-icon.png" style="width:100px" /> <br />
                                    </div>
                                </div>
                                แจ้งกิจกรรม
                                  <p style="margin-bottom: 0px; font-size:smaller; color: gray;">แจ้งกิจกรรมที่พนักงานได้ทำ</p>
                            </div>
                        </div>
                            </asp:LinkButton>
                        </div>
                    </div>--%>

                </div>
            </div>
            <div class="row pt-2">
                <div class="col-6 pt-1">
                    <asp:Label ID="lblAlertmsg" runat="server">  </asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="content_footer" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>
