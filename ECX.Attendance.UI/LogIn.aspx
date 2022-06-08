<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogIn.aspx.cs" Inherits="ECX.Attendance.UI.LogIn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" style="height:500px;">
    <tr>
      <td align="center" valign="middle">
          <table width="35%">
              <tr>
                  <td>
                      <asp:Label ID="lblUserName" runat="server" Text="User Name: "></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                  </td>
             </tr>
             <tr>
                  <td>
                      <asp:Label ID="lblPassword" runat="server" Text="Password: "></asp:Label>
                  </td>
                  <td>
                      <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                  </td>
              </tr>
              <tr>
                  <td></td>
                  <td>
                      <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
                  </td>
              </tr>
              <tr>
                  <td></td>
                  <td>
                      <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
                  </td>
              </tr>
          </table>
        
      </td>
    </tr>
  </table>
    </div>
    </form>
</body>
</html>
