using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Core_Sample_Project_OnionSharp.Controllers
{
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TestController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Index()
        {
            var onionhttpClient = _httpClientFactory.CreateClient("Blockstream-info");

            var response = await onionhttpClient.GetAsync("http://explorerzydxu5ecjrkwceayqybizmpjjznk5izmitf2modhcusuqlid.onion/api/mempool/recent");
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Response Status: {response.StatusCode}");

            return Ok(responseContent);
        }
    }
}
