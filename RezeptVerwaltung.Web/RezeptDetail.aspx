<%@ Page Title="Rezept Detail" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RezeptDetail.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptDetail" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td style="width:100%; vertical-align: top; text-align:left">
                <h2>
                    <asp:Literal ID="Literal_Rezept" runat="server"></asp:Literal>
                </h2>
                <h3>
                    <asp:Literal ID="Literal_Kategorie" runat="server"></asp:Literal>
                </h3> 
                <br />   
                <asp:Literal ID="Literal_Zutatenüberschrift" runat="server"></asp:Literal>
                <br />
                <asp:Panel ID="Panel_Rezeptabteilungen" runat="server">
                </asp:Panel>                
            </td>
            <td style="width:250px; height: 250px;">
                <asp:Image ID="Image_Rezept" runat="server"/>
            </td>
        </tr>
    </table>
    <div>
        <asp:Literal ID="Literal_Anleitung" runat="server"></asp:Literal>
    </div>
    <p>
        <asp:HyperLink ID="HyperLink_Edit" runat="server" ImageUrl="images/file_edit.png"></asp:HyperLink>
    </p>
    <p>
        <asp:Button ID="Button_zurück" runat="server" Text="zurück" 
            onclick="Button_zurück_Click" />
    </p>
</asp:Content>
