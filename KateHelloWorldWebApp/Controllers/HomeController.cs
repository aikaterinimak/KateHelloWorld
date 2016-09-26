using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KateHelloWorldHelperLib;
using KateHelloWorldHelperLib.Models.Responses;
using System.Configuration;

namespace KateHelloWorldWebApp.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            Uri KateHelloWorldApisUri = new Uri(ConfigurationManager.AppSettings["KateHelloWorldApisUri"]);
            IKateHelloWorldServiceProvider _KateHelloWorldHelper = new KateHelloWorldServiceProvider(KateHelloWorldApisUri, null, null);

            getCitiesResponse citiesResponse = new getCitiesResponse();

            try
            {
                citiesResponse.cities = await _KateHelloWorldHelper.getCityListAsync();
            }
            catch (Exception ex)
            {
                //For dev purposes, we will show the error
                ViewBag.Error = ex.Message;
            }

            return View(citiesResponse.cities);
        }
    }
}