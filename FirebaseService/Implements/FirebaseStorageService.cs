using Firebase.Auth.Providers;
using Firebase.Storage;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using FirebaseService.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseService.Implements
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        public async Task<bool> DeleteFile(string bucket, string url, string token)
        {
            var storage = new FirebaseStorage(bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = async () => token,
                ThrowOnCancel = true,
            });
            string resultContent = "N/A";
            try
            {
                using HttpClient http = await storage.Options.CreateHttpClientAsync().ConfigureAwait(continueOnCapturedContext: false);
                HttpResponseMessage result = await http.DeleteAsync(url).ConfigureAwait(continueOnCapturedContext: false);
                resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                result.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<string> UploadFile(string bucket, IFormFile file, string path, string token)
        {
            // Get any Stream - it can be FileStream, MemoryStream or any other type of Stream
            var stream = file.OpenReadStream();

            // Constructr FirebaseStorage, path to where you want to upload the file and Put it there
            var task = new FirebaseStorage(bucket,
                 new FirebaseStorageOptions
                 {
                     AuthTokenAsyncFactory = async () => token,
                     ThrowOnCancel = true,
                 })
                .Child(path)
                .PutAsync(stream);

            // await the task to wait until upload completes and get the download url
            var downloadUrl = await task;

            return downloadUrl;
        }
    }
}
