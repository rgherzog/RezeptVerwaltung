<%@ Page Title="Einheiten verwalten" Language="C#" MasterPageFile="~/Pages/Site.master" AutoEventWireup="true"
    CodeBehind="Einheiten.aspx.cs" Inherits="RezeptVerwaltung.Web.Einheiten" %>
    <%@ OutputCache Location="None" VaryByParam="None" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <h2>
        Einheiten verwalten
    </h2>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                Bezeichnung<br />
                <asp:TextBox ID="TextBox_Bezeichnung" runat="server" Width="200"></asp:TextBox>
                <br />
                Kurzzeichen<br />
                <asp:TextBox ID="TextBox_Kurzzeichen" runat="server" Width="200"></asp:TextBox>
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
            <asp:GridView ID="GridView_Einheit" runat="server" AllowPaging="True" 
                AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="ID" 
                DataSourceID="SqlDataSourceRezeptVerwaltung" Height="247px" Width="200px">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" 
                        ReadOnly="True" SortExpression="ID" Visible="False" />
                    <asp:BoundField DataField="Bezeichnung" HeaderText="Bezeichnung" 
                        SortExpression="Bezeichnung" />
                    <asp:BoundField DataField="Kurzzeichen" HeaderText="Kurzzeichen" 
                        SortExpression="Kurzzeichen" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceRezeptVerwaltung" runat="server" 
                ConnectionString="<%$ ConnectionStrings:rherzog_70515_rzvwContext %>" 
                OldValuesParameterFormatString="original_{0}" 
                SelectCommand="SELECT [ID], [Bezeichnung], [Kurzzeichen] FROM [Einheit]" 
                ConflictDetection="CompareAllValues" 
                DeleteCommand="DELETE FROM [Einheit] WHERE [ID] = @original_ID AND [Bezeichnung] = @original_Bezeichnung AND [Kurzzeichen] = @original_Kurzzeichen" 
                InsertCommand="INSERT INTO [Einheit] ([Bezeichnung], [Kurzzeichen]) VALUES (@Bezeichnung, @Kurzzeichen)" 
                UpdateCommand="UPDATE [Einheit] SET [Bezeichnung] = @Bezeichnung, [Kurzzeichen] = @Kurzzeichen WHERE [ID] = @original_ID AND [Bezeichnung] = @original_Bezeichnung AND [Kurzzeichen] = @original_Kurzzeichen">
                <DeleteParameters>
                    <asp:Parameter Name="original_ID" Type="Int32" />
                    <asp:Parameter Name="original_Bezeichnung" Type="String" />
                    <asp:Parameter Name="original_Kurzzeichen" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:ControlParameter ControlID="TextBox_Bezeichnung" Name="Bezeichnung" PropertyName="Text"/>
                    <asp:ControlParameter ControlID="TextBox_Kurzzeichen" Name="Kurzzeichen" PropertyName="Text"/>
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Bezeichnung" Type="String" />
                    <asp:Parameter Name="Kurzzeichen" Type="String" />
                    <asp:Parameter Name="original_ID" Type="Int32" />
                    <asp:Parameter Name="original_Bezeichnung" Type="String" />
                    <asp:Parameter Name="original_Kurzzeichen" Type="String" />
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
