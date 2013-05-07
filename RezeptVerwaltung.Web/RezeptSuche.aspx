<%@ Page Title="Rezepte suchen" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="RezeptSuche.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptSuche" %>
    <%@ OutputCache Location="None" VaryByParam="None" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
    <h2>
        Nach Rezepten suchen
    </h2>
    <div style="margin: 20px 0px 20px 40px">
        <asp:TextBox ID="TextBox_Suche" runat="server" Width="200"></asp:TextBox> 
        <asp:Button ID="Button_Suche" runat="server" Text="suchen..." style="margin-top: 15px" onclick="ButtonSuche_Click" /> 
    </div>   
    <div>
        <asp:Table ID="Table_Suchergebnis" runat="server"></asp:Table>        
    </div>
</asp:Content>
