﻿/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using MixERP.Net.Common;
using MixERP.Net.WebControls.Common.Resources;

namespace MixERP.Net.WebControls.Common
{
    public sealed class MixERPGridView : GridView
    {
        private readonly bool htmlEncode;

        public MixERPGridView()
        {
            this.htmlEncode = true;
            this.Initialize();
        }

        public MixERPGridView(bool htmlEncode)
        {
            this.htmlEncode = htmlEncode;
            this.Initialize();
        }

        private void Initialize()
        {
            this.CssClass += "ui striped table ";
            this.GridLines = GridLines.None;
            this.EmptyDataText = CommonResource.NoRecordFound;
        }

        protected override void OnDataBound(EventArgs e)
        {
            if (this.Rows.Count.Equals(0))
            {
                return;
            }

            if (this.HeaderRow == null)
            {
                return;
            }

            this.HeaderRow.TableSection = TableRowSection.TableHeader;
            base.OnDataBound(e);
        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            foreach (TableCell cell in e.Row.Cells)
            {
                DataControlFieldCell dataControlField = cell as DataControlFieldCell;

                if (dataControlField != null)
                {
                    AutoGeneratedField field = dataControlField.ContainingField as AutoGeneratedField;

                    if (field != null)
                    {

                        field.HeaderStyle.CssClass = "text left";

                        switch (field.DataType.FullName)
                        {
                            case "System.String":
                                field.HtmlEncode = this.htmlEncode;
                            break;
                            case "System.Decimal":
                            case "System.Double":
                            case "System.Single":
                                cell.CssClass = "text right";
                                field.HeaderStyle.CssClass = "text right";

                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    decimal value = Conversion.TryCastDecimal(cell.Text);

                                    if (!value.Equals(0))
                                    {
                                        cell.Text = value.ToString("N", CultureInfo.CurrentCulture);
                                    }
                                }
                                break;
                            case "System.DateTime":
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    DateTime date = Conversion.TryCastDate(cell.Text);

                                    if (date.Date == date)
                                    {
                                        cell.Text = Conversion.TryCastDate(cell.Text).ToString("D", CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        cell.Text = Conversion.TryCastDate(cell.Text).ToString("F", CultureInfo.CurrentCulture);
                                    }
                                }

                                break;
                        }

                    }
                }
            }

            base.OnRowDataBound(e);
        }
    }
}