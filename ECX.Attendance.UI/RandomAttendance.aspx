<%@ Page Title="Assign Token Randomazation" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="RandomAttendance.aspx.cs" Inherits="ECX.Attendance.UI.RandomAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h2>Electronic Attendance System-Randomization</h2>
    <asp:Label ID="lblNoWorkstationsNTradingCenter" runat="server"></asp:Label>

    <table>
        <tr>
            <td>Member ID:<asp:TextBox ID="txtMemberId" runat="server"></asp:TextBox>
            </td>

            <ajaxToolkit:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                Enabled="True" TargetControlID="txtMemberId" FilterType="Numbers"></ajaxToolkit:FilteredTextBoxExtender>

            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" /></td>
        </tr>
        <tr>
            <td>Rep Id:
                <asp:DropDownList ID="ddlRep" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRep_SelectedIndexChanged" /></td>


        </tr>
        <tr>
            <td>
            <asp:RadioButtonList ID="rdCommodity" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdCommodity_SelectedIndexChanged">
                <asp:ListItem Value="1" Selected="True">Non-Coffee</asp:ListItem>
                <asp:ListItem Value="2">Coffee</asp:ListItem>
            </asp:RadioButtonList></td></tr>
    </table>
    <asp:Label ID="lblNotification" runat="server" />
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvMemberAllowedToTrade" runat="server" AutoGenerateColumns="False"
                    CellPadding="5" ForeColor="Black" BackColor="White" BorderColor="#CCCCCC" OnRowCommand="gvMemberAllowedToTrade_RowCommand"
                    BorderStyle="None" BorderWidth="1px" CellSpacing="10" OnRowDeleting="gvMemberAllowedToTrade_RowDeleting" DataKeyNames="SessionId" CssClass="label"
                    GridLines="Vertical">
                    <AlternatingRowStyle BackColor="#E5E5E5" />
                    <Columns>
                        <%--<asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <div id="gv">
                                    <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="true" OnCheckedChanged="chkToken_CheckedChanged" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Session Name">

                            <ItemTemplate>
                                <asp:Label ID="lblSessionName" runat="server" Text='<%# Eval("SessionName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assigned Reps" Visible="true">

                            <ItemTemplate>
                                <asp:Label ID="lblAssignedReps" runat="server" Text='<%# Eval("AssignedReps") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="ECXNewId" ItemStyle-Width="100px" HeaderText="IdNo">
                            <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>--%>
                        <asp:BoundField DataField="AssignedTokens" HeaderText="Assigned Tokens" />
                        <%--<asp:BoundField DataField="Status" ItemStyle-Width="100px" HeaderText="Attendance Status">
                            <ItemStyle Width="300px"></ItemStyle>
                        </asp:BoundField>--%>
                        <%--<asp:TemplateField HeaderText="Status" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" Width="80px" Text='<%#Eval("Status")%>' runat="server" Enabled="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:TemplateField>--%>
                        <%-- <asp:TemplateField HeaderText="Rep IdNo" >
                            <ItemTemplate>
                                <asp:Label ID="lblRepId" Width="80px" Text='<%#Eval("RepIdNo")%>' runat="server" Enabled="false"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="200px"></HeaderStyle>
                        </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Token" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtToken" Width="80px" Text='<%#Eval("Token")%>' runat="server" Enabled="false"></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Width="100px"></HeaderStyle>
                        </asp:TemplateField>--%>
                        <asp:BoundField DataField="CreatedByName" HeaderText="Created By" SortExpression="CreatedByName" />
                        <%--<asp:BoundField DataField="CreatedDate" HeaderText="Created Date" SortExpression="CreatedDate" />-- %>
                        <%--<asp:BoundField DataField="UpdatedByName" HeaderText="Updated By" SortExpression="UpdatedByName" />
                        <asp:BoundField DataField="UpdatedDate" HeaderText="Updated Date" SortExpression="UpdatedDate" />--%>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkAssign" runat="server" CausesValidation="False" CommandName="Assign" CommandArgument='<%#Eval("SessionId")%>' Text="Assign"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDelete" runat="server" CausesValidation="False" OnClientClick="return confirm ('Are you sure you want to delete this record?');" CommandName="Delete" Text="Delete"></asp:LinkButton>
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
                <asp:Button ID="btnAssigenToken" Visible="false" runat="server" Text="Assigen Token" OnClick="btnAssigenToken_Click" />
                <asp:Button ID="btnDelete" runat="server" Visible="false" Text="Delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
                <br />
                <hr />

            </td>
        </tr>
    </table>
</asp:Content>
