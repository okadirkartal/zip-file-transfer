using System.Threading.Tasks;
using Core.Entities;

namespace Sender.Services.Contracts
{
    public interface ITransferService
    {
        Task<ResultViewModel> PostToRecipientAsync(string endPoint, TransferModel model);
    }
}