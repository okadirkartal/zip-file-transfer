using System.Threading.Tasks;
using Core.Entities;

namespace Sender.Services.Contracts
{
    public interface ITransferService
    {
        Task<ResultViewModel> PostToReceiverAsync(string endPoint, TransferModel model);
    }
}