﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MixERP.Net.Common.Helpers;
using MixERP.Net.i18n.Resources;

namespace MixERP.Net.WebControls.ScrudFactory.Controls
{
    internal sealed class CommandPanel : IDisposable
    {
        private HtmlGenericControl commandPanel;
        public event EventHandler DeleteButtonClick;
        public event EventHandler WordExportButtonClick;
        public event EventHandler ExcelExportButtonClick;
        public event EventHandler PdfExportButtonClick;
        public event EventHandler EditButtonClick;

        public string AddButtonIconCssClass { get; set; }
        public string AllButtonIconCssClass { get; set; }
        public string ButtonCssClass { get; set; }
        public string CompactButtonIconCssClass { get; set; }
        public string CssClass { get; set; }
        public string DeleteButtonIconCssClass { get; set; }

        public Button DeleteButton { get; private set; }
        public Button EditButton { get; private set; }
        public Button ExcelExportButton { get; private set; }
        public Button PdfExportButton { get; private set; }
        public Button WordExportButton { get; private set; }

        public string EditButtonIconCssClass { get; set; }
        public string PrintButtonIconCssClass { get; set; }
        public string SelectButtonIconCssClass { get; set; }

        public HtmlGenericControl GetCommandPanel(string controlSuffix)
        {
            this.commandPanel = new HtmlGenericControl("div");
            this.commandPanel.Attributes.Add("role", "toolbar");
            this.commandPanel.Attributes.Add("class", this.CssClass);
            this.AddSelectButton(this.commandPanel);
            this.AddShowCompactButton(this.commandPanel);
            this.AddShowAllButton(this.commandPanel);
            this.AddAddButton(this.commandPanel);
            this.AddEditButtonHidden(this.commandPanel, controlSuffix);
            this.AddEditButtonVisible(this.commandPanel, controlSuffix);
            this.AddDeleteButtonHidden(this.commandPanel, controlSuffix);
            this.AddDeleteButtonVisible(this.commandPanel, controlSuffix);
            this.AddExportButtons(this.commandPanel, controlSuffix);
            this.AddWordExportButtonHidden(this.commandPanel, controlSuffix);
            this.AddExcelExportButtonHidden(this.commandPanel, controlSuffix);
            this.AddPdfExportButtonHidden(this.commandPanel, controlSuffix);
            this.AddPrintButton(this.commandPanel);
            return this.commandPanel;
        }

        private void AddAddButton(HtmlGenericControl p)
        {
            using (HtmlButton addButton = this.GetInputButton("CTRL + SHIFT + A", "return(scrudAddNew());", Titles.AddNew, this.ButtonCssClass, this.AddButtonIconCssClass))
            {
                p.Controls.Add(addButton);
            }
        }

        private void AddDeleteButtonHidden(HtmlGenericControl p, string controlSuffix)
        {
            this.DeleteButton = this.GetButton("CTRL + SHIFT + D", "return(scrudConfirmAction());", Titles.DeleteSelected);
            this.DeleteButton.ID = "DeleteButton" + controlSuffix;
            this.DeleteButton.CssClass = "hidden";
            this.DeleteButton.CausesValidation = false;
            this.DeleteButton.Click += this.OnDeleteButtonClick;
            p.Controls.Add(this.DeleteButton);
        }

        private void AddDeleteButtonVisible(HtmlGenericControl p, string controlSuffix)
        {
            using (HtmlButton deleteButton = this.GetInputButton("CTRL + SHIFT + D", "$('#DeleteButton" + controlSuffix + "').click();return false;", Titles.DeleteSelected, this.ButtonCssClass, this.DeleteButtonIconCssClass))
            {
                p.Controls.Add(deleteButton);
            }
        }

        private void AddEditButtonHidden(HtmlGenericControl p, string controlSuffix)
        {
            this.EditButton = this.GetButton("CTRL + SHIFT + E", "return(scrudConfirmAction());", Titles.EditSelected);
            this.EditButton.Attributes.Add("role", "edit");
            this.EditButton.ID = "EditButton" + controlSuffix;
            this.EditButton.CssClass = "hidden";
            this.EditButton.Click += this.OnEditButtonClick;
            p.Controls.Add(this.EditButton);
        }

