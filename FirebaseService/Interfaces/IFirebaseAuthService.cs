using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseService.Interfaces
{
    public interface IFirebaseAuthService: IFirebaseAuthClient
    {
        Task<string> LoginGoogle(string url);
    }
}
