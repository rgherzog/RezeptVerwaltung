using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using RezeptVerwaltung.DataAccess.Models;

namespace RezeptVerwaltung.Web
{
	public partial class RezeptBearbeiten : Page
	{
		private const string RezeptBild = "RezeptBild";
		private const int RezeptNameFeldBreite = 220;
		private const int RezeptMengeFeldBreite = 60;
		private const int RezeptabteilungFeldBreite = 400;

		private int rezeptIdForEditing;
		private bool rezeptIdForEditingParsed;
		public int RezeptIdForEditing
		{
			get
			{
				if (!this.rezeptIdForEditingParsed)
				{
					Int32.TryParse(this.Request.Params["RezeptID"], out this.rezeptIdForEditing);
					this.rezeptIdForEditingParsed = true;
				}

				return this.rezeptIdForEditing;
			}
		}

        private int ViewStateRezeptAbteilungPanelAnzahl
        {
            get
            {
                var rezeptAbteilungPanelAnzahl = 1;
                if (ViewState[Helper.REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl] != null)
                    rezeptAbteilungPanelAnzahl = (int)ViewState[Helper.REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl];

                return rezeptAbteilungPanelAnzahl;
            }
        }
        private void IncRezeptAbteilungPanelAnzahl()
        {
            var rezeptAbteilungPanelAnzahl = 1;
            if (ViewState[Helper.REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl] != null)
                rezeptAbteilungPanelAnzahl = (int)ViewState[Helper.REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl];

            ViewState[Helper.REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl] = rezeptAbteilungPanelAnzahl + 1;
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			this.PreparePageForEditingExistingOrCreatingNewRezept();
		}

		#region display Rezept
		private void PreparePageForEditingExistingOrCreatingNewRezept()
		{
			using (var db = new rherzog_70515_rzvwContext()) //TODO: Create Property for DB connection. Page Lifecycle events could be helpful
			{
				this.SetEinheitAndKategorieDropDowns(db);

				if (!this.Page.IsPostBack && RezeptIdForEditing != 0)
				{
					this.PreparePageForEditing();
				}
				else
				{
					Rezept rezept;
					if (RezeptIdForEditing != 0)
					{
						//prepare page for editing
						rezept = db.Rezepts.First(d => d.ID == RezeptIdForEditing);
						this.Literal_Überschrift.Text = "Rezept bearbeiten";
						this.Button_abbrechen.Visible = true;
					}
					else
					{
						//prepare for adding new recipe
						rezept = new Rezept();
						this.Literal_Überschrift.Text = "Neues Rezept";
						this.Button_abbrechen.Visible = false;
					}

					this.DisplayZutatenGroupedByRezeptabteilungen(db, rezept, false);
				}
			}
		}


		private void SetEinheitAndKategorieDropDowns(rherzog_70515_rzvwContext db)
		{
			//Einheiten Liste befüllen
			var listItem = new ListItem(string.Empty, string.Empty);
			this.DropDownList_AnzahlEinheit.Items.Add(listItem);
			foreach (var item in db.Einheits)
			{
				listItem = new ListItem(item.Bezeichnung, item.ID.ToString(CultureInfo.InvariantCulture));
				this.DropDownList_AnzahlEinheit.Items.Add(listItem);
			}

			//Kategorie Liste befüllen
			listItem = new ListItem(string.Empty, string.Empty);
			this.DropDownList_Kategorie.Items.Add(listItem);
			foreach (var item in db.Kategories)
			{
				listItem = new ListItem(item.Name, item.ID.ToString(CultureInfo.InvariantCulture));
				this.DropDownList_Kategorie.Items.Add(listItem);
			}
		}


