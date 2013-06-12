<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="RezeptBearbeiten.aspx.cs" Inherits="RezeptVerwaltung.Web.RezeptBearbeiten" ValidateRequest="false"%>
<%@ OutputCache Location="None" VaryByParam="None" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <h1><asp:Literal ID="Literal_Überschrift" runat="server"></asp:Literal></h1>

    <section>
        <!--Rezept-->
        <asp:TextBox ID="TextBox_Rezept" runat="server" Width="100%"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Rezept" runat="server" ErrorMessage="!!!" ControlToValidate="TextBox_Rezept" Text="*"></asp:RequiredFieldValidator>

        <br/>
        <div class="floatLeft">
            Das Rezept ist für 
            <asp:TextBox ID="TextBox_Menge" runat="server" Width="30px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Menge" runat="server" ErrorMessage="!!!" ControlToValidate="TextBox_Menge" Text="*"></asp:RequiredFieldValidator>
            <asp:DropDownList ID="DropDownList_AnzahlEinheit" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_DropDownList_AnzahlEinheit" runat="server" ErrorMessage="!!!" ControlToValidate="DropDownList_AnzahlEinheit" Text="*"></asp:RequiredFieldValidator>
        </div>                    
        <div>
            Kategorie: <asp:DropDownList ID="DropDownList_Kategorie" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_DropDownList_Kategorie" runat="server" ErrorMessage="!!!" ControlToValidate="DropDownList_Kategorie" Text="*"></asp:RequiredFieldValidator>
        </div>
    </section>
    <section id="recipeWorkZutaten" class="clear" style="text-align:left">
        <!--Zutaten-->
        <h2>Zutaten</h2>
        <asp:UpdatePanel ID="UpdatePanelZutaten" runat="server">
            <ContentTemplate>
                            
            </ContentTemplate>
        </asp:UpdatePanel>    
    </section>
    <aside id="recipeWorkUploadPicture">
        <h2>Bild hochladen</h2>
        <div><asp:Image ID="Image_Rezept" runat="server" /></div>
        <br />
        <div>
            <asp:FileUpload ID="FileUpload_RezeptBild" runat="server" /><br/>
            <asp:LinkButton ID="LinkButton_RezeptBild" runat="server" Text="Bild hochladen" onclick="ButtonRezeptBildClick" CausesValidation="false"/>
            <asp:Label ID="Label_RezeptBild" runat="server" Text=""></asp:Label>
        </div>
    </aside>
    <section class="clear">
        <br />
        <h2>Anleitung</h2>
        <script type="text/javascript" src="../../Scripts/tinymce/tinymce.min.js"></script>
        <script type="text/javascript">
            tinymce.init({
                selector: "textarea",
                force_br_newlines : true,
                force_p_newlines: false,
                forced_root_block: false,
                menubar: false,
                statusbar: false
            });
        </script>
        <div>
            <asp:TextBox ID="TextBox_Anleitung" runat="server" Width="900px" Height="150px" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator_TextBox_Anleitung" runat="server" ErrorMessage="!!!" ControlToValidate="TextBox_Anleitung" Text="*"></asp:RequiredFieldValidator>
        </div>
    </section>    
    <section>
        <br />
        <div>
            <asp:Button ID="Button_speichern" runat="server" Text="speichern" onclick="ButtonSpeichernClick"/>
            <asp:Button ID="Button_abbrechen" runat="server" Text="zurück" onclick="ButtonAbbrechenClick" />
            <asp:Button ID="Button_löschen" runat="server" Text="löschen" onclick="ButtonLöschenClick" />
        </div>
    </section>    
</asp:Content>
