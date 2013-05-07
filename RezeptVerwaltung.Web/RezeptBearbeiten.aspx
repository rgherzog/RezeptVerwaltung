<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RezeptBearbeiten.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptBearbeiten" %>
<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h2><asp:Literal ID="Literal_Überschrift" runat="server"></asp:Literal></h2>
        <table>
            <tr>
                <td style="width:100%; vertical-align: top; text-align:left">
                    <asp:TextBox ID="TextBox_Rezept" runat="server" Width="500px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Rezept" runat="server" ErrorMessage="*" ControlToValidate="TextBox_Rezept" Text="*"></asp:RequiredFieldValidator>
                    <table>
                        <tr>
                            <td>
                                <div>
                                    Das Rezept ist für 
                                    <asp:TextBox ID="TextBox_Menge" runat="server" Width="30px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Menge" runat="server" ErrorMessage="*" ControlToValidate="TextBox_Menge" Text="*"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="DropDownList_AnzahlEinheit" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_DropDownList_AnzahlEinheit" runat="server" ErrorMessage="*" ControlToValidate="DropDownList_AnzahlEinheit" Text="*"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div>
                                    Kategorie: <asp:DropDownList ID="DropDownList_Kategorie" runat="server"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_DropDownList_Kategorie" runat="server" ErrorMessage="*" ControlToValidate="DropDownList_Kategorie" Text="*"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br/>
                    <p>Zutaten:</p>
                    <asp:UpdatePanel ID="UpdatePanelZutaten" runat="server">
                        <ContentTemplate>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </td>
                <td style="width:250px; height: 250px; vertical-align: top; text-align:left">
                    <div>Bild hochladen:</div>
                    <div>
                        <asp:FileUpload ID="FileUpload_RezeptBild" runat="server" /><br/>
                        <asp:LinkButton ID="LinkButton_RezeptBild" runat="server" Text="Bild hochladen" onclick="ButtonRezeptBildClick" />
                        <asp:Label ID="Label_RezeptBild" runat="server" Text=""></asp:Label>
                    </div>
                    <div><asp:Image ID="Image_Rezept" runat="server" /></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <p>
                        Anleitung:
                    </p>
                    <div>
                        <asp:TextBox ID="TextBox_Anleitung" runat="server" Width="900px" Height="150px" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Anleitung" runat="server" ErrorMessage="*" ControlToValidate="TextBox_Anleitung" Text="*"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
        </table>
    <div>
        <asp:Button ID="Button_speichern" runat="server" Text="speichern" onclick="ButtonSpeichernClick"/>
        <asp:Button ID="Button_abbrechen" runat="server" Text="zurück" onclick="ButtonAbbrechenClick" />
    </div>
</asp:Content>
