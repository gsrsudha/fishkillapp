<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FishKillReport.Default" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:label runat="server" id="lblName">Enter your name</asp:label><asp:TextBox runat="server" id="txtName" type="text"></asp:TextBox><br />
            <asp:FileUpload runat="server" ID="fileCtrl" ToolTip="Upload image"/>
            <asp:button id="btnSubmit" runat="server" type="submit" Text="Submit" OnClick="btnSubmit_Click"></asp:button>
            <asp:Label ID="lblResponse" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