		private void PreparePageForEditing()
		{
			using (var db = new rherzog_70515_rzvwContext())
			{
				//DB Abfrage
				var rezept = db.Rezepts.FirstOrDefault(d => d.ID == RezeptIdForEditing);
				if(rezept == null)
					return; 

				//rezept
				TextBox_Rezept.Text = rezept.Name;

				//Menge und Einheit
				TextBox_Menge.Text = rezept.Menge.Trim();
				DropDownList_AnzahlEinheit.SelectedIndex = DropDownList_AnzahlEinheit.Items.IndexOf(DropDownList_AnzahlEinheit.Items.FindByText(rezept.Einheit.Bezeichnung));

				//Kategorie
				DropDownList_Kategorie.SelectedIndex = DropDownList_Kategorie.Items.IndexOf(DropDownList_Kategorie.Items.FindByText(rezept.Kategorie.Name));

				//Anleitung
				TextBox_Anleitung.Text = rezept.Anleitung;

				//Bild
				var bytes = rezept.Bild;
				if (bytes != null)
				{
					Helper.SetImageFromByteArray(Image_Rezept, bytes);
				}

				//Zutaten
				DisplayZutatenGroupedByRezeptabteilungen(db, rezept, true);
			}
		}


		/// <summary>
		/// Displays all Zutaten including the Rezeptabteilung for that Rezept
		/// </summary>
		/// <param name="db"></param>
		/// <param name="rezept"></param>
		/// <param name="setValues"></param>
		private void DisplayZutatenGroupedByRezeptabteilungen(rherzog_70515_rzvwContext db, Rezept rezept, bool setValues)
		{
			var zutatenOhneRezeptAbteilung = rezept.RezeptZutats.Where(d => d.RezeptabteilungID == null).ToList();
            var zutatenNachRezeptAbteilung = rezept.RezeptZutats.GroupBy(d => d.Rezeptabteilung).Where(d => d.Key != null).OrderBy(d => d.Key.Name).ToList();
			
			//Zutaten mit Rezeptabteilung anzeigen
			var zutatenNachRezeptAbteilungList = zutatenNachRezeptAbteilung.ToList();
			for (int i = 1; i <= zutatenNachRezeptAbteilungList.Count(); i++)
			{
				this.DisplaySingleRezeptabteilung(db, zutatenNachRezeptAbteilungList[i-1].Key.RezeptZutats, setValues);
			}

            //Zutaten ohne Rezeptabteilung anzeigen
            this.DisplaySingleRezeptabteilung(db, zutatenOhneRezeptAbteilung, setValues);

            //Neue Rezeptabteilungen anzeigen
            for (int i = 1; i <= ViewStateRezeptAbteilungPanelAnzahl; i++)
            {
                DisplaySingleRezeptabteilungNeu(i);
            }
		}


		/// <summary>
		/// Displays the Zutaten including the rezeptateilung textbox for the given rezeptZutatenList all RezeptZutaten must be of the same Rezeptabteilung
		/// </summary>
		/// <param name="db"></param>
		/// <param name="rezeptZutatenList"></param>
		/// <param name="rezepAbteilungNumber">Number of the displayed rezeptpalen - needed for numbering the panels</param>
		/// <param name="setValues"></param>
		private void DisplaySingleRezeptabteilung(rherzog_70515_rzvwContext db, ICollection<RezeptZutat> rezeptZutatenList, bool setValues)
		{
			if (rezeptZutatenList.GroupBy(d=>d.Rezeptabteilung).Count() > 1)
				new ArgumentException("The parameter rezeptZutatenList contains more than one than one RezeptAbteilung. You must provide a list of RezeptZutaten that all have the same RezeptAbteilung or all have no RezeptAbteilung at all");

			if (rezeptZutatenList.Count != 0)
			{
				var rezeptZutatFirst = rezeptZutatenList.First();

				//Panel erstellen. Ein Panel je Rezeptabteilung.
				var rezeptAbteilungPanel = CreateAndAddRezeptAbteilungPanel(rezeptZutatFirst, null);

				//Rezeptabteilung anzeigen (leer, wenn keine Rezeptabteilung zugewiesen ist)
				this.DisplayRezeptabteilungTextbox(rezeptZutatFirst.Rezeptabteilung, rezeptAbteilungPanel, null);

				//Zutaten anzeigen
				DisplayExistingZutatenForRezeptabteilung(db, rezeptZutatenList, rezeptAbteilungPanel, setValues);

				//Neue Zutatenliste anzeigen
				this.DisplayNeueZutatentenListe(rezeptAbteilungPanel, false);
			}
		}


