using DAL_Punonjes.Entities;

namespace Employees.ViewModels
{
    public class ReviewViewModels
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string Comment { get; set; }
            public IEnum Rate { get; set; }
        }
}
