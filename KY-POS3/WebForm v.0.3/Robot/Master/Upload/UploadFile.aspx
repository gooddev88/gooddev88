<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Master/SiteB.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="UploadFile.aspx.cs" Inherits="Robot.Master.Upload.UploadFile" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:HiddenField ID="hddPreviouspage" runat="server" />

    <script type="text/javascript">
  
        function onFileUploadComplete(s, e) {
            if (e.callbackData) {
                var fileData = e.callbackData.split('|');
                var fileName = fileData[0],
                    fileUrl = fileData[1],
                    fileSize = fileData[2];
                DXUploadedFilesContainer.AddFile(fileName, fileUrl, fileSize);
            }
        }
        var myurl = document.getElementById('<%= hddPreviouspage.ClientID%>').value;
        function GoToFinishPage(s, e) {
            window.location = myurl;

        }
    </script>


    <div class="row kanit pb-2">
        <div class="col-lg-8 col-md-9 col-sm-12 col-12 mx-auto">
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header py-1">
                            <div class="row">
                                <div class="col-lg-5 col-md-5 col-sm-5 col-5 text-left">
                                    <asp:LinkButton ID="btnBack"
                                        OnClick="btnBack_Click"
                                        CssClass="btn btn-default" runat="server">                                          
                            <span> <i class="fas fa-reply-all fa-lg"></i> Back</span> 
                                    </asp:LinkButton>
                                </div>
                                <div class="col-lg-7 col-md-7 col-sm-7 col-7 pt-2 text-right">
                                    <asp:Label runat="server">อัพโหลด</asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
  
    <div class="row" runat="server" id="divUpload">
        <div class="col-lg-8 col-md-9 col-sm-12 col-12 mx-auto">
            <div class="card">
                <div class="card-header kanit">
                    <span class="kanit">เลือกไฟล์อัพโหลด</span>
                </div>
                <div class="card-body">
                    <div class="row kanit">
                        <div class="col-md-12">
                            <div class="uploadContainer ">
                                <dx:ASPxUploadControl
                                    ID="UploadControl" runat="server"
                                    ClientInstanceName="UploadControl" Width="100%"
                                    NullText="Select Image files..."
                                    UploadMode="Advanced"
                                    ShowUploadButton="True"
                                    ShowProgressPanel="True"
                                    OnFileUploadComplete="UploadControl_FileUploadComplete"
                                    Theme="Metropolis"
                                    AddUploadButtonsHorizontalPosition="Right">
                                    <ClientSideEvents FileUploadComplete="GoToFinishPage" />
                                    <AdvancedModeSettings EnableMultiSelect="false" EnableFileList="True" EnableDragAndDrop="false" />
                              <%--      <ValidationSettings   MaxFileSize="4194304" AllowedFileExtensions=".jpg, .jpeg, .gif, .png">
                                    </ValidationSettings>--%>

                                    <AddButton>
                                        <Image IconID="actions_add_16x16">
                                        </Image>
                                    </AddButton>
                                    <BrowseButton>
                                        <Image IconID="actions_open_16x16">
                                        </Image>
                                    </BrowseButton>
                                    <RemoveButton>
                                        <Image IconID="actions_cancel_16x16">
                                        </Image>
                                    </RemoveButton>
                                    <UploadButton>
                                        <Image IconID="navigation_up_16x16">
                                        </Image>
                                    </UploadButton>

                                    <TextBoxStyle Font-Size="Large">
                                        <DisabledStyle Border-BorderStyle="Solid">
                                        </DisabledStyle>
                                    </TextBoxStyle>

                                    <TextBoxStyle Font-Size="Large">
                                        <DisabledStyle Border-BorderStyle="Solid">
                                        </DisabledStyle>
                                    </TextBoxStyle>

                                </dx:ASPxUploadControl>
                                <br />
                                <br />
                                <p class="note kanit">
                                    <dx:ASPxLabel ID="AllowedFileExtensionsLabel" runat="server" Text="Allowed file extensions: .jpg, .jpeg, .gif, .png" Font-Size="8pt">
                                    </dx:ASPxLabel>
                                    <br />
                                    <dx:ASPxLabel ID="MaxFileSizeLabel" runat="server" Text="Maximum file size: 40 MB." Font-Size="8pt">
                                    </dx:ASPxLabel>
                                </p>
                            </div>
                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>
    <div class="row kanit text-center" runat="server" id="divShowUploadCompleted">
        <div class="col-lg-8 col-md-9 col-sm-12 col-12 mx-auto">
            <div class="card bg-light">
                <div class="card-body">
                    <h5 class="card-title">Upload completed</h5>
                    <p class="card-text">Upload เสร็จสมบูรณ์</p>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