        /// <summary>
        /// Displays a new empty Rezeptabeilung
        /// </summary>
        private void DisplaySingleRezeptabteilungNeu(int? panelNumber)
        {
            if(panelNumber == null) panelNumber = ViewStateRezeptAbteilungPanelAnzahl;

            //Panel erstellen. Ein Panel je Rezeptabteilung.
            var rezeptAbteilungPanel = CreateAndAddRezeptAbteilungPanel(null, (int)panelNumber);

            //Rezeptabteilung anzeigen (leer, wenn keine Rezeptabteilung zugewiesen ist)
            this.DisplayRezeptabteilungTextbox(null, rezeptAbteilungPanel, panelNumber);

            //Leere Zutatenliste anzeigen
            DisplayNeueZutatentenListe(rezeptAbteilungPanel, false);
        }


        /// <summary>
        /// Adds a new Rezeptabteilung panel
        /// </summary>
        /// <param name="rezeptZutatFirst"></param>
        /// <param name="updatePanel"></param>
        /// <returns></returns>
        public Panel CreateAndAddRezeptAbteilungPanel(RezeptZutat rezeptZutatFirst, int? panelNumber)
        {
            if (panelNumber == null && rezeptZutatFirst==null)
                throw new ArgumentException("CreateAndAddRezeptAbteilungPanel: Specifiy a RezeptZutat or a panelNumber - both parameters must not be null");

            Panel rezeptAbteilungPanel;
            if (rezeptZutatFirst != null)
            {
                rezeptAbteilungPanel = new Panel { ID = Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL + rezeptZutatFirst.RezeptabteilungID };
            }
            else
            {                
                rezeptAbteilungPanel = new Panel { ID = Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL + Helper.REZEPBEARBEITEN_IDENT_NEU + panelNumber };
            }

            this.UpdatePanelZutaten.ContentTemplateContainer.Controls.Add(rezeptAbteilungPanel);
            return rezeptAbteilungPanel;
        }


		/// <summary>
		/// Display the give Rezeptabteilung
		/// </summary>
        private void DisplayRezeptabteilungTextbox(Rezeptabteilung rezeptAbteilung, Panel rezeptAbteilungPanel, int? panelNumber)
		{
			var label = new Label {Text = "<br/>"};
			var textbox = new TextBox { Width = RezeptabteilungFeldBreite };

			if (rezeptAbteilung == null)
			{
				//Leere Textbox anzeigen (Rezeptabteilung kann hinzugefügt werden)
                textbox.ID = Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG + Helper.REZEPBEARBEITEN_IDENT_NEU + panelNumber;
			}
			else
			{
				//Textbox mit Rezeptnamen einfügen
				textbox.ID = Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG + rezeptAbteilung.ID;
				textbox.Text = rezeptAbteilung.Name;
			}
			rezeptAbteilungPanel.Controls.Add(label);
			rezeptAbteilungPanel.Controls.Add(textbox);
			Helper.InsertLineBreak(rezeptAbteilungPanel);
		}


		/// <summary>
		/// Displays the existing Zutaten (Zutaten that are loaded from the database)
		/// </summary>
		/// <param name="db"></param>
		/// <param name="rezeptZutatenList"></param>
		/// <param name="rezeptAbteilungPanel">The panel to which the Zutaten are added to</param>
		/// <param name="setValues">if true the textboxes, dropdowns etc. have their value set - used for editing an existing recipe</param>
		private void DisplayExistingZutatenForRezeptabteilung(rherzog_70515_rzvwContext db, IEnumerable<RezeptZutat> rezeptZutatenList, Panel rezeptAbteilungPanel, bool setValues)
		{
			foreach (var rZut in rezeptZutatenList)
			{
				//Name
				var textbox = new TextBox {ID = Helper.REZEPBEARBEITEN_IDENT_ZUTAT + rZut.ZutatID};
				if (setValues)
					textbox.Text = rZut.Zutat.Name;
				textbox.Width = RezeptNameFeldBreite;
				rezeptAbteilungPanel.Controls.Add(textbox);

				//Menge
				textbox = new TextBox {ID = Helper.REZEPBEARBEITEN_IDENT_MENGE + rZut.ZutatID};
				if (setValues && rZut.Menge != null)
				{
					textbox.Text = rZut.Menge;
				}
				textbox.Width = RezeptMengeFeldBreite;
				rezeptAbteilungPanel.Controls.Add(textbox);

				//Einheit
				var dropdownlist = new DropDownList {ID = Helper.REZEPBEARBEITEN_IDENT_EINHEIT + rZut.ZutatID};
				//leerer Eintrag
				var listItem = new ListItem(string.Empty, string.Empty);
				dropdownlist.Items.Add(listItem);
				foreach (var item in db.Einheits.OrderBy(d => d.Bezeichnung))
				{
					listItem = new ListItem(item.Bezeichnung, item.ID.ToString(CultureInfo.InvariantCulture));
					dropdownlist.Items.Add(listItem);
				}
				rezeptAbteilungPanel.Controls.Add(dropdownlist);
				if (setValues && rZut.Einheit != null)
				{
					dropdownlist.SelectedIndex = dropdownlist.Items.IndexOf(dropdownlist.Items.FindByText(rZut.Einheit.Bezeichnung));
				}

				//Lösch Knopf
				var deleteButton = new Button {Text = "löschen", ID = rZut.ID.ToString(CultureInfo.InvariantCulture)};
				deleteButton.Click += this.DeleteButtonClick;
				rezeptAbteilungPanel.Controls.Add(deleteButton);

				Helper.InsertLineBreak(rezeptAbteilungPanel);
			}
		}

