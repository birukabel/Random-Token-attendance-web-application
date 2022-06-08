<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="ManualAttendance.aspx.cs" Inherits="ECX.Attendance.UI.ManualAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <h2>Electronic Attendance System-Manual</h2>
    <asp:Label ID="lblNoWorkstationsNTradingCenter" runat="server"></asp:Label>
    <table>
        <tr>
            <td>Member ID:<asp:TextBox ID="txtMemberId" runat="server"></asp:TextBox>
            </td>

            <ajaxToolkit:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                Enabled="True" TargetControlID="txtMemberId" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

            <td>Session:<asp:DropDownList ID="ddlSession" runat="server" /></td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvMemberAllowedToTrade" runat="server" AutoGenerateColumns="False"
                    CellPadding="1" ForeColor="Black" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" BorderWidth="1px" DataKeyNames="RepId" CssClass="label" AllowPaging="True"
                    PageSize="20" GridLines="Vertical" OnRowDeleting="gvMemberAllowedToTrade_RowDeleting">
                    <AlternatingRowStyle BackColor="#E5E5E5" />
                    <Columns>                       
                        <asp:TemplateField HeaderText="RepId" Visible="true">
                          
                            <ItemTemplate>
                                <asp:TextBox ID="txtRepId" Enabled="false" runat="server" Text='<%# Eval("ECXNewId") %>'></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle Width="200px" Wrap="True" />
                        </asp:TemplateField>
                       <%-- <asp:BoundField DataField="ECXNewId" ItemStyle-Width="100px" HeaderText="IdNo">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="RepName" ItemStyle-Width="100px" HeaderText="Rep Name">
                            <ItemStyle Width="300px"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="Status" ItemStyle-Width="100px" HeaderText="Attendance Status Name">
                            <ItemStyle Width="300px"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Token" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtToken" Width="80px" Text='<%#Eval("Token")%>' runat="server"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" SortExpression="CreatedByName" />
                         <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" />
                         <asp:BoundField DataField="UpdatedByName" HeaderText="Updated By" SortExpression="UpdatedByName" />
                         <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" />
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm ('Are you sure you want to delete this record?');" CommandName="Delete" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmptyData" runat="server" BackColor="White" BorderColor="White"
                            BorderStyle="None" BorderWidth="0px" Text="There is no data with your selection criteria"></asp:Label>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#88AB2D" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <br />
                <asp:Button ID="btnAssigenToken" runat="server" Text="Assigen Token" OnClick="btnAssigenToken_Click" />
                <br />
            <hr />
    <asp:Label ID="lblNotification" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
