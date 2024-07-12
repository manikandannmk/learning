using RestClienttService;
using RestClientModels;


namespace RestClientService.Service
{
    public class RestASer : IRestcs
    {
        RestClient client;
        public RestASer() {
            client = new RestClient();
        }
       
        public async Task<List<Users>> makegetCall()
        {
            var users = await client.GetAsync<List<Users>>("api/User/GetUserList?id=10");
            return users;
        }
    }
}
