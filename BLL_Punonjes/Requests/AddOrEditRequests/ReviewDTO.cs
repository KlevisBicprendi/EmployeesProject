using DAL_Punonjes.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Requests.AddOrEditRequests
{
    public class ReviewDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public IEnum Rate { get; set; }
    }
}
