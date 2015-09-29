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
// ReSharper disable All
using PetaPoco;
using System;

namespace MixERP.Net.Entities.HRM
{
    [PrimaryKey("", autoIncrement = false)]
    [TableName("hrm.salary_tax_scurd_view")]
    [ExplicitColumns]
    public sealed class SalaryTaxScurdView : PetaPocoDB.Record<SalaryTaxScurdView>, IPoco
    {
        [Column("salary_tax_id")]
        [ColumnDbType("int4", 0, true, "")]
        public int? SalaryTaxId { get; set; }

        [Column("salary_tax_code")]
        [ColumnDbType("varchar", 12, true, "")]
        public string SalaryTaxCode { get; set; }

        [Column("salary_tax_name")]
        [ColumnDbType("varchar", 128, true, "")]
        public string SalaryTaxName { get; set; }

        [Column("tax_authority")]
        [ColumnDbType("text", 0, true, "")]
        public string TaxAuthority { get; set; }

        [Column("standard_deduction")]
        [ColumnDbType("money_strict2", 0, true, "")]
        public decimal? StandardDeduction { get; set; }

        [Column("personal_exemption")]
        [ColumnDbType("money_strict2", 0, true, "")]
        public decimal? PersonalExemption { get; set; }
    }
}