using Microsoft.AspNetCore.Mvc;

namespace FootballRadar.Web.Utilities
{
    public static class ControllerName
    {
        public static string For<TController>() where TController : ControllerBase
        {
            var fullname = typeof(TController).Name;

            var shortname = fullname.Remove(fullname.LastIndexOf("Controller"));

            return shortname;
        }
    }
}
