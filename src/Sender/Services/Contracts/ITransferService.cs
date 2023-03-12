using System.Threading.Tasks;
using Domain.Entities;

namespace Sender.Services.Contracts;
public interface ITransferService
{
    Task<ResultViewModel> PostToRecipientAsync(TransferModel model);
}
