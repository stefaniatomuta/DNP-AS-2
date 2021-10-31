using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Models;

namespace FamilyManager2UI.WebClient {
    public class RestClient : IRestClient {
        private string requestUrl = "https://localhost:5003";
        
        //Get methods 
        public async Task<T> GetAsync<T>(string username, string password) {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync($"{requestUrl}/User?username={username}&password={password}");
            string result = await responseMessage.Content.ReadAsStringAsync();
            
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T item = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return item;
        }
        
        public async Task<List<T>> GetAsync<T>() {
            using HttpClient client = new HttpClient();
            string url = "";
            switch (typeof(T).Name) {
                case "Child":
                    url = "children";
                    break;
                case "Pet":
                    url = "pets";
                    break;
                case "Adult":
                    url = "adults";
                    break;
                case "Family":
                    url = "families";
                    break;
            }

            HttpResponseMessage responseMessage = await client.GetAsync($"{requestUrl}/{url}");
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            List<T> items = JsonSerializer.Deserialize<List<T>>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return items;
        }

        public async Task<IList<string>> GetColorAsync(string type) {
            using HttpClient client = new HttpClient();
            string url = "data";
            switch (type) {
                case "eyecolors":
                    url += "/eyecolors";
                    break; 
                case "haircolors":
                    url += "/haircolors";
                    break;
            }

            HttpResponseMessage responseMessage = await client.GetAsync($"{requestUrl}/{url}");
            string result = await responseMessage.Content.ReadAsStringAsync();
            
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            result = result.Replace("[", "");
            result = result.Replace("]", "");
            result = result.Replace("\"", "");
            IList<string> colors = result.Split(",");
            
            return colors;
        }
        
        public async Task<Person> GetPerson(int id, string firstName, string lastName) {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.GetAsync($"{requestUrl}/people/{id}?firstName={firstName}&lastName={lastName}");
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode) throw new Exception(result);

            if (result.ToLower().Contains("interests") && result.ToLower().Contains("pets")) {
                return JsonSerializer.Deserialize<Child>(result, new JsonSerializerOptions {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }

            return JsonSerializer.Deserialize<Adult>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public async Task<T> GetAsync<T>(int id) {
            using HttpClient client = new HttpClient();
            string url = "";
            switch (typeof(T).Name) {
                case "Child":
                    url = "children";
                    break;
                case "Pet":
                    url = "pets";
                    break;
                case "Adult":
                    url = "adults";
                    break;
            }

            HttpResponseMessage responseMessage = await client.GetAsync($"{requestUrl}/{url}/{id}");
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T item = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return item;
        }

        public async Task<T> GetAsync<T>(string streetName, int streetNumber) {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage =
                await client.GetAsync($"{requestUrl}/families/{streetName}/{streetNumber}");
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T items = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return items;
        }

        //Post methods
        
        public async Task PostAsync<T>(T user) {
            using HttpClient client = new HttpClient();

            string itemAsJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(
                itemAsJson, Encoding.UTF8, "application/Json");
            HttpResponseMessage responseMessage = await client.PostAsync($"{requestUrl}/User", content);
            string result = await responseMessage.Content.ReadAsStringAsync();
            
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T newItem = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            // return newItem;
        }
        
        
        public async Task<T> PostAsync<T>(T item, string streetName, int streetNumber) {
            using HttpClient client = new HttpClient();
            string url = "";
            switch (typeof(T).Name) {
                case "Child":
                    url = $"{requestUrl}/children/{streetName}/{streetNumber}";
                    break;
                case "Pet":
                    url = $"{requestUrl}/pets/{streetName}/{streetNumber}";
                    break;
                case "Adult":
                    url = $"{requestUrl}/adults/{streetName}/{streetNumber}";
                    break;
                case "Family":
                    url = $"{requestUrl}/families";
                    break;
            }

            string itemAsJson = JsonSerializer.Serialize(item);
            StringContent content = new StringContent(
                itemAsJson, Encoding.UTF8, "application/Json");
            HttpResponseMessage responseMessage = await client.PostAsync($"{url}", content);
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T newItem = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return newItem;
        }

        public async Task<T> PostAsync<T>(T item, string streetName, int streetNumber, int childId) {
            using HttpClient client = new HttpClient();
            string itemAsJson = JsonSerializer.Serialize(item);
            StringContent content = new StringContent(
                itemAsJson, Encoding.UTF8, "application/Json");
            HttpResponseMessage responseMessage =
                await client.PostAsync($"{requestUrl}/pets/{streetName}/{streetNumber}/{childId}",
                    content);
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T newItem = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return newItem;
        }

        //Put methods
        public async Task<T> PutAsync<T>(T item, int id) {
            using HttpClient client = new HttpClient();
            string url = "";
            switch (typeof(T).Name) {
                case "Child":
                    url = $"children/{id}";
                    break;
                case "Pet":
                    url = $"pets/{id}";
                    break;
                case "Adult":
                    url = $"adults/{id}";
                    break;
                case "Family":
                    url = "families";
                    break;
            }

            string itemAsJson = JsonSerializer.Serialize(item);
            StringContent content = new StringContent(
                itemAsJson, Encoding.UTF8, "application/Json");
            HttpResponseMessage responseMessage = await client.PutAsync($"{requestUrl}/{url}", content);
            
            string result = await responseMessage.Content.ReadAsStringAsync();
            if (!responseMessage.IsSuccessStatusCode)
                throw new Exception(result);

            T newItem = JsonSerializer.Deserialize<T>(result, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return newItem;
        }

        //Delete methods
        public async Task<object> DeleteAsync<T>(int id) {
            using HttpClient client = new HttpClient();
            string url = "";
            switch (typeof(T).Name) {
                case "Child":
                    url = $"children/{id}";
                    break;
                case "Pet":
                    url = $"pets/{id}";
                    break;
                case "Adult":
                    url = $"adults/{id}";
                    break;
                case "Family":
                    url = "families";
                    break;
            }

            HttpResponseMessage responseMessage = await client.DeleteAsync($"{requestUrl}/{url}");
            if (!responseMessage.IsSuccessStatusCode) {
                string error = await responseMessage.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return null;
        }

        public async Task<object> DeleteAsync<T>(string streetName, int streetNumber) {
            using HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = await client.DeleteAsync($"{requestUrl}/families/{streetName}/{streetNumber}");
            if (!responseMessage.IsSuccessStatusCode) {
                string error = await responseMessage.Content.ReadAsStringAsync();
                throw new Exception(error);
            }

            return null;
        }
    }
}