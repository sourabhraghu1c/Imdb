using IMDBSample.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IMDBSample.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly SupabaseSettings _settings;
        private readonly Supabase.Client _client;

        public SupabaseService(IOptions<SupabaseSettings> options)
        {
            _settings = options.Value;

            _client = new Supabase.Client(_settings.Url, _settings.ApiKey);
            _client.InitializeAsync().Wait();
        }

        public async Task<string> UploadMoviePosterAsync(int movieId, IFormFile file)
        {
            try
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"{movieId}_{Guid.NewGuid()}{extension}";

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream); //internally generates stream and copy to memory stream 

                var fileBytes = memoryStream.ToArray();

                var bucket = _client.Storage.From(_settings.Bucket);

                await bucket.Upload(fileBytes, fileName);

                return bucket.GetPublicUrl(fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file", ex);
            }
        }
    }
}