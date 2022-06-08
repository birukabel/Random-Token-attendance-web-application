<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="RegionalNMDT.aspx.cs" Inherits="ECX.Attendance.UI.RegionalNMDT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%">
        <tr>
            <td style="width: 217px">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 217px">
                <asp:FileUpload ID="fuImport" runat="server" />
            </td>
            <td>
                <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="btnImport_Click" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:Label ID="lblMsg" runat="server" />
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="height: 20px; width: 98px"></td>
            <td style="height: 20px"></td>
        </tr>
        <tr>
            <td style="width: 98px">Member Id</td>
            <td>
                <asp:TextBox ID="txtMemberId" runat="server" width="348px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 98px">Member Name</td>
            <td>
                <asp:TextBox ID="txtMemberName" runat="server" width="351px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 98px">Trading Center</td>
            <td>
                <asp:DropDownList ID="ddlCenter" runat="server" width="359px">
                 
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 98px">&nbsp;</td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                 <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />                             
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <tr>
            <td style="height: 20px; width: 98px"></td>
            <td style="height: 20px"></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvList" runat="server"
                    CellPadding="1" ForeColor="Black" BackColor="White" BorderColor="#CCCCCC"
                    BorderStyle="None" BorderWidth="1px" PageSize="20" AllowPaging="true" GridLines="Vertical"
                     DataKeyNames="ID" AutoGenerateColumns="False" OnPageIndexChanging="gvList_PageIndexChanging" OnSelectedIndexChanging="gvList_SelectedIndexChanging" OnSelectedIndexChanged="gvList_SelectedIndexChanged">
                     <AlternatingRowStyle BackColor="#E5E5E5" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" Visible="false"  />
                        <asp:BoundField DataField="MemberName" HeaderText="MemberName" SortExpression="MemberName" />
                        <asp:BoundField DataField="MemberIdNo" HeaderText="MemberIdNo" SortExpression="MemberIdNo" />                        
                        <asp:BoundField DataField="RegionalRTC" HeaderText="RegionalRTC" SortExpression="RegionalRTC" />
                        <asp:CheckBoxField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" SortExpression="CreatedDate" />
                        <asp:BoundField DataField="Creater" HeaderText="CreatedBy" SortExpression="CreatedBy" />
                        <asp:BoundField DataField="Updater" HeaderText="UpdatedBy" SortExpression="UpdatedBy" />
                        <asp:BoundField DataField="UpdatedDate" HeaderText="UpdatedDate" SortExpression="UpdatedDate" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Select" ButtonType="Link"/>
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
            </td>
        </tr>
    </table>
</asp:Content>
