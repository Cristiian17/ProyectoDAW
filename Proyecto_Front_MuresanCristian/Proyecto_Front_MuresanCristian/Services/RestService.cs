using Blazored.LocalStorage;
using Newtonsoft.Json;
using Proyecto_Front_MuresanCristian.Entities;
using Proyecto_Front_MuresanCristian.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto_Front_MuresanCristian.Services
{
    public class RestService : IRestService
    {
        public readonly string baseUrl = "https://localhost:7291/";
        private readonly HttpClientHandler _httpClientHandler;

        public RestService(HttpClientHandler httpClientHandler)
        {
            _httpClientHandler = httpClientHandler;
        }

        public async Task<bool> AddUsser(User user)
        {
            using (var client = GetHttpClient())
            {

                var userEnpoit = $"{baseUrl}Users";
                var encryptedPasswd = Encrypt(user.Password);
                user.Password = encryptedPasswd;
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(userEnpoit, content);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                return bool.Parse(json);
            }
        }
        public async Task<bool> GetUserByEmail(string email, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {
                var userEnpoit = $"{baseUrl}Users/{email}";
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(userEnpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return bool.Parse(json);
                }

                return bool.Parse(json);
            }
        }

        public async Task<UserInfo> Login(User user)
        {
            using (var client = GetHttpClient())
            {
                var loginEndpoit = $"{baseUrl}Login";

                user.Password = Encrypt(user.Password);

                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(loginEndpoit, content);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var userInfo = JsonConvert.DeserializeObject<UserInfo>(json);

                return userInfo;
            }
        }

        public async Task<UserInfo> RegenerateToken(UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {
                var regenerateTockenEndpoint = $"{baseUrl}Login/RegenerateToken";

                var content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(regenerateTockenEndpoint, content);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var newUserInfo = JsonConvert.DeserializeObject<UserInfo>(json);

                return newUserInfo;
            }
        }

        public async Task<List<Note>> GetUserNotes(UserInfo userInfo) 
        {
            using (var client = new HttpClient())
            {
                var notesEndpoit = $"{baseUrl}Notes";

                var request = new HttpRequestMessage() 
                { 
                    RequestUri = new Uri(notesEndpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var notes = JsonConvert.DeserializeObject<List<Note>>(json);

                return notes;
            }
        }
        public async Task<List<Note>> GetSharedUserNotes(UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {
                var notesEndpoit = $"{baseUrl}Notes/GetSharedNotes";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(notesEndpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var notes = JsonConvert.DeserializeObject<List<Note>>(json);

                return notes;
            }
        }

        public async Task<Note> AddNote(Note newNote, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes";
                var content = new StringContent(JsonConvert.SerializeObject(newNote), Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonConvert.SerializeObject(newNote), Encoding.UTF8, "application/json")
            };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var note = JsonConvert.DeserializeObject<Note>(json);

                return note;
            }
        }
        public async Task<Note> EditNote(Note editedNote, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Put,
                    Content = new StringContent(JsonConvert.SerializeObject(editedNote), Encoding.UTF8, "application/json")
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var note = JsonConvert.DeserializeObject<Note>(json);

                return note;
            }
        }
        public async Task<bool> AddFav(Note note, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes/AddFav?noteId={note.Id}";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var result = JsonConvert.DeserializeObject<bool>(json);

                return result;
            }
        }

        public async Task<bool> DeleteNote(Note note, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes?noteId={note.Id}";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Delete
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var deleted = JsonConvert.DeserializeObject<bool>(json);

                return deleted;
            }
        }
        public async Task<bool> RemoveFav(Note note, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes/RemoveFav?noteId={note.Id}";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var removed = JsonConvert.DeserializeObject<bool>(json);

                return removed;
            }
        }
        public async Task<bool> AddSharedNote(string email, Note note, UserInfo userInfo)
        {
            using (var client = new HttpClient())
            {

                var noteEnpoit = $"{baseUrl}Notes/AddSharedNote?email={email}&noteId={note.Id}";

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(noteEnpoit),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("Authorization", $"Bearer {userInfo.Token}");

                var response = await client.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(json);
                }

                var result = JsonConvert.DeserializeObject<bool>(json);

                return result;
            }
        }


        public string Encrypt(string password)
        {
            string hash = "coding password";
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);

            MD5 md5 = MD5.Create();
            TripleDES tripldes = TripleDES.Create();

            tripldes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripldes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripldes.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        public string Decrypt(string encryptPasswd)
        {
            string hash = "coding password";
            byte[] data = Convert.FromBase64String(encryptPasswd);

            MD5 md5 = MD5.Create();
            TripleDES tripldes = TripleDES.Create();

            tripldes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripldes.Mode = CipherMode.ECB;

            ICryptoTransform transform = tripldes.CreateEncryptor();
            byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }

        private HttpClient GetHttpClient()
        {
            return _httpClientHandler == null ? new HttpClient() : new HttpClient(_httpClientHandler);
        }
    }
}
