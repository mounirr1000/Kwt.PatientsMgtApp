<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Kwt.PatientsMgtApp.WebUI.Reports.Payment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <link href="../Content/kendo.common.min.css" rel="stylesheet" />
    <link href="../Content/kendo.metro.min.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />
    <link href="../Content/fontawesome/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <%--<div class="row">

            <div class="col-md-4">
                <label>Serch By:</label>
                <asp:Label ID="Label1" runat="server" Text="Start Date:"></asp:Label>
                <asp:TextBox ID="StartDate" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label3" runat="server" Text="End Date:"></asp:Label>
                <asp:TextBox ID="EndDate" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label4" runat="server" Text="Patient Cid:"></asp:Label>
                <asp:TextBox ID="PatientCid" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

        </div>
        <div class="row">
            <div class="col-md-4">
                <asp:Button ID="Search" runat="server" Text="Search" OnClick="Search_Click" />
            </div>
            <div class="col-md-4">
                <asp:Button ID="All" runat="server" Text="Clear" OnClick="Clear_Click" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="alert-success">
                    <asp:Label ID="ResultMessage" runat="server"></asp:Label>
                </div>
            </div>
        </div>--%>
        <%--<div class="row">
            <asp:ImageButton ID="btnPrint" AlternateText="Print Report" runat="server" OnClick="btnPrint_Click"  />
        </div>--%>
           <div class="panel-group">
            <div id="panelbar" class="panel">
                <div class="panel-success">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#collapse1"><span class="p-2"><i class="fa fa-search"></i> Filter Payment Report</span> <i class="fa fa-angle-down"></i></a>
                    </h4>

                </div>
                <div id="collapse1" class="panel-collapse collapse">
                    <div class="panel-body" >
                        <ul class="fieldlistReport" style="margin: 0">
                            <li class="editor-field">

                                <label class="" for="StartDate">Payment Start Date</label>
                               
                               <asp:TextBox ID="StartDate" runat="server" ></asp:TextBox>
                            </li>
                           <li class="editor-field">

                                <label class="" for="EndDate">Payment End Date</label>
                               
                              <asp:TextBox ID="EndDate" runat="server" ></asp:TextBox>
                            </li>
                             <li class="editor-field">

                                <label class="" for="PatientCid">Patient Cid</label>
                               
                               <asp:TextBox ID="PatientCid" runat="server" Class="k-textbox"></asp:TextBox>
                            </li>
                            <li class="editor-field">
                                <label class="">&nbsp;</label>
                               
                                <asp:LinkButton ID="LinkSearchButton" runat="server" Class="k-button" OnClick="Search_Click"><i class="fa fa-search" style="color:yellowgreen"></i> Search</asp:LinkButton>
                                <asp:LinkButton ID="LinkClearButton" runat="server" Class="k-button" OnClick="Clear_Click"><i class="fa fa-ban" style="color:lightblue"></i> Clear</asp:LinkButton>
                                
                            </li>
                        </ul>
                    </div>

                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="ErrorMessage" runat="server" Visible="False">
           <div class="mt-3 mb-3" >
            <asp:Label ID="Message" runat="server"  Class="alert alert-danger"></asp:Label>
        </div> 
        </asp:PlaceHolder>  
        <div>

           <%-- <asp:Label ID="Message" runat="server" Text="" CssClass="alert-danger"></asp:Label>--%>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false"
                PageCountMode="Actual"
                ShowPageNavigationControls="True"
                ShowPrintButton="True"
                SizeToReportContent="true">
            </rsweb:ReportViewer>
        </div>
        <div>
            <iframe id="frmPrint" name="IframeName" width="500"
                height="200" runat="server"
                style="display: none"></iframe>
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
            //$("#DoctorList, #HospitalList, #SpecialityList").kendoDropDownList({

            //});
            $("#StartDate, #EndDate").kendoDatePicker();
        });
    </script>
</body>
</html>
