<%@ Page Title="MenuDevTest " Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Menu6666.aspx.cs" Inherits="Robot.DashBoard.Menu6666" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="content-wrapper">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <div class="row">
                            <div class="col-md-10 kanit">
                               <i class="fas fa-user-crown"></i>&nbsp <asp:Label ID="lblTopic" runat="server" Text="หน้าเทสงาน"> </asp:Label>
                            </div>
                        
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="row text-center">
                            <div class="col-md-4 col-sm-12 col-12 pb-3" >
                               <div class="list-group">  
                                      <asp:LinkButton runat="server" OnClick="btnPOList_Click" CssClass="list-group-item list-group-item-action">PO List</asp:LinkButton>
                                     <asp:LinkButton runat="server" OnClick="btnPODeltail_Click" CssClass="list-group-item list-group-item-action">PO Detail</asp:LinkButton>
                                    <asp:LinkButton runat="server" OnClick="btnDlvHead_Click" CssClass="list-group-item list-group-item-action">DlvHeadList</asp:LinkButton>
                                    <asp:LinkButton runat="server" OnClick="btnDlvLine_Click" CssClass="list-group-item list-group-item-action">DlvLineList</asp:LinkButton>
                                    <asp:LinkButton runat="server" OnClick="btnDlvDetail_Click" CssClass="list-group-item list-group-item-action">DlvDetail</asp:LinkButton>
                                   <asp:LinkButton runat="server" OnClick="btnDlvLicense_Click" CssClass="list-group-item list-group-item-action">DlvLicenselist</asp:LinkButton>
                                </div>
                            </div>
                                        

                        </div>

                    </div>
                </div>
            </div>
        </div>

 

    </div>


</asp:Content>
