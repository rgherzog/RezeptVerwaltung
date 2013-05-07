<%@ Page Title="Einheiten verwalten" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Kategorien.aspx.cs" Inherits="RezeptVerwaltung.Web.Kategorien" %>
    <%@ OutputCache Location="None" VaryByParam="None" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h2>
        Kategorien verwalten
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                Bezeichnung<br />
                <asp:TextBox ID="TextBox_Bezeichnung" runat="server" Width="200"></asp:TextBox>
            </div>
            <asp:Button ID="Button_Einfügen" runat="server" Text="Einfügen" 
                onclick="Button_Einfügen_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Button_Einfügen" EventName="Click"/>
        </Triggers>
        <ContentTemplate>
            <asp:GridView ID="GridView_Kategorien" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" 
                DataSourceID="SqlDataSourceRezeptVerwaltung" Height="247px" Width="200px">
                <Columns>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"/>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                        ReadOnly="True" SortExpression="ID" Visible="False" />
                    <asp:BoundField DataField="Name" HeaderText="Name" 
                        SortExpression="Name" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceRezeptVerwaltung" runat="server" 
                ConnectionString="<%$ ConnectionStrings:rherzog_70515_rzvwContext %>" 
                OldValuesParameterFormatString="original_{0}" 
                SelectCommand="SELECT [ID], [Name] FROM [Kategorie]"
                ConflictDetection="CompareAllValues" 
                DeleteCommand="DELETE FROM [Kategorie] WHERE [ID] = @original_ID"
                InsertCommand="INSERT INTO [Kategorie] ([Name]) VALUES (@Name)"
                UpdateCommand="UPDATE [Kategorie] SET [Name] = @Name WHERE [ID] = @original_ID">
                <DeleteParameters>
                    <asp:Parameter Name="original_ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:ControlParameter ControlID="TextBox_Bezeichnung" Name="Name" PropertyName="Text"/>
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="original_ID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div>Daten werden geladen...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
