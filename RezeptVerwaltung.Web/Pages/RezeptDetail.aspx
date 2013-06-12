<%@ Page Title="Rezept Detail" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="RezeptDetail.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptDetail" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup>
        <h1><asp:Literal ID="Literal_Rezept" runat="server"></asp:Literal></h1>
        <h2><asp:Literal ID="Literal_Kategorie" runat="server"></asp:Literal></h2>
    </hgroup>

    <section id="recipeWorkZutaten" class="clear">        
        <br />   
        <asp:Literal ID="Literal_Zutatenüberschrift" runat="server"></asp:Literal>
        <br />
        <asp:Panel ID="Panel_Rezeptabteilungen" runat="server">
        </asp:Panel>                
    </section>        
    <section id="recipeWorkUploadPicture">
        <asp:Image ID="Image_Rezept" runat="server"/>
    </section>

    <section class="clear">
        <div>
            <asp:Literal ID="Literal_Anleitung" runat="server"></asp:Literal>
        </div>
        <p>
            <asp:HyperLink ID="HyperLink_Edit" runat="server" ImageUrl="~/images/file_edit.png"></asp:HyperLink>
        </p>
        <p>
            <asp:Button ID="Button_zurück" runat="server" Text="zurück" 
                onclick="Button_zurück_Click" />
        </p>
    </section>
</asp:Content>
