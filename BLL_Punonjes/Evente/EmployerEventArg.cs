using DAL_Punonjes.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Evente
{
    public class EmployerEventArg : EventArgs
    {
        public Employer employer { get; set; }
        public string Action { get; set; }
    }
}
