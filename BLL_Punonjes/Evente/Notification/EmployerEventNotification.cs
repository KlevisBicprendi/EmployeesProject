using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Evente.Notification
{
    public class EmployerEventNotification
    {
        public void OnEmployerChanged(object sender, EmployerEventArg e)
        {
            Console.WriteLine($"[Njoftim] : Employer {e.employer.Name} u {e.Action} me sukses !!!");
        }
    }
}
