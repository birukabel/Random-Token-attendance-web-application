<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="RegionalNMDTReport.aspx.cs" Inherits="ECX.Attendance.UI.Reports.RegionalNMDTReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>RTC</td>
            <td>
                <asp:DropDownList ID="ddlCenter" runat="server" Width="359px">
                </asp:DropDownList></td>
            <td>
                <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" /></td>
        </tr>
    </table>
</asp:Content>
