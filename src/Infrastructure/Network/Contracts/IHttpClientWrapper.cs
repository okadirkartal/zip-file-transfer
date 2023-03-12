 
namespace Infrastructure.Network.Contracts
{
    public interface IHttpClientWrapper
    {
        string BaseUrl { get; set; }

        string AuthorizationHeader { get; set; }

        string AuthorizationValue { get; set; }

        Task<TDto?> PostAsync<TDto>(object dto);
    }
}