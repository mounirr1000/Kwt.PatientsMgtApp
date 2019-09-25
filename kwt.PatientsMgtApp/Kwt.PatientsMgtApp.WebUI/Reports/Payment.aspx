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
    <link href="../Content/font-awesome.min.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">

        <div class="panel-group">
            <div id="panelbar" class="panel">
                <div class="panel-success">
                    <h4 class="panel-title">
                        <a data-toggle="collapse" href="#collapse1" style="display: block;"><i class="fa fa-search"></i>&nbsp; <span class="p-2" style="text-decoration: underline">Filter Payment Report
                        </span><i class="fa fa-angle-down"></i></a>
                    </h4>

                </div>
                <div id="collapse1" class="panel-collapse collapse">
                    <div class="panel-body">
                        <ul class="row" style="list-style: none">
                            <li class="col-md-3">
                                <label style="text-transform: uppercase;" for="ReportTypes">Select Report Type</label>
                                <asp:DropDownList ID="ReportTypes" runat="server" Style="margin-bottom: 21px;" CssClass="k-dropdown" OnSelectedIndexChanged="reportTypes_SelectedIndexChanged">
                                    <asp:ListItem Text="BANK REPORT" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="ARCHIVE REPORT" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="DETAILS REPORT" Value="3" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="MINISTRY REPORT" Value="4"></asp:ListItem>
                                    <%--@* Uncomment the next lines once the invoice is complete *@--%>
                                    <%--<asp:ListItem Text="STATISTICAL REPORT-1" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="STATISTICAL REPORT-2" Value="6"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </li>
                        </ul>
                        <hr />
                        <ul class="fieldlistReport" style="margin: 0">
                            <%-- new --%>
                            <%--<li class="editor-field">
                                
                            </li>--%>
                            <li class="editor-field">

                                <label class="" for="StartDate">Payment Start Date</label>

                                <asp:TextBox ID="StartDate" runat="server"></asp:TextBox>
                            </li>
                            <li class="editor-field">

                                <label class="" for="EndDate">Payment End Date</label>

                                <asp:TextBox ID="EndDate" runat="server"></asp:TextBox>
                            </li>
                            <li class="editor-field">

                                <label class="" for="PatientCid">Patient Cid</label>

                                <asp:TextBox ID="PatientCid" runat="server" Class="k-textbox"></asp:TextBox>
                            </li>
                            <%--  --%>
                            <li>
                                <%--<label style="text-transform: uppercase;" for="Banks">Select Bank</label>--%>
                                <asp:DropDownList ID="Banks" runat="server" Style="margin-bottom: 21px;" CssClass="k-dropdown">
                                </asp:DropDownList>
                            </li>

                            <%--  --%>
                            <li class="editor-field">
                                <label class="">&nbsp;</label>

                                <asp:LinkButton ID="LinkSearchButton" runat="server" Class="k-button" OnClick="Search_Click"><i class="fa fa-search" style="color:yellowgreen"></i> Search</asp:LinkButton>
                                <asp:LinkButton ID="LinkClearButton" runat="server" Class="k-button" OnClick="Clear_Click"><i class="fa fa-ban" style="color:lightblue"></i> Clear</asp:LinkButton>


                            </li>

                        </ul>
                        <%--<div>
                            <asp:Button ID="printreport" runat="server" Text="Button" />
                        </div>--%>
                    </div>

                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="ErrorMessage" runat="server" Visible="False">
            <div class="mt-3 mb-3 col-md-10">
                <asp:Label ID="Message" runat="server" Class="alert alert-info" Style="display: block;"></asp:Label>
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

        //new

        // Print function (require the reportviewer client ID)
        function printReport(reportId) {
            var rv1 = $('#' + reportId);
            var iDoc = rv1.parents('html');

            // Reading the report styles
            var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
            if ((styles == undefined) || (styles == '')) {
                iDoc.find('head script').each(function () {
                    var cnt = $(this).html();
                    var p1 = cnt.indexOf('ReportStyles":"');
                    if (p1 > 0) {
                        p1 += 15;
                        var p2 = cnt.indexOf('"', p1);
                        styles = cnt.substr(p1, p2 - p1);
                    }
                });
            }
            if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
            styles = '<style type="text/css">' + styles + "</style>";

            // Reading the report html
            var table = rv1.find("div[id$='_oReportDiv']");
            if (table == undefined) {
                alert("Report source not found.");
                return;
            }

            // Generating a copy of the report in a new window
            var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
            var docCnt = styles + table.parent().html();
            var docHead = '<head><title>Printing ...</title><style>body{margin:5;padding:0;}</style></head>';
            var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
            var newWin = window.open("", "_blank", winAttr);
            writeDoc = newWin.document;
            writeDoc.open();
            writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
            writeDoc.close();

            // The print event will fire as soon as the window loads
            newWin.focus();
            // uncomment to autoclose the preview window when printing is confirmed or canceled.
            // newWin.close();
        };

        // Linking the print function to the print button
        $('#printreport').click(function () {
            printReport('ReportViewer1');
        });
    </script>
</body>
</html>
