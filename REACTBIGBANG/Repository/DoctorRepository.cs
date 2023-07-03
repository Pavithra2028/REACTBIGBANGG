﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REACTBIGBANG.Models;
using REACTBIGBANG.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace REACTBIGBANG.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalContext hospitalContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DoctorRepository(HospitalContext con, IWebHostEnvironment webHostEnvironment)
        {
            hospitalContext = con;
            _webHostEnvironment = webHostEnvironment;
        }

        public ICollection<Doctor> GetDoctor()
        {
            var user = hospitalContext.doctors.Include(x => x.patients).ToList();
            return user;
        }

        public Doctor DoctorById(int Doctor_id)
        {
            try
            {
                return hospitalContext.doctors.FirstOrDefault(x => x.Doctor_id == Doctor_id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Doctor> CreateDoctor([FromForm] Doctor doctor, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            doctor.Doctor_image = fileName;
            doctor.Status = "Not Admitted";
            hospitalContext.doctors.Add(doctor);
            await hospitalContext.SaveChangesAsync();

            return doctor;
        }

        public async Task<Doctor> UpdateDoctor(int doctorid, Doctor doctor, IFormFile imageFile)
        {
            var existingDoctor = await hospitalContext.doctors.FindAsync(doctorid);
            if (existingDoctor == null)
            {
                return null;
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Delete the old image file
                var oldFilePath = Path.Combine(uploadsFolder, existingDoctor.Doctor_image);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }

                existingDoctor.Doctor_image = fileName;
            }

            existingDoctor.Doctor_name = doctor.Doctor_name;
            existingDoctor.Specialization = doctor.Specialization;
            existingDoctor.Doctor_gender = doctor.Doctor_gender;
            existingDoctor.Doctor_experience = doctor.Doctor_experience;
            await hospitalContext.SaveChangesAsync();

            return existingDoctor;
        }

        public async Task<Models.Dto.UpdateStatus> UpdateStatus(Models.Dto.UpdateStatus status)
        {
            var doc = await hospitalContext.doctors.FirstOrDefaultAsync(s => s.Doctor_id == status.id);
            if (doc != null)
            {
                if (doc.Status == "Not Admitted")
                {
                    doc.Status = "Accepted";
                    await hospitalContext.SaveChangesAsync();
                    return status;
                }
                return status;

            }
            return null;
        }

        public async Task<Models.Dto.UpdateStatus> DeclineDoctorStatus(Models.Dto.UpdateStatus status)
        {
            var doc = await hospitalContext.doctors.FirstOrDefaultAsync(s => s.Doctor_id == status.id);
            if (doc != null)
            {
                if (doc.Status == "Not Admitted")
                {
                    doc.Status = "Declined";
                    await hospitalContext.SaveChangesAsync();
                    return status;
                }
                return status;

            }
            return null;
        }

        public async Task<ICollection<Doctor>> RequestedDoctor()
        {
            var doc = await hospitalContext.doctors.Where(s => s.Status == "Not Admitted").ToListAsync();
            if (doc != null)
            {
                return doc;
            }
            return null;
        }

        public async Task<ICollection<Doctor>> AcceptedDoctor()
        {
            var doc = await hospitalContext.doctors.Where(s => s.Status == "Accepted").ToListAsync();
            if (doc != null)
            {
                return doc;
            }
            return null;
        }

        public Doctor DeleteDoctor(int doctorid)
        {
            try
            {
                Doctor doc = hospitalContext.doctors.FirstOrDefault(x => x.Doctor_id == doctorid);
                if (doc != null)
                {
                    hospitalContext.doctors.Remove(doc);
                    hospitalContext.SaveChanges();
                    return doc;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Doctor DoctorbyId(int doctorid)
        {
            throw new NotImplementedException();
        }
    }
}