		/// <summary>
		/// Displays an empty Zutetenlist which is used when new Zutaten are added.
		/// </summary>
		/// <param name="rezeptAbteilungPanel"></param>
        /// <param name="newestOnly">true when called form the AddZutaten OnClick event</param>
		private void DisplayNeueZutatentenListe(Panel rezeptAbteilungPanel, bool newestOnly)
		{
			var eventtarget = Request.Params.Get("__EVENTTARGET"); //is null when a submit button is clicked or page is loaded without postback
			var rezeptAbteilungPanelID = Helper.IsolateRezepAbteilungPanelIDFromEventtargetString(eventtarget);

			var zutatenAnzahl = 0;
			if (ViewState[rezeptAbteilungPanel.ID] != null)
				zutatenAnzahl = (int) ViewState[rezeptAbteilungPanel.ID];
		
            if(zutatenAnzahl == 0)
            {
                //ZutatenCounter für Rezeptabteilung erhöhen
                zutatenAnzahl += 1;
                ViewState[rezeptAbteilungPanel.ID] = zutatenAnzahl;
            }

            if (newestOnly)
            {
                //Bestehende neue Zutat und Rezeptabteilung Buttons entfernen - sonst steht die neue Zeile nach den Buttons
                var neueZutatButton = rezeptAbteilungPanel.FindControl(Helper.REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON + rezeptAbteilungPanel.ID);
                var neueRezeptAbteilungButton = rezeptAbteilungPanel.FindControl(Helper.REZEPBEARBEITEN_IDENT_NEUE_REZEPTABTEILUNG_BUTTON + rezeptAbteilungPanel.ID);
                var neueZutatButtonLit = rezeptAbteilungPanel.FindControl("Lit " + Helper.REZEPBEARBEITEN_IDENT_NEUE_REZEPTABTEILUNG_BUTTON + rezeptAbteilungPanel.ID);
                rezeptAbteilungPanel.Controls.Remove(neueZutatButton);
                rezeptAbteilungPanel.Controls.Remove(neueRezeptAbteilungButton);
                rezeptAbteilungPanel.Controls.Remove(neueZutatButtonLit);

                //Neue Zutatenliste Eingaben einfügen inkl. leeren Eintrag                
                this.DisplayNeueZutatenListeEingaben(rezeptAbteilungPanel, zutatenAnzahl-1);
                this.DisplayNeueZutatButton(rezeptAbteilungPanel);
                rezeptAbteilungPanel.Controls.Add(new Literal { Text = "&nbsp;" });
                DisplayNeueRezeptabteilungButton(rezeptAbteilungPanel);
            }
            else
            {
                for (int i = 0; i < zutatenAnzahl; i++)
                {
                    //Neue Zutatenliste Eingaben einfügen inkl. leeren Eintrag
                    this.DisplayNeueZutatenListeEingaben(rezeptAbteilungPanel, i);

                    //Der leeren Eintrag erhält den neue Zutat Button und den neuen Rezeptabteilung Button
                    if (i == zutatenAnzahl - 1)
                    {
                        this.DisplayNeueZutatButton(rezeptAbteilungPanel);
                        rezeptAbteilungPanel.Controls.Add(new Literal { Text = "&nbsp;", ID = "Lit " + Helper.REZEPBEARBEITEN_IDENT_NEUE_REZEPTABTEILUNG_BUTTON + rezeptAbteilungPanel.ID });
                        DisplayNeueRezeptabteilungButton(rezeptAbteilungPanel);
                    }
                }
            }
		}

