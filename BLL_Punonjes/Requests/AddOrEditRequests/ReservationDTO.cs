using DAL_Punonjes.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Requests.AddOrEditRequests
{
    public class ReservationDTO
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Purpose { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Range(1, int.MaxValue)]
        public decimal Offer { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public int EmployerId { get; set; }
        //[System.ComponentModel.DataAnnotations.Required]
        //public int UserId { get; set; }
        //[System.ComponentModel.DataAnnotations.Required]
        //public StatusEnum Status { get; set; }
    }
}
