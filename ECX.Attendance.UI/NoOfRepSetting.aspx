<%@ Page Title="Rep Setting" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="NoOfRepSetting.aspx.cs" Inherits="ECX.Attendance.UI.NoOfRepSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Number Of Rep Setting Per Trading Center</h2>
    <asp:Label ID="lblNoWorkstationsNTradingCenter" runat="server"></asp:Label>

    <table>
        <tr>
            <td>Trading Center: <asp:DropDownList ID="drpTradingCenter" runat="server">
                <asp:ListItem>Select Trading Center</asp:ListItem>
                <asp:ListItem>Addis Ababa</asp:ListItem>
                <asp:ListItem>Hawassa</asp:ListItem>
                <asp:ListItem>Hummera</asp:ListItem>
                <asp:ListItem>Nekemte</asp:ListItem>
                <asp:ListItem>Gondar</asp:ListItem>
                <asp:ListItem>Adama</asp:ListItem>
                
              </asp:DropDownList></td>
            <td>Commodity: <asp:DropDownList ID="drpCommodity" runat="server"  /></td>
            <td>Number Of Rep: <asp:TextBox ID="txtNoRep" runat="server"  /></td>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
        </tr>

    </table>
    <asp:GridView ID="gvNoOfRep" runat="server" AutoGenerateColumns="False"
                    CellPadding="1" ForeColor="Black" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" BorderWidth="1px"  CssClass="label"
                    GridLines="Vertical">
                    <AlternatingRowStyle BackColor="#E5E5E5" />
                    <Columns>
                        <asp:TemplateField HeaderText="Trading Center">

                            <ItemTemplate>
                                <asp:Label ID="lblTradingCenter" runat="server" Text='<%# Eval("TradingCenter") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" Wrap="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Commodity" >

                            <ItemTemplate>
                                <asp:Label ID="lblCommodity" runat="server" Text='<%# Eval("CommodityName") %>'></asp:Label>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No Of reps" >

                            <ItemTemplate>
                                <asp:Label ID="lblReps" runat="server" Text='<%# Eval("NumberOfReps") %>'></asp:Label>
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
</asp:Content>
