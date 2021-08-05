using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var jsonStr = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T>(jsonStr);

            return obj;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            var jsonStr = await response.Content.ReadAsStringAsync();
            var objList = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonStr);

            return objList;
        }

        public async Task<bool> CreateAsync(string url, T objectToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (objectToCreate != null)
            {
                //Appending the object to request
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToCreate),
                                                    Encoding.UTF8, MediaTypeNames.Application.Json);
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> UpdateAsync(string url, T objectToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);

            if (objectToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objectToUpdate),
                                                    Encoding.UTF8, MediaTypeNames.Application.Json);
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}