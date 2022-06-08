<%@ Page Title="Assigned Work Station Report" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="RepAssignedWorkStations.aspx.cs" Inherits="ECX.Attendance.UI.Reports.RepAssignedWorkStations" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <table style="width: 100%">
        <tr>
            <td style="width: 7%">&nbsp;</td>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="height: 20px; width: 7%">Rep ID</td>
            <td style="height: 20px; width: 128px">
                <asp:TextBox ID="txtRepId" runat="server"></asp:TextBox>
            </td>
            <td style="height: 20px; width: 77%">
                <asp:Button ID="btnSearch" runat="server" OnClick="Button1_Click" Text="Search" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <table style="width: 100%">
        <tr>
            <td>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="553px" Height="616px" style="margin-right: 0px">
                </rsweb:ReportViewer>
            </td>
        </tr>
    </table>
    
</asp:Content>