		private void DisplayNeueZutatenListeEingaben(Panel rezeptAbteilungPanel, int zutatenAnzahl)
		{
			using (var db = new rherzog_70515_rzvwContext())
			{
				//Name
				var textbox = new TextBox { ID = Helper.REZEPBEARBEITEN_IDENT_ZUTAT + Helper.REZEPBEARBEITEN_IDENT_NEU + rezeptAbteilungPanel.ID + zutatenAnzahl, Text = string.Empty, Width = RezeptNameFeldBreite };
				rezeptAbteilungPanel.Controls.Add(textbox);
				textbox.Focus();

				//Menge
				textbox = new TextBox { ID = Helper.REZEPBEARBEITEN_IDENT_MENGE + Helper.REZEPBEARBEITEN_IDENT_NEU + rezeptAbteilungPanel.ID + zutatenAnzahl, Text = string.Empty, Width = RezeptMengeFeldBreite };
				rezeptAbteilungPanel.Controls.Add(textbox);

				//Einheit
				var dropdownlist = new DropDownList { ID = Helper.REZEPBEARBEITEN_IDENT_EINHEIT + Helper.REZEPBEARBEITEN_IDENT_NEU + rezeptAbteilungPanel.ID + zutatenAnzahl };
				//leerer Eintrag
				var listItem = new ListItem(string.Empty, string.Empty);
				dropdownlist.Items.Add(listItem);
				foreach (var item in db.Einheits)
				{
					listItem = new ListItem(item.Bezeichnung, item.ID.ToString(CultureInfo.InvariantCulture));
					dropdownlist.Items.Add(listItem);
				}
				rezeptAbteilungPanel.Controls.Add(dropdownlist);

				//Zeilenumbruch
				var literalPageBreak = new Literal {Text = "<br/>"};
				rezeptAbteilungPanel.Controls.Add(literalPageBreak);
			}
		}

		private void DisplayNeueZutatButton(Panel rezeptAbteilungPanel)
		{
			var neueZutatButton = new LinkButton { ID = Helper.REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON + rezeptAbteilungPanel.ID, Text = "Neue Zutat", CausesValidation = false };
			neueZutatButton.Click += this.NeueZutatButtonKlick;
			rezeptAbteilungPanel.Controls.Add(neueZutatButton);
		}

		private void DisplayNeueRezeptabteilungButton(Panel rezeptAbteilungPanel)
		{
            var id = Helper.REZEPBEARBEITEN_IDENT_NEUE_REZEPTABTEILUNG_BUTTON + rezeptAbteilungPanel.ID;
			var neueRezeptAbteilungLinkButton = new LinkButton { ID = id, Text = "Neue Rezeptabteilung<br/>", CausesValidation = false };
			neueRezeptAbteilungLinkButton.Click += LinkButtonRezeptAbteilungAddClick;
			rezeptAbteilungPanel.Controls.Add(neueRezeptAbteilungLinkButton);
		}

		#endregion

		#region Eventhandler

		protected void ButtonRezeptBildClick(object sender, EventArgs e)
		{
			if (FileUpload_RezeptBild.HasFile)
			{
				try
				{
					var bytes = FileUpload_RezeptBild.FileBytes;
					Helper.SetImageFromByteArray(this.Image_Rezept, bytes);
					this.Session.Add(RezeptBild, bytes);
				}
				catch (Exception ex)
				{
					Label_RezeptBild.Text = "Beim Upload ist ein Fehler aufgetreten. Ursache: " + ex.Message;
				}
			}
		}


		protected void ButtonAbbrechenClick(object sender, EventArgs e)
		{
			Response.Redirect("RezeptDetail.aspx?RezeptID=" + RezeptIdForEditing);
			Session.Remove(RezeptBild);
		}


