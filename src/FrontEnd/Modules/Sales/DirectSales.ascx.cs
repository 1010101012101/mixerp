﻿/********************************************************************************
Copyright (C) MixERP Inc. (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, version 2 of the License.


MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using MixERP.Net.Entities;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.i18n.Resources;
using MixERP.Net.WebControls.StockTransactionViewFactory;
using System;

namespace MixERP.Net.Core.Modules.Sales
{
    public partial class DirectSales : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            //using (StockTransactionView view = new StockTransactionView())
            //{
            //    view.Text = Titles.DirectSales;
            //    view.Book = TranBook.Sales;
            //    view.SubBook = SubTranBook.Direct;
            //    view.AddNewUrl = "~/Modules/Sales/Entry/DirectSales.mix";
            //    view.PreviewUrl = "~/Modules/Sales/Reports/DirectSalesInvoiceReport.mix";
            //    view.ChecklistUrl = "~/Modules/Sales/Confirmation/DirectSales.mix";
            //    view.ShowReturnButton = true;

            //    view.ReturnButtonUrl = "~/Modules/Sales/Entry/Return.mix";

            //    view.DbTableName = "transactions.transaction_master";
            //    view.PrimaryKey = "transaction_master_id";

            //    view.UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            //    view.OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            //    view.Catalog = AppUsers.GetCurrentUserDB();

            //    this.Placeholder1.Controls.Add(view);
            //}
        }
    }
}