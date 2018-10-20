using System;
using System.Collections.Generic;
using System.Text;
using Hangfire.Dashboard;

namespace BasisFrameWork.Extension.Hangfire.Filter
{
    public class HangfireCustomAuthorizeFilter : IDashboardAuthorizationFilter {
        public bool Authorize(DashboardContext context) {
            return true;
        }
    }
}
