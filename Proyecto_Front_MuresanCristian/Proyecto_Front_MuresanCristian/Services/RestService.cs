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
        

        public RestService()
        {
        }

        public async Task<bool> AddUsser(User user)
        {
            using (var client = new HttpClient())
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

        public async Task<UserInfo> Login(User user)
        {
            using (var client = new HttpClient())
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
        public Task<Note> GetSharedUserNotes()
        {
            return null;
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
    }
}
