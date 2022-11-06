﻿using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Microsoft.Extensions.Http;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new APIResponse();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();  
        

                message.Headers.Add("Accept", "application/json");    
                message.RequestUri = new Uri(apiRequest.Url);   
                if(apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case MagicVilla_Utility.SD.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case MagicVilla_Utility.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case MagicVilla_Utility.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case MagicVilla_Utility.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);  
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception e)
            {

                var dto = new APIResponse
                {
                    ErrorMessages = new List<string>() { e.Message.ToString() },
                    IsSuccess = false,
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }
        }
    }
}