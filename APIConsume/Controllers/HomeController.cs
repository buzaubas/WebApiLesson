using APIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Reservation> reservations = new List<Reservation>();
            using(var httpClient = new HttpClient()) //можно использовать RestSharp в данном случае используется HttpClient
            {
                using(var response = await httpClient.GetAsync("http://localhost:5063/api/Reservation"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservations = JsonConvert.DeserializeObject<List<Reservation>>(apiResponse);
                }
            }
            return View(reservations);
        }

        public ViewResult GetReservation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetReservation(int id)
        {
            Reservation reservation = new Reservation();

            using (var httpClient = new HttpClient()) //можно использовать RestSharp в данном случае используется HttpClient
            {
                using (var response = await httpClient.GetAsync("http://localhost:5063/api/Reservation/" + id))
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        reservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                    }
                    else
                    {
                        ViewBag.StatusCode = response.StatusCode;
                    }
                }
                return View(reservation);
            }
        } 

        public ViewResult AddReservation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(Reservation reservation)
        {
            Reservation receivedReservation = new Reservation();

            using (var httpClient = new HttpClient()) //можно использовать RestSharp в данном случае используется HttpClient
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:5063/api/Reservation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    receivedReservation = JsonConvert.DeserializeObject<Reservation>(apiResponse);
                }
                return View(receivedReservation);
            }
        }
    }
}

//необходимо пропустить через Kubernetes что бы работал с Docker

//Angular позволяет отправлять отдельно легкии HTML и с помощью ajax подтягивать запросы по отдельности