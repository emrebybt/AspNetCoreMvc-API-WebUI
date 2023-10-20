using AspNetCoreMvc_API_WebUI.Extensions;
using AspNetCoreMvc_API_WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AspNetCoreMvc_API_WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var http = _httpClientFactory.CreateClient();
            var jsondata = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsondata, encoding: Encoding.UTF8, "application/json");
            var result = await http.PostAsync("https://localhost:7049/api/Account/Register",content);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var http = _httpClientFactory.CreateClient();
            var jsondata = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsondata, encoding:Encoding.UTF8, "application/json");
            var result = await http.PostAsync("https://localhost:7049/api/Account/Login", content);
            if (result.IsSuccessStatusCode)
            {
                UserViewModel user = await result.Content.ReadFromJsonAsync<UserViewModel>();
                HttpContext.Session.SetJson("user", user);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
