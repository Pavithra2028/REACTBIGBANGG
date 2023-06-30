using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REACTBIGBANG.Models;
using REACTBIGBANG.Repository;

namespace REACTBIGBANG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository doc;

        public DoctorController(IDoctorRepository doc)
        {
            this.doc = doc;
        }

        [HttpGet]
        public IEnumerable<Doctor>? Get()
        {

            return doc.GetDoctor();
        }

        [HttpGet("{DoctorId}")]
        public Doctor? DoctorbyId(int doctorid)
        {

            return doc.DoctorById(doctorid);


        }
        [HttpPost]
        public async Task<ActionResult<Doctor>> Post([FromForm] Doctor doctor, IFormFile imageFile)
        {

            try
            {
                var createdCourse = await doc.CreateDoctor(doctor, imageFile);
                return CreatedAtAction("Get", new { id = createdCourse.Doctor_id }, createdCourse);

            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{DoctorId}")]
        public async Task<ActionResult<Doctor>> Put(int doctorid, [FromForm] Doctor doctor, IFormFile imageFile)
        {
            try
            {
                var updatedCake = await doc.UpdateDoctor(doctorid, doctor, imageFile);
                if (updatedCake == null)
                {
                    return NotFound();
                }

                return Ok(updatedCake);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }


        [HttpDelete("{DoctorId}")]
        public Doctor? DeleteDoctor(int doctorid)
        {
            return doc.DeleteDoctor(doctorid);
        }
      

       
    }
}
