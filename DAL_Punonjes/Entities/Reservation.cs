using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Punonjes.Entities
{
    public enum StatusEnum
    {
        Pending = 0,
        Confirmed = 1,
        Canceled = 2,
        Rejected = 3
    }
    public class Reservation
    {
        public int Id { get; set; }
        public string Purpose { get; set; }
        public decimal Offer { get; set; }
        public DateTime CreatedOn { get; set; }
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public StatusEnum Status { get; set; }
    }
}
