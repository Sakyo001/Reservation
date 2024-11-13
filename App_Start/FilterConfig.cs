using System.Web;
using System.Web.Mvc;

namespace LAB_FINAL_ACT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