        private void AddWordExportButtonHidden(HtmlGenericControl p, string controlSuffix)
        {
            this.WordExportButton = this.GetButton("", "scrudUpdateMarkup();", Titles.Export);
            this.WordExportButton.Text = Titles.ExportToDoc;
            this.WordExportButton.ID = "WordExportButton" + controlSuffix;
            this.WordExportButton.CausesValidation = false;
            this.WordExportButton.CssClass = "hidden";
            this.WordExportButton.Click += this.OnWordExportButtonClick;

            p.Controls.Add(this.WordExportButton);
        }

        private void AddExcelExportButtonHidden(HtmlGenericControl p, string controlSuffix)
        {
            this.ExcelExportButton = this.GetButton("", "scrudUpdateMarkup();", Titles.ExportToExcel);
            this.ExcelExportButton.ID = "ExcelExportButton" + controlSuffix;
            this.ExcelExportButton.CausesValidation = false;
            this.ExcelExportButton.CssClass = "hidden";
            this.ExcelExportButton.Click += this.OnExcelExportButtonClick;

            p.Controls.Add(this.ExcelExportButton);
        }

        private void AddPdfExportButtonHidden(HtmlGenericControl p, string controlSuffix)
        {
            this.PdfExportButton = this.GetButton("", "scrudUpdateMarkup();", Titles.ExportToPDF);
            this.PdfExportButton.ID = "PdfExportButton" + controlSuffix;
            this.PdfExportButton.CausesValidation = false;
            this.PdfExportButton.CssClass = "hidden";
            this.PdfExportButton.Click += this.OnPdfExportButtonClick;

            p.Controls.Add(this.PdfExportButton);
        }

        private void AddExportButtons(HtmlGenericControl p, string controlSuffix)
        {
            using (HtmlGenericControl dropDown = new HtmlGenericControl("div"))
            {
                dropDown.Attributes.Add("class", "ui floating dropdown button " + this.ButtonCssClass);

                using (HtmlGenericControl span = new HtmlGenericControl("span"))
                {
                    span.Attributes.Add("class", "text");
                    span.InnerText = Titles.Export;
                    dropDown.Controls.Add(span);
                }

                using (HtmlGenericControl menu = new HtmlGenericControl("div"))
                {
                    menu.Attributes.Add("class", "menu");

                    menu.Controls.Add(this.GetExportItem(Titles.ExportToDoc, "file word outline blue icon", "scrudUpdateMarkup('WordExportButton" + controlSuffix + "');"));
                    menu.Controls.Add(this.GetExportItem(Titles.ExportToExcel, "file excel outline green icon", "scrudUpdateMarkup('ExcelExportButton" + controlSuffix + "');"));
                    menu.Controls.Add(this.GetExportItem(Titles.ExportToPDF, "file pdf outline red icon", "scrudUpdateMarkup('PdfExportButton" + controlSuffix + "');"));

                    dropDown.Controls.Add(menu);
                }

                p.Controls.Add(dropDown);
            }
        }

        private HtmlGenericControl GetExportItem(string text, string icon, string onclick)
        {
            using (HtmlGenericControl item = new HtmlGenericControl("a"))
            {
                item.Attributes.Add("class", "item");

                using (HtmlGenericControl i = HtmlControlHelper.GetIcon(icon))
                {
                    item.Controls.Add(i);
                }

                using (Literal literal = new Literal())
                {
                    literal.Text = text;
                    item.Controls.Add(literal);
                }

                item.Attributes.Add("onclick", onclick);

                return item;
            }
        }

        private void AddEditButtonVisible(HtmlGenericControl p, string controlSuffix)
        {
            using (HtmlButton editButton = this.GetInputButton("CTRL + SHIFT + E", "$('#EditButton" + controlSuffix + "').click();return false;", Titles.EditSelected, this.ButtonCssClass, this.EditButtonIconCssClass))
            {
                p.Controls.Add(editButton);
            }
        }

        private void AddPrintButton(HtmlGenericControl p)
        {
            using (HtmlButton printButton = this.GetInputButton("CTRL + SHIFT + P", "scrudPrintGridView();", Titles.Print, this.ButtonCssClass, this.PrintButtonIconCssClass))
            {
                p.Controls.Add(printButton);
            }
        }

        private void AddSelectButton(HtmlGenericControl p)
        {
            if (IsModal())
            {
                using (HtmlButton addSelectButton = this.GetInputButton("RETURN", "scrudSelectAndClose();", Titles.Select, this.ButtonCssClass, this.SelectButtonIconCssClass))
                {
                    p.Controls.Add(addSelectButton);
                }
            }
        }

