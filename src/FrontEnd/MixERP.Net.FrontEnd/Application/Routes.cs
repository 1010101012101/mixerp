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

using System.Web.Routing;
using Serilog;

namespace MixERP.Net.FrontEnd.Application
{
    internal static class Routes
    {
        internal static void RegisterRoutes(RouteCollection routes)
        {
            if (routes != null)
            {
                Log.Information("Registering routes.");
                routes.Ignore("{resource}.axd");
                routes.MapPageRoute("DefaultRoute", "", "~/SignIn.aspx");
                routes.MapPageRoute("Reporting", "Reports/{path}", "~/Reports/ReportMaster.aspx");
                routes.MapPageRoute("Modules", "Modules/{*path}", "~/Modules/Default.aspx");
            }
        }
    }
}