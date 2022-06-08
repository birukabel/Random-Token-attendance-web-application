<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="ECX.Attendance.UI.Reports.AttendanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 640px">
        <tr>
            <td style="width: 81px">&nbsp;</td>
            <td style="width: 81px">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 81px">
                <label>Start Date</label>

            </td>
            <td style="width: 81px">
                <label>End Date</label></td>
            <td>
                <asp:Label ID="lblPlatform" runat="server" Text="Trading Platform"></asp:Label></td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Trade Session Name"></asp:Label></td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="Memeber ID"></asp:Label></td>
            <td>
                <asp:Label ID="lblRepId" runat="server" Text="Rep ID"></asp:Label></td>
            <td>
                <asp:Label ID="Label3" runat="server" Text="Attended Trade"></asp:Label></td>
        </tr>
        <tr>

            <td style="width: 228px">
                <asp:TextBox ID="txtStartDate" runat="server" Width="112px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Please select start date" ForeColor="Red">*</asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="RequiredFieldValidator1">
                </ajaxToolkit:ValidatorCalloutExtender>
            </td>

            <td style="width: 236px">
                <asp:TextBox ID="txtEndDate" runat="server" Width="112px"></asp:TextBox>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Please select end date" ForeColor="Red">*</asp:RequiredFieldValidator>

                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator2_ValidatorCalloutExtender" TargetControlID="RequiredFieldValidator2">
                </ajaxToolkit:ValidatorCalloutExtender>

            </td>
            <td width="17%">

                <asp:DropDownList ID="ddlPlateForm" runat="server" AutoPostBack="false" Enabled="False" >
                    <asp:ListItem Value="2">Electronic</asp:ListItem>
                    <asp:ListItem Value="1">Outcry</asp:ListItem>
                    <asp:ListItem Value="3">Both</asp:ListItem>
                </asp:DropDownList>
            </td>

            <td width="12%">
                <asp:DropDownList ID="ddlSession" runat="server" AutoPostBack="false" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" Width="112px">
                </asp:DropDownList>
            </td>
            <td width="12%">
                <asp:DropDownList ID="ddlMemberId" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMemberId_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td width="12%">
                <asp:DropDownList ID="ddlRepId" runat="server" AutoPostBack="false">
                </asp:DropDownList>
            </td>

            <td>

                <asp:DropDownList ID="ddlTradeAttend" runat="server" AutoPostBack="false">

                    <asp:ListItem Value="1">YES</asp:ListItem>
                    <asp:ListItem Value="2">NO</asp:ListItem>
                </asp:DropDownList>
            </td>

        </tr>
        <tr>
            <td>
                <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
            </td>
            <td>

                <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>

            </td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td>

                <asp:Button ID="btnRun" runat="server" Text="Run Report" OnClick="btnRun_Click" BackColor="#339966" />
            </td>
        </tr>
        <tr>
            <td colspan="6" align="right">

                <%-- <asp:Button ID="btnExport" runat="server" Text="Export" OnClick="btnExport_Click" BackColor="#339966"/>--%>
            </td>
        </tr>
        <tr>
            <td colspan="6" align="right">&nbsp;</td>
        </tr>
    </table>

</asp:Content>
