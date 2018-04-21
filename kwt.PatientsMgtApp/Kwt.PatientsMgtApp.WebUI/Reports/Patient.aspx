<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Patient.aspx.cs" Inherits="Kwt.PatientsMgtApp.WebUI.Reports.Patient" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-group">
            <label>Serch By:</label>
            <asp:Label ID="Label1" runat="server" Text="Status:"></asp:Label>
            <asp:CheckBox ID="Status" runat="server" Class="form-control"/>
            <asp:Label ID="Label2" runat="server" Text="Doctor:"></asp:Label>
            <asp:TextBox ID="Doctor" runat="server" CssClass="form-control"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Text="Hospital:"></asp:Label>
            <asp:TextBox ID="Hospital" runat="server" Class="form-control"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="Patient Cid:"></asp:Label>
            <asp:TextBox ID="PatientCid" runat="server" Class="form-control"></asp:TextBox>
            <asp:Label ID="Label5" runat="server"  Text="Speciality:"></asp:Label>
            <asp:TextBox ID="Speciality" runat="server" Class="form-control"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="Search" runat="server"  Text="Search" OnClick="Search_Click" />
            <asp:Button ID="All" runat="server"  Text="Clear" OnClick="Clear_Click" />
        </div>
    <div>
        <asp:Label ID="Message" runat="server" Text="" CssClass="alert-danger"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" 
            PageCountMode="Actual" 
            ShowPageNavigationControls="True"
            ShowPrintButton="True"
            Font="sen-serif"
            Width="100%"
             SizeToReportContent="true">
           
        </rsweb:ReportViewer>  
    </div>
    </form>
</body>
</html>
