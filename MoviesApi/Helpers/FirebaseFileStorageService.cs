using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MoviesApi.Helpers
{
    public class FirebaseFileStorageService:IFileStorageService
    {
        
        

        private readonly string connectionString;
        private readonly string apiKey;
        private readonly string email;
        private readonly string password;

        public FirebaseFileStorageService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("FirebaseStorageConnection");
            apiKey = configuration.GetValue<string>("Firebase:FirebaseApiKey");
            email = configuration.GetValue<string>("Firebase:FirebaseLoginEmail");
            password = configuration.GetValue<string>("Firebase:FirebaseLoginPassword");

        }
        public async Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return;
            }
            var fileName = Path.GetFileName(fileRoute).Split('?')[0].Split("%2F")[1];
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, password);

            var delete = new FirebaseStorage(
                connectionString,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                }).Child(containerName).Child(fileName).DeleteAsync();


        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            //authentication
            var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, password);

            //Cancellation token
            var cancellation = new CancellationTokenSource();

            var upload = new FirebaseStorage(
                connectionString,
                new FirebaseStorageOptions { 
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true,
                    })
                .Child(containerName)
                .Child(fileName)
                .PutAsync(file.OpenReadStream());

            //// await the task to wait until upload completes and get the download url
            var downloadUrl = await upload;
            return downloadUrl;
        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);
            return await SaveFile(containerName, file);
        }
    }
}

