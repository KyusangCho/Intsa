using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Cafe.Shared;

namespace Intsa.Managers
{
    public class NoticeAppFileStorageManager : IFileStorageManager
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _containerName;
        private readonly string _folderPath;

        public NoticeAppFileStorageManager(IWebHostEnvironment environment)
        {
            this._environment = environment;
            _containerName = "files"; 
            _folderPath = Path.Combine(_environment.WebRootPath, _containerName);
        }


        public async Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            if (File.Exists(Path.Combine(_folderPath, folderPath, fileName)))
            {
                File.Delete(Path.Combine(_folderPath, folderPath, fileName)); 
            }
            return await Task.FromResult(true); 
        }

        public Task<byte[]> DownloadAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, string ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, long ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public string GetFolderPath(string ownerType, int ownerId, string fileType)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(byte[] bytes, string fileName, string folderPath, bool overwrite)
        {
            await File.WriteAllBytesAsync(Path.Combine(_folderPath, folderPath, fileName), bytes); // byte 배열 저장 
            return fileName; 
        }
        
        public async Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite)
        {
            using(var fileStream = new FileStream(Path.Combine(_folderPath, folderPath, fileName), FileMode.Create))
            {
                await stream.CopyToAsync(fileStream); 
            }

            return fileName; 
        }




    }
}
