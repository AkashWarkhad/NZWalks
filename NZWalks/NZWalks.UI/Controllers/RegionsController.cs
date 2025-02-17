using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        // --------------------- Get All Regions Data --------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                // Created a new Client which is used to communicate with the NZWalks.API project.
                var client = httpClientFactory.CreateClient();

                // To get all regions from web api
                var httpResponseMessage = await client.GetAsync("https://localhost:7130/api/Regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
               // Log warning
            }

            return View(response);
        }

        // -------------------- Get Add View -------- No Implementation
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // --------------------- Create Add Region Data ---------------------------------
        [HttpPost]
        public async Task<IActionResult> Add(AddRegionsViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7130/api/Regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null) 
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        // --------------------- Get Region By Id ---------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var httpResponseMessage = await client.GetAsync($"https://localhost:7130/api/Regions/{id.ToString()}");

            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null) 
            {
                return View(response);
            }

            return View(null);
        }

        // ------------------------ POST Edit  ------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto regionDtoReq)
        {
            var client = httpClientFactory.CreateClient();

            var inputData = new AddRegionsViewModel
            {
                Name = regionDtoReq.Name,
                Code = regionDtoReq.Code,
                RegionImageUrl = regionDtoReq.RegionImageUrl
            };

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7130/api/Regions/{regionDtoReq.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(inputData), Encoding.UTF8, "application/json")
            };

            var httpResponse = await client.SendAsync(httpRequestMessage);

            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<RegionDto>();

            if (response != null) 
            {
                return RedirectToAction("Index", "Regions");
            }

            return View(null);
        }

        // ---------------------------------- Delete By Id --------------------------------------
        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponse = await client.DeleteAsync($"https://localhost:7130/api/Regions/{request.Id}");

                httpResponse.EnsureSuccessStatusCode();

                // Response data content after deletion
                var response = await httpResponse.Content.ReadFromJsonAsync<RegionDto>();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex) 
            {
                return View(ex);
            }
        }
    }
}