<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RezeptVerwaltung._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Rezepte Suchen
    </h2>
    <p>
        <asp:Literal ID="Literal_Rezepte" runat="server"></asp:Literal>
    </p>
</asp:Content>
