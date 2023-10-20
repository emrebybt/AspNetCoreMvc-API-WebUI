using AspNetCoreMvc_API_WebUI.Extensions;
using AspNetCoreMvc_API_WebUI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AspNetCoreMvc_API_WebUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EmployeeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var http = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetJson<UserViewModel>("user").Token;
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = await http.GetAsync("https://localhost:7049/api/Employee");
            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsondata = await result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<EmployeeViewModel>>(jsondata);
                return View(data);
            }
            return View();
        }
    }
}
