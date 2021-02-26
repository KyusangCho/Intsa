using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Intsa.Shared.Utils
{
    /// <summary>
    /// AmazonS3 파일 업로드 처리 
    /// </summary>
    public class AmazonS3
    {
        private const string bucketName = "int.boards";
        private static string keyName = "*** provide a name for the uploaded object ***";
        private static string filePath = "*** provide the full path name of the file to upload ***";
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;
        
        public static void Main(string key, string path)
        {
            keyName = key;
            filePath = path;

            string accessKey = ""; 
            string secretKey = ""; 

            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/UploadS3accessKey.txt"))
                accessKey = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/UploadS3accessKey.txt").Trim();
            if (File.Exists(System.IO.Directory.GetCurrentDirectory() + "/UploadS3secretKey.txt"))
                secretKey = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/UploadS3secretKey.txt").Trim();

            BasicAWSCredentials basicCredentials = new BasicAWSCredentials(accessKey, secretKey); 
            s3Client = new AmazonS3Client(basicCredentials, bucketRegion);

            UploadFileAsync().Wait();
        }

        private static async Task UploadFileAsync()
        {
            try
            {
                var fileTransferUtility = new TransferUtility(s3Client);
                
                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload, bucketName, keyName);
                }
                //Console.WriteLine("Upload 3 completed");

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }
    }
}
