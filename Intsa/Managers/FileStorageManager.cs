using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using Cafe.Shared;

namespace Intsa.Managers
{
    public class FileStorageManager : IFileStorageManager
    {
        public Task<bool> DeleteAsync(string fileName, string folderPath)
        {
            throw new NotImplementedException();
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
            await File.WriteAllBytesAsync(Path.Combine(folderPath, fileName), bytes); // byte 배열 저장 
            return fileName; 
        }
        
        public async Task<string> UploadAsync(Stream stream, string fileName, string folderPath, bool overwrite)
        {
            using(var fileStream = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create))
            {
                await stream.CopyToAsync(fileStream); 
            }

            return fileName; 
        }




    }
}
