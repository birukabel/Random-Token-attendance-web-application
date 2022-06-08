<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ECX.Attendance.UI.Admin" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table style="width: 100%">
        <tr>
            <td style="height: 24px" colspan="3">
                &nbsp;</td>
            <td style="height: 24px; width: 43%;">
                &nbsp;</td>
        </tr>
        <tr>
            <td style="height: 24px; width: 17%;">
                <asp:Label ID="lblWS" runat="server" Text="Work Sattion No :"></asp:Label>
            </td>
            <td style="height: 24px; width: 39%;">
                <asp:TextBox ID="txtWS" runat="server" Width="300px" ValidationGroup="G1"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="txtWS_FilteredTextBoxExtender" runat="server" FilterType="Numbers" TargetControlID="txtWS">
                </asp:FilteredTextBoxExtender>
            </td>
            <td style="height: 24px; width: 9px;">
                 <asp:RequiredFieldValidator ID="rfvWorkStation" runat="server" Text="*" ControlToValidate="txtWS" ForeColor="Red"  ValidationGroup="G1"/>
            </td>
            <td style="height: 24px; width: 43%;">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="G1" />
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td>
                <asp:GridView ID="gvWS" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" Width="499px" OnRowDeleting="gvWS_RowDeleting">
                    <Columns>
                        
                        <asp:BoundField DataField="TradingCenter" HeaderText="TradingCenter" SortExpression="TradingCenter" />
                        <asp:BoundField DataField="TradeDate" HeaderText="TradeDate" SortExpression="TradeDate" />
                        <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="WorkinStationNumber" HeaderText="Work Station Number" SortExpression="WorkinStationNumber" />
                         <asp:BoundField DataField="CreatedBy" HeaderText="Created By" SortExpression="CreatedBy" />
                         <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" />
                         <asp:BoundField DataField="UpdatedBy" HeaderText="Updated By" SortExpression="UpdatedBy" />
                         <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" />


                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('Are you sure to delete this record ?');" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 20px">
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMsg" runat="server" Font-Bold="True"></asp:Label>
            </td>
        </tr>
    </table>

</asp:Content>
