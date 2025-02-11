using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Controllers
{
    // https://localhost:PortNumber/api/Students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    { 
        // Get : https://localhost:PortNumber/api/Students
        [HttpGet]
        public IActionResult GetStudents()
        {
            var guidData = Guid.NewGuid().ToString();
            var studentsNames = new List<string>()
            {
                "Akash", "RushiKesh", "Shubham", "Shankar", "Archana" , guidData
            };

            return Ok(studentsNames);
        }
    }
}
