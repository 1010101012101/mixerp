// ReSharper disable All
/********************************************************************************
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
using MixERP.Net.Framework;
using MixERP.Net.Entities.Transactions;
using MixERP.Net.Schemas.Transactions.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MixERP.Net.ApplicationState.Cache;
using MixERP.Net.Common.Extensions;
using PetaPoco;
namespace MixERP.Net.Api.Transactions
{
    /// <summary>
    /// Provides a direct HTTP access to execute the function GetSalesTax.
    /// </summary>
    [RoutePrefix("api/v1.5/transactions/procedures/get-sales-tax")]
    public class GetSalesTaxController : ApiController
    {
        /// <summary>
        /// Login id of application user accessing this API.
        /// </summary>
        public long _LoginId { get; set; }

        /// <summary>
        /// User id of application user accessing this API.
        /// </summary>
        public int _UserId { get; set; }

        /// <summary>
        /// Currently logged in office id of application user accessing this API.
        /// </summary>
        public int _OfficeId { get; set; }

        /// <summary>
        /// The name of the database where queries are being executed on.
        /// </summary>
        public string _Catalog { get; set; }

        private GetSalesTaxProcedure procedure;
        public class Annotation
        {
            public string TranBook { get; set; }
            public int StoreId { get; set; }
            public string PartyCode { get; set; }
            public string ShippingAddressCode { get; set; }
            public int PriceTypeId { get; set; }
            public string ItemCode { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal Discount { get; set; }
            public decimal ShippingCharge { get; set; }
            public int SalesTaxId { get; set; }
        }

        public GetSalesTaxController()
        {
            this._LoginId = AppUsers.GetCurrent().View.LoginId.ToLong();
            this._UserId = AppUsers.GetCurrent().View.UserId.ToInt();
            this._OfficeId = AppUsers.GetCurrent().View.OfficeId.ToInt();
            this._Catalog = AppUsers.GetCurrentUserDB();
            this.procedure = new GetSalesTaxProcedure
            {
                _Catalog = this._Catalog,
                _LoginId = this._LoginId,
                _UserId = this._UserId
            };
        }

        [AcceptVerbs("POST")]
        [Route("execute")]
        [Route("~/api/transactions/procedures/get-sales-tax/execute")]
        public IEnumerable<DbGetSalesTaxResult> Execute([FromBody] Annotation annotation)
        {
            try
            {
                this.procedure.TranBook = annotation.TranBook;
                this.procedure.StoreId = annotation.StoreId;
                this.procedure.PartyCode = annotation.PartyCode;
                this.procedure.ShippingAddressCode = annotation.ShippingAddressCode;
                this.procedure.PriceTypeId = annotation.PriceTypeId;
                this.procedure.ItemCode = annotation.ItemCode;
                this.procedure.Price = annotation.Price;
                this.procedure.Quantity = annotation.Quantity;
                this.procedure.Discount = annotation.Discount;
                this.procedure.ShippingCharge = annotation.ShippingCharge;
                this.procedure.SalesTaxId = annotation.SalesTaxId;


                return this.procedure.Execute();
            }
            catch (UnauthorizedException)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }
            catch (MixERPException ex)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    Content = new StringContent(ex.Message),
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            catch
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        }
    }
}