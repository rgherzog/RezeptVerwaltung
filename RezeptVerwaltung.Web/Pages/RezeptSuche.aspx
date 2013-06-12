<%@ Page Title="Rezepte suchen" Language="C#" MasterPageFile="~/Pages/Site.master" AutoEventWireup="true"
    CodeBehind="RezeptSuche.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptSuche" %>
    <%@ OutputCache Location="None" VaryByParam="None" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
    <h1>
        Nach Rezepten suchen
    </h1>
    <section ID="recipeSearchBox">
        <asp:TextBox ID="TextBox_Suche" runat="server" Width="200"></asp:TextBox> 
        <asp:Button ID="Button_Suche" runat="server" Text="suchen..." style="margin-top: 15px" onclick="ButtonSuche_Click" /> 
    </section>   
    <section>
        <asp:Panel ID="Panel_Suchergebnis" runat="server"></asp:Panel>
    </section>
</asp:Content>
