using System.Web;
using System.Web.Mvc;
using VideoMonitor.Web.Filters;

namespace VideoMonitor.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginValidateAttribute());
        }
    }
}
