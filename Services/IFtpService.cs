namespace MyGarden_API.Services
{
    public interface IFtpService
    {
        public Task<string> FtpTransferAsync(string fileName, string folderName, byte[] fileData);
    }
}
