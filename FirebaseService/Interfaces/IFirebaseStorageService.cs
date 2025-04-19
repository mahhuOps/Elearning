using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseService.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFile(string bucket,IFormFile file, string path, string token);

        Task<bool> DeleteFile(string bucket,string url, string token);
    }
}
