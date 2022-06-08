<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="NotWorkingWorkStationsReport.aspx.cs" Inherits="ECX.Attendance.UI.Reports.NotWorkingWorkStationsReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 640px">
      
        <tr>
            <td style="width: 81px">
                <label>Start Date</label>

            </td>
            <td style="width: 81px">
                <label>End Date</label></td>
           
            <td style="width: 81px">
                &nbsp;</td>
           
        </tr>
        <tr>

            <td style="width: 228px">
                <asp:TextBox ID="txtStartDate" runat="server" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Please select start date" ForeColor="Red">*</asp:RequiredFieldValidator>
                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator1_ValidatorCalloutExtender" TargetControlID="RequiredFieldValidator1">
                </ajaxToolkit:ValidatorCalloutExtender>
            </td>

            <td style="width: 236px">
                <asp:TextBox ID="txtEndDate" runat="server" Width="208px"></asp:TextBox>

                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Please select end date" ForeColor="Red">*</asp:RequiredFieldValidator>

                <ajaxToolkit:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender" runat="server" BehaviorID="RequiredFieldValidator2_ValidatorCalloutExtender" TargetControlID="RequiredFieldValidator2">
                </ajaxToolkit:ValidatorCalloutExtender>

            </td>

            <td style="width: 236px">

                <asp:Button ID="btnRun" runat="server" Text="Run Report" OnClick="btnRun_Click" BackColor="#339966" />

            </td>
        </tr>
        <tr>
            <td>
                <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" BehaviorID="txtStartDate_CalendarExtender" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
            </td>
            <td>

                <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" BehaviorID="txtEndDate_CalendarExtender" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>

            </td>        
          
            <td>

                &nbsp;</td>         
          
        </tr>
    </table>
</asp:Content>