        private void AddShowAllButton(HtmlGenericControl p)
        {
            using (HtmlButton showAllButton = this.GetInputButton("CTRL + SHIFT + S", "scrudShowAll();", Titles.ShowAll, this.ButtonCssClass, this.AllButtonIconCssClass))
            {
                p.Controls.Add(showAllButton);
            }
        }

        private void AddShowCompactButton(HtmlGenericControl p)
        {
            using (HtmlButton showCompactButton = this.GetInputButton("CTRL + SHIFT + C", "scrudShowCompact();", Titles.ShowCompact, this.ButtonCssClass, this.CompactButtonIconCssClass))
            {
                p.Controls.Add(showCompactButton);
            }
        }

        private Button GetButton(string toolTip, string onClientClick, string text)
        {
            using (Button button = new Button())
            {
                button.CssClass = this.ButtonCssClass;
                button.ToolTip = toolTip;
                button.OnClientClick = onClientClick;
                button.Text = text;
                return button;
            }
        }

        private HtmlButton GetInputButton(string title, string onclick, string value, string cssClass, string iconCssClass)
        {
            using (HtmlButton inputButton = new HtmlButton())
            {
                inputButton.Attributes.Add("class", cssClass);
                inputButton.Attributes.Add("type", "button");
                inputButton.Attributes.Add("title", title);
                inputButton.Attributes.Add("onclick", onclick);

                if (!string.IsNullOrWhiteSpace(iconCssClass))
                {
                    inputButton.InnerHtml = @"<i class='" + iconCssClass + "'></i> " + value;
                }
                else
                {
                    inputButton.InnerHtml = value;
                }

                return inputButton;
            }
        }

        private static bool IsModal()
        {
            Page page = HttpContext.Current.CurrentHandler as Page;

            if (page != null)
            {
                string modal = page.Request.QueryString["modal"];
                if (modal != null)
                {
                    if (modal.Equals("1"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnDeleteButtonClick(object sender, EventArgs e)
        {
            if (this.DeleteButtonClick != null)
            {
                this.DeleteButtonClick(this, e);
            }
        }

        private void OnWordExportButtonClick(object sender, EventArgs e)
        {
            if (this.WordExportButtonClick != null)
            {
                this.WordExportButtonClick(this, e);
            }
        }

        private void OnExcelExportButtonClick(object sender, EventArgs e)
        {
            if (this.ExcelExportButtonClick != null)
            {
                this.ExcelExportButtonClick(this, e);
            }
        }

        private void OnPdfExportButtonClick(object sender, EventArgs e)
        {
            if (this.PdfExportButtonClick != null)
            {
                this.PdfExportButtonClick(this, e);
            }
        }

        private void OnEditButtonClick(object sender, EventArgs e)
        {
            if (this.EditButtonClick != null)
            {
                this.EditButtonClick(this, e);
            }
        }

        #region IDisposable

        private bool disposed;

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.Dispose(true);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.commandPanel != null)
            {
                this.commandPanel.Dispose();
                this.commandPanel = null;
            }

            if (this.EditButton != null)
            {
                this.EditButton.Click -= this.OnEditButtonClick;
                this.EditButtonClick = null;
                this.EditButton.Dispose();
                this.EditButton = null;
            }

            if (this.DeleteButton != null)
            {
                this.DeleteButton.Click -= this.OnDeleteButtonClick;
                this.DeleteButtonClick = null;
                this.DeleteButton.Dispose();
                this.DeleteButton = null;
            }

            if (this.WordExportButton != null)
            {
                this.WordExportButton.Click -= this.OnWordExportButtonClick;
                this.WordExportButton.Dispose();
                this.WordExportButton = null;
            }

            if (this.ExcelExportButton != null)
            {
                this.ExcelExportButton.Click -= this.OnExcelExportButtonClick;
                this.ExcelExportButton.Dispose();
                this.ExcelExportButton = null;
            }

            if (this.PdfExportButton != null)
            {
                this.PdfExportButton.Click -= this.OnPdfExportButtonClick;
                this.PdfExportButton.Dispose();
                this.PdfExportButton = null;
            }


            this.disposed = true;
        }

        #endregion
    }
}