		protected void ButtonSpeichernClick(object sender, EventArgs e)
		{
			using (var db = new rherzog_70515_rzvwContext())
			{
				//Rezept bearbeiten
				Rezept rezept;
				if (RezeptIdForEditing != 0)
				{
					//DB Abfrage
					rezept = db.Rezepts.FirstOrDefault(d => d.ID == RezeptIdForEditing);
				}
				//Neu einfügen
				else
				{
					rezept = new Rezept();
					db.Rezepts.Add(rezept);
				}

				if(rezept == null) return;
				//Textboxen
				rezept.Anleitung = TextBox_Anleitung.Text;
				rezept.Menge = TextBox_Menge.Text;
				rezept.Name = TextBox_Rezept.Text;

				//Bild speichern
				var bild = (byte[])Session[RezeptBild];
				if (bild != null)
				{
					rezept.Bild = bild;
				}
                Session.Remove(RezeptBild);

				//Einheit speichern
				rezept.EinheitID = Int32.Parse(DropDownList_AnzahlEinheit.SelectedItem.Value);

				//Kategorie speichern
				rezept.KategorieID = Int32.Parse(DropDownList_Kategorie.SelectedItem.Value);

				//Rezeptabteilungen speichern
                this.RezeptAbteilungenSpeichern(rezept, db);

				db.SaveChanges();

				//Page refresh
				//Nach Neuerfassung Editieren
				if (RezeptIdForEditing != 0)
					Response.Redirect(Request.Url.ToString());
				else
					Response.Redirect(this.Request.Url + "?RezeptID=" + rezept.ID);
			}
		}


		protected void NeueZutatButtonKlick(object sender, EventArgs e)
		{
            var eventtarget = Request.Params.Get("__EVENTTARGET"); //is null when a submit button is clicked or page is loaded without postback
            var rezeptAbteilungPanelID = Helper.IsolateRezepAbteilungPanelIDFromEventtargetString(eventtarget);

            var zutatenAnzahl = 0;
            if (ViewState[rezeptAbteilungPanelID] != null)
                zutatenAnzahl = (int)ViewState[rezeptAbteilungPanelID];

            //ZutatenCounter für Rezeptabteilung erhöhen
            zutatenAnzahl += 1;
            ViewState[rezeptAbteilungPanelID] = zutatenAnzahl;

            Panel rezeptAbteilungPanel = (Panel)Master.FindControl("MainContent").FindControl(rezeptAbteilungPanelID);
                        
            DisplayNeueZutatentenListe(rezeptAbteilungPanel, true);
		}

		void DeleteButtonClick(object sender, EventArgs e)
		{
			var button = (Button)sender;
			var rezZutatId = Int32.Parse(button.ID);
			using (var db = new rherzog_70515_rzvwContext())
			{
				db.RezeptZutats.Remove(db.RezeptZutats.First(d => d.ID == rezZutatId));
				db.SaveChanges();
			}

			//Page refresh
			Response.Redirect(Request.Url.ToString());
		}

		protected void LinkButtonRezeptAbteilungAddClick(object sender, EventArgs e)
		{
            //Counter erhöhen
            IncRezeptAbteilungPanelAnzahl();

            DisplaySingleRezeptabteilungNeu(null);
		}

		protected void LinkButtonRezeptAbteilungDeleteClick(object sender, EventArgs e)
		{
			
		}

		#endregion

		#region Rezeptabteilung + Zutaten Speichern


        private void RezeptAbteilungenSpeichern(Rezept rezept, rherzog_70515_rzvwContext db)
        {
            //split up
            var rezeptAbteilungPanelExisting = new List<Panel>();
            var rezeptAbteilungPanelNew = new List<Panel>();

            foreach (Control control in UpdatePanelZutaten.ContentTemplateContainer.Controls)
            {
                if (control.ID != null)
                { 
                    if(control.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL + Helper.REZEPBEARBEITEN_IDENT_NEU))
                        rezeptAbteilungPanelNew.Add((Panel)control);
                    else if(control.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL))
                        rezeptAbteilungPanelExisting.Add((Panel)control);
                }
            }

