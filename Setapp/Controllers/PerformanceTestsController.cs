using System.Web.Mvc;
using Setapp.Business;
using Setapp.Models;

namespace Setapp.Controllers
{
    public class PerformanceTestsController : Controller
    {
        public ActionResult Index()
        {
            var performanceTests = new PerformanceTests();
            PerformanceTestsResults testsResults = performanceTests.RunTests();

            return View(testsResults);
        }
    }
}