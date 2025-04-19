using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using FirebaseService.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirebaseService.Implements
{
    public class FirebaseAuthService: IFirebaseAuthService
    {
        FirebaseAuthClient _firebaseProvider;
        IConfigurationSection _firebaseConfig;

        public FirebaseAuthService(IConfiguration configuration)
        {
            _firebaseConfig = configuration.GetSection("Firebase");

            var config = new FirebaseAuthConfig();
            config.ApiKey = _firebaseConfig["API_KEY"];
            config.AuthDomain = _firebaseConfig["AUTH_DOMAIN"];
            config.Providers = new FirebaseAuthProvider[]
            {
                new GoogleProvider().AddScopes("email"),
                new EmailProvider()
            };

            config.UserRepository = new FileUserRepository("LoginData");

            _firebaseProvider = new FirebaseAuthClient(config);
        }
        public User User => _firebaseProvider.User;

        public event EventHandler<UserEventArgs> AuthStateChanged;

        public async Task<UserCredential> CreateUserWithEmailAndPasswordAsync(string email, string password, string displayName = null)
        {
            return await _firebaseProvider.CreateUserWithEmailAndPasswordAsync(email, password, displayName);
        }

        public Task<FetchUserProvidersResult> FetchSignInMethodsForEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task ResetEmailPasswordAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserCredential> SignInAnonymouslyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserCredential> SignInWithCredentialAsync(AuthCredential credential)
        {
            throw new NotImplementedException();
        }

        public async Task<UserCredential> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            return await _firebaseProvider.SignInWithEmailAndPasswordAsync(email, password);
        }

        public async Task<UserCredential> SignInWithRedirectAsync(FirebaseProviderType authType, SignInRedirectDelegate redirectDelegate)
        {
            var result = await _firebaseProvider.SignInWithRedirectAsync(authType, redirectDelegate);
            return result;
        }

        public async Task<string> LoginGoogle(string url)
        {
            // TODO: Implement Google Sign-In using Firebase SDK
            // This is a placeholder implementation. Please replace with the correct implementation.
            // For now, just return the Google Sign-In URL
            return $"https://accounts.google.com/o/oauth2/v2/auth?client_id={_firebaseConfig["API_KEY"]}&redirect_uri={url}&response_type=token&scope=openid%20email%20profile";
        }

        //public async Task<User> SignInWithGoogleIdToken(string idToken)
        //{
        //    var userCredential = await _firebaseProvider.SignInWithGoogleIdTokenAsync(idToken);
        //    var firebaseUser = userCredential.User;

        //    // Check if user exists in database
        //    var user = await _databaseService.GetByField<User>("FirebaseID", firebaseUser.Uid);

        //    if (user == null)
        //    {
        //        // Create new user
        //        user = new User
        //        {
        //            FirebaseID = firebaseUser.Uid,
        //            Email = firebaseUser.Email,
        //            FullName = firebaseUser.DisplayName,
        //            Avatar = firebaseUser.PhotoUrl,
        //            IsAdmin = false
        //        };

        //        await _databaseService.Insert(user);
        //    }

        //    return user;
        //}

        public void SignOut()
        {
            _firebaseProvider.SignOut();
        }
    }
}
