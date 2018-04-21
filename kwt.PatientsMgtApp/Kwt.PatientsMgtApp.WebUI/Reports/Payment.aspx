<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Kwt.PatientsMgtApp.WebUI.Reports.Payment" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Content/bootstrap.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
        <div class="row">

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
        </div>
        <%--<div class="row">
            <asp:ImageButton ID="btnPrint" AlternateText="Print Report" runat="server" OnClick="btnPrint_Click"  />
        </div>--%>
            
        <div>

            <asp:Label ID="Message" runat="server" Text="" CssClass="alert-danger"></asp:Label>
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
</body>
</html>
