using Microsoft.AspNetCore.Mvc;
using REACTBIGBANG.Models;
using System.Data;

namespace REACTBIGBANG.Repository
{
    public interface IDoctorRepository
    {
        public ICollection<Doctor> GetDoctor();

        public Doctor DoctorById(int Doctor_id);

        Task<Doctor> CreateDoctor([FromForm] Doctor doctor, IFormFile imageFile);

        Task<Doctor> UpdateDoctor(int doctorid, Doctor doctor, IFormFile imageFile);

        Task<Models.Dto.UpdateStatus> UpdateStatus(Models.Dto.UpdateStatus status);
        Task<Models.Dto.UpdateStatus> DeclineDoctorStatus(Models.Dto.UpdateStatus status);

        public Task<ICollection<Doctor>> RequestedDoctor();
        public Task<ICollection<Doctor>> AcceptedDoctor();

        public Doctor DeleteDoctor(int doctorid);
    }
}
