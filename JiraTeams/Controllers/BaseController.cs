using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JiraTeams.Controllers
{
    public class BaseController : Controller
    {
        public DateTime CurrentDate = DateTime.Now.ToLocalTime();
    }
}
