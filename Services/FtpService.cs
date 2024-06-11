using Microsoft.Extensions.Options;
using MyGarden_API.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyGarden_API.Services
{
    public class FtpService : IFtpService
    {
        private readonly FtpOptions _ftpOptions;
        private readonly ILogger<FtpService> _logger;
        private readonly HttpClient _httpClient;

        public FtpService(IOptions<FtpOptions> ftpOptions, ILogger<FtpService> logger, HttpClient httpClient)
        {
            _ftpOptions = ftpOptions.Value;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<string> FtpTransferAsync(string fileName, string folderName, byte[] fileData)
        {
            try
            {
                string ftpAddress = _ftpOptions.FtpAddress;
                string username = _ftpOptions.Username;
                string password = _ftpOptions.Password;

                string ftpUri = $"ftp://{ftpAddress}/Images/{folderName}/{fileName}";

                var credentials = new NetworkCredential(username, password);

                using (var content = new ByteArrayContent(fileData))
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(ftpUri))
                    {
                        Content = content
                    };

                    var handler = new HttpClientHandler
                    {
                        Credentials = credentials,
                        PreAuthenticate = true
                    };

                    using (var client = new HttpClient(handler))
                    {
                        var response = await client.SendAsync(requestMessage);
                        response.EnsureSuccessStatusCode();

                        _logger.LogInformation($"Upload of file {fileName} complete, status: {response.ReasonPhrase}");
                        return response.ReasonPhrase ?? "Success";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "HttpRequestException occurred while uploading file to FTP.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while uploading file to FTP.");
                throw;
            }
        }
    }
}