            //existing Rezeptabteilungen: Save changes in Zutaten and Rezeptabteilung
            foreach (var rezeptAbteilungPanel in rezeptAbteilungPanelExisting)
            {
                var rezeptAbteilung = RezeptabteilungSpeichern(rezept, rezeptAbteilungPanel, db);
                ZutatenSpeichern(rezept, rezeptAbteilungPanel, rezeptAbteilung);
            }

            //for new Rezeptabteilungen:
            foreach (var rezeptAbteilungPanel in rezeptAbteilungPanelNew)
            {
                var rezeptAbteilung = RezeptabteilungSpeichern(rezept, rezeptAbteilungPanel, db);
                ZutatenSpeichern(rezept, rezeptAbteilungPanel, rezeptAbteilung);
            }
        }

        private Rezeptabteilung RezeptabteilungSpeichern(Rezept rezept, Panel rezeptAbteilungPanel, rherzog_70515_rzvwContext db)
        {
            var rezeptAbteilungID = Helper.FindIdOnDynamicControl(rezeptAbteilungPanel);            
            Rezeptabteilung rezeptAbteilung = null;

            //Create Rezeptabteilung
            var rezeptAbteilungTextbox = (TextBox)Helper.FindControl(rezeptAbteilungPanel, Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG + Helper.REZEPBEARBEITEN_IDENT_NEU);
            if (rezeptAbteilungID == null && rezeptAbteilungTextbox != null && rezeptAbteilungTextbox.Text != String.Empty)
            {
                rezeptAbteilung = new Rezeptabteilung { Name = rezeptAbteilungTextbox.Text };
            }

            //Rename existing Rezeptabteilung
            rezeptAbteilungTextbox = (TextBox)Helper.FindControl(rezeptAbteilungPanel, Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG);
            if (rezeptAbteilungID != null && rezeptAbteilungTextbox != null)
            {
                rezeptAbteilung = db.Rezeptabteilungs.First(d => d.ID == rezeptAbteilungID);
                rezeptAbteilung.Name = rezeptAbteilungTextbox.Text;
            }            

            return rezeptAbteilung;
        }

        private void ZutatenSpeichern(Rezept rezept, Panel rezeptAbteilungPanel, Rezeptabteilung rezeptAbteilung)
        {
            var zutatCtrlList = new List<TextBox>();
            var mengeCtrlList = new List<TextBox>();
            var einheitCtrlList = new List<DropDownList>();

            //Control Listen erstellen
            foreach (Control control in rezeptAbteilungPanel.Controls)
            {
                if (control.ID != null)
                {
                    if (control.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_EINHEIT))
                        einheitCtrlList.Add((DropDownList)control);
                    else if (control.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_MENGE))
                        mengeCtrlList.Add((TextBox)control);
                    else if (control.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_ZUTAT))
                        zutatCtrlList.Add((TextBox)control);
                }
            }


            //Set RezeptAbteilung for existing Zutaten that have no RezeptAbteilung assigned, yet
            if(rezeptAbteilung != null && rezeptAbteilung.ID == 0 && zutatCtrlList.Count(d => !d.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_NEU)) > 0)
                rezept.RezeptZutats.Select(d => { if (d.RezeptabteilungID == null) { d.Rezeptabteilung = rezeptAbteilung; } return d; }).ToList();

            //Ggf. neue Zutat einfügen
            NeueZutatenEinfügen(rezept, zutatCtrlList, mengeCtrlList, einheitCtrlList, rezeptAbteilung);

            //Zutaten aktualisieren
            this.BestehendeZutatenAktualisieren(rezept, zutatCtrlList, mengeCtrlList, einheitCtrlList);
        }


        private void BestehendeZutatenAktualisieren(Rezept rezept, IEnumerable<TextBox> zutatCtrlList, List<TextBox> mengeCtrlList, List<DropDownList> einheitCtrlList)
		{
			foreach (var zutatCtrl in zutatCtrlList)
			{
				int? idFound = Helper.FindIdOnDynamicControl(zutatCtrl);
				if (idFound != null)
				{
					var zutat = rezept.RezeptZutats.First(d => d.ZutatID == idFound).Zutat;
					var rezZutat = rezept.RezeptZutats.First(d => d.ZutatID == idFound);

					//Zutat Name
					zutat.Name = zutatCtrl.Text;

					//Zutat Menge
					var mengeCtrl = this.GetMengeTextbox(idFound, mengeCtrlList);
					//Bruchzahlen umwandeln
					rezZutat.Menge = mengeCtrl.Text;

					//Zutat Einheit
					var einheitCtrl = this.GetEinheitDropDownList(idFound, einheitCtrlList);
					
					if (einheitCtrl != null)
					{
						if (string.IsNullOrEmpty(einheitCtrl.SelectedItem.Value))
						{
							rezZutat.EinheitID = null;
						}
						else
						{
							rezZutat.EinheitID = Int32.Parse(einheitCtrl.SelectedItem.Value);
						}
					}
				}
			}
		}


		private TextBox GetMengeTextbox(int? idSuchen, IEnumerable<TextBox> mengeCtrlListe)
		{
			foreach (var mengeCtrl in mengeCtrlListe)
			{
				var idStr = mengeCtrl.ID.Remove(0, Helper.REZEPBEARBEITEN_IDENT_MENGE.Length);
				int id;
				if (Int32.TryParse(idStr, out id) && id == idSuchen)
				{
					return mengeCtrl;
				}
			}

			return null;
		}

		private DropDownList GetEinheitDropDownList(int? idSuchen, IEnumerable<DropDownList> einheitCtrlListe)
		{
			foreach (var einheitCtrl in einheitCtrlListe)
			{
				string idStr = einheitCtrl.ID.Remove(0, Helper.REZEPBEARBEITEN_IDENT_EINHEIT.Length);
				int id;
				if (Int32.TryParse(idStr, out id) && id == idSuchen)
				{
					return einheitCtrl;
				}
			}
			return null;
		}

        private void NeueZutatenEinfügen(Rezept rezept, IEnumerable<TextBox> zutatCtrlList, IEnumerable<TextBox> mengeCtrlList, IEnumerable<DropDownList> einheitCtrlList, Rezeptabteilung rezeptAbteilung)
		{
			//Neue eingefügt Zutaten ermitteln
			var zutatCtrlListNew = zutatCtrlList.Where(d => d.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_NEU)).ToList();
			var mengeCtrlListNew = mengeCtrlList.Where(d => d.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_NEU)).ToList();
			var einheitCtrlListNew = einheitCtrlList.Where(d => d.ID.Contains(Helper.REZEPBEARBEITEN_IDENT_NEU)).ToList();

			//wenn eine Eingabe erfolgt ist
			if (zutatCtrlListNew.Count != 0 && mengeCtrlListNew.Count != 0)
			{
				//jede neue Zutat dem Rezept hinzufügen
				foreach (var zutatCtrlNew in zutatCtrlListNew)
				{
                    NeueZutatEinfügen(rezept, zutatCtrlNew, einheitCtrlListNew, mengeCtrlListNew, rezeptAbteilung);
				}
			}
		}

        private static void NeueZutatEinfügen(Rezept rezept, TextBox zutatCtrlNew, IEnumerable<DropDownList> einheitCtrlListNew, IEnumerable<TextBox> mengeCtrlListNew, Rezeptabteilung rezeptAbteilung)
		{
			var idNummer = Helper.FindIdOnDynamicNewControl(zutatCtrlNew);
			//zugeh. Einheit suchen
			var einheitCtrlNew = einheitCtrlListNew.First(d => d.ID.Contains(""+idNummer));
			
			//zugeh. Menge suchen
			var mengeCtrlNew = mengeCtrlListNew.First(d => d.ID.Contains(""+idNummer));
			if (!string.IsNullOrEmpty(zutatCtrlNew.Text) && !string.IsNullOrEmpty(mengeCtrlNew.Text))
			{
                var rezZutat = new RezeptZutat { Menge = mengeCtrlNew.Text, Rezeptabteilung = rezeptAbteilung };
				var zutat = new Zutat {Name = zutatCtrlNew.Text};
				rezZutat.Zutat = zutat;
                if(einheitCtrlNew.SelectedIndex != 0)
				    rezZutat.EinheitID = Int32.Parse(einheitCtrlNew.SelectedItem.Value);
				rezept.RezeptZutats.Add(rezZutat);
			}
		}

		#endregion
	}
}