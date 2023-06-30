﻿using System.ComponentModel.DataAnnotations;

namespace REACTBIGBANG.Models
{
    public class Patient
    {
        [Key]

        public int Patient_id { get; set; }

        public string Patient_name { get; set; }

        public int Patient_age { get; set; }

        public string Patient_gender { get; set; }

        public string Medical_treatment { get; set; }

        public string Patient_Address { get; set; }

        public long Phonenumber { get; set; }

        public string Patient_password { get; set; }

        public Doctor Doctor { get; set; }
    }
}
