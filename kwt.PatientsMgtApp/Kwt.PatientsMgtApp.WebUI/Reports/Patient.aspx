<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Patient.aspx.cs" Inherits="Kwt.PatientsMgtApp.WebUI.Reports.Patient" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/kendo.common.min.css" rel="stylesheet" />
    <link href="../Content/kendo.metro.min.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel-group">
            <div id="panelbar" class="panel">
                <div class="panel-success">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#collapse1" style="display: block;">
                            <i class="fa fa-search"></i>&nbsp;<span class="p-2" style="text-decoration: underline"> Filter Patient Report</span> <i class="fa fa-angle-down"></i>
                        </a>
                    </h4>

                </div>
                <div id="collapse1" class="panel-collapse collapse">
                    <div class="panel-body" >
                        <ul class="fieldlistReport row" style="margin: 0">
                            <li class="editor-field col-md-3">

                                <label class="" for="StartDate">Created Date</label>

                                <asp:TextBox ID="StartDate" runat="server"></asp:TextBox>
                            </li>
                            <li class="editor-field col-md-3">

                                <label class="" for="EndDate">Created Date</label>

                                <asp:TextBox ID="EndDate" runat="server"></asp:TextBox>
                            </li>

                            <li class="editor-field col-md-3">

                                <label class="" for="IsActiveList">Active</label>
                                <%--<asp:CheckBox ID="Status" runat="server" Class="k-checkbox" />--%>
                                <asp:DropDownList ID="IsActiveList" runat="server" Width="100%" ></asp:DropDownList>
                            </li>
                            <%-- new isdead --%>
                            <li class="editor-field col-md-3">

                                <label class="" for="IsDeadList">Is Dead?</label>
                                <%--<asp:CheckBox ID="Status" runat="server" Class="k-checkbox" />--%>
                                <asp:DropDownList ID="IsDeadList" runat="server" Width="100%" ></asp:DropDownList>
                            </li>
                            <%--  --%>
                            <li class="editor-field col-md-3">

                                <label class="" for="PatientCid">Patient Cid</label>
                                <asp:TextBox ID="PatientCid" runat="server" CssClass="k-textbox" Width="100%"></asp:TextBox>
                            </li>
                            <li class="editor-field col-md-3">

                                <label class="" for="DoctorList">Doctor</label>
                                <asp:DropDownList ID="DoctorList" runat="server" Width="100%" ></asp:DropDownList>
                            </li>
                            <li class="editor-field col-md-3">

                                <label class="" for="HospitalList">Hospital</label>
                                <asp:DropDownList ID="HospitalList" runat="server" Width="100%"></asp:DropDownList>
                            </li>

                            <li class="editor-field col-md-3">

                                <label class="" for="SpecialityList">Speciality</label>
                                <asp:DropDownList ID="SpecialityList" runat="server" Width="100%"></asp:DropDownList>
                            </li>
                            <li class="editor-field col-md-6">
                                <label class="">&nbsp;</label>
                               
                                <asp:LinkButton ID="LinkSearchButton" runat="server" Class="k-button" OnClick="Search_Click"><i class="fa fa-search" style="color:yellowgreen"></i> Search</asp:LinkButton>
                                <%--<asp:Button ID="Search" runat="server" Text="Search" Class="k-button" OnClick="Search_Click" />--%>
                                <asp:LinkButton ID="LinkClearButton" runat="server" Class="k-button" OnClick="Clear_Click"><i class="fa fa-ban" style="color:lightblue"></i> Clear</asp:LinkButton>
                                <%--<asp:Button ID="All" runat="server" Text="Clear" Class="k-button" OnClick="Clear_Click" />--%>
                            </li>
                        </ul>
                    </div>

                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="ErrorMessage" runat="server" Visible="False">
           <div class="col-md-12" >
            <asp:Label ID="Message" runat="server"  Class="alert alert-info" Style="display: block;"></asp:Label>
        </div> 
        </asp:PlaceHolder>
        
        <div>
            
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false"
                PageCountMode="Actual"
                ShowPageNavigationControls="True"
                ShowPrintButton="True"
                Font="sen-serif"
                Width="100%"
                SizeToReportContent="true"
                ConsumeContainerWhitespace="true">
            </rsweb:ReportViewer>
        </div>
    </form>
    <script src="../Scripts/jquery-3.3.1.min.js"></script>
    <script src="../Scripts/jquery-ui-1.12.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/kendo.all.min.js"></script>
    <script src="../Scripts/jquery-migrate-3.0.0.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#PatientCid").kendoMaskedTextBox({
                mask: "AAAAAAAAAAAA"
            });
            $("#StartDate, #EndDate").kendoDatePicker();
            //$("#DoctorList, #HospitalList, #SpecialityList").kendoDropDownList({

            //});
        });
    </script>
</body>
</html>
