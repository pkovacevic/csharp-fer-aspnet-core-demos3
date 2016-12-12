using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;

        public HomeController(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IActionResult Index()
        {
            var value = HttpContext.Session.GetString("SomeKey");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult SessionTest()
        {
            HttpContext.Session.SetString("SomeKey", "1234");
            return View();
        }

        public async Task<IActionResult> CacheTest()
        {
            const string cacheKey = "That_hard_to_get_string";
            var hardToGet = _cache.Get(cacheKey) as string;
            if (hardToGet == null)
            {
                // not in cache - get it then...
                hardToGet = await GetThatHardToGetStringAsync();
                // save in cache for future use
                _cache.Set(cacheKey, hardToGet);
            }

            return View("CacheTest",hardToGet);
        }

        private async Task<string> GetThatHardToGetStringAsync()
        {
            await Task.Delay(5000);
            return ":)";
        }
    }
}
