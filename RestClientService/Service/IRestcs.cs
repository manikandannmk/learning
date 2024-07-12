using RestClientModels;

namespace RestClientService.Service
{
    public interface IRestcs
    {
        Task<List<Users>> makegetCall();
    }
}
