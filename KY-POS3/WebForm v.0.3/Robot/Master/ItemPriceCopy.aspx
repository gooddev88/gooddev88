﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master/SiteA.Master" AutoEventWireup="true" CodeBehind="ItemPriceCopy.aspx.cs" Inherits="Robot.Master.ItemPriceCopy" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <asp:HiddenField ID="hddmenu" runat="server" />
    <script>
        //begin show msg
        function ShowAlert(message, messagetype) {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "block";

            var cssclass;
            switch (messagetype) {
                case 'Success':
                    cssclass = 'alert alert-success alert-dismissible fade show'
                    break;
                case 'Error':
                    cssclass = 'alert alert-danger alert-dismissible fade show'
                    break;
                case 'Warning':
                    cssclass = 'alert alert-warning alert-dismissible fade show'
                    break;
                default:
                    cssclass = 'alert alert-info alert-dismissible fade show'
            }
            document.getElementById("divalert").setAttribute("class", cssclass);
            document.getElementById("myalertHead").textContent = messagetype;
            document.getElementById("myalertBody").textContent = message;
            $('.alert').alert()
        }
        function CloseAlert() {
            var divalert = document.getElementById("divalert");
            divalert.style.display = "none";

        }
        //end show msg
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
       <div class="col-md-6 mx-auto">
    

    <div class="row pt-5 text-center">
        <div class="col-md-12">
            <h2>
                <span>ก๊อปปี้ราคาสินค้าจากสาขาอื่น</span>
            </h2>
        </div>
    </div>
    <div class="row pt-4">
        <div class="col-md-12">
            <span>ก็อปปี้จากสาขา </span>
            <asp:DropDownList ID="cboCompanyFr" runat="server"
                CssClass="form-control form-control-sm border-primary "
                DataTextField="Name2" DataValueField="CompanyID">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row pt-4">
        <div class="col-md-12">
            <span>ก็อปปี้ไปที่สาขา </span>
            <asp:DropDownList ID="cboCompanyTo" runat="server"
                CssClass="form-control form-control-sm border-primary "
                DataTextField="Name2" DataValueField="CompanyID">
            </asp:DropDownList>
        </div>
    </div>
     
                    <div class="row pt-2  pb-3">
                        <div class="col-md-12 pl-4 pt-1 ">
                            <div class="row">                          
                                <div class="col-md-12 pt-1 text-right">                                      
                                    <asp:LinkButton ID="btnOK" runat="server" CssClass="btn btn-success" OnClick="btnOK_Click">
                                      <span >ตกลง</span> 
                                    </asp:LinkButton>&nbsp   
                                       <asp:LinkButton ID="btnBack" runat="server" Text="" CssClass="btn btn-danger" OnClick="btnBack_Click">
                                   
                                        <span >ยกเลิก </span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>                
                    </div>
                  </div>
        </div>
    <div id="divalert" style="display: none" class="alert alert-success" role="alert">
        <hr />
        <strong>
            <span id="myalertHead"></span></strong>
        <br />
        <span id="myalertBody"></span>
        <asp:Label ID="lblAlertBody" runat="server" ForeColor="Red"></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FooterScript" runat="server">
</asp:Content>