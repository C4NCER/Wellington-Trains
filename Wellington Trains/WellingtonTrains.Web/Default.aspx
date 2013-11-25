<%@ Page Language="C#" Inherits="WellingtonTrains.Web.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>Default</title>
</head>
<body>
	<form id="form1" runat="server">
	  
		<asp:DropDownList ID="DropDownListLines" runat="server" />
    <asp:DropDownList ID="DropDownListFrom" runat="server" />
    <asp:DropDownList ID="DropDownListTo" runat="server" />
    <asp:Button ID="ButtonLookup" runat="server" Text="Find Trains" OnClick="buttonLookupClicked" />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
		<asp:Button id="button1" runat="server" Text="Click me!" OnClick="button1Clicked" />
	</form>
</body>
</html>
