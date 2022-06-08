<%@ Page Title="" Language="C#" MasterPageFile="~/mMenu.master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="ECX.Attendance.UI.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1><asp:Label ID="lblMsg" runat="server" Text="Label">Access Denied</asp:Label></h1>
    <asp:Panel ID="pnlError" runat="server" />
</asp:Content>
