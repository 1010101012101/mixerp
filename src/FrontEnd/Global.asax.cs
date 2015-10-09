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

using System;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using MixERP.Net.Entities.Office;
using MixERP.Net.Entities.Public.Meta;
using MixERP.Net.FrontEnd.Application;
using MixERP.Net.ReportManager;

namespace MixERP.Net.FrontEnd
{
    public class Global : HttpApplication
    {
        private void Application_Error(object sender, EventArgs e)
        {
            ApplicationError.Handle(this.Server.GetLastError());
        }

        private void Application_Start(object sender, EventArgs e)
        {
            LogManager.IntializeLogger();
            WebJobManager.Register();
            UpdateManager.CheckForUpdates(this.Application);
            Routes.RegisterRoutes(RouteTable.Routes);

            GlobalLogin.CreateTable();
            SalesQuotationValidation.CreateTable();
            Repository.DownloadAndInstallReports();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}