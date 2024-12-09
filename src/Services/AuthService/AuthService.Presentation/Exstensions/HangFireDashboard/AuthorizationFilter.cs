using Hangfire.Dashboard;

namespace AuthService.Presentation.Exstensions.HangFireDashboard
{
    public class AuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }

}
