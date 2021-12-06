<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="L5Order.aspx.cs" Inherits="L5Order.Uzsakymai" StyleSheetTheme="" Theme="Theme1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Style.css">
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        </div>
        <asp:Label ID="Label14" runat="server" Text="L5 Neišpildytas užsakymas" Font-Bold="True" Font-Size="X-Large" ForeColor="White"></asp:Label>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label8" runat="server" ForeColor="White" ></asp:Label>
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="White"></asp:Label>
        <br />
        <asp:Table ID="Table1" runat="server" BackColor="White" BorderColor="Black" >
        </asp:Table>
        <br />
        <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="White"></asp:Label>
        <asp:Table ID="Table2" runat="server" BackColor="White" BorderColor="Black" >
        </asp:Table>
        <asp:Label ID="Label12" runat="server" Text="Choose order file:" Font-Bold="True" ForeColor="White"></asp:Label>
        <br />
        <asp:FileUpload ID="FileUpload1" runat="server" Width="219px" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Choose order file" ForeColor="Red">*</asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Label ID="Label3" runat="server" Text="Enter order amount:" Font-Bold="True" ForeColor="White"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" TextMode="Number"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBox1" ErrorMessage="Enter order amount" ForeColor="Red">*</asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Format" Font-Bold="True" Height="32px" Width="117px" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" CausesValidation="False" Font-Bold="True" OnClick="Button2_Click" Text="Download results (.txt)" ValidateRequestMode="Disabled" />
        <br />
        <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="White"></asp:Label>
        <br />
        <asp:Table ID="Table5" runat="server" BackColor="White" BorderColor="Black" ForeColor="Black"  >
        </asp:Table>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label11" runat="server" ForeColor="White" ></asp:Label>
        <br />
        <asp:Label ID="Label7" runat="server"></asp:Label>
        <br />
        <asp:Table ID="Table4" runat="server" GridLines="Both" BackColor="White" BorderColor="Black" ForeColor="#006600">
        </asp:Table>
        <br />
        <asp:Label ID="Label6" runat="server"></asp:Label>
        <br />
        <asp:Label ID="Label5" runat="server"></asp:Label>
        <br />
        <asp:Table ID="Table3" runat="server" BackColor="White" BorderColor="Black" ForeColor="#FF6600"  >
        </asp:Table>
        <br />
        <asp:Label ID="Label13" runat="server" ForeColor="#33CC33"></asp:Label>
        <br />
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
