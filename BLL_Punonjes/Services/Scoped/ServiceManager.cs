using DAL_Punonjes.UNITOFWORK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Scoped
{
    public interface IServiceManager
    {
        Task<T> ExecuteTransactions<T>(Func<Task<T>> func);
    }

    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> ExecuteTransactions<T>(Func<Task<T>> func)
        {
            await semaphore.WaitAsync();
            try
            {
                return await _unitOfWork.ExecuteTransaction(func);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
