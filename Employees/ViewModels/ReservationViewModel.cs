using DAL_Punonjes.Entities;

namespace Employees.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public string Purpose { get; set; }
        public decimal Offer { get; set; }
        public string CreatedOn { get; set; }
        public int EmployerId { get; set; }
        public int UserId { get; set; }
        public StatusEnum Status { get; set; }
    }
}
