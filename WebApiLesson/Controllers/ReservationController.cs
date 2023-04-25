using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiLesson.Models;

namespace WebApiLesson.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private IRepository repository;

        public ReservationController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet] //получение
        public IEnumerable<Reservation> Get() => repository.Reservations;

        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            if (id == 0)
                return BadRequest("Value must be passed in the request");
            return Ok(repository[id]);
        }

        [HttpPost] //отправление
        public Reservation Post([FromBody] Reservation res) => repository.AddReservation(
                new Reservation
                {
                    Name = res.Name,
                    StartLocation = res.StartLocation,
                    EndLocation = res.EndLocation,
                }
            ); // готовые формочки для ввода данных FromBody

        [HttpPut]
        public Reservation Put([FromForm] Reservation res) => repository.UpdateReservation(res);

        [HttpDelete("{id}")] //удаление
        public void Delete(int id) => repository.DeleteReservation(id);
    }
}